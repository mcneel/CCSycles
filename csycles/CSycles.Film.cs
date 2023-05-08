using System.Runtime.InteropServices;
using System;

namespace ccl
{
	public partial class CSycles
	{
#region film
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_film_set_filter(IntPtr sessionId, uint filter_type, float filter_width);
		public static void film_set_filter(IntPtr sessionId, FilterType filter_type, float filter_width)
		{
			cycles_film_set_filter(sessionId, (uint)filter_type, filter_width);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_film_set_exposure(IntPtr sessionId, float exposure);
		public static void film_set_exposure(IntPtr sessionId, float exposure)
		{
			cycles_film_set_exposure(sessionId, exposure);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_film_set_use_sample_clamp(IntPtr sessionId, bool use_sample_clamp);
		public static void film_set_use_sample_clamp(IntPtr sessionId, bool use_sample_clamp)
		{
			cycles_film_set_use_sample_clamp(sessionId, use_sample_clamp);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_film_tag_update(IntPtr sessionId);
		public static void film_tag_update(IntPtr sessionId)
		{
			cycles_film_tag_update(sessionId);
		}

#endregion
	}
}
