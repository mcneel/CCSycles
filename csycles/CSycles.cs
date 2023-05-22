/**
Copyright 2014 Robert McNeel and Associates

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
**/

using System;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

/** \namespace ccl
 * \brief Namespace containing the low-level wrapping API of ccycles.dll and a set of higher-level classes.
 */
namespace ccl
{
	/// <summary>
	/// CSycles wraps the ccycles.dll, providing a very low-level API into the
	/// render engine Cycles.
	/// </summary>
	public partial class CSycles
	{
#region misc
		private static bool g_ccycles_loaded;

		[DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
		private static extern IntPtr LoadLibrary(string filename);

		/// <summary>
		/// Load the ccycles DLL.
		/// </summary>
		private static void LoadCCycles()
		{
			if (g_ccycles_loaded) return;

#if WIN32
			var path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
			var ccycles_dll = System.IO.Path.Combine(path, "ccycles.dll");
			LoadLibrary(ccycles_dll);
#endif
			LoadShaderNodes();
			g_ccycles_loaded = true;
		}

		private static readonly Dictionary<string, Type> g_registered_shadernodes = new Dictionary<string, Type>();

		/// <summary>
		/// Load all shader nodes from the assembly using reflection
		/// </summary>
		private static void LoadShaderNodes()
		{
			Assembly ccass = Assembly.GetExecutingAssembly();
			var constructTypes = new Type[1];
			constructTypes[0] = typeof(string);

			var exported_types = ccass.GetExportedTypes();
			var shadernode_type = typeof(ShaderNodes.ShaderNode);
			for (int i = 0; i < exported_types.Length; i++)
			{
				var exported_type = exported_types[i];
				if (!exported_type.IsSubclassOf(shadernode_type))
					continue;
				var attr = exported_type.GetCustomAttributes(typeof(Attributes.ShaderNodeAttribute), false);
				if (attr.Length < 1)
				{
					throw new NotImplementedException(String.Format("Class {0} must include a ShaderNode attribute", exported_type));
				}
				var shnattr = attr[0] as Attributes.ShaderNodeAttribute;
				if (shnattr == null || shnattr.IsBase)
					continue;

				var constructor = exported_type.GetConstructor(constructTypes);
				if (constructor == null)
				{
					throw new NotImplementedException(String.Format("Class {0} must include a constructor that takes a name", exported_type));
				}

				if (!g_registered_shadernodes.ContainsKey(shnattr.Name))
				{
					g_registered_shadernodes.Add(shnattr.Name, exported_type);
				}
			}
		}

		/// <summary>
		/// Create a ShaderNode based on XML name. This name has been registered
		/// using the ShaderNodeAttribute on each ShaderNode derived class
		/// </summary>
		/// <param name="xmlName"></param>
		/// <param name="nodeName"></param>
		/// <returns>a new ShaderNode if xmlName is registered, null otherwise</returns>
		internal static ShaderNodes.ShaderNode CreateShaderNode(string xmlName, string nodeName)
		{
			if (xmlName.Equals("shader")) return null;

			if (g_registered_shadernodes.ContainsKey(xmlName))
			{
				var constructTypes = new Type[1];
				constructTypes[0] = typeof(string);
				var shnt = g_registered_shadernodes[xmlName];
				var constructor = shnt.GetConstructor(constructTypes);
				var param = new object[1];
				param[0] = nodeName;

				return constructor.Invoke(param) as ShaderNodes.ShaderNode;
			}

			throw new InvalidDataException($"Node with xmlname '{xmlName}' not found.");
		}

		public ShaderNodes.ShaderNode CreateShaderNode(Shader shader, string nodeTypeName, string nodeName)
		{
			if(!g_registered_shadernodes.ContainsKey(nodeTypeName))
			{
				throw new InvalidDataException($"Node type '{nodeTypeName}' not found.");
			}
			var constructTypes = new Type[2];
			constructTypes[0] = typeof(Shader);
			constructTypes[1] = typeof(string);

			var shnt = g_registered_shadernodes[nodeTypeName];
			var constructor = shnt.GetConstructor(constructTypes);

			var parameters = new object[2];
			parameters[0] = shader;
			parameters[1] = nodeTypeName;

			return constructor.Invoke(parameters) as ShaderNodes.ShaderNode;
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_initialise(uint mask);
		/// <summary>
		/// Initialise the Cycles render engine.
		///
		/// This will ensure ccycles.dll is loaded. The initialisation will also prepare
		/// the devices list for use with Cycles.
		///
		/// Note: call <c>set_kernel_path</c> before initialising CSycles, otherwise default
		/// kernel path "lib" will be used.
		/// </summary>
		/// <returns></returns>
		public static void initialise(DeviceTypeMask mask)
		{
			LoadCCycles();
			cycles_initialise((uint)mask);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_path_init(string path, string userPath);
		/// <summary>
		/// Set the paths for Cycles to look for pre-compiled or cached kernels, or kernel code
		///
		/// Note: to have any effect needs to be called before <c>initialise</c>.
		/// </summary>
		/// <param name="path"></param>
		/// <param name="userPath"></param>
		public static void path_init([MarshalAs(UnmanagedType.LPStr)] string path, [MarshalAs(UnmanagedType.LPStr)] string userPath)
		{
			LoadCCycles();
			cycles_path_init(path, userPath);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_putenv(string path, string userPath);
		/// <summary>
		/// Set variable environment in Cycles.
		/// </summary>
		/// <param name="var"></param>
		/// <param name="val"></param>
		public static void putenv([MarshalAs(UnmanagedType.LPStr)] string var, [MarshalAs(UnmanagedType.LPStr)] string val)
		{
			LoadCCycles();
			cycles_putenv(var, val);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shutdown();
		/// <summary>
		/// Clean up CSycles, CCycles and Cycles.
		/// </summary>
		/// <returns></returns>
		public static void shutdown()
		{
			cycles_shutdown();
		}

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		/** <summary>
		 * Signature for a logger callback.
		 *
		 * CCycles will call logger callbacks only if built in Debug mode.
		 * </summary>
		 */
		public delegate void LoggerCallback([MarshalAsAttribute(UnmanagedType.LPStr)] string msg);

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_set_logger(IntPtr loggerCb);
		/// <summary>
		/// Set the logger function to CCycles.
		/// </summary>
		/// <param name="clientId">ID of client</param>
		/// <param name="loggerCb">The logger callback function.</param>
		public static void set_logger(LoggerCallback loggerCb)
		{
			var intptr_delegate = Marshal.GetFunctionPointerForDelegate(loggerCb);
			cycles_set_logger(intptr_delegate);
		}

		public static void remove_logger()
		{
			cycles_set_logger(IntPtr.Zero);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_log_to_stdout(int stdout);
		/**
		 * Set to true if logger output should be sent to std::cout as well.
		 *
		 * Note that this is global to the logger.
		 */
		public static void log_to_stdout(bool stdOut)
		{
			cycles_log_to_stdout(stdOut ? 1 : 0);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint cycles_new_client();
		public static uint new_client()
		{
			return cycles_new_client();
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_release_client();
		public static void release_client()
		{
			cycles_release_client();
		}


		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_debug_set_cpu_kernel(int stdout);
		/**
		 * Set true to ensure Cpu uses split kernel
		 */
		public static void debug_set_cpu_kernel(bool split)
		{
			LoadCCycles();
			cycles_debug_set_cpu_kernel(split ? 1 : 0);
		}
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_debug_set_cpu_allow_qbvh(int stdout);
		/**
		 * Set to true if logger output should be sent to std::cout as well.
		 *
		 * Note that this is global to the logger.
		 */
		public static void debug_set_cpu_allow_qbvh(bool allowQbvh)
		{
			LoadCCycles();
			cycles_debug_set_cpu_allow_qbvh(allowQbvh ? 1 : 0);
		}
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_debug_set_cuda_kernel(int stdout);
		/**
		 * Set to true if logger output should be sent to std::cout as well.
		 *
		 * Note that this is global to the logger.
		 */
		public static void debug_set_cuda_kernel(bool useSplit)
		{
			LoadCCycles();
			cycles_debug_set_cuda_kernel(useSplit ? 1 : 0);
		}
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_debug_set_opencl_kernel(int kernelType);
		///<summary>
		/// Set the OpenCL kernel type to use. 1 is split, 0 is mega. -1 means decide
		/// automatically based on officially supported devices.
		///</summary>
		/// <param name="kernelType">1 = split, 0 = mega, -1 = default based on officially supported devices.</param>
		public static void debug_set_opencl_kernel(int kernelType)
		{
			LoadCCycles();
			cycles_debug_set_opencl_kernel(kernelType);
		}
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_debug_set_opencl_single_program(int singleProgram);
		/// <summary>
		/// Give true to force OpenCL compilation to use single program
		/// </summary>
		/// <param name="useSingleProgram">true to compile as single program, false to compile as separate programs</param>
		public static void debug_set_opencl_single_program(bool useSingleProgram)
		{
			LoadCCycles();
			cycles_debug_set_opencl_single_program(useSingleProgram ? 1 : 0);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_debug_set_opencl_device_type(int type);
		/// <summary>
		/// Set to govern what OpenCL device types will be queried.
		/// </summary>
		/// <param name="type">
		/// 0 = force disable OpenCL
		/// 1 = all
		/// 2 = default
		/// 3 = Cpu
		/// 4 = GPU
		/// 5 = accelerator
		/// </param>
		public static void debug_set_opencl_device_type(int type)
		{
			LoadCCycles();
			cycles_debug_set_opencl_device_type(type);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_set_rhino_perlin_noise_table(IntPtr data, uint count);
		public static void set_rhino_perlin_noise_table(IntPtr data, uint count)
		{
			LoadCCycles();
			cycles_set_rhino_perlin_noise_table(data, count);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_set_rhino_impulse_noise_table(IntPtr data, uint count);
		public static void set_rhino_impulse_noise_table(IntPtr data, uint count)
		{
			LoadCCycles();
			cycles_set_rhino_impulse_noise_table(data, count);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_set_rhino_vc_noise_table(IntPtr data, uint count);
		public static void set_rhino_vc_noise_table(IntPtr data, uint count)
		{
			LoadCCycles();
			cycles_set_rhino_vc_noise_table(data, count);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_set_rhino_aaltonen_noise_table(IntPtr data, uint count);
		public static void set_rhino_aaltonen_noise_table(IntPtr data, uint count)
		{
			LoadCCycles();
			cycles_set_rhino_aaltonen_noise_table(data, count);
		}
		#endregion

	}
}
