using System.Runtime.InteropServices;
using System;

namespace ccl
{
	public partial class CSycles
	{
#region integrator
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_tag_update(IntPtr sessionId);
		public static void integrator_tag_update(IntPtr sessionId)
		{
			cycles_integrator_tag_update(sessionId);
		}
		
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_max_bounce(IntPtr sessionId, int value);
		public static void integrator_set_max_bounce(IntPtr sessionId, int value)
		{
			cycles_integrator_set_max_bounce(sessionId, value);
		}
		
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_min_bounce(IntPtr sessionId, int value);
		public static void integrator_set_min_bounce(IntPtr sessionId, int value)
		{
			cycles_integrator_set_min_bounce(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_max_diffuse_bounce(IntPtr sessionId, int value);
		public static void integrator_set_max_diffuse_bounce(IntPtr sessionId, int value)
		{
			cycles_integrator_set_max_diffuse_bounce(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_max_glossy_bounce(IntPtr sessionId, int value);
		public static void integrator_set_max_glossy_bounce(IntPtr sessionId, int value)
		{
			cycles_integrator_set_max_glossy_bounce(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_max_transmission_bounce(IntPtr sessionId, int value);
		public static void integrator_set_max_transmission_bounce(IntPtr sessionId, int value)
		{
			cycles_integrator_set_max_transmission_bounce(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_max_volume_bounce(IntPtr sessionId, int value);
		public static void integrator_set_max_volume_bounce(IntPtr sessionId, int value)
		{
			cycles_integrator_set_max_volume_bounce(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_transparent_max_bounce(IntPtr sessionId, int value);
		public static void integrator_set_transparent_max_bounce(IntPtr sessionId, int value)
		{
			cycles_integrator_set_transparent_max_bounce(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_transparent_min_bounce(IntPtr sessionId, int value);
		public static void integrator_set_transparent_min_bounce(IntPtr sessionId, int value)
		{
			cycles_integrator_set_transparent_min_bounce(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_diffuse_samples(IntPtr sessionId, int value);
		public static void integrator_set_diffuse_samples(IntPtr sessionId, int value)
		{
			cycles_integrator_set_diffuse_samples(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_glossy_samples(IntPtr sessionId, int value);
		public static void integrator_set_glossy_samples(IntPtr sessionId, int value)
		{
			cycles_integrator_set_glossy_samples(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_transmission_samples(IntPtr sessionId, int value);
		public static void integrator_set_transmission_samples(IntPtr sessionId, int value)
		{
			cycles_integrator_set_transmission_samples(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_ao_samples(IntPtr sessionId, int value);
		public static void integrator_set_ao_samples(IntPtr sessionId, int value)
		{
			cycles_integrator_set_ao_samples(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_mesh_light_samples(IntPtr sessionId, int value);
		public static void integrator_set_mesh_light_samples(IntPtr sessionId, int value)
		{
			cycles_integrator_set_mesh_light_samples(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_subsurface_samples(IntPtr sessionId, int value);
		public static void integrator_set_subsurface_samples(IntPtr sessionId, int value)
		{
			cycles_integrator_set_subsurface_samples(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_volume_samples(IntPtr sessionId, int value);
		public static void integrator_set_volume_samples(IntPtr sessionId, int value)
		{
			cycles_integrator_set_volume_samples(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_aa_samples(IntPtr sessionId, int value);
		public static void integrator_set_aa_samples(IntPtr sessionId, int value)
		{
			cycles_integrator_set_aa_samples(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_no_caustics(IntPtr sessionId, bool value);
		public static void integrator_set_no_caustics(IntPtr sessionId, bool value)
		{
			cycles_integrator_set_no_caustics(sessionId, value);
		}
		
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_no_shadows(IntPtr sessionId, bool value);
		public static void integrator_set_no_shadows(IntPtr sessionId, bool value)
		{
			cycles_integrator_set_no_shadows(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_filter_glossy(IntPtr sessionId, float value);
		public static void integrator_set_filter_glossy(IntPtr sessionId, float value)
		{
			cycles_integrator_set_filter_glossy(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_method(IntPtr sessionId, int value);
		public static void integrator_set_method(IntPtr sessionId, IntegratorMethod value)
		{
			cycles_integrator_set_method(sessionId, (int)value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_sample_all_lights_direct(IntPtr sessionId, bool value);
		public static void integrator_set_sample_all_lights_direct(IntPtr sessionId, bool value)
		{
			cycles_integrator_set_sample_all_lights_direct(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_sample_all_lights_indirect(IntPtr sessionId, bool value);
		public static void integrator_set_sample_all_lights_indirect(IntPtr sessionId, bool value)
		{
			cycles_integrator_set_sample_all_lights_indirect(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_volume_step_size(IntPtr sessionId, float value);
		public static void integrator_set_volume_step_size(IntPtr sessionId, float value)
		{
			cycles_integrator_set_volume_step_size(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_volume_max_steps(IntPtr sessionId, int value);
		public static void integrator_set_volume_max_steps(IntPtr sessionId, int value)
		{
			cycles_integrator_set_volume_max_steps(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_seed(IntPtr sessionId, int value);
		public static void integrator_set_seed(IntPtr sessionId, int value)
		{
			cycles_integrator_set_seed(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_sampling_pattern(IntPtr sessionId, uint value);
		public static void integrator_set_sampling_pattern(IntPtr sessionId, SamplingPattern value)
		{
			cycles_integrator_set_sampling_pattern(sessionId, (uint)value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_sample_clamp_direct(IntPtr sessionId, float value);
		public static void integrator_set_sample_clamp_direct(IntPtr sessionId, float value)
		{
			cycles_integrator_set_sample_clamp_direct(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_sample_clamp_indirect(IntPtr sessionId, float value);
		public static void integrator_set_sample_clamp_indirect(IntPtr sessionId, float value)
		{
			cycles_integrator_set_sample_clamp_indirect(sessionId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_light_sampling_threshold(IntPtr sessionId, float value);
		public static void integrator_set_light_sampling_threshold(IntPtr sessionId, float value)
		{
			cycles_integrator_set_light_sampling_threshold(sessionId, value);
		}

  

#endregion
	}
}
