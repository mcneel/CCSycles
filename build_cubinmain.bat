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
		FOR %%s IN (compute_30, compute_35, compute_50, compute_52, compute_60, compute_61, compute_70, compute_75, compute_80, compute_86) DO (
			ECHO Compiling for %%s
			CALL cudacompile.bat %main% %%s
		)
	) ELSE (
		ECHO Compiling for only sm_%shadermodelnum%
		CALL cudacompile.bat %main% sm_%shadermodelnum%
	)
)

:DONE