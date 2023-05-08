using System.Runtime.InteropServices;
using System;

namespace ccl
{
	public partial class CSycles
	{
#region light

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint cycles_create_light(IntPtr sessionId, uint lightShaderId);
		public static uint create_light(IntPtr sessionId, uint lightShaderId)
		{
			return cycles_create_light(sessionId, lightShaderId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_type(IntPtr sessionId, uint lightId, uint type);
		public static void light_set_type(IntPtr sessionId, uint lightId, LightType type)
		{
			cycles_light_set_type(sessionId, lightId, (uint)type);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_samples(IntPtr sessionId, uint lightId, uint samples);
		public static void light_set_samples(IntPtr sessionId, uint lightId, uint samples)
		{
			cycles_light_set_samples(sessionId, lightId, samples);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_max_bounces(IntPtr sessionId, uint lightId, uint maxBounces);
		public static void light_set_max_bounces(IntPtr sessionId, uint lightId, uint maxBounces)
		{
			cycles_light_set_max_bounces(sessionId, lightId, maxBounces);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_map_resolution(IntPtr sessionId, uint lightId, uint mapResolution);
		public static void light_set_map_resolution(IntPtr sessionId, uint lightId, uint mapResolution)
		{
			cycles_light_set_map_resolution(sessionId, lightId, mapResolution);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_spot_angle(IntPtr sessionId, uint lightId, float spotAngle);
		public static void light_set_spot_angle(IntPtr sessionId, uint lightId, float spotAngle)
		{
			cycles_light_set_spot_angle(sessionId, lightId, spotAngle);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_spot_smooth(IntPtr sessionId, uint lightId, float spotSmooth);
		public static void light_set_spot_smooth(IntPtr sessionId, uint lightId, float spotSmooth)
		{
			cycles_light_set_spot_smooth(sessionId, lightId, spotSmooth);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_use_mis(IntPtr sessionId, uint lightId, uint useMis);
		public static void light_set_use_mis(IntPtr sessionId, uint lightId, bool useMis)
		{
			cycles_light_set_use_mis(sessionId, lightId, (uint)(useMis ? 1 : 0));
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_cast_shadow(IntPtr sessionId, uint lightId, uint useMis);
		public static void light_set_cast_shadow(IntPtr sessionId, uint lightId, bool castShadow)
		{
			cycles_light_set_cast_shadow(sessionId, lightId, (uint)(castShadow ? 1 : 0));
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_sizeu(IntPtr sessionId, uint lightId, float sizeu);
		public static void light_set_sizeu(IntPtr sessionId, uint lightId, float sizeu)
		{
			cycles_light_set_sizeu(sessionId, lightId, sizeu);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_sizev(IntPtr sessionId, uint lightId, float sizev);
		public static void light_set_sizev(IntPtr sessionId, uint lightId, float sizev)
		{
			cycles_light_set_sizev(sessionId, lightId, sizev);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_axisu(IntPtr sessionId, uint lightId, float axisux, float axisuy, float axisuz);
		public static void light_set_axisu(IntPtr sessionId, uint lightId, float axisux, float axisuy, float axisuz)
		{
			cycles_light_set_axisu(sessionId, lightId, axisux, axisuy, axisuz);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_axisv(IntPtr sessionId, uint lightId, float axisvx, float axisvy, float axisvz);
		public static void light_set_axisv(IntPtr sessionId, uint lightId, float axisvx, float axisvy, float axisvz)
		{
			cycles_light_set_axisv(sessionId, lightId, axisvx, axisvy, axisvz);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_angle(IntPtr sessionId, uint lightId, float angle);
		public static void light_set_angle(IntPtr sessionId, uint lightId, float angle)
		{
			cycles_light_set_angle(sessionId, lightId, angle);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_size(IntPtr sessionId, uint lightId, float size);
		public static void light_set_size(IntPtr sessionId, uint lightId, float size)
		{
			cycles_light_set_size(sessionId, lightId, size);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_dir(IntPtr sessionId, uint lightId, float dirx, float diry, float dirz);
		public static void light_set_dir(IntPtr sessionId, uint lightId, float dirx, float diry, float dirz)
		{
			cycles_light_set_dir(sessionId, lightId, dirx, diry, dirz);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_set_co(IntPtr sessionId, uint lightId, float cox, float coy, float coz);
		public static void light_set_co(IntPtr sessionId, uint lightId, float cox, float coy, float coz)
		{
			cycles_light_set_co(sessionId, lightId, cox, coy, coz);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_light_tag_update(IntPtr sessionId, uint lightId);
		public static void light_tag_update(IntPtr sessionId, uint lightId)
		{
			cycles_light_tag_update(sessionId, lightId);
		}

#endregion
	}
}
