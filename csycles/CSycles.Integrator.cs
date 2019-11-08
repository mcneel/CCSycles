using System.Runtime.InteropServices;

namespace ccl
{
	public partial class CSycles
	{
#region integrator
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_tag_update(uint clientId, uint sceneId);
		public static void integrator_tag_update(uint clientId, uint sceneId)
		{
			cycles_integrator_tag_update(clientId, sceneId);
		}
		
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_max_bounce(uint clientId, uint sceneId, int value);
		public static void integrator_set_max_bounce(uint clientId, uint sceneId, int value)
		{
			cycles_integrator_set_max_bounce(clientId, sceneId, value);
		}
		
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_min_bounce(uint clientId, uint sceneId, int value);
		public static void integrator_set_min_bounce(uint clientId, uint sceneId, int value)
		{
			cycles_integrator_set_min_bounce(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_max_diffuse_bounce(uint clientId, uint sceneId, int value);
		public static void integrator_set_max_diffuse_bounce(uint clientId, uint sceneId, int value)
		{
			cycles_integrator_set_max_diffuse_bounce(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_max_glossy_bounce(uint clientId, uint sceneId, int value);
		public static void integrator_set_max_glossy_bounce(uint clientId, uint sceneId, int value)
		{
			cycles_integrator_set_max_glossy_bounce(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_max_transmission_bounce(uint clientId, uint sceneId, int value);
		public static void integrator_set_max_transmission_bounce(uint clientId, uint sceneId, int value)
		{
			cycles_integrator_set_max_transmission_bounce(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_max_volume_bounce(uint clientId, uint sceneId, int value);
		public static void integrator_set_max_volume_bounce(uint clientId, uint sceneId, int value)
		{
			cycles_integrator_set_max_volume_bounce(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_transparent_max_bounce(uint clientId, uint sceneId, int value);
		public static void integrator_set_transparent_max_bounce(uint clientId, uint sceneId, int value)
		{
			cycles_integrator_set_transparent_max_bounce(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_transparent_min_bounce(uint clientId, uint sceneId, int value);
		public static void integrator_set_transparent_min_bounce(uint clientId, uint sceneId, int value)
		{
			cycles_integrator_set_transparent_min_bounce(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_diffuse_samples(uint clientId, uint sceneId, int value);
		public static void integrator_set_diffuse_samples(uint clientId, uint sceneId, int value)
		{
			cycles_integrator_set_diffuse_samples(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_glossy_samples(uint clientId, uint sceneId, int value);
		public static void integrator_set_glossy_samples(uint clientId, uint sceneId, int value)
		{
			cycles_integrator_set_glossy_samples(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_transmission_samples(uint clientId, uint sceneId, int value);
		public static void integrator_set_transmission_samples(uint clientId, uint sceneId, int value)
		{
			cycles_integrator_set_transmission_samples(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_ao_samples(uint clientId, uint sceneId, int value);
		public static void integrator_set_ao_samples(uint clientId, uint sceneId, int value)
		{
			cycles_integrator_set_ao_samples(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_mesh_light_samples(uint clientId, uint sceneId, int value);
		public static void integrator_set_mesh_light_samples(uint clientId, uint sceneId, int value)
		{
			cycles_integrator_set_mesh_light_samples(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_subsurface_samples(uint clientId, uint sceneId, int value);
		public static void integrator_set_subsurface_samples(uint clientId, uint sceneId, int value)
		{
			cycles_integrator_set_subsurface_samples(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_volume_samples(uint clientId, uint sceneId, int value);
		public static void integrator_set_volume_samples(uint clientId, uint sceneId, int value)
		{
			cycles_integrator_set_volume_samples(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_aa_samples(uint clientId, uint sceneId, int value);
		public static void integrator_set_aa_samples(uint clientId, uint sceneId, int value)
		{
			cycles_integrator_set_aa_samples(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_no_caustics(uint clientId, uint sceneId, bool value);
		public static void integrator_set_no_caustics(uint clientId, uint sceneId, bool value)
		{
			cycles_integrator_set_no_caustics(clientId, sceneId, value);
		}
		
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_no_shadows(uint clientId, uint sceneId, bool value);
		public static void integrator_set_no_shadows(uint clientId, uint sceneId, bool value)
		{
			cycles_integrator_set_no_shadows(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_filter_glossy(uint clientId, uint sceneId, float value);
		public static void integrator_set_filter_glossy(uint clientId, uint sceneId, float value)
		{
			cycles_integrator_set_filter_glossy(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_method(uint clientId, uint sceneId, int value);
		public static void integrator_set_method(uint clientId, uint sceneId, IntegratorMethod value)
		{
			cycles_integrator_set_method(clientId, sceneId, (int)value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_sample_all_lights_direct(uint clientId, uint sceneId, bool value);
		public static void integrator_set_sample_all_lights_direct(uint clientId, uint sceneId, bool value)
		{
			cycles_integrator_set_sample_all_lights_direct(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_sample_all_lights_indirect(uint clientId, uint sceneId, bool value);
		public static void integrator_set_sample_all_lights_indirect(uint clientId, uint sceneId, bool value)
		{
			cycles_integrator_set_sample_all_lights_indirect(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_volume_step_size(uint clientId, uint sceneId, float value);
		public static void integrator_set_volume_step_size(uint clientId, uint sceneId, float value)
		{
			cycles_integrator_set_volume_step_size(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_volume_max_steps(uint clientId, uint sceneId, int value);
		public static void integrator_set_volume_max_steps(uint clientId, uint sceneId, int value)
		{
			cycles_integrator_set_volume_max_steps(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_seed(uint clientId, uint sceneId, int value);
		public static void integrator_set_seed(uint clientId, uint sceneId, int value)
		{
			cycles_integrator_set_seed(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_sampling_pattern(uint clientId, uint sceneId, uint value);
		public static void integrator_set_sampling_pattern(uint clientId, uint sceneId, SamplingPattern value)
		{
			cycles_integrator_set_sampling_pattern(clientId, sceneId, (uint)value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_sample_clamp_direct(uint clientId, uint sceneId, float value);
		public static void integrator_set_sample_clamp_direct(uint clientId, uint sceneId, float value)
		{
			cycles_integrator_set_sample_clamp_direct(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_sample_clamp_indirect(uint clientId, uint sceneId, float value);
		public static void integrator_set_sample_clamp_indirect(uint clientId, uint sceneId, float value)
		{
			cycles_integrator_set_sample_clamp_indirect(clientId, sceneId, value);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_integrator_set_light_sampling_threshold(uint clientId, uint sceneId, float value);
		public static void integrator_set_light_sampling_threshold(uint clientId, uint sceneId, float value)
		{
			cycles_integrator_set_light_sampling_threshold(clientId, sceneId, value);
		}

  

#endregion
	}
}
