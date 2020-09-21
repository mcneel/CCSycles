REM @ECHO OFF

ECHO me is %me%
ECHO parent is %parent%

SET nvcc=C:\CUDA8\bin\nvcc.exe
SET nvcc9=C:\CUDA9\bin\nvcc.exe
SET nvcc10=C:\CUDA10\bin\nvcc.exe
SET nvcc11=C:\CUDA11\bin\nvcc.exe
SET typechoice=%1

SET _optix=0
IF "%typechoice%"=="optix" (
	SET _optix=1
)

SET _apptype=0
IF "%typechoice%"=="standalone" (
	SET _apptype=1
	SHIFT
)
IF "%typechoice%"=="rhino" (
	SET _apptype=1
	SHIFT
)


IF %_apptype% EQU 1 (
	IF "%typechoice%"=="standalone" (
		SET cyclesroot=%parent%
		SET cyclesout=%parent%
	) else (
		SET cyclesroot=%parent%
		SET cyclesout=%parent%..\..\..\..\..\big_libs\RhinoCycles\
	)
) else (
	SET cyclesroot=%parent%
	SET cyclesout=%parent%..\..\..\..\..\big_libs\RhinoCycles\
)

IF NOT EXIST "%cyclesroot%/lib" (
	MKDIR "%cyclesroot%/lib"
)

SET cudaversion=11
SET cudaversion75=8
SET cudaversion91=9
SET cudaversion10=10
SET cudaversion11=11
SET shadermodelnum=%1
REM SET definitions="-DCCL_NAMESPACE_BEGIN= -DCCL_NAMESPACE_END= -DNVCC -D__NO_CAMERA_MOTION__ -D__NO_OBJECT_MOTION__ -D__NO_HAIR__ -D__NO_BAKING__ -D__NO_VOLUME__ -D__NO_BRANCHED_PATH__ -D__NO_PATCH_EVAL__ -D__NO_DENOISING__"
SET definitions=-DCCL_NAMESPACE_BEGIN= -DCCL_NAMESPACE_END= -DNVCC -D__NO_CAMERA_MOTION__ -D__NO_OBJECT_MOTION__ -D__NO_DENOISING__
SET optixdefinitions=--ptx --use_fast_math
SET optixinclude=OptiX\include

