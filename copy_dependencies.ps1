$ErrorActionPreference = 'Stop'

$cwd = $PSScriptRoot

$configs = @('Debug', 'Release')

$os = "win"
if ($IsMacOS) {$os = "osx"}

$biglibs_location = (Convert-Path "$cwd\..\..\..\..\..\big_libs\RhinoCycles")
$orginal_dep_location = (Convert-Path "$biglibs_location\ccycles\win\deps\release")

$folders = Get-ChildItem -Path $orginal_dep_location -Directory

foreach($buildConfig in $configs)
{
	$lowerconfig = $buildConfig.ToLower()
	$first_letter = $buildConfig[0]

Write-Host "Copying dependencies for configuration: $buildConfig"

    if ($IsMacOS -and $buildConfig -eq "Debug")
    {
        continue;  
    }

Write-Host "Copying dependencies for configuration: $buildConfig"

	$new_dep_location = "D:\deps\builder_cmake\windows\build\S\VS1564$first_letter\$buildConfig"
    if ($IsMacOS) {$new_dep_location = "/Users/Lars/build_darwin/deps_arm64/Release"}

    $current_dep_location = (Convert-Path "$biglibs_location\ccycles\$os\deps\$lowerconfig")
	
	Remove-Item -Confirm -Recurse -Force $current_dep_location
	
	foreach($folder in $folders)
	{
		$name = $folder.Name
        if ($IsMacOS)
        {
            if ($name -eq "pthreads") 
            {
                continue;
            }
        }
		if ($name.Equals("openjpeg") -and $IsWindows) #copy openjpeg_msvc to openjpeg
		{
			Copy-Item -Recurse -Path "$new_dep_location\openjpeg_msvc" -Destination "$current_dep_location\$name"
		}	
		else
		{
			Copy-Item -Recurse -Path "$new_dep_location\$name" -Destination "$current_dep_location\$name" 
		}
	}
}

if ($IsWindows)
{
    Rename-Item "$biglibs_location\ccycles\win\deps\debug\zlib\lib\zlibstaticd.lib" "$biglibs_location\ccycles\win\deps\debug\zlib\lib\libz_st.lib"
    Rename-Item "$biglibs_location\ccycles\win\deps\release\zlib\lib\zlibstatic.lib" "$biglibs_location\ccycles\win\deps\release\zlib\lib\libz_st.lib"
}
