using System.Runtime.InteropServices;
using System;

namespace ccl
{
	public partial class CSycles
	{
#region object
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_matrix(IntPtr sessionId, uint objectId,
			float a, float b, float c, float d,
			float e, float f, float g, float h,
			float i, float j, float k, float l);
		public static void object_set_matrix(IntPtr sessionId, uint objectId, Transform t)
		{
			cycles_scene_object_set_matrix(sessionId, objectId,
				t.x.x, t.x.y, t.x.z, t.x.w,
				t.y.x, t.y.y, t.y.z, t.y.w,
				t.z.x, t.z.y, t.z.z, t.z.w);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_ocs_frame(IntPtr sessionId, uint objectId,
			float a, float b, float c, float d,
			float e, float f, float g, float h,
			float i, float j, float k, float l);
		public static void object_set_ocs_frame(IntPtr sessionId, uint objectId, Transform t)
		{
			cycles_scene_object_set_ocs_frame(sessionId, objectId,
				t.x.x, t.x.y, t.x.z, t.x.w,
				t.y.x, t.y.y, t.y.z, t.y.w,
				t.z.x, t.z.y, t.z.z, t.z.w);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_mesh(IntPtr sessionId, uint objectId, uint meshId);
		public static void object_set_mesh(IntPtr sessionId, uint objectId, uint meshId)
		{
			cycles_scene_object_set_mesh(sessionId, objectId, meshId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint cycles_scene_object_get_mesh(IntPtr sessionId, uint objectId);
		public static uint object_get_mesh(IntPtr sessionId, uint objectId)
		{
			return cycles_scene_object_get_mesh(sessionId, objectId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_visibility(IntPtr sessionId, uint objectId, uint visibility);
		public static void object_set_visibility(IntPtr sessionId, uint objectId, PathRay visibility)
		{
			cycles_scene_object_set_visibility(sessionId, objectId, (uint)visibility);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_shader(IntPtr sessionId, uint objectId, uint shader);
		public static void object_set_shader(IntPtr sessionId, uint objectId, uint shader)
		{
			cycles_scene_object_set_shader(sessionId, objectId, shader);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_is_shadowcatcher(IntPtr sessionId, uint objectId, bool is_shadowcatcher);
		public static void object_set_is_shadowcatcher(IntPtr sessionId, uint objectId, bool is_shadowcatcher)
		{
			cycles_scene_object_set_is_shadowcatcher(sessionId, objectId, is_shadowcatcher);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_mesh_light_no_cast_shadow(IntPtr sessionId, uint objectId, bool mesh_light_no_cast_shadow);
		public static void object_set_mesh_light_no_cast_shadow(IntPtr sessionId, uint objectId, bool mesh_light_no_cast_shadow)
		{
			cycles_scene_object_set_mesh_light_no_cast_shadow(sessionId, objectId, mesh_light_no_cast_shadow);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_is_block_instance(IntPtr sessionId, uint objectId, bool is_block_instance);
		public static void object_set_is_block_instance(IntPtr sessionId, uint objectId, bool is_block_instance)
		{
			cycles_scene_object_set_is_block_instance(sessionId, objectId, is_block_instance);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_cutout(IntPtr sessionId, uint objectId, bool cutout);
		public static void object_set_cutout(IntPtr sessionId, uint objectId, bool cutout)
		{
			cycles_scene_object_set_cutout(sessionId, objectId, cutout);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_ignore_cutout(IntPtr sessionId, uint objectId, bool ignore_cutout);
		public static void object_set_ignore_cutout(IntPtr sessionId, uint objectId, bool ignore_cutout)
		{
			cycles_scene_object_set_ignore_cutout(sessionId, objectId, ignore_cutout);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_object_tag_update(IntPtr sessionId, uint objectId);
		public static void object_tag_update(IntPtr sessionId, uint objectId)
		{
			cycles_object_tag_update(sessionId, objectId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_object_set_pass_id(IntPtr sessionId, uint objectId, int pass_id);
		public static void object_set_pass_id(IntPtr sessionId, uint objectId, int pass_id)
		{
			cycles_object_set_pass_id(sessionId, objectId, pass_id);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_object_set_random_id(IntPtr sessionId, uint objectId, uint random_id);
		public static void object_set_random_id(IntPtr sessionId, uint objectId, uint random_id)
		{
			cycles_object_set_random_id(sessionId, objectId, random_id);
		}

		#endregion

		#region clipping planes

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint cycles_scene_add_clipping_plane(IntPtr sessionId, float a, float b, float c, float d);
  
		public static uint scene_add_clipping_plane(IntPtr sessionId, float4 equation)
		{
			return scene_add_clipping_plane(sessionId, equation.x, equation.y, equation.z, equation.w);
		}
		public static uint scene_add_clipping_plane(IntPtr sessionId, float a, float b, float c, float d)
		{
			return cycles_scene_add_clipping_plane(sessionId, a, b, c, d);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_set_clipping_plane(IntPtr sessionId, uint cpId, float a, float b, float c, float d);
  
		public static void scene_set_clipping_plane(IntPtr sessionId, uint cpId, float a, float b, float c, float d)
		{
			cycles_scene_set_clipping_plane(sessionId, cpId, a, b, c, d);
		}
		public static void scene_set_clipping_plane(IntPtr sessionId, uint cpId, float4 equation)
		{
			scene_set_clipping_plane(sessionId, cpId, equation.x, equation.y, equation.z, equation.w);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_clear_clipping_planes(IntPtr sessionId);
  
		public static void scene_clear_clipping_planes(IntPtr sessionId)
		{
			cycles_scene_clear_clipping_planes(sessionId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_discard_clipping_plane(IntPtr sessionId, uint cpId);
  
		public static void scene_discard_clipping_plane(IntPtr sessionId, uint cpId)
		{
			cycles_scene_discard_clipping_plane(sessionId, cpId);
		}

		#endregion
	}
}
