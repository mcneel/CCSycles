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

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr cycles_add_shader_node(IntPtr shaderId, [MarshalAs(UnmanagedType.LPStr)]string shnType);
		public static IntPtr add_shader_node(IntPtr shaderId, string node_type)
		{
			return cycles_add_shader_node(shaderId, node_type);
		}

		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_texmapping_set_transformation(IntPtr shadernodeId, uint transformType, float x, float y, float z);
		public static void shadernode_texmapping_set_transformation(IntPtr shadernodeId, uint transformType, float x, float y, float z)
		{
			cycles_shadernode_texmapping_set_transformation(shadernodeId, transformType, x, y, z);
		}

		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_texmapping_set_mapping(IntPtr shadernodeId, uint mappingx, uint mappingy, uint mappingz);
		public static void shadernode_texmapping_set_mapping(IntPtr shadernodeId, uint mappingx, uint mappingy, uint mappingz)
		{
			cycles_shadernode_texmapping_set_mapping(shadernodeId, mappingx, mappingy, mappingz);
		}

		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_texmapping_set_projection(IntPtr shadernodeId, uint projection);
		[Obsolete("No longer existing")]
		public static void shadernode_texmapping_set_projection(IntPtr shadernodeId, uint projection)
		{
			cycles_shadernode_texmapping_set_projection(shadernodeId, projection);
		}

		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_texmapping_set_type(IntPtr shadernodeId, uint type);
		public static void shadernode_texmapping_set_type(IntPtr shadernodeId, uint type)
		{
			cycles_shadernode_texmapping_set_type(shadernodeId, type);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_enum(IntPtr shadernodeId, [MarshalAs(UnmanagedType.LPStr)] string enum_name, int value);
		/// <summary>
		/// Set enumeration value for a shader node
		/// </summary>
		/// <param name="clientId">Session</param>
		/// <param name="sceneId">Session</param>
		/// <param name="shaderId">Shader ID</param>
		/// <param name="shadernodeId">Node ID in shader</param>
		/// <param name="shnType">Type of shader node</param>
		/// <param name="enum_name">Name of enumeration. Used mostly for nodes that have multiple</param>
		/// <param name="value">Int value of the enumeration</param>
		public static void shadernode_set_enum(IntPtr shadernodeId, string enum_name, int value)
		{
			cycles_shadernode_set_enum(shadernodeId, enum_name, value);
		}
		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_attribute_int(IntPtr shadernodeId, string name, int val);
		public static void shadernode_set_attribute_int(IntPtr shadernodeId,  [MarshalAs(UnmanagedType.LPStr)] string name, int val)
		{
			cycles_shadernode_set_attribute_int(shadernodeId, name, val);
		}
		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_attribute_float(IntPtr shadernodeId, string name, float val);
		public static void shadernode_set_attribute_float(IntPtr shadernodeId,  [MarshalAs(UnmanagedType.LPStr)] string name, float val)
		{
			cycles_shadernode_set_attribute_float(shadernodeId, name, val);
		}
		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_attribute_vec(IntPtr shadernodeId, string name, float x, float y, float z);
		public static void shadernode_set_attribute_vec(IntPtr shadernodeId,  [MarshalAs(UnmanagedType.LPStr)] string name, float4 val)
		{
			cycles_shadernode_set_attribute_vec(shadernodeId, name, val.x, val.y, val.z);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_member_float(IntPtr shadernodeId, string name, float val);
		public static void shadernode_set_member_float(IntPtr shadernodeId, 
			[MarshalAs(UnmanagedType.LPStr)] string name, float val)
		{
			cycles_shadernode_set_member_float(shadernodeId, name, val);
		}
		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_member_int(IntPtr  shadernodeId, string name, int val);
		public static void shadernode_set_member_int(IntPtr  shadernodeId, 
			[MarshalAs(UnmanagedType.LPStr)] string name, int val)
		{
			cycles_shadernode_set_member_int(shadernodeId, name, val);
		}
		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_member_bool(IntPtr shadernodeId, string name, bool val);
		public static void shadernode_set_member_bool(IntPtr shadernodeId, [MarshalAs(UnmanagedType.LPStr)] string name, bool val)
		{
			cycles_shadernode_set_member_bool(shadernodeId, name, val);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_member_vec(IntPtr shadernodeId, string name, float x, float y, float z);
		public static void shadernode_set_member_vec(IntPtr shadernodeId, [MarshalAs(UnmanagedType.LPStr)] string name, float x, float y, float z)
		{
			cycles_shadernode_set_member_vec(shadernodeId, name, x, y, z);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_member_string(IntPtr shadernodeId, string name, string value);
		public static void shadernode_set_member_string(IntPtr shadernodeId, [MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string value)
		{
			cycles_shadernode_set_member_string(shadernodeId, name, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_member_vec4_at_index(IntPtr shadernodeId, string name, float x, float y, float z, float w, int index);
		public static void shadernode_set_member_vec4_at_index(IntPtr shadernodeId,
			[MarshalAs(UnmanagedType.LPStr)] string name, float x, float y, float z, float w, int index)
		{
			cycles_shadernode_set_member_vec4_at_index(shadernodeId, name, x, y, z, w, index);
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
		private static extern void cycles_shader_set_name(IntPtr shaderId, [MarshalAs(UnmanagedType.LPStr)] string name);
		public static void shader_set_name(IntPtr shaderId, string name)
		{
			cycles_shader_set_name(shaderId, name);
		}
		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern bool cycles_shader_get_name(IntPtr shaderId, IntPtr stringholder);
		public static string shader_get_name(IntPtr shaderId)
		{
			using (CSStringHolder stringHolder = new CSStringHolder()) {
				bool success = cycles_shader_get_name(shaderId, stringHolder.Ptr);
				if(success) {
					string name = stringHolder.Value;
				  return name;
				}
			}
			return $"Name failed for {shaderId}";
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
		private static extern void cycles_shader_new_graph(IntPtr shaderId);
		public static void shader_new_graph(IntPtr shaderId)
		{
			cycles_shader_new_graph(shaderId);
		}
		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern int cycles_shader_node_count(IntPtr shader);
		public static int shader_node_count(IntPtr shader)
		{
			return cycles_shader_node_count(shader);
		}
		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr cycles_shader_node_get(IntPtr shader, int idx);
		public static IntPtr shader_node_get(IntPtr shader, int idx)
		{
			return cycles_shader_node_get(shader, idx);
		}
		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern bool cycles_shadernode_get_name(IntPtr shn, IntPtr strholder);
		public static string shadernode_get_name(IntPtr shn)
		{
			using (CSStringHolder stringHolder = new CSStringHolder()) {
				bool success = cycles_shadernode_get_name(shn, stringHolder.Ptr);
				if(success) {
					string name = stringHolder.Value;
				  return name;
				}
			}

			return "";
		}


#endregion
	}
}
