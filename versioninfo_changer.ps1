$cwd = $PSScriptRoot

$bd = [System.DateTime]::UtcNow

$yy = $bd.ToString("yy")
$doy = $bd.DayOfYear.ToString("D3")
$bhr = $bd.ToString("HH")
$bmm = $bd.Minute.ToString("D2")

$dotted = "8.0.$yy$doy.$bhr$bmm" + "1"
$commas = $dotted.Replace(".", ",")

Write-Host "-> $dotted"
Write-Host "-> $commas"

(Get-Content .\dll_version_replace.template).Replace("VERSIONCOMMAS", $commas).Replace("VERSIONDOTS", $dotted) | Set-Content .\cycles\install\dll_version_replace.rc

Push-Location .\cycles\install

Remove-Item *gyd* -ErrorAction SilentlyContinue
Remove-Item *_d.dll -ErrorAction SilentlyContinue
Remove-Item *.so -ErrorAction SilentlyContinue
Remove-Item *.so.* -ErrorAction SilentlyContinue
Remove-Item lib\*.so -ErrorAction SilentlyContinue
Remove-Item lib\*.so.* -ErrorAction SilentlyContinue
Remove-Item *_d_*.dll -ErrorAction SilentlyContinue

ResourceHacker -open .\dll_version_replace.rc -save .\dll_version_replace.res -action compile
ResourceHacker -open ccycles.dll -save ccycles.dll -resource .\dll_version_replace.res -action addoverwrite -mask VERSIONINFO
ResourceHacker -open cycles_kernel_oneapi_jit.dll -save cycles_kernel_oneapi_jit.dll -resource .\dll_version_replace.res -action addoverwrite -mask VERSIONINFO

Write-Host "ready"
Write-Host ""

Pop-Location