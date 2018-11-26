@ECHO OFF
SETLOCAL ENABLEEXTENSIONS

SET me=%~n0
SET parent=%~dp0

ECHO we have: %1 %2 %3 %4
SET main=%1
SHIFT
ECHO we have: %1 %2 %3 %4

CALL cudaenv.bat %1 %2 %3 %4

IF [%shadermodelnum%]==[] (
	FOR %%s IN (sm_20, sm_21) DO (
		ECHO Compiling for %%s
		CALL cudacompile.bat %main% %%s
	)
	FOR %%s IN (sm_30, sm_35, sm_37, sm_50, sm_52, sm_60, sm_61, sm_70, sm_75) DO (
		ECHO Compiling for %%s
		CALL cudacompile.bat %main% %%s
	)
) ELSE (
	ECHO Compiling for only sm_%shadermodelnum%
	CALL cudacompile.bat %main% sm_%shadermodelnum%
)
