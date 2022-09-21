@ECHO OFF
SETLOCAL ENABLEEXTENSIONS

SET me=%~n0
SET parent=%~dp0

CALL cudaenv.bat %1 %2 %3 %4

ECHO we have: %1 %2 %3 %4
SET main=%1
SHIFT
SET shadermodelnum=%1
ECHO we have: %1 %2 %3 %4

ECHO main is %main% and shadermodelnum is %shadermodelnum%
ECHO optix is %_optix%

IF %_optix% EQU 1 (
	ECHO Compiling OptiX kernel
	CALL cudacompile.bat %main%
) ELSE (
	IF [%shadermodelnum%]==[]  (
		REM old: FOR %%s IN (sm_30, sm_35, sm_50, sm_52, sm_60, sm_61, sm_70, sm_75, sm_80, sm_86) DO (
		REM FOR %%s IN (sm_30, sm_35, sm_50, sm_52, compute) DO (
		FOR %%s IN (compute, sm_30, sm_35, sm_50, sm_52, sm_60, sm_61, sm_70, sm_75, sm_80, sm_86) DO (
			ECHO Compiling for %%s
			CALL cudacompile.bat %main% %%s
		)
	) ELSE (
		ECHO Compiling for only sm_%shadermodelnum%
		CALL cudacompile.bat %main% sm_%shadermodelnum%
	)
)

:DONE
