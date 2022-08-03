@ECHO OFF

call build_cubinmain.bat filter
call build_cubinmain.bat kernel
call build_cubinmain.bat optix

copy D:\Dev\Rhino\rhino7\big_libs\RhinoCycles\lib\*.cubin  D:\Dev\Rhino\rhino7\src4\bin\Debug\Plug-ins\RhinoCycles\lib
copy D:\Dev\Rhino\rhino7\big_libs\RhinoCycles\lib\*.cubin  D:\Dev\Rhino\rhino7\src4\bin\Release\Plug-ins\RhinoCycles\lib
copy D:\Dev\Rhino\rhino7\big_libs\RhinoCycles\lib\*.ptx  D:\Dev\Rhino\rhino7\src4\bin\Debug\Plug-ins\RhinoCycles\lib
copy D:\Dev\Rhino\rhino7\big_libs\RhinoCycles\lib\*.ptx  D:\Dev\Rhino\rhino7\src4\bin\Release\Plug-ins\RhinoCycles\lib
