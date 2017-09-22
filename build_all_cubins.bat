call build_filter_cubins.bat
call build_kernel_cubins.bat
call build_split_cubins.bat

copy D:\Dev\Rhino\rhino\big_libs\RhinoCycles\lib\*.cubin  D:\Dev\Rhino\rhino\src4\bin\Debug\Plug-ins\RhinoCycles\lib
copy D:\Dev\Rhino\rhino\big_libs\RhinoCycles\lib\*.cubin  D:\Dev\Rhino\rhino\src4\bin\Release\Plug-ins\RhinoCycles\lib
