SET nvcc=C:\CUDA8\bin\nvcc.exe
SET typechoice=%1

IF "%typechoice%"=="standalone" (
	SET cyclesroot=D:\Dev\CCSycles
	SET cyclesout=D:\Dev\CCSycles
) else (
	SET cyclesroot=D:\Dev\Rhino\rhino\src4\rhino4\Plug-ins\RDK\cycles
	SET cyclesout=D:\Dev\Rhino\rhino\big_libs\RhinoCycles
)

SHIFT

IF NOT EXIST "%cyclesroot%/lib" (
	MKDIR "%cyclesroot%/lib"
)

SET cudaversion=8
SET cudaversion75=8
SET shadermodelnum=%1
SET shadermodel=sm_%shadermodelnum%

IF [%shadermodelnum%]==[] (
	FOR %%s IN ("sm_20", "sm_21") DO (
		"%nvcc%" -arch=%%s -m64 --cubin %cyclesroot%/cycles/src/kernel/kernels/cuda/kernel.cu -o %cyclesout%/lib/kernel_%%s.cubin --ptxas-options="-v" -D__KERNEL_CUDA_VERSION__=%cudaversion% --use_fast_math -I%cyclesroot%/cycles/src -DCCL_NAMESPACE_BEGIN= -DCCL_NAMESPACE_END= -DNVCC
	)
	FOR %%s IN ("sm_20", "sm_21") DO (
		"%nvcc%" -arch=%%s -m64 --cubin %cyclesroot%/cycles/src/kernel/kernels/cuda/kernel_split.cu -o %cyclesout%/lib/kernel_split_%%s.cubin --ptxas-options="-v" -D__KERNEL_CUDA_VERSION__=%cudaversion% --use_fast_math -I%cyclesroot%/cycles/src -DCCL_NAMESPACE_BEGIN= -DCCL_NAMESPACE_END= -DNVCC
	)
	FOR %%s IN ("sm_30", "sm_35", "sm_50", "sm_52", "sm_60", "sm_61") DO (
		"%nvcc%" -arch=%%s -m64 --cubin %cyclesroot%/cycles/src/kernel/kernels/cuda/kernel.cu -o %cyclesout%/lib/kernel_%%s.cubin --ptxas-options="-v" -D__KERNEL_CUDA_VERSION__=%cudaversion75% --use_fast_math -I%cyclesroot%/cycles/src -DCCL_NAMESPACE_BEGIN= -DCCL_NAMESPACE_END= -DNVCC
	)
	FOR %%s IN ("sm_30", "sm_35", "sm_50", "sm_52", "sm_60", "sm_61") DO (
		"%nvcc%" -arch=%%s -m64 --cubin %cyclesroot%/cycles/src/kernel/kernels/cuda/kernel_split.cu -o %cyclesout%/lib/kernel_split_%%s.cubin --ptxas-options="-v" -D__KERNEL_CUDA_VERSION__=%cudaversion75% --use_fast_math -I%cyclesroot%/cycles/src -DCCL_NAMESPACE_BEGIN= -DCCL_NAMESPACE_END= -DNVCC
	)
) ELSE (
	"%nvcc%" -arch=%shadermodel% -m64 --cubin %cyclesroot%/cycles/src/kernel/kernels/cuda/kernel.cu -o %cyclesout%/lib/kernel_%shadermodel%.cubin --ptxas-options="-v" -D__KERNEL_CUDA_VERSION__=%cudaversion75% --use_fast_math -I%cyclesroot%/cycles/src -DCCL_NAMESPACE_BEGIN= -DCCL_NAMESPACE_END= -DNVCC
	"%nvcc%" -arch=%shadermodel% -m64 --cubin %cyclesroot%/cycles/src/kernel/kernels/cuda/kernel_split.cu -o %cyclesout%/lib/kernel_split_%shadermodel%.cubin --ptxas-options="-v" -D__KERNEL_CUDA_VERSION__=%cudaversion75% --use_fast_math -I%cyclesroot%/cycles/src -DCCL_NAMESPACE_BEGIN= -DCCL_NAMESPACE_END= -DNVCC
)
