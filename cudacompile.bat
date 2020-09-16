SET bintype=%1
SET sm=%2

ECHO %sm%
ECHO %bintype%

SET _oldarch=0
ECHO HAVE %sm%
IF (%sm%) EQU (sm_20) SET _oldarch=1
IF (%sm%) EQU (sm_21) SET _oldarch=1
REM IF (%sm%) EQU (sm_30) SET _oldarch=1
REM IF (%sm%) EQU (sm_35) SET _oldarch=1

IF %_oldarch% EQU 1 (
	ECHO Compiling for a pre CUDA10 architecture
	SET command="%nvcc9%" -arch=%sm% -m64 --cubin %cyclesroot%cycles\src\kernel\kernels\cuda\%bintype%.cu -o %cyclesout%lib\%bintype%_%sm%.cubin --ptxas-options="-v" -D__KERNEL_CUDA_VERSION__=%cudaversion91% --use_fast_math -I%cyclesroot%cycles\src %definitions%
) ELSE (
	SET command="%nvcc11%" -arch=%sm% -m64 --cubin %cyclesroot%cycles\src\kernel\kernels\cuda\%bintype%.cu -o %cyclesout%lib\%bintype%_%sm%.cubin --ptxas-options="-v" -D__KERNEL_CUDA_VERSION__=%cudaversion11% --use_fast_math -I%cyclesroot%cycles\src %definitions%
)

ECHO our command is %command%
%command%