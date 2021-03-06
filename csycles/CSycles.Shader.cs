﻿using System;
using System.Runtime.InteropServices;

namespace ccl
{
	public partial class CSycles
	{
#region shaders
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint cycles_create_shader(uint clientId, uint sceneId);
		public static uint create_shader(uint clientId, uint sceneId)
		{
			return cycles_create_shader(clientId, sceneId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint cycles_scene_tag_shader(uint clientId, uint sceneId, uint shaderId, bool use);
		public static uint scene_tag_shader(uint clientId, uint sceneId, uint shaderId, bool use)
		{
			return cycles_scene_tag_shader(clientId, sceneId, shaderId, use);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint cycles_scene_add_shader(uint clientId, uint sceneId, uint shaderId);
		public static uint scene_add_shader(uint clientId, uint sceneId, uint shaderId)
		{
			return cycles_scene_add_shader(clientId, sceneId, shaderId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint cycles_scene_shader_id(uint clientId, uint sceneId, uint shaderId);
		public static uint scene_shader_id(uint clientId, uint sceneId, uint shaderId)
		{
			return cycles_scene_shader_id(clientId, sceneId, shaderId);
		}

		/// <summary>
		/// The output shader node ID for any graph is always 0.
		/// </summary>
		public const uint OUTPUT_SHADERNODE_ID = 0;

		public const uint DEFAULT_SURFACE_SHADER = 0;
		public const uint DEFAULT_LIGHT_SHADER = 1;
		public const uint DEFAULT_BACKGROUND_SHADER = 2;
		public const uint DEFAULT_EMPTY_SHADER = 3;


		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint cycles_add_shader_node(uint clientId, uint sceneId, uint shaderId, uint shnType);
		public static uint add_shader_node(uint clientId, uint sceneId, uint shaderId, ShaderNodeType shnType)
		{
			return cycles_add_shader_node(clientId, sceneId, shaderId, (uint) shnType);
		}

		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_texmapping_set_transformation(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, uint shnType, uint transformType, float x, float y, float z);
		public static void shadernode_texmapping_set_transformation(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, ShaderNodeType shnType, uint transformType, float x, float y, float z)
		{
			cycles_shadernode_texmapping_set_transformation(clientId, sceneId, shaderId, shadernodeId, (uint) shnType, transformType, x, y, z);
		}

		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_texmapping_set_mapping(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, uint shnType, uint mappingx, uint mappingy, uint mappingz);
		public static void shadernode_texmapping_set_mapping(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, ShaderNodeType shnType, uint mappingx, uint mappingy, uint mappingz)
		{
			cycles_shadernode_texmapping_set_mapping(clientId, sceneId, shaderId, shadernodeId, (uint) shnType, mappingx, mappingy, mappingz);
		}

		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_texmapping_set_projection(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, uint shnType, uint projection);
		[Obsolete("No longer existing")]
		public static void shadernode_texmapping_set_projection(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, ShaderNodeType shnType, uint projection)
		{
			cycles_shadernode_texmapping_set_projection(clientId, sceneId, shaderId, shadernodeId, (uint) shnType, projection);
		}

		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_texmapping_set_type(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, uint shnType, uint type);
		public static void shadernode_texmapping_set_type(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, ShaderNodeType shnType, uint type)
		{
			cycles_shadernode_texmapping_set_type(clientId, sceneId, shaderId, shadernodeId, (uint) shnType, type);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_enum(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, uint shnType, [MarshalAs(UnmanagedType.LPStr)] string enum_name, int value);
		/// <summary>
		/// Set enumeration value for a shader node
		/// </summary>
		/// <param name="clientId">Client</param>
		/// <param name="sceneId">Scene</param>
		/// <param name="shaderId">Shader ID</param>
		/// <param name="shadernodeId">Node ID in shader</param>
		/// <param name="shnType">Type of shader node</param>
		/// <param name="enum_name">Name of enumeration. Used mostly for nodes that have multiple</param>
		/// <param name="value">Int value of the enumeration</param>
		public static void shadernode_set_enum(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, ShaderNodeType shnType, string enum_name, int value)
		{
			cycles_shadernode_set_enum(clientId, sceneId, shaderId, shadernodeId, (uint) shnType, enum_name, value);
		}
		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_attribute_int(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, string name, int val);
		public static void shadernode_set_attribute_int(uint clientId, uint sceneId, uint shaderId, uint shadernodeId,  [MarshalAs(UnmanagedType.LPStr)] string name, int val)
		{
			cycles_shadernode_set_attribute_int(clientId, sceneId, shaderId, shadernodeId, name, val);
		}
		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_attribute_float(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, string name, float val);
		public static void shadernode_set_attribute_float(uint clientId, uint sceneId, uint shaderId, uint shadernodeId,  [MarshalAs(UnmanagedType.LPStr)] string name, float val)
		{
			cycles_shadernode_set_attribute_float(clientId, sceneId, shaderId, shadernodeId, name, val);
		}
		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_attribute_vec(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, string name, float x, float y, float z);
		public static void shadernode_set_attribute_vec(uint clientId, uint sceneId, uint shaderId, uint shadernodeId,  [MarshalAs(UnmanagedType.LPStr)] string name, float4 val)
		{
			cycles_shadernode_set_attribute_vec(clientId, sceneId, shaderId, shadernodeId, name, val.x, val.y, val.z);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_member_float(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, uint shnType, string name, float val);
		public static void shadernode_set_member_float(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, ShaderNodeType shnType,
			[MarshalAs(UnmanagedType.LPStr)] string name, float val)
		{
			cycles_shadernode_set_member_float(clientId, sceneId, shaderId, shadernodeId, (uint)shnType, name, val);
		}
		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_member_int(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, uint shnType, string name, int val);
		public static void shadernode_set_member_int(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, ShaderNodeType shnType,
			[MarshalAs(UnmanagedType.LPStr)] string name, int val)
		{
			cycles_shadernode_set_member_int(clientId, sceneId, shaderId, shadernodeId, (uint)shnType, name, val);
		}
		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_member_bool(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, uint shnType, string name, bool val);
		public static void shadernode_set_member_bool(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, ShaderNodeType shnType,
			[MarshalAs(UnmanagedType.LPStr)] string name, bool val)
		{
			cycles_shadernode_set_member_bool(clientId, sceneId, shaderId, shadernodeId, (uint)shnType, name, val);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_member_vec(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, uint shnType, string name, float x, float y, float z);
		public static void shadernode_set_member_vec(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, ShaderNodeType shnType,
			[MarshalAs(UnmanagedType.LPStr)] string name, float x, float y, float z)
		{
			cycles_shadernode_set_member_vec(clientId, sceneId, shaderId, shadernodeId, (uint)shnType, name, x, y, z);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_member_string(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, uint shnType, string name, string value);
		public static void shadernode_set_member_string(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, ShaderNodeType shnType,
			[MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string value)
		{
			cycles_shadernode_set_member_string(clientId, sceneId, shaderId, shadernodeId, (uint)shnType, name, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_member_vec4_at_index(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, uint shnType, string name, float x, float y, float z, float w, int index);
		public static void shadernode_set_member_vec4_at_index(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, ShaderNodeType shnType,
			[MarshalAs(UnmanagedType.LPStr)] string name, float x, float y, float z, float w, int index)
		{
			cycles_shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, shadernodeId, (uint)shnType, name, x, y, z, w, index);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shadernode_set_member_float_img(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, uint shnType, string name, string  imgName, IntPtr img, uint width, uint height, uint depth, uint channels);
		public static void shadernode_set_member_float_img(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, ShaderNodeType shnType,
			[MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string imgName, IntPtr img, uint width, uint height, uint depth, uint channels)
		{
			cycles_shadernode_set_member_float_img(clientId, sceneId, shaderId, shadernodeId, (uint)shnType, name, imgName, img, width, height, depth, channels);
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
		private static extern void cycles_shadernode_set_member_byte_img(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, uint shnType, string name, string  imgName, IntPtr img, uint width, uint height, uint depth, uint channels);
		public static void shadernode_set_member_byte_img(uint clientId, uint sceneId, uint shaderId, uint shadernodeId, ShaderNodeType shnType,
			[MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string imgName, IntPtr img, uint width, uint height, uint depth, uint channels)
		{
			cycles_shadernode_set_member_byte_img(clientId, sceneId, shaderId, shadernodeId, (uint)shnType, name, imgName, img, width, height, depth, channels);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shader_connect_nodes(uint clientId, uint sceneId, uint shaderId, uint fromId, string from, uint toId,
			string to);
		public static void shader_connect_nodes(uint clientId, uint sceneId, uint shaderId, uint fromId, string from, uint toId, string to)
		{
			cycles_shader_connect_nodes(clientId, sceneId, shaderId, fromId, from, toId, to);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CharSet = CharSet.Ansi,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shader_set_name(uint clientId, uint sceneId, uint shaderId, [MarshalAs(UnmanagedType.LPStr)] string name);
		public static void shader_set_name(uint clientId, uint sceneId, uint shaderId, string name)
		{
			cycles_shader_set_name(clientId, sceneId, shaderId, name);
		}

		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shader_set_use_mis(uint clientId, uint sceneId, uint shaderId, uint useMis);
		public static void shader_set_use_mis(uint clientId, uint sceneId, uint shaderId, bool useMis)
		{
			cycles_shader_set_use_mis(clientId, sceneId, shaderId, (uint) (useMis ? 1 : 0));
		}

		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shader_set_use_transparent_shadow(uint clientId, uint sceneId, uint shaderId, uint useTransparentShadow);
		public static void shader_set_use_transparent_shadow(uint clientId, uint sceneId, uint shaderId, bool useTransparentShadow)
		{
			cycles_shader_set_use_transparent_shadow(clientId, sceneId, shaderId, (uint) (useTransparentShadow ? 1 : 0));
		}

		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shader_set_heterogeneous_volume(uint clientId, uint sceneId, uint shaderId, uint heterogeneousVolume);
		public static void shader_set_heterogeneous_volume(uint clientId, uint sceneId, uint shaderId, bool heterogeneousVolume)
		{
			cycles_shader_set_heterogeneous_volume(clientId, sceneId, shaderId, (uint) (heterogeneousVolume ? 1 : 0));
		}

		[DllImport(Constants.ccycles, SetLastError = false,
			CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_shader_new_graph(uint clientId, uint sceneId, uint shaderId);
		public static void shader_new_graph(uint clientId, uint sceneId, uint shaderId)
		{
			cycles_shader_new_graph(clientId, sceneId, shaderId);
		}


#endregion
	}
}
