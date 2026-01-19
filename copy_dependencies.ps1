$ErrorActionPreference = 'Stop'

$cwd = $PSScriptRoot

$configs = @('Debug', 'Release')

$biglibs_location = (Convert-Path "$cwd\..\..\..\..\..\big_libs\RhinoCycles")
$orginal_dep_location = (Convert-Path "$biglibs_location\ccycles\win\deps\release")

$folders = Get-ChildItem -Path $orginal_dep_location -Directory

foreach($buildConfig in $configs)
{
	$lowerconfig = $buildConfig.ToLower()
	$first_letter = $buildConfig[0]
	$new_dep_location = "D:\deps\builder_cmake\windows\build\S\VS1564$first_letter\$buildConfig"
	$current_dep_location = (Convert-Path "$biglibs_location\ccycles\win\deps\$lowerconfig")
	
	Remove-Item -Confirm -Recurse -Force $current_dep_location
	
	foreach($folder in $folders)
	{
		$name = $folder.Name
		if ($name.Equals("openjpeg")) #copy openjpeg_msvc to openjpeg
		{
			Copy-Item -Recurse -Path "$new_dep_location\openjpeg_msvc" -Destination "$current_dep_location\$name"
		}	
		else
		{
			Copy-Item -Recurse -Path "$new_dep_location\$name" -Destination "$current_dep_location\$name" 
		}
	}
}

Rename-Item "$biglibs_location\ccycles\win\deps\debug\zlib\lib\zlibstaticd.lib" "$biglibs_location\ccycles\win\deps\debug\zlib\lib\libz_st.lib"
Rename-Item "$biglibs_location\ccycles\win\deps\release\zlib\lib\zlibstatic.lib" "$biglibs_location\ccycles\win\deps\release\zlib\lib\libz_st.lib"
