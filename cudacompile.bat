SET bintype=%1
SET sm=%2

ECHO %sm%
ECHO %bintype%

SET _oldarch=0
ECHO HAVE %sm%
IF (%sm%) EQU (sm_20) SET _oldarch=1
IF (%sm%) EQU (sm_21) SET _oldarch=1

IF %_oldarch% EQU 1 (
	ECHO Compiling for a deprecated architecture
	"%nvcc%" -arch=%sm% -m64 --cubin %cyclesroot%cycles\src\kernel\kernels\cuda\%bintype%.cu -o %cyclesout%lib\%bintype%_%sm%.cubin --ptxas-options="-v" -D__KERNEL_CUDA_VERSION__=%cudaversion% --use_fast_math -I%cyclesroot%cycles\src %definitions%
) ELSE (
	"%nvcc10%" -arch=%sm% -m64 --cubin %cyclesroot%cycles\src\kernel\kernels\cuda\%bintype%.cu -o %cyclesout%lib\%bintype%_%sm%.cubin --ptxas-options="-v" -D__KERNEL_CUDA_VERSION__=%cudaversion10% --use_fast_math -I%cyclesroot%cycles\src %definitions%
)
