call build_filter_cubins.bat
call build_kernel_cubins.bat rhino 52 
call build_kernel_cubins.bat rhino 60

call build_kernel_cubins.bat rhino 61

copy D:\Dev\Rhino\rhino\big_libs\RhinoCycles\lib\*.cubin  D:\Dev\Rhino\rhino\src4\bin\Debug\Plug-ins\RhinoCycles\lib
copy D:\Dev\Rhino\rhino\big_libs\RhinoCycles\lib\*.cubin  D:\Dev\Rhino\rhino\src4\bin\Release\Plug-ins\RhinoCycles\lib
