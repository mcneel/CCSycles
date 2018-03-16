@ECHO OFF
REM call build_filter_cubins.bat
REM call build_kernel_cubins.bat
REM call build_split_cubins.bat

call build_cubinmain.bat filter
call build_cubinmain.bat kernel
call build_cubinmain.bat kernel_split

REM copy D:\Dev\Rhino\rhino\big_libs\RhinoCycles\lib\*.cubin  D:\Dev\Rhino\rhino\src4\bin\Debug\Plug-ins\RhinoCycles\lib
REM copy D:\Dev\Rhino\rhino\big_libs\RhinoCycles\lib\*.cubin  D:\Dev\Rhino\rhino\src4\bin\Release\Plug-ins\RhinoCycles\lib
