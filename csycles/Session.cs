/**
Copyright 2014 Robert McNeel and Associates

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
**/

using System;
using System.Collections.Generic;

namespace ccl
{
	/// <summary>
	/// The Cycles session representation.
	///
	/// The Session is used to manage the render process at the highest level. Here one can set
	/// the different callbacks to retrieve render results and updates and statistics during a render.
	///
	/// A session is created using SessionParameters and a Scene.
	///
	/// Through Session the render process is started and ended.
	/// </summary>
	public class Session
	{
		/// <summary>
		/// True if the session has already been destroyed.
		/// </summary>
		private bool Destroyed { get; set; }

		private Scene sc;
		/// <summary>
		/// Get or set the Scene used for this Session
		/// </summary>
		public Scene Scene
		{
			get { return sc; }
			set {
				//CSycles.session_set_scene(Client.Id, Id, value.Id);
				sc = value;
			}
		}
		/// <summary>
		/// Get the SessionParams used for this Session
		/// </summary>
		public SessionParameters SessionParams { get; private set; }
		/// <summary>
		/// Get the ID for this session
		/// </summary>
		public uint Id { get; }

		/// <summary>
		/// Reference to client used for this session
		/// </summary>
		private Client Client { get; }

		/// <summary>
		/// Create a new session using the given SessionParameters and Scene
		/// </summary>
		/// <param name="sessionParams">Previously created SessionParameters to create Session with</param>
		/// <param name="scene">Previously created Scene to create Session with</param>
		public Session(Client client, SessionParameters sessionParams)
		{
			Client = client;
			SessionParams = sessionParams;
			Id = CSycles.session_create(Client.Id, sessionParams.Id);
		}

		/// <summary>
		/// private reference to the update callback
		/// </summary>
		CSycles.UpdateCallback updateCallback;

		/// <summary>
		/// Set the CSycles.UpdateCallback to Cycles to receive progress updates
		/// </summary>
		public CSycles.UpdateCallback UpdateCallback
		{
			set
			{
				if (Destroyed) return;
				updateCallback = value;
				CSycles.session_set_update_callback(Client.Id, Id, value);
			}
		}

		/// <summary>
		/// Private reference to the test cancel callback
		/// </summary>
		CSycles.TestCancelCallback testCancelCallback;

		/// <summary>
		/// Set the CSycles.TestCancelCallback to Cycles to test for session cancel
		/// </summary>
		public CSycles.TestCancelCallback TestCancelCallback
		{
			set
			{
				if (Destroyed) return;
				testCancelCallback = value;
				CSycles.session_set_cancel_callback(Client.Id, Id, value);
			}
		}

		/// <summary>
		/// Private reference to the tile update callback.
		/// </summary>
		CSycles.RenderTileCallback updateTileCallback;

		/// <summary>
		/// Set the CSycles.RenderTileCallback to use for render tile updates
		/// </summary>
		public CSycles.RenderTileCallback UpdateTileCallback
		{
			set
			{
				if (Destroyed) return;
				updateTileCallback = value;
				CSycles.session_set_update_tile_callback(Client.Id, Id, value);
			}
		}

		/// <summary>
		/// Private reference to the tile write callback
		/// </summary>
		CSycles.RenderTileCallback writeTileCallback;
		/// <summary>
		/// Set the CSycles.RenderTileCallback to use for render tile writes
		/// </summary>
		public CSycles.RenderTileCallback WriteTileCallback
		{
			set
			{
				if (Destroyed) return;
				writeTileCallback = value;
				CSycles.session_set_write_tile_callback(Client.Id, Id, value);
			}
		}

		private CSycles.DisplayUpdateCallback displayUpdateCallback;
		/// <summary>
		/// Set the CSycles.DisplayUpdateCallback to use for signalling rendered
		/// sample pass
		/// </summary>
		public CSycles.DisplayUpdateCallback DisplayUpdateCallback
		{
			set
			{
				if (Destroyed) return;
				displayUpdateCallback = value;
				CSycles.session_set_display_update_callback(Client.Id, Id, value);
			}
		}

		/// <summary>
		/// Start the rendering session. After this one should Wait() for
		/// the session to complete.
		/// </summary>
		public void Start()
		{
			if (Destroyed) return;
			CSycles.progress_reset(Client.Id, Id);
			CSycles.session_start(Client.Id, Id);
		}

		/// <summary>
		/// Set up the session for running through the sampler.
		/// </summary>
		public void PrepareRun()
		{
			if(Destroyed) return;
			CSycles.session_prepare_run(Client.Id, Id);
		}

		/// <summary>
		/// Sample one pass. Return -1 when no pass was rendered, zero or positive when something was rendered.
		/// </summary>
		public int Sample()
		{
			if(Destroyed) return -1;
			return CSycles.session_sample(Client.Id, Id);
		}

		/// <summary>
		/// Tear down the set up run for sampling.
		/// </summary>
		public void EndRun()
		{
			if(Destroyed) return;
			CSycles.session_end_run(Client.Id, Id);
		}

		/// <summary>
		/// Wait for the rendering session to complete
		/// </summary>
		public void Wait()
		{
			if (Destroyed) return;
			CSycles.session_wait(Client.Id, Id);
		}

		/// <summary>
		/// Cancel the currently in progress render
		/// </summary>
		/// <param name="cancelMessage"></param>
		public void Cancel(string cancelMessage)
		{
			if (Destroyed) return;
			CSycles.session_cancel(Client.Id, Id, cancelMessage);
		}

		/// <summary>
		/// Destroy the session and all related.
		/// </summary>
		public void Destroy()
		{
			if (Destroyed) return;
			// TODO: XXXX scene no longer managed separately, should all go through session.
			CSycles.session_destroy(Client.Id, Id, 0);
			Destroyed = true;
		}

		public void GetPixelBuffer(PassType pt, ref IntPtr pixel_buffer)
		{
			if (Destroyed)
			{
				pixel_buffer = IntPtr.Zero;
			}
			else
			{
				CSycles.session_get_float_buffer(Client.Id, Id, pt, ref pixel_buffer);
			}
		}


		/// <summary>
		/// Reset a Session
		/// </summary>
		/// <param name="width">Width of the resolution to reset with</param>
		/// <param name="height">Height of the resolutin to reset with</param>
		/// <param name="samples">The amount of samples to reset with</param>
		/// <returns>0 on success. -1 when the session is already destroyed. -13 when a crash happened.</returns>
		public int Reset(uint width, uint height, uint samples, uint full_x, uint full_y, uint full_width, uint full_height )
		{
			if (Destroyed) return -1;
			CSycles.progress_reset(Client.Id, Id);
			return CSycles.session_reset(Client.Id, Id, width, height, samples, full_x, full_y, full_width, full_height);
		}

		public int Reset(int width, int height, int samples, int full_x, int full_y, int full_width, int full_height )
		{
			return Reset((uint)width, (uint)height, (uint)samples, (uint)full_x, (uint)full_y, (uint)full_width, (uint)full_height);
		}

		/// <summary>
		/// Pause or un-pause a render session.
		/// </summary>
		/// <param name="pause"></param>
		public void SetPause(bool pause)
		{
			if (Destroyed) return;
			CSycles.session_set_pause(Client.Id, Id, pause);
		}

		public bool IsPaused()
		{
			if (Destroyed) return false;
			return CSycles.session_is_paused(Client.Id, Id);
		}

		/// <summary>
		/// Set sample count for session to render. This can be used to increase the sample
		/// count for an interactive render session.
		/// </summary>
		/// <param name="samples"></param>
		public void SetSamples(int samples)
		{
			if (Destroyed) return;
			CSycles.session_set_samples(Client.Id, Id, samples);
		}


		public IEnumerable<PassType> Passes {
			get {
				foreach(var pass in _passes) {
						yield return pass;
				}
			}
		}
		private List<PassType> _passes = new List<PassType>();
		/// <summary>
		/// Add given pass to output layers
		/// </summary>
		/// <param name="pass"></param>
		public void AddPass(PassType pass)
		{
			if (Destroyed) return;
			CSycles.session_add_pass(Client.Id, Id, pass);
			if (!_passes.Contains(pass)) {
				_passes.Add(pass);
			}
	}

		/// <summary>
		/// Clear all passes for session. Note, will always add Combined pass back
		/// </summary>
		public void ClearPasses()
		{
			if (Destroyed) return;
			_passes.Clear();
			CSycles.session_clear_passes(Client.Id, Id);
			CSycles.session_add_pass(Client.Id, Id, PassType.Combined);
			_passes.Add(PassType.Combined);
		}
	}
}
