using System;
using System.Runtime.InteropServices;

namespace ccl
{
	public partial class CSycles
	{
#region session
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern int cycles_session_reset(IntPtr sessionId, uint width, uint height, uint samples, uint full_x, uint full_y, uint full_width, uint full_height );
		public static int session_reset(IntPtr sessionId, uint width, uint height, uint samples, uint full_x, uint full_y, uint full_width, uint full_height )
		{
			return cycles_session_reset(sessionId, width, height, samples, full_x, full_y, full_width, full_height );
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_add_pass(IntPtr sessionId, int pass_id);
		public static void session_add_pass(IntPtr sessionId, PassType pass_id)
		{
			cycles_session_add_pass(sessionId, (int)pass_id);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_clear_passes(IntPtr sessionId);
		public static void session_clear_passes(IntPtr sessionId)
		{
			cycles_session_clear_passes(sessionId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr cycles_session_create(IntPtr sessionParamsId);
		public static IntPtr session_create(IntPtr sessionParamsId)
		{
			return cycles_session_create(sessionParamsId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern uint cycles_session_destroy(IntPtr sessionId);
		public static uint session_destroy(IntPtr sessionId)
		{
			return cycles_session_destroy(sessionId);
		}

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void UpdateCallback(IntPtr sid);
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_set_update_callback(IntPtr sessionId, UpdateCallback update);
		public static void session_set_update_callback(IntPtr sessionId, UpdateCallback update)
		{
			cycles_session_set_update_callback(sessionId, update);
		}

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void TestCancelCallback(IntPtr sid);
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_set_cancel_callback(IntPtr sessionId, TestCancelCallback cancel);
		public static void session_set_cancel_callback(IntPtr sessionId, TestCancelCallback cancel)
		{
			cycles_session_set_cancel_callback(sessionId, cancel);
		}

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void RenderTileCallback(IntPtr sessionId, uint x, uint y, uint w, uint h, uint sample, uint depth, PassType passtype, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 9)][In, Out] float[] pixels, [In] int len);
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_set_update_tile_callback(IntPtr sessionId, RenderTileCallback renderTileCb);
		public static void session_set_update_tile_callback(IntPtr sessionId, RenderTileCallback renderTileCb)
		{
			cycles_session_set_update_tile_callback(sessionId, renderTileCb);
		}
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_set_write_tile_callback(IntPtr sessionId, RenderTileCallback renderTileCb);
		public static void session_set_write_tile_callback(IntPtr sessionId, RenderTileCallback renderTileCb)
		{
			cycles_session_set_write_tile_callback(sessionId, renderTileCb);
		}

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void DisplayUpdateCallback(IntPtr sessionId, int sample);
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_set_display_update_callback(IntPtr sessionId, DisplayUpdateCallback displayUpdateCallback);
		public static void session_set_display_update_callback(IntPtr sessionId, DisplayUpdateCallback displayUpdateCallback)
		{
			cycles_session_set_display_update_callback(sessionId, displayUpdateCallback);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_start(IntPtr sessionId);
		public static void session_start(IntPtr sessionId)
		{
			cycles_session_start(sessionId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_prepare_run(IntPtr sessionId);
		public static void session_prepare_run(IntPtr sessionId)
		{
			cycles_session_prepare_run(sessionId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern int cycles_session_sample(IntPtr sessionId);
		public static int session_sample(IntPtr sessionId)
		{
			return cycles_session_sample(sessionId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_end_run(IntPtr sessionId);
		public static void session_end_run(IntPtr sessionId)
		{
			cycles_session_end_run(sessionId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_wait(IntPtr sessionId);
		public static void session_wait(IntPtr sessionId)
		{
			cycles_session_wait(sessionId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_set_pause(IntPtr sessionId, bool pause);
		public static void session_set_pause(IntPtr sessionId, bool pause)
		{
			cycles_session_set_pause(sessionId, pause);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern bool cycles_session_is_paused(IntPtr sessionId);
		public static bool session_is_paused(IntPtr sessionId)
		{
			return cycles_session_is_paused(sessionId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_set_samples(IntPtr sessionId, int samples);
		public static void session_set_samples(IntPtr sessionId, int samples)
		{
			cycles_session_set_samples(sessionId, samples);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA2101:Specify marshaling for P/Invoke string arguments", Justification = "Using simple c string")]
		private static extern void cycles_session_cancel(IntPtr sessionId, [MarshalAs(UnmanagedType.LPStr)] string cancelMessage);
		public static void session_cancel(IntPtr sessionId, string cancelMessage)
		{
			cycles_session_cancel(sessionId, cancelMessage);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_get_float_buffer(IntPtr sessionId, int passType, ref IntPtr pixels);
		public static void session_get_float_buffer(IntPtr sessionId, PassType passType, ref IntPtr pixels)
		{
			cycles_session_get_float_buffer(sessionId, (int)passType, ref pixels);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_retain_float_buffer(IntPtr sessionId, int passType, int width, int height, ref IntPtr pixels);
		public static void session_retain_float_buffer(IntPtr sessionId, PassType passType, int width, int height, ref IntPtr pixels)
		{
			cycles_session_retain_float_buffer(sessionId, (int)passType, width, height, ref pixels);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_release_float_buffer(IntPtr sessionId, int passType);
		public static void session_release_float_buffer(IntPtr sessionId, PassType passType)
		{
			cycles_session_release_float_buffer(sessionId, (int)passType);
		}
		#endregion

		#region session parameters
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr cycles_session_params_create(uint deviceId);
		public static IntPtr session_params_create(uint deviceId)
		{
			return cycles_session_params_create(deviceId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_params_set_device(IntPtr sessionParamsId, uint deviceId);
		public static void session_params_set_device(IntPtr sessionParamsId, uint deviceId)
		{
			cycles_session_params_set_device(sessionParamsId, deviceId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_params_set_background(IntPtr sessionParamsId, uint background);
		public static void session_params_set_background(IntPtr sessionParamsId, bool background)
		{
			cycles_session_params_set_background(sessionParamsId, (uint)(background ? 1 : 0));
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_params_set_progressive_refine(IntPtr sessionParamsId, uint progressiveRefine);
		public static void session_params_set_progressive_refine(IntPtr sessionParamsId, bool progressiveRefine)
		{
			cycles_session_params_set_progressive_refine(sessionParamsId, (uint)(progressiveRefine?1:0));
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA2101:Specify marshaling for P/Invoke string arguments", Justification = "Using simple c string")]
		private static extern void cycles_session_params_set_output_path(IntPtr sessionParamsId, [MarshalAs(UnmanagedType.LPStr)] string outputPath);
		public static void session_params_set_output_path(IntPtr sessionParamsId, string outputPath)
		{
			cycles_session_params_set_output_path(sessionParamsId, outputPath);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_params_set_progressive(IntPtr sessionParamsId, uint progressive);
		public static void session_params_set_progressive(IntPtr sessionParamsId, bool progressive)
		{
			cycles_session_params_set_progressive(sessionParamsId, (uint)(progressive?1:0));
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_params_set_experimental(IntPtr sessionParamsId, uint experimental);
		public static void session_params_set_experimental(IntPtr sessionParamsId, bool experimental)
		{
			cycles_session_params_set_experimental(sessionParamsId, (uint)(experimental?1:0));
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_params_set_samples(IntPtr sessionParamsId, int samples);
		public static void session_params_set_samples(IntPtr sessionParamsId, int samples)
		{
			cycles_session_params_set_samples(sessionParamsId, samples);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_params_set_tile_size(IntPtr sessionParamsId, uint tile_size);
		public static void session_params_set_tile_size(IntPtr sessionParamsId, uint tile_size)
		{
			cycles_session_params_set_tile_size(sessionParamsId, tile_size);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_params_set_tile_order(IntPtr sessionParamsId, uint tileOrder);
		public static void session_params_set_tile_order(IntPtr sessionParamsId, TileOrder tileOrder)
		{
			cycles_session_params_set_tile_order(sessionParamsId, (uint)tileOrder);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_params_set_start_resolution(IntPtr sessionParamsId, int startResolution);
		public static void session_params_set_start_resolution(IntPtr sessionParamsId, int startResolution)
		{
			cycles_session_params_set_start_resolution(sessionParamsId, startResolution);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_params_set_threads(IntPtr sessionParamsId, uint threads);
		public static void session_params_set_threads(IntPtr sessionParamsId, uint threads)
		{
			cycles_session_params_set_threads(sessionParamsId, threads);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_params_set_display_buffer_linear(IntPtr sessionParamsId, uint displayBufferLinear);
		public static void session_params_set_display_buffer_linear(IntPtr sessionParamsId, bool displayBufferLinear)
		{
			cycles_session_params_set_display_buffer_linear(sessionParamsId, (uint)(displayBufferLinear ? 1 : 0));
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_params_set_skip_linear_to_srgb_conversion(IntPtr sessionParamsId, uint skipLinearToSrgbConversion);
		public static void session_params_set_skip_linear_to_srgb_conversion(IntPtr sessionParamsId, bool skipLinearToSrgbConversion)
		{
			cycles_session_params_set_skip_linear_to_srgb_conversion(sessionParamsId, (uint)(skipLinearToSrgbConversion ? 1 : 0));
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_params_set_cancel_timeout(IntPtr sessionParamsId, double cancelTimeout);
		public static void session_params_set_cancel_timeout(IntPtr sessionParamsId, double cancelTimeout)
		{
			cycles_session_params_set_cancel_timeout(sessionParamsId, cancelTimeout);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_params_set_reset_timeout(IntPtr sessionParamsId, double resetTimeout);
		public static void session_params_set_reset_timeout(IntPtr sessionParamsId, double resetTimeout)
		{
			cycles_session_params_set_reset_timeout(sessionParamsId, resetTimeout);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_params_set_text_timeout(IntPtr sessionParamsId, double textTimeout);
		public static void session_params_set_text_timeout(IntPtr sessionParamsId, double textTimeout)
		{
			cycles_session_params_set_text_timeout(sessionParamsId, textTimeout);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_params_set_shadingsystem(IntPtr sessionParamsId, uint shadingsystem);
		public static void session_params_set_shadingsystem(IntPtr sessionParamsId, ShadingSystem shadingSystem)
		{
			cycles_session_params_set_shadingsystem(sessionParamsId, (uint)shadingSystem);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_session_params_set_pixel_size(IntPtr sessionParamsId, uint pixel_size);
		public static void session_params_set_pixel_size(IntPtr sessionParamsId, uint pixelSize)
		{
			cycles_session_params_set_pixel_size(sessionParamsId, pixelSize);
		}
#endregion

#region progress
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_progress_reset(IntPtr sessionId);
		public static void progress_reset(IntPtr sessionId)
		{
			cycles_progress_reset(sessionId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern int cycles_progress_get_sample(IntPtr sessionId);
		public static int progress_get_sample(IntPtr sessionId)
		{
			return cycles_progress_get_sample(sessionId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_progress_get_time(IntPtr sessionId, out double totalTime, out double sampleTime);
		public static void progress_get_time(IntPtr sessionId, out double totalTime, out double sampleTime)
		{
			cycles_progress_get_time(sessionId,  out totalTime, out sampleTime);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_progress_get_progress(IntPtr sessionId, out float progress);
		public static void progress_get_progress(IntPtr sessionId, out float progress)
		{
			cycles_progress_get_progress(sessionId, out progress);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_tilemanager_get_sample_info(IntPtr sessionId, out uint samples, out uint numSamples);
		public static void tilemanager_get_sample_info(IntPtr sessionId, out uint samples, out uint numSamples)
		{
			cycles_tilemanager_get_sample_info(sessionId, out samples, out numSamples);
		}

		internal class CSStringHolder : IDisposable
		{
			IntPtr stringHolderPtr;
			public CSStringHolder()
			{
				stringHolderPtr = cycles_string_holder_new();
			}

			public string Value {
				get {
					if(stringHolderPtr!=IntPtr.Zero) {
						IntPtr strPtr = cycles_string_holder_get(stringHolderPtr);
						string s = Marshal.PtrToStringAnsi(strPtr);
						return s;
					}
					return "";
				}
			}

			public IntPtr Ptr { get { return stringHolderPtr; } }

			#region IDisposable Support
			private bool disposedValue = false; // To detect redundant calls

			protected virtual void Dispose(bool disposing)
			{
				if (!disposedValue) {
					if (disposing) {
					}

					cycles_string_holder_delete(stringHolderPtr);
					stringHolderPtr = IntPtr.Zero;

					disposedValue = true;
				}
			}

			// This code added to correctly implement the disposable pattern.
			public void Dispose()
			{
				Dispose(true);
			}
			#endregion


	}
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern IntPtr cycles_string_holder_new();
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern void cycles_string_holder_delete(IntPtr strHolder);
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern IntPtr cycles_string_holder_get(IntPtr strHolder);

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern bool cycles_progress_get_status(IntPtr sessionId, IntPtr strHolder);
		public static string progress_get_status(IntPtr sessionId)
		{
			using (CSStringHolder stringHolder = new CSStringHolder()) {
				bool success = cycles_progress_get_status(sessionId, stringHolder.Ptr);
				if(success) {
					string status = stringHolder.Value;
				  return status;
				}
			}

			return "";
		}


		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention=CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern bool cycles_progress_get_substatus(IntPtr sessionId, IntPtr strHolder);
		public static string progress_get_substatus(IntPtr sessionId)
		{
			using (CSStringHolder stringHolder = new CSStringHolder()) {
				bool success = cycles_progress_get_substatus(sessionId, stringHolder.Ptr);
				if (success) {
					string status = stringHolder.Value;
					return status;
				}
			}

	  return "";
	}

#endregion
	}
}
