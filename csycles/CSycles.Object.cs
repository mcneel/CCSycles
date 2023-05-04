using System.Runtime.InteropServices;

namespace ccl
{
	public partial class CSycles
	{
#region object
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_matrix(uint sceneId, uint objectId,
			float a, float b, float c, float d,
			float e, float f, float g, float h,
			float i, float j, float k, float l);
		public static void object_set_matrix(uint sceneId, uint objectId, Transform t)
		{
			cycles_scene_object_set_matrix(sceneId, objectId,
				t.x.x, t.x.y, t.x.z, t.x.w,
				t.y.x, t.y.y, t.y.z, t.y.w,
				t.z.x, t.z.y, t.z.z, t.z.w);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_ocs_frame(uint sceneId, uint objectId,
			float a, float b, float c, float d,
			float e, float f, float g, float h,
			float i, float j, float k, float l);
		public static void object_set_ocs_frame(uint sceneId, uint objectId, Transform t)
		{
			cycles_scene_object_set_ocs_frame(sceneId, objectId,
				t.x.x, t.x.y, t.x.z, t.x.w,
				t.y.x, t.y.y, t.y.z, t.y.w,
				t.z.x, t.z.y, t.z.z, t.z.w);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_mesh(uint sceneId, uint objectId, uint meshId);
		public static void object_set_mesh(uint sceneId, uint objectId, uint meshId)
		{
			cycles_scene_object_set_mesh(sceneId, objectId, meshId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint cycles_scene_object_get_mesh(uint sceneId, uint objectId);
		public static uint object_get_mesh(uint sceneId, uint objectId)
		{
			return cycles_scene_object_get_mesh(sceneId, objectId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_visibility(uint sceneId, uint objectId, uint visibility);
		public static void object_set_visibility(uint sceneId, uint objectId, PathRay visibility)
		{
			cycles_scene_object_set_visibility(sceneId, objectId, (uint)visibility);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_shader(uint sceneId, uint objectId, uint shader);
		public static void object_set_shader(uint sceneId, uint objectId, uint shader)
		{
			cycles_scene_object_set_shader(sceneId, objectId, shader);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_is_shadowcatcher(uint sceneId, uint objectId, bool is_shadowcatcher);
		public static void object_set_is_shadowcatcher(uint sceneId, uint objectId, bool is_shadowcatcher)
		{
			cycles_scene_object_set_is_shadowcatcher(sceneId, objectId, is_shadowcatcher);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_mesh_light_no_cast_shadow(uint sceneId, uint objectId, bool mesh_light_no_cast_shadow);
		public static void object_set_mesh_light_no_cast_shadow(uint sceneId, uint objectId, bool mesh_light_no_cast_shadow)
		{
			cycles_scene_object_set_mesh_light_no_cast_shadow(sceneId, objectId, mesh_light_no_cast_shadow);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_is_block_instance(uint sceneId, uint objectId, bool is_block_instance);
		public static void object_set_is_block_instance(uint sceneId, uint objectId, bool is_block_instance)
		{
			cycles_scene_object_set_is_block_instance(sceneId, objectId, is_block_instance);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_cutout(uint sceneId, uint objectId, bool cutout);
		public static void object_set_cutout(uint sceneId, uint objectId, bool cutout)
		{
			cycles_scene_object_set_cutout(sceneId, objectId, cutout);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_ignore_cutout(uint sceneId, uint objectId, bool ignore_cutout);
		public static void object_set_ignore_cutout(uint sceneId, uint objectId, bool ignore_cutout)
		{
			cycles_scene_object_set_ignore_cutout(sceneId, objectId, ignore_cutout);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_object_tag_update(uint sceneId, uint objectId);
		public static void object_tag_update(uint sceneId, uint objectId)
		{
			cycles_object_tag_update(sceneId, objectId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_object_set_pass_id(uint sceneId, uint objectId, int pass_id);
		public static void object_set_pass_id(uint sceneId, uint objectId, int pass_id)
		{
			cycles_object_set_pass_id(sceneId, objectId, pass_id);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_object_set_random_id(uint sceneId, uint objectId, uint random_id);
		public static void object_set_random_id(uint sceneId, uint objectId, uint random_id)
		{
			cycles_object_set_random_id(sceneId, objectId, random_id);
		}

		#endregion

		#region clipping planes

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint cycles_scene_add_clipping_plane(uint sceneId, float a, float b, float c, float d);
  
		public static uint scene_add_clipping_plane(uint sceneId, float4 equation)
		{
			return scene_add_clipping_plane(sceneId, equation.x, equation.y, equation.z, equation.w);
		}
		public static uint scene_add_clipping_plane(uint sceneId, float a, float b, float c, float d)
		{
			return cycles_scene_add_clipping_plane(sceneId, a, b, c, d);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_set_clipping_plane(uint sceneId, uint cpId, float a, float b, float c, float d);
  
		public static void scene_set_clipping_plane(uint sceneId, uint cpId, float a, float b, float c, float d)
		{
			cycles_scene_set_clipping_plane(sceneId, cpId, a, b, c, d);
		}
		public static void scene_set_clipping_plane(uint sceneId, uint cpId, float4 equation)
		{
			scene_set_clipping_plane(sceneId, cpId, equation.x, equation.y, equation.z, equation.w);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_clear_clipping_planes(uint sceneId);
  
		public static void scene_clear_clipping_planes(uint sceneId)
		{
			cycles_scene_clear_clipping_planes(sceneId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_discard_clipping_plane(uint sceneId, uint cpId);
  
		public static void scene_discard_clipping_plane(uint sceneId, uint cpId)
		{
			cycles_scene_discard_clipping_plane(sceneId, cpId);
		}

		#endregion
	}
}
