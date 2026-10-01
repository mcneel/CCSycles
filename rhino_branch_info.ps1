function Get-RhinoBranchNameCandidate {
    param([AllowNull()][string]$Value)

    if ([string]::IsNullOrWhiteSpace($Value)) {
        return $null
    }

    $trimmedValue = $Value.Trim()
    $match = [System.Text.RegularExpressions.Regex]::Match($trimmedValue, '(^|[\\/])(?<branch>\d+\.x)($|[\\/])')
    if (-not $match.Success) {
        return $null
    }

    return $match.Groups["branch"].Value
}

function Get-RhinoRepoRoot {
    param([Parameter(Mandatory = $true)][string]$StartPath)

    $resolvedStartPath = [System.IO.Path]::GetFullPath($StartPath)
    $match = [System.Text.RegularExpressions.Regex]::Match(
        $resolvedStartPath,
        '^(?<root>.+?)[\\/]src4[\\/]rhino4[\\/]Plug-ins[\\/]RDK[\\/]cycles(?:[\\/]|$)',
        [System.Text.RegularExpressions.RegexOptions]::IgnoreCase
    )
    if ($match.Success) {
        return [System.IO.Path]::GetFullPath($match.Groups["root"].Value)
    }

    return $null
}

function Get-RhinoVersionFromHeader {
    # RH-98439: the version stamp must follow the source tree, not the folder or branch
    # name. A 9.x branch checked out in a tree named "8.x" stamped an 8.0 version, and
    # Windows Installer then kept the old DLL on upgrade.
    param(
        [AllowNull()][string]$RepoRoot,
        [ValidateSet("MAJOR", "MINOR")][string]$Part = "MAJOR"
    )

    if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
        return $null
    }

    $versionHeader = Join-Path $RepoRoot "src4/version.h"
    if (-not (Test-Path -LiteralPath $versionHeader)) {
        return $null
    }

    $match = [System.Text.RegularExpressions.Regex]::Match(
        (Get-Content -LiteralPath $versionHeader -Raw),
        "(?m)^\s*#\s*define\s+RMA_VERSION_$Part\s+(?<value>\d+)")
    if (-not $match.Success) {
        return $null
    }

    return $match.Groups["value"].Value
}

function Invoke-GitString {
    param(
        [Parameter(Mandatory = $true)][string]$WorkingDirectory,
        [Parameter(Mandatory = $true)][string[]]$Arguments
    )

    if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
        return $null
    }

    if (-not (Test-Path $WorkingDirectory)) {
        return $null
    }

    $output = & git -C $WorkingDirectory @Arguments 2>$null
    if ($LASTEXITCODE -ne 0) {
        return $null
    }

    return (($output | Where-Object { -not [string]::IsNullOrWhiteSpace($_) }) -join [System.Environment]::NewLine).Trim()
}

function Resolve-RhinoBranchInfo {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)][string]$StartPath,
        [string]$RhinoBranchName
    )

    $resolvedStartPath = [System.IO.Path]::GetFullPath($StartPath)
    $rhinoRepoRoot = Get-RhinoRepoRoot -StartPath $resolvedStartPath
    $gitWorkingDirectory = if ($rhinoRepoRoot) { $rhinoRepoRoot } else { $resolvedStartPath }
    $gitRoot = Invoke-GitString -WorkingDirectory $gitWorkingDirectory -Arguments @("rev-parse", "--show-toplevel")
    if ($gitRoot) {
        $gitRoot = [System.IO.Path]::GetFullPath($gitRoot)
    }

    $resolvedBranchName = $null
    $source = $null

    if ($PSBoundParameters.ContainsKey("RhinoBranchName") -and -not [string]::IsNullOrWhiteSpace($RhinoBranchName)) {
        $resolvedBranchName = Get-RhinoBranchNameCandidate -Value $RhinoBranchName
        if (-not $resolvedBranchName) {
            throw "Invalid Rhino branch override '$RhinoBranchName'. Expected something like '8.x' or '9.x', or a branch path containing it."
        }
        $source = "parameter"
    }

    if (-not $resolvedBranchName -and $rhinoRepoRoot) {
        $resolvedBranchName = Get-RhinoBranchNameCandidate -Value (Split-Path -Leaf $rhinoRepoRoot)
        $source = "path"
    }

    if (-not $resolvedBranchName) {
        $gitBranch = Invoke-GitString -WorkingDirectory $gitWorkingDirectory -Arguments @("branch", "--show-current")
        $resolvedBranchName = Get-RhinoBranchNameCandidate -Value $gitBranch
        if ($resolvedBranchName) {
            $source = "git"
        }
    }

    if (-not $resolvedBranchName) {
        $envBranchName = Get-RhinoBranchNameCandidate -Value $env:RHINO_BRANCH_NAME
        if ($envBranchName) {
            $resolvedBranchName = $envBranchName
            $source = "env:RHINO_BRANCH_NAME"
        }
    }

    $majorFromHeader = Get-RhinoVersionFromHeader -RepoRoot $rhinoRepoRoot
    if (-not $majorFromHeader -and $gitRoot) {
        $majorFromHeader = Get-RhinoVersionFromHeader -RepoRoot $gitRoot
    }

    # The minor is the service release (9.1 = SR1); stamping it keeps the DLL newer than any
    # build of an earlier service release, so the installer replaces it on upgrade.
    $minorFromHeader = Get-RhinoVersionFromHeader -RepoRoot $rhinoRepoRoot -Part MINOR
    if (-not $minorFromHeader -and $gitRoot) {
        $minorFromHeader = Get-RhinoVersionFromHeader -RepoRoot $gitRoot -Part MINOR
    }
    if (-not $minorFromHeader) {
        Write-Warning "Could not read RMA_VERSION_MINOR from src4/version.h; using minor 0."
        $minorFromHeader = "0"
    }

    if (-not $resolvedBranchName) {
        if (-not $majorFromHeader) {
            $startLeaf = Split-Path -Leaf $resolvedStartPath
            throw "Could not determine the Rhino major version: no RMA_VERSION_MAJOR in src4/version.h under '$startLeaf', and no ancestor folder or git branch containing '8.x' or '9.x'. You can pass -RhinoBranchName 8.x to override the label."
        }
        # The label is cosmetic; the major is what matters.
        $resolvedBranchName = "$majorFromHeader.x"
        $source = "version.h"
    }

    $majorVersionMatch = [System.Text.RegularExpressions.Regex]::Match($resolvedBranchName, '^(?<major>\d+)\.x$')
    if (-not $majorVersionMatch.Success) {
        throw "Resolved Rhino branch '$resolvedBranchName' is invalid. Expected format like '8.x' or '9.x'."
    }

    $resolvedMajorVersion = $majorVersionMatch.Groups["major"].Value
    if ($majorFromHeader) {
        if ($majorFromHeader -ne $resolvedMajorVersion) {
            Write-Warning "Branch '$resolvedBranchName' (source $source) says major $resolvedMajorVersion, but src4/version.h says $majorFromHeader. Using $majorFromHeader."
        }
        $resolvedMajorVersion = $majorFromHeader
        $majorSource = "version.h"
    }
    else {
        Write-Warning "Could not read RMA_VERSION_MAJOR from src4/version.h; falling back to the major implied by branch '$resolvedBranchName'."
        $majorSource = $source
    }

    $resolvedBranchRoot = if ($rhinoRepoRoot) { $rhinoRepoRoot } elseif ($gitRoot) { $gitRoot } else { $resolvedStartPath }

    [PSCustomObject]@{
        BranchName   = $resolvedBranchName
        BranchRoot   = $resolvedBranchRoot
        MajorVersion = $resolvedMajorVersion
        MinorVersion = $minorFromHeader
        Source       = $source
        MajorSource  = $majorSource
        GitRoot      = $gitRoot
    }
}
