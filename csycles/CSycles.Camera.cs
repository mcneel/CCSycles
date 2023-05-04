using System.Runtime.InteropServices;

namespace ccl
{
	public partial class CSycles
	{
#region camera
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_camera_set_size(uint sceneId, uint width, uint height);
		public static void camera_set_size(uint sceneId, uint width, uint height)
		{
			cycles_camera_set_size(sceneId, width, height);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint cycles_camera_get_width(uint sceneId);
		public static uint camera_get_width(uint sceneId)
		{
			return cycles_camera_get_width(sceneId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint cycles_camera_get_height(uint sceneId);
		public static uint camera_get_height(uint sceneId)
		{
			return cycles_camera_get_height(sceneId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_camera_set_matrix(uint sceneId,
			float a, float b, float c, float d,
			float e, float f, float g, float h,
			float i, float j, float k, float l);
		public static void camera_set_matrix(uint sceneId, Transform t)
		{
			cycles_camera_set_matrix(sceneId,
				t.x.x, t.x.y, t.x.z, t.x.w,
				t.y.x, t.y.y, t.y.z, t.y.w,
				t.z.x, t.z.y, t.z.z, t.z.w);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_camera_set_type(uint sceneId, uint type);
		public static void camera_set_type(uint sceneId, CameraType type)
		{
			cycles_camera_set_type(sceneId, (uint)type);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_camera_set_panorama_type(uint sceneId, uint type);
		public static void camera_set_panorama_type(uint sceneId, PanoramaType type)
		{
			cycles_camera_set_panorama_type(sceneId, (uint)type);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_camera_compute_auto_viewplane(uint sceneId);
		public static void camera_compute_auto_viewplane(uint sceneId)
		{
			cycles_camera_compute_auto_viewplane(sceneId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_camera_set_viewplane(uint sceneId, float left, float right, float top, float bottom);
		public static void camera_set_viewplane(uint sceneId, float left, float right, float top, float bottom)
		{
			cycles_camera_set_viewplane(sceneId, left, right, top, bottom);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_camera_update(uint sceneId);
		public static void camera_update(uint sceneId)
		{
			cycles_camera_update(sceneId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_camera_set_fov(uint sceneId, float fov);
		public static void camera_set_fov(uint sceneId, float fov)
		{
			cycles_camera_set_fov(sceneId, fov);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_camera_set_sensor_width(uint sceneId, float sensorWidth);
		public static void camera_set_sensor_width(uint sceneId, float sensorWidth)
		{
			cycles_camera_set_sensor_width(sceneId, sensorWidth);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_camera_set_sensor_height(uint sceneId, float sensorHeight);
		public static void camera_set_sensor_height(uint sceneId, float sensorHeight)
		{
			cycles_camera_set_sensor_height(sceneId, sensorHeight);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_camera_set_nearclip(uint sceneId, float nearclip);
		public static void camera_set_nearclip(uint sceneId, float nearclip)
		{
			cycles_camera_set_nearclip(sceneId, nearclip);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_camera_set_farclip(uint sceneId, float farclip);
		public static void camera_set_farclip(uint sceneId, float farclip)
		{
			cycles_camera_set_farclip(sceneId, farclip);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_camera_set_aperturesize(uint sceneId, float aperturesize);
		public static void camera_set_aperturesize(uint sceneId, float aperturesize)
		{
			cycles_camera_set_aperturesize(sceneId, aperturesize);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_camera_set_aperture_ratio(uint sceneId, float apertureRatio);
		public static void camera_set_aperture_ratio(uint sceneId, float apertureRatio)
		{
			cycles_camera_set_aperture_ratio(sceneId, apertureRatio);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_camera_set_blades(uint sceneId, uint blades);
		public static void camera_set_blades(uint sceneId, uint blades)
		{
			cycles_camera_set_blades(sceneId, blades);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_camera_set_bladesrotation(uint sceneId, float bladesrotation);
		public static void camera_set_bladesrotation(uint sceneId, float bladesrotation)
		{
			cycles_camera_set_bladesrotation(sceneId, bladesrotation);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_camera_set_shuttertime(uint sceneId, float shuttertime);
		public static void camera_set_shuttertime(uint sceneId, float shuttertime)
		{
			cycles_camera_set_shuttertime(sceneId, shuttertime);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_camera_set_focaldistance(uint sceneId, float focaldistance);
		public static void camera_set_focaldistance(uint sceneId, float focaldistance)
		{
			cycles_camera_set_focaldistance(sceneId, focaldistance);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_camera_set_fisheye_fov(uint sceneId, float fisheyeFov);
		public static void camera_set_fisheye_fov(uint sceneId, float fisheyeFov)
		{
			cycles_camera_set_fisheye_fov(sceneId, fisheyeFov);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_camera_set_fisheye_lens(uint sceneId, float fisheyeLens);
		public static void camera_set_fisheye_lens(uint sceneId, float fisheyeLens)
		{
			cycles_camera_set_fisheye_lens(sceneId, fisheyeLens);
		}
#endregion
	}
}
