using System;
using System.Runtime.InteropServices;

namespace ccl
{
	public partial class CSycles
	{
#region shaders
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr cycles_create_shader(IntPtr sessionId);
		public static IntPtr create_shader(IntPtr sessionId)
		{
			return cycles_create_shader(sessionId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint cycles_scene_tag_shader(IntPtr sessionId, IntPtr shaderId, bool use);
		public static uint scene_tag_shader(IntPtr sessionId, IntPtr shaderId, bool use)
		{
			return cycles_scene_tag_shader(sessionId, shaderId, use);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint cycles_scene_add_shader(IntPtr sessionId, IntPtr shaderId);
		public static uint scene_add_shader(IntPtr sessionId, IntPtr shaderId)
		{
			return cycles_scene_add_shader(sessionId, shaderId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint cycles_scene_shader_id(IntPtr sessionId, IntPtr shaderId);
		public static uint scene_shader_id(IntPtr sessionId, IntPtr shaderId)
		{
			return cycles_scene_shader_id(sessionId, shaderId);
		}

		/// <summary>
		/// The output shader node ID for any graph is always 0.
		/// </summary>
		// TODO: XXXX figure out correct approach
		static public readonly IntPtr OUTPUT_SHADERNODE_ID = (IntPtr)0;

		static public readonly IntPtr DEFAULT_SURFACE_SHADER = (IntPtr)0;
		static public readonly IntPtr DEFAULT_LIGHT_SHADER = (IntPtr)1;
		static public readonly IntPtr DEFAULT_BACKGROUND_SHADER = (IntPtr)2;
		static public readonly IntPtr DEFAULT_EMPTY_SHADER = (IntPtr)3;


		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint cycles_add_shader_node(IntPtr sessionId, IntPtr shaderId, uint shnType);
		public static uint add_shader_node(IntPtr sessionId, IntPtr shaderId, ShaderNodeType shnType)
		{
			return cycles_add_shader_node(sessionId, shaderId, (uint) shnType);
		}

		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_texmapping_set_transformation(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, uint shnType, uint transformType, float x, float y, float z);
		public static void shadernode_texmapping_set_transformation(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, ShaderNodeType shnType, uint transformType, float x, float y, float z)
		{
			cycles_shadernode_texmapping_set_transformation(sessionId, shaderId, shadernodeId, (uint) shnType, transformType, x, y, z);
		}

		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_texmapping_set_mapping(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, uint shnType, uint mappingx, uint mappingy, uint mappingz);
		public static void shadernode_texmapping_set_mapping(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, ShaderNodeType shnType, uint mappingx, uint mappingy, uint mappingz)
		{
			cycles_shadernode_texmapping_set_mapping(sessionId, shaderId, shadernodeId, (uint) shnType, mappingx, mappingy, mappingz);
		}

		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_texmapping_set_projection(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, uint shnType, uint projection);
		[Obsolete("No longer existing")]
		public static void shadernode_texmapping_set_projection(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, ShaderNodeType shnType, uint projection)
		{
			cycles_shadernode_texmapping_set_projection(sessionId, shaderId, shadernodeId, (uint) shnType, projection);
		}

		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_texmapping_set_type(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, uint shnType, uint type);
		public static void shadernode_texmapping_set_type(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, ShaderNodeType shnType, uint type)
		{
			cycles_shadernode_texmapping_set_type(sessionId, shaderId, shadernodeId, (uint) shnType, type);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_enum(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, uint shnType, [MarshalAs(UnmanagedType.LPStr)] string enum_name, int value);
		/// <summary>
		/// Set enumeration value for a shader node
		/// </summary>
		/// <param name="clientId">Client</param>
		/// <param name="sceneId">Session</param>
		/// <param name="shaderId">Shader ID</param>
		/// <param name="shadernodeId">Node ID in shader</param>
		/// <param name="shnType">Type of shader node</param>
		/// <param name="enum_name">Name of enumeration. Used mostly for nodes that have multiple</param>
		/// <param name="value">Int value of the enumeration</param>
		public static void shadernode_set_enum(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, ShaderNodeType shnType, string enum_name, int value)
		{
			cycles_shadernode_set_enum(sessionId, shaderId, shadernodeId, (uint) shnType, enum_name, value);
		}
		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_attribute_int(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, string name, int val);
		public static void shadernode_set_attribute_int(IntPtr sessionId, IntPtr shaderId, uint shadernodeId,  [MarshalAs(UnmanagedType.LPStr)] string name, int val)
		{
			cycles_shadernode_set_attribute_int(sessionId, shaderId, shadernodeId, name, val);
		}
		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_attribute_float(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, string name, float val);
		public static void shadernode_set_attribute_float(IntPtr sessionId, IntPtr shaderId, uint shadernodeId,  [MarshalAs(UnmanagedType.LPStr)] string name, float val)
		{
			cycles_shadernode_set_attribute_float(sessionId, shaderId, shadernodeId, name, val);
		}
		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_attribute_vec(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, string name, float x, float y, float z);
		public static void shadernode_set_attribute_vec(IntPtr sessionId, IntPtr shaderId, uint shadernodeId,  [MarshalAs(UnmanagedType.LPStr)] string name, float4 val)
		{
			cycles_shadernode_set_attribute_vec(sessionId, shaderId, shadernodeId, name, val.x, val.y, val.z);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_member_float(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, uint shnType, string name, float val);
		public static void shadernode_set_member_float(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, ShaderNodeType shnType,
			[MarshalAs(UnmanagedType.LPStr)] string name, float val)
		{
			cycles_shadernode_set_member_float(sessionId, shaderId, shadernodeId, (uint)shnType, name, val);
		}
		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_member_int(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, uint shnType, string name, int val);
		public static void shadernode_set_member_int(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, ShaderNodeType shnType,
			[MarshalAs(UnmanagedType.LPStr)] string name, int val)
		{
			cycles_shadernode_set_member_int(sessionId, shaderId, shadernodeId, (uint)shnType, name, val);
		}
		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_member_bool(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, uint shnType, string name, bool val);
		public static void shadernode_set_member_bool(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, ShaderNodeType shnType,
			[MarshalAs(UnmanagedType.LPStr)] string name, bool val)
		{
			cycles_shadernode_set_member_bool(sessionId, shaderId, shadernodeId, (uint)shnType, name, val);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_member_vec(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, uint shnType, string name, float x, float y, float z);
		public static void shadernode_set_member_vec(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, ShaderNodeType shnType,
			[MarshalAs(UnmanagedType.LPStr)] string name, float x, float y, float z)
		{
			cycles_shadernode_set_member_vec(sessionId, shaderId, shadernodeId, (uint)shnType, name, x, y, z);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_member_string(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, uint shnType, string name, string value);
		public static void shadernode_set_member_string(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, ShaderNodeType shnType,
			[MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string value)
		{
			cycles_shadernode_set_member_string(sessionId, shaderId, shadernodeId, (uint)shnType, name, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_member_vec4_at_index(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, uint shnType, string name, float x, float y, float z, float w, int index);
		public static void shadernode_set_member_vec4_at_index(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, ShaderNodeType shnType,
			[MarshalAs(UnmanagedType.LPStr)] string name, float x, float y, float z, float w, int index)
		{
			cycles_shadernode_set_member_vec4_at_index(sessionId, shaderId, shadernodeId, (uint)shnType, name, x, y, z, w, index);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_member_float_img(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, uint shnType, string name, string  imgName, IntPtr img, uint width, uint height, uint depth, uint channels);
		public static void shadernode_set_member_float_img(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, ShaderNodeType shnType,
			[MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string imgName, IntPtr img, uint width, uint height, uint depth, uint channels)
		{
			cycles_shadernode_set_member_float_img(sessionId, shaderId, shadernodeId, (uint)shnType, name, imgName, img, width, height, depth, channels);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_apply_gamma_to_byte_buffer(IntPtr rgba_buffer, int size_in_bytes, float gamma);
		public static void apply_gamma_to_byte_buffer(IntPtr rgba_buffer, int size_in_bytes, float gamma)
		{
			cycles_apply_gamma_to_byte_buffer(rgba_buffer, size_in_bytes, gamma);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_apply_gamma_to_float_buffer(IntPtr rgba_buffer, int size_in_bytes, float gamma);
		public static void apply_gamma_to_float_buffer(IntPtr rgba_buffer, int size_in_bytes, float gamma)
		{
			cycles_apply_gamma_to_float_buffer(rgba_buffer, size_in_bytes, gamma);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_member_byte_img(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, uint shnType, string name, string  imgName, IntPtr img, uint width, uint height, uint depth, uint channels);
		public static void shadernode_set_member_byte_img(IntPtr sessionId, IntPtr shaderId, uint shadernodeId, ShaderNodeType shnType,
			[MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string imgName, IntPtr img, uint width, uint height, uint depth, uint channels)
		{
			cycles_shadernode_set_member_byte_img(sessionId, shaderId, shadernodeId, (uint)shnType, name, imgName, img, width, height, depth, channels);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shader_connect_nodes(IntPtr sessionId, IntPtr shaderId, uint fromId, string from, uint toId,
			string to);
		public static void shader_connect_nodes(IntPtr sessionId, IntPtr shaderId, uint fromId, string from, uint toId, string to)
		{
			cycles_shader_connect_nodes(sessionId, shaderId, fromId, from, toId, to);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shader_set_name(IntPtr sessionId, IntPtr shaderId, [MarshalAs(UnmanagedType.LPStr)] string name);
		public static void shader_set_name(IntPtr sessionId, IntPtr shaderId, string name)
		{
			cycles_shader_set_name(sessionId, shaderId, name);
		}

		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shader_set_use_mis(IntPtr sessionId, IntPtr shaderId, uint useMis);
		public static void shader_set_use_mis(IntPtr sessionId, IntPtr shaderId, bool useMis)
		{
			cycles_shader_set_use_mis(sessionId, shaderId, (uint) (useMis ? 1 : 0));
		}

		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shader_set_use_transparent_shadow(IntPtr sessionId, IntPtr shaderId, uint useTransparentShadow);
		public static void shader_set_use_transparent_shadow(IntPtr sessionId, IntPtr shaderId, bool useTransparentShadow)
		{
			cycles_shader_set_use_transparent_shadow(sessionId, shaderId, (uint) (useTransparentShadow ? 1 : 0));
		}

		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shader_set_heterogeneous_volume(IntPtr sessionId, IntPtr shaderId, uint heterogeneousVolume);
		public static void shader_set_heterogeneous_volume(IntPtr sessionId, IntPtr shaderId, bool heterogeneousVolume)
		{
			cycles_shader_set_heterogeneous_volume(sessionId, shaderId, (uint) (heterogeneousVolume ? 1 : 0));
		}

		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shader_new_graph(IntPtr sessionId, IntPtr shaderId);
		public static void shader_new_graph(IntPtr sessionId, IntPtr shaderId)
		{
			cycles_shader_new_graph(sessionId, shaderId);
		}


#endregion
	}
}
