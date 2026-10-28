[CmdletBinding()]
param(
    [string]$RhinoBranchName,
    # Check the new stamp and the committed payload versions only; writes and stamps nothing.
    [switch]$DryRun,
    # Stamp without comparing against the ccycles.dll committed in big_libs.
    [switch]$SkipVersionGuard
)

$bd = [System.DateTime]::UtcNow

$scriptRoot = if ($PSScriptRoot) { $PSScriptRoot } else { Split-Path -Parent $MyInvocation.MyCommand.Path }
. (Join-Path $scriptRoot "rhino_branch_info.ps1")
. (Join-Path $scriptRoot "ccycles_version_guard.ps1")

$branchInfo = Resolve-RhinoBranchInfo -StartPath $scriptRoot -RhinoBranchName $RhinoBranchName
$rhinoBranchRoot = $branchInfo.BranchRoot
$rhinoBranchName = $branchInfo.BranchName
$rhinoMajorVersion = $branchInfo.MajorVersion
$rhinoMinorVersion = $branchInfo.MinorVersion

# Other calendars (th-TH, ar-SA) would give a different year.
$yy = $bd.ToString("yy", [System.Globalization.CultureInfo]::InvariantCulture)
$doy = $bd.DayOfYear.ToString("D3")
$bhr = $bd.ToString("HH", [System.Globalization.CultureInfo]::InvariantCulture)
$bmm = $bd.Minute.ToString("D2")

$dotted = "$rhinoMajorVersion.$rhinoMinorVersion.$yy$doy.$bhr$bmm" + "1"
$commas = $dotted.Replace(".", ",")

Write-Host "-> branch: $rhinoBranchName (label from $($branchInfo.Source)); major $rhinoMajorVersion from $($branchInfo.MajorSource), minor $rhinoMinorVersion"
Write-Host "-> $dotted"
Write-Host "-> $commas"

if ($SkipVersionGuard) {
    Write-Warning "Version guard skipped; don't commit this ccycles.dll unless it is stamped higher than the one in big_libs."
}
else {
    try {
        $bigLibsRoot = Join-Path $rhinoBranchRoot "big_libs"
        $committedVersion = Get-CommittedDllVersion -BigLibsRoot $bigLibsRoot
        if ($DryRun) {
            Assert-CommittedPayload -BigLibsRoot $bigLibsRoot -Major $rhinoMajorVersion -Minor $rhinoMinorVersion
        }
        Assert-CcyclesVersionIncreases -NewVersion $dotted -CommittedVersion $committedVersion
    }
    catch {
        Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
        exit 1
    }
}

if ($DryRun) {
    Write-Host "dry run; nothing written"
    exit 0
}

# Write .rc files
# -- ccycles
(Get-Content .\dll_version_replace.template).Replace("VERSIONCOMMAS", $commas).Replace("VERSIONDOTS", $dotted) | Set-Content .\cycles\install\dll_version_replace.rc
# -- openvdb
(Get-Content .\openvdb_manifest_replace.template).Replace("VERSIONCOMMAS", $commas).Replace("VERSIONDOTS", $dotted) | Set-Content .\cycles\install\openvdb_manifest_replace.rc
# Write manifest files
# -- ccycles
(Get-Content .\ccycles_manifest.txt).Replace("VERSIONCOMMAS", $commas).Replace("VERSIONDOTS", $dotted) | Set-Content .\cycles\install\ccycles_manifest.txt
# -- openvdb
(Get-Content .\openvdb_manifest.txt).Replace("VERSIONCOMMAS", $commas).Replace("VERSIONDOTS", $dotted) | Set-Content .\cycles\install\openvdb_manifest.txt

Push-Location .\cycles\install

Remove-Item *gyd* -ErrorAction SilentlyContinue
Remove-Item *_d.dll -ErrorAction SilentlyContinue
Remove-Item *.so -ErrorAction SilentlyContinue
Remove-Item *.so.* -ErrorAction SilentlyContinue
Remove-Item lib\*.so -ErrorAction SilentlyContinue
Remove-Item lib\*.so.* -ErrorAction SilentlyContinue
Remove-Item *_d_*.dll -ErrorAction SilentlyContinue

cmd.exe /c "ResourceHacker -open .\dll_version_replace.rc -save .\dll_version_replace.res -action compile"
cmd.exe /c "ResourceHacker -open .\openvdb_manifest_replace.rc -save .\openvdb_manifest_replace.res -action compile"
cmd.exe /c "ResourceHacker -open openvdb.dll -save openvdb.dll -resource .\openvdb_manifest_replace.res -action addoverwrite -mask MANIFEST,,"
# MSI treats unversioned files as user data and may keep them on upgrade, so give the oneAPI
# runtime our stamp too; sycl6.dll and ze_loader.dll keep their upstream versions.
foreach ($dll in $StampedDlls | Where-Object { $_ -ne "ccycles.dll" }) {
    cmd.exe /c "ResourceHacker -open $dll -save $dll -resource .\dll_version_replace.res -action addoverwrite -mask VERSIONINFO,,"
}

Pop-Location

cmd.exe /c "ResourceHacker -script .\ccycles_resource_update.txt"

# ResourceHacker failures don't reach the exit code, so check what actually got stamped.
# Wrapper builds stage only ccycles.dll.
$unstamped = foreach ($dll in $StampedDlls) {
    $path = Join-Path ".\cycles\install" $dll
    if (-not (Test-Path -LiteralPath $path)) {
        if ($dll -eq "ccycles.dll") { "$dll is missing" }
        continue
    }
    try {
        $actual = Get-DllVersion (Resolve-Path -LiteralPath $path).ProviderPath
    }
    catch {
        "$dll can't be read: $($_.Exception.Message)"
        continue
    }
    if ($actual -ne [version]$dotted) {
        "$dll is $actual"
    }
}
if ($unstamped) {
    Write-Host "ERROR: Stamping $dotted failed: $($unstamped -join '; ')." -ForegroundColor Red
    exit 1
}

Write-Host "ready"
Write-Host ""
