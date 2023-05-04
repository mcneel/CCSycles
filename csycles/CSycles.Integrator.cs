using System.Runtime.InteropServices;

namespace ccl
{
	public partial class CSycles
	{
#region integrator
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_tag_update(uint sceneId);
		public static void integrator_tag_update(uint sceneId)
		{
			cycles_integrator_tag_update(sceneId);
		}
		
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_max_bounce(uint sceneId, int value);
		public static void integrator_set_max_bounce(uint sceneId, int value)
		{
			cycles_integrator_set_max_bounce(sceneId, value);
		}
		
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_min_bounce(uint sceneId, int value);
		public static void integrator_set_min_bounce(uint sceneId, int value)
		{
			cycles_integrator_set_min_bounce(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_max_diffuse_bounce(uint sceneId, int value);
		public static void integrator_set_max_diffuse_bounce(uint sceneId, int value)
		{
			cycles_integrator_set_max_diffuse_bounce(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_max_glossy_bounce(uint sceneId, int value);
		public static void integrator_set_max_glossy_bounce(uint sceneId, int value)
		{
			cycles_integrator_set_max_glossy_bounce(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_max_transmission_bounce(uint sceneId, int value);
		public static void integrator_set_max_transmission_bounce(uint sceneId, int value)
		{
			cycles_integrator_set_max_transmission_bounce(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_max_volume_bounce(uint sceneId, int value);
		public static void integrator_set_max_volume_bounce(uint sceneId, int value)
		{
			cycles_integrator_set_max_volume_bounce(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_transparent_max_bounce(uint sceneId, int value);
		public static void integrator_set_transparent_max_bounce(uint sceneId, int value)
		{
			cycles_integrator_set_transparent_max_bounce(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_transparent_min_bounce(uint sceneId, int value);
		public static void integrator_set_transparent_min_bounce(uint sceneId, int value)
		{
			cycles_integrator_set_transparent_min_bounce(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_diffuse_samples(uint sceneId, int value);
		public static void integrator_set_diffuse_samples(uint sceneId, int value)
		{
			cycles_integrator_set_diffuse_samples(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_glossy_samples(uint sceneId, int value);
		public static void integrator_set_glossy_samples(uint sceneId, int value)
		{
			cycles_integrator_set_glossy_samples(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_transmission_samples(uint sceneId, int value);
		public static void integrator_set_transmission_samples(uint sceneId, int value)
		{
			cycles_integrator_set_transmission_samples(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_ao_samples(uint sceneId, int value);
		public static void integrator_set_ao_samples(uint sceneId, int value)
		{
			cycles_integrator_set_ao_samples(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_mesh_light_samples(uint sceneId, int value);
		public static void integrator_set_mesh_light_samples(uint sceneId, int value)
		{
			cycles_integrator_set_mesh_light_samples(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_subsurface_samples(uint sceneId, int value);
		public static void integrator_set_subsurface_samples(uint sceneId, int value)
		{
			cycles_integrator_set_subsurface_samples(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_volume_samples(uint sceneId, int value);
		public static void integrator_set_volume_samples(uint sceneId, int value)
		{
			cycles_integrator_set_volume_samples(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_aa_samples(uint sceneId, int value);
		public static void integrator_set_aa_samples(uint sceneId, int value)
		{
			cycles_integrator_set_aa_samples(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_no_caustics(uint sceneId, bool value);
		public static void integrator_set_no_caustics(uint sceneId, bool value)
		{
			cycles_integrator_set_no_caustics(sceneId, value);
		}
		
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_no_shadows(uint sceneId, bool value);
		public static void integrator_set_no_shadows(uint sceneId, bool value)
		{
			cycles_integrator_set_no_shadows(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_filter_glossy(uint sceneId, float value);
		public static void integrator_set_filter_glossy(uint sceneId, float value)
		{
			cycles_integrator_set_filter_glossy(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_method(uint sceneId, int value);
		public static void integrator_set_method(uint sceneId, IntegratorMethod value)
		{
			cycles_integrator_set_method(sceneId, (int)value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_sample_all_lights_direct(uint sceneId, bool value);
		public static void integrator_set_sample_all_lights_direct(uint sceneId, bool value)
		{
			cycles_integrator_set_sample_all_lights_direct(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_sample_all_lights_indirect(uint sceneId, bool value);
		public static void integrator_set_sample_all_lights_indirect(uint sceneId, bool value)
		{
			cycles_integrator_set_sample_all_lights_indirect(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_volume_step_size(uint sceneId, float value);
		public static void integrator_set_volume_step_size(uint sceneId, float value)
		{
			cycles_integrator_set_volume_step_size(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_volume_max_steps(uint sceneId, int value);
		public static void integrator_set_volume_max_steps(uint sceneId, int value)
		{
			cycles_integrator_set_volume_max_steps(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_seed(uint sceneId, int value);
		public static void integrator_set_seed(uint sceneId, int value)
		{
			cycles_integrator_set_seed(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_sampling_pattern(uint sceneId, uint value);
		public static void integrator_set_sampling_pattern(uint sceneId, SamplingPattern value)
		{
			cycles_integrator_set_sampling_pattern(sceneId, (uint)value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_sample_clamp_direct(uint sceneId, float value);
		public static void integrator_set_sample_clamp_direct(uint sceneId, float value)
		{
			cycles_integrator_set_sample_clamp_direct(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_sample_clamp_indirect(uint sceneId, float value);
		public static void integrator_set_sample_clamp_indirect(uint sceneId, float value)
		{
			cycles_integrator_set_sample_clamp_indirect(sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_light_sampling_threshold(uint sceneId, float value);
		public static void integrator_set_light_sampling_threshold(uint sceneId, float value)
		{
			cycles_integrator_set_light_sampling_threshold(sceneId, value);
		}

  

#endregion
	}
}
