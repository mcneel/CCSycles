using System.Runtime.InteropServices;

namespace ccl
{
	public partial class CSycles
	{
#region light

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint cycles_create_light(uint clientId, uint sceneId, uint lightShaderId);
		public static uint create_light(uint clientId, uint sceneId, uint lightShaderId)
		{
			return cycles_create_light(clientId, sceneId, lightShaderId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_type(uint clientId, uint sceneId, uint lightId, uint type);
		public static void light_set_type(uint clientId, uint sceneId, uint lightId, LightType type)
		{
			cycles_light_set_type(clientId, sceneId, lightId, (uint)type);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_samples(uint clientId, uint sceneId, uint lightId, uint samples);
		public static void light_set_samples(uint clientId, uint sceneId, uint lightId, uint samples)
		{
			cycles_light_set_samples(clientId, sceneId, lightId, samples);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_max_bounces(uint clientId, uint sceneId, uint lightId, uint maxBounces);
		public static void light_set_max_bounces(uint clientId, uint sceneId, uint lightId, uint maxBounces)
		{
			cycles_light_set_max_bounces(clientId, sceneId, lightId, maxBounces);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_map_resolution(uint clientId, uint sceneId, uint lightId, uint mapResolution);
		public static void light_set_map_resolution(uint clientId, uint sceneId, uint lightId, uint mapResolution)
		{
			cycles_light_set_map_resolution(clientId, sceneId, lightId, mapResolution);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_spot_angle(uint clientId, uint sceneId, uint lightId, float spotAngle);
		public static void light_set_spot_angle(uint clientId, uint sceneId, uint lightId, float spotAngle)
		{
			cycles_light_set_spot_angle(clientId, sceneId, lightId, spotAngle);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_spot_smooth(uint clientId, uint sceneId, uint lightId, float spotSmooth);
		public static void light_set_spot_smooth(uint clientId, uint sceneId, uint lightId, float spotSmooth)
		{
			cycles_light_set_spot_smooth(clientId, sceneId, lightId, spotSmooth);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_use_mis(uint clientId, uint sceneId, uint lightId, uint useMis);
		public static void light_set_use_mis(uint clientId, uint sceneId, uint lightId, bool useMis)
		{
			cycles_light_set_use_mis(clientId, sceneId, lightId, (uint)(useMis ? 1 : 0));
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_cast_shadow(uint clientId, uint sceneId, uint lightId, uint useMis);
		public static void light_set_cast_shadow(uint clientId, uint sceneId, uint lightId, bool castShadow)
		{
			cycles_light_set_cast_shadow(clientId, sceneId, lightId, (uint)(castShadow ? 1 : 0));
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_sizeu(uint clientId, uint sceneId, uint lightId, float sizeu);
		public static void light_set_sizeu(uint clientId, uint sceneId, uint lightId, float sizeu)
		{
			cycles_light_set_sizeu(clientId, sceneId, lightId, sizeu);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_sizev(uint clientId, uint sceneId, uint lightId, float sizev);
		public static void light_set_sizev(uint clientId, uint sceneId, uint lightId, float sizev)
		{
			cycles_light_set_sizev(clientId, sceneId, lightId, sizev);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_axisu(uint clientId, uint sceneId, uint lightId, float axisux, float axisuy, float axisuz);
		public static void light_set_axisu(uint clientId, uint sceneId, uint lightId, float axisux, float axisuy, float axisuz)
		{
			cycles_light_set_axisu(clientId, sceneId, lightId, axisux, axisuy, axisuz);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_axisv(uint clientId, uint sceneId, uint lightId, float axisvx, float axisvy, float axisvz);
		public static void light_set_axisv(uint clientId, uint sceneId, uint lightId, float axisvx, float axisvy, float axisvz)
		{
			cycles_light_set_axisv(clientId, sceneId, lightId, axisvx, axisvy, axisvz);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_size(uint clientId, uint sceneId, uint lightId, float size);
		public static void light_set_size(uint clientId, uint sceneId, uint lightId, float size)
		{
			cycles_light_set_size(clientId, sceneId, lightId, size);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_dir(uint clientId, uint sceneId, uint lightId, float dirx, float diry, float dirz);
		public static void light_set_dir(uint clientId, uint sceneId, uint lightId, float dirx, float diry, float dirz)
		{
			cycles_light_set_dir(clientId, sceneId, lightId, dirx, diry, dirz);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_co(uint clientId, uint sceneId, uint lightId, float cox, float coy, float coz);
		public static void light_set_co(uint clientId, uint sceneId, uint lightId, float cox, float coy, float coz)
		{
			cycles_light_set_co(clientId, sceneId, lightId, cox, coy, coz);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_tag_update(uint clientId, uint sceneId, uint lightId);
		public static void light_tag_update(uint clientId, uint sceneId, uint lightId)
		{
			cycles_light_tag_update(clientId, sceneId, lightId);
		}

#endregion
	}
}
