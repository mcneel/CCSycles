# Windows Installer keeps an installed versioned file unless the packaged one is higher, so a
# Cycles DLL stamped lower than what big_libs already ships breaks rendering on upgrade.

$script:CyclesPayloadDir = "RhinoCycles/ccycles/win/release"
# versioninfo_changer.ps1 stamps these; the other versioned payload DLLs keep upstream versions.
$script:StampedDlls = "ccycles.dll", "cycles_kernel_oneapi_jit.dll", "pi_level_zero.dll", "xptifw.dll"
$script:UpstreamDlls = "sycl6.dll", "ze_loader.dll"

function Get-DllVersion {
    param([Parameter(Mandatory = $true)][string]$Path)

    $info = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($Path)
    return [version]::new($info.FileMajorPart, $info.FileMinorPart, $info.FileBuildPart, $info.FilePrivatePart)
}

function Get-CommittedDllVersion {
    # Reads the version from the local LFS object behind the committed pointer; never fetches.
    # Returns $null when nothing is committed at that path.
    param(
        [Parameter(Mandatory = $true)][string]$BigLibsRoot,
        [string]$Name = "ccycles.dll",
        [string]$Revision = "HEAD"
    )

    # An uninitialized submodule would make git answer for the enclosing Rhino repo instead.
    $prefix = & git -C $BigLibsRoot rev-parse --show-prefix 2>$null
    if ($LASTEXITCODE -ne 0 -or $prefix) {
        throw "'$BigLibsRoot' is not a big_libs checkout (submodule not initialized?); cannot check the committed $Name version."
    }

    & git -C $BigLibsRoot rev-parse --verify --quiet "$Revision^{commit}" *> $null
    if ($LASTEXITCODE -ne 0) {
        throw "Cannot resolve '$Revision' in big_libs at '$BigLibsRoot'; cannot check the committed $Name version."
    }

    $path = "$script:CyclesPayloadDir/$Name"
    $spec = "${Revision}:$path"
    & git -C $BigLibsRoot cat-file -e $spec 2>$null
    if ($LASTEXITCODE -ne 0) {
        return $null
    }

    if ([long](& git -C $BigLibsRoot cat-file -s $spec) -gt 1024) {
        throw "big_libs ${Revision}:$path is not a Git LFS pointer."
    }

    $pointer = (& git -C $BigLibsRoot cat-file blob $spec) -join "`n"
    $oid = [System.Text.RegularExpressions.Regex]::Match($pointer, '(?m)^oid sha256:(?<oid>[0-9a-f]{64})$').Groups["oid"].Value
    $size = [System.Text.RegularExpressions.Regex]::Match($pointer, '(?m)^size (?<size>\d+)$').Groups["size"].Value
    if (-not $oid -or -not $size) {
        throw "big_libs ${Revision}:$path is not a Git LFS pointer."
    }

    $gitDir = & git -C $BigLibsRoot rev-parse --path-format=absolute --git-common-dir
    $object = Join-Path $gitDir "lfs/objects/$($oid.Substring(0, 2))/$($oid.Substring(2, 2))/$oid"
    if (-not (Test-Path -LiteralPath $object) -or (Get-Item -LiteralPath $object).Length -ne [long]$size) {
        $mb = [System.Math]::Max(1, [System.Math]::Round([long]$size / 1MB))
        throw ("$Name at big_libs $Revision (LFS $($oid.Substring(0, 12))) is not in the local LFS store, so its version can't be checked. " +
            "Fetch it ($mb MB) with: git -C `"$BigLibsRoot`" lfs fetch origin $Revision --include=`"$path`", " +
            "or rerun with -SkipVersionGuard and don't commit the result.")
    }

    return Get-DllVersion $object
}

function Assert-CcyclesVersionIncreases {
    param(
        [Parameter(Mandatory = $true)][version]$NewVersion,
        [AllowNull()][version]$CommittedVersion,
        [string]$CommittedLabel = "big_libs HEAD"
    )

    if ($null -eq $CommittedVersion) {
        Write-Host "-> $CommittedLabel has no ccycles.dll; nothing to compare against"
        return
    }

    if ($NewVersion -gt $CommittedVersion) {
        Write-Host "-> $NewVersion is higher than ccycles.dll $CommittedVersion in $CommittedLabel"
        return
    }

    $hint = if ($NewVersion.Major -ne $CommittedVersion.Major -or $NewVersion.Minor -ne $CommittedVersion.Minor) {
        "Check RMA_VERSION_MAJOR/MINOR in src4/version.h, or whether big_libs is from a newer branch."
    }
    else {
        "Check the system clock."
    }
    throw ("Refusing to stamp Cycles $NewVersion; $CommittedLabel already ships ccycles.dll $CommittedVersion. " +
        "Installer upgrades only replace it with a higher version, so they would keep the old DLL. $hint")
}

function Assert-CommittedPayload {
    # Nothing re-stamps what a revert or a merge from an older branch commits, so check each
    # versioned payload DLL against src4/version.h and against the version it replaced.
    param(
        [Parameter(Mandatory = $true)][string]$BigLibsRoot,
        [Parameter(Mandatory = $true)][int]$Major,
        [Parameter(Mandatory = $true)][int]$Minor,
        [string]$Revision = "HEAD"
    )

    $problems = foreach ($name in $script:StampedDlls + $script:UpstreamDlls) {
        try {
            $committed = Get-CommittedDllVersion -BigLibsRoot $BigLibsRoot -Name $name -Revision $Revision
            if ($null -eq $committed) {
                continue
            }

            if ($script:StampedDlls -contains $name) {
                if ($committed -eq [version]::new(0, 0, 0, 0)) {
                    "$name has no version"
                }
                elseif ([version]::new($committed.Major, $committed.Minor) -lt [version]::new($Major, $Minor)) {
                    "$name $committed is older than src4/version.h $Major.$Minor"
                }
            }

            $changedIn = & git -C $BigLibsRoot log --first-parent -n 1 --format=%h $Revision -- "$script:CyclesPayloadDir/$name"
            if (-not $changedIn) {
                continue
            }
            $parent = & git -C $BigLibsRoot rev-parse --short --verify --quiet "$changedIn^1" 2>$null
            if ($LASTEXITCODE -ne 0) {
                continue
            }
            $previous = Get-CommittedDllVersion -BigLibsRoot $BigLibsRoot -Name $name -Revision $parent
            if ($null -ne $previous -and $committed -lt $previous) {
                "$name went from $previous down to $committed in $changedIn"
            }
        }
        catch {
            $_.Exception.Message
        }
    }

    if ($problems) {
        throw ("The Cycles payload in big_libs $Revision fails the version check: " + (($problems | ForEach-Object { $_.TrimEnd(".") }) -join "; ") + ". " +
            "Installer upgrades keep an installed DLL unless the new one is higher; rebuild or re-stamp Cycles before shipping.")
    }

    Write-Host "-> payload DLLs in big_libs $Revision are not older than src4/version.h $Major.$Minor or than the versions they replaced"
}
