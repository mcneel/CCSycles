using System.Runtime.InteropServices;

namespace ccl
{
	public partial class CSycles
	{
#region object
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_matrix(uint clientId, uint sceneId, uint objectId,
			float a, float b, float c, float d,
			float e, float f, float g, float h,
			float i, float j, float k, float l);
		public static void object_set_matrix(uint clientId, uint sceneId, uint objectId, Transform t)
		{
			cycles_scene_object_set_matrix(clientId, sceneId, objectId,
				t.x.x, t.x.y, t.x.z, t.x.w,
				t.y.x, t.y.y, t.y.z, t.y.w,
				t.z.x, t.z.y, t.z.z, t.z.w);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_mesh(uint clientId, uint sceneId, uint objectId, uint meshId);
		public static void object_set_mesh(uint clientId, uint sceneId, uint objectId, uint meshId)
		{
			cycles_scene_object_set_mesh(clientId, sceneId, objectId, meshId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint cycles_scene_object_get_mesh(uint clientId, uint sceneId, uint objectId);
		public static uint object_get_mesh(uint clientId, uint sceneId, uint objectId)
		{
			return cycles_scene_object_get_mesh(clientId, sceneId, objectId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_visibility(uint clientId, uint sceneId, uint objectId, uint visibility);
		public static void object_set_visibility(uint clientId, uint sceneId, uint objectId, PathRay visibility)
		{
			cycles_scene_object_set_visibility(clientId, sceneId, objectId, (uint)visibility);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_shader(uint clientId, uint sceneId, uint objectId, uint shader);
		public static void object_set_shader(uint clientId, uint sceneId, uint objectId, uint shader)
		{
			cycles_scene_object_set_shader(clientId, sceneId, objectId, shader);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_is_shadowcatcher(uint clientId, uint sceneId, uint objectId, bool is_shadowcatcher);
		public static void object_set_is_shadowcatcher(uint clientId, uint sceneId, uint objectId, bool is_shadowcatcher)
		{
			cycles_scene_object_set_is_shadowcatcher(clientId, sceneId, objectId, is_shadowcatcher);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_mesh_light_no_cast_shadow(uint clientId, uint sceneId, uint objectId, bool mesh_light_no_cast_shadow);
		public static void object_set_mesh_light_no_cast_shadow(uint clientId, uint sceneId, uint objectId, bool mesh_light_no_cast_shadow)
		{
			cycles_scene_object_set_mesh_light_no_cast_shadow(clientId, sceneId, objectId, mesh_light_no_cast_shadow);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_is_block_instance(uint clientId, uint sceneId, uint objectId, bool is_block_instance);
		public static void object_set_is_block_instance(uint clientId, uint sceneId, uint objectId, bool is_block_instance)
		{
			cycles_scene_object_set_is_block_instance(clientId, sceneId, objectId, is_block_instance);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_cutout(uint clientId, uint sceneId, uint objectId, bool cutout);
		public static void object_set_cutout(uint clientId, uint sceneId, uint objectId, bool cutout)
		{
			cycles_scene_object_set_cutout(clientId, sceneId, objectId, cutout);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_scene_object_set_ignore_cutout(uint clientId, uint sceneId, uint objectId, bool ignore_cutout);
		public static void object_set_ignore_cutout(uint clientId, uint sceneId, uint objectId, bool ignore_cutout)
		{
			cycles_scene_object_set_ignore_cutout(clientId, sceneId, objectId, ignore_cutout);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_object_tag_update(uint clientId, uint sceneId, uint objectId);
		public static void object_tag_update(uint clientId, uint sceneId, uint objectId)
		{
			cycles_object_tag_update(clientId, sceneId, objectId);
		}

		#endregion
	}
}
