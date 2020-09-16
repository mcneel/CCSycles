@ECHO OFF

call build_cubinmain.bat filter
call build_cubinmain.bat kernel

REM copy D:\Dev\Rhino\rhino\big_libs\RhinoCycles\lib\*.cubin  D:\Dev\Rhino\rhino\src4\bin\Debug\Plug-ins\RhinoCycles\lib
REM copy D:\Dev\Rhino\rhino\big_libs\RhinoCycles\lib\*.cubin  D:\Dev\Rhino\rhino\src4\bin\Release\Plug-ins\RhinoCycles\lib
