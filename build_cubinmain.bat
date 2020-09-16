@ECHO OFF
SETLOCAL ENABLEEXTENSIONS

SET me=%~n0
SET parent=%~dp0

ECHO we have: %1 %2 %3 %4
SET main=%1
SHIFT
SET shadermodelnum=%1
ECHO we have: %1 %2 %3 %4

ECHO main is %main% and shadermodelnum is %shadermodelnum%

CALL cudaenv.bat %1 %2 %3 %4

IF [%shadermodelnum%]==[] (
	FOR %%s IN (sm_30, sm_35, sm_50, sm_52, sm_60, sm_61, sm_70, sm_75, sm_80) DO (
		ECHO Compiling for %%s
		CALL cudacompile.bat %main% %%s
	)
) ELSE (
	ECHO Compiling for only sm_%shadermodelnum%
	CALL cudacompile.bat %main% sm_%shadermodelnum%
)
