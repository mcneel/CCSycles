using System.Runtime.InteropServices;

namespace ccl
{
	public partial class CSycles
	{
#region film
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_film_set_filter(uint sceneId, uint filter_type, float filter_width);
		public static void film_set_filter(uint sceneId, FilterType filter_type, float filter_width)
		{
			cycles_film_set_filter(sceneId, (uint)filter_type, filter_width);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_film_set_exposure(uint sceneId, float exposure);
		public static void film_set_exposure(uint sceneId, float exposure)
		{
			cycles_film_set_exposure(sceneId, exposure);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_film_set_use_sample_clamp(uint sceneId, bool use_sample_clamp);
		public static void film_set_use_sample_clamp(uint sceneId, bool use_sample_clamp)
		{
			cycles_film_set_use_sample_clamp(sceneId, use_sample_clamp);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_film_tag_update(uint sceneId);
		public static void film_tag_update(uint sceneId)
		{
			cycles_film_tag_update(sceneId);
		}

#endregion
	}
}
