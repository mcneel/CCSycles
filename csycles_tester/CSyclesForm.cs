using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ef = Eto.Forms;
using ed = Eto.Drawing;
using ccl;
using System.Drawing;
using System.IO;

namespace csycles_tester
{

	internal class RendererModel : INotifyPropertyChanged
	{
		public Client Client { get; private set; }
		public RendererModel()
		{
			Client = new Client();
		}

		ed.Bitmap bitmap;
		public ed.Bitmap Result
		{
			get { return bitmap; }
			set
			{
				if(bitmap != value)
				{
					bitmap = value;
					OnPropertyChanged();
				}
			}
		}

		void OnPropertyChanged([CallerMemberName] string memberName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
		public event PropertyChangedEventHandler PropertyChanged;

		const uint samples = 50;
		Random r = new Random();
		private static CSycles.RenderTileCallback g_write_render_tile_callback;
		private static CSycles.RenderTileCallback g_update_render_tile_callback;
		public void WriteRenderTileCallback(uint sessionId, uint tx, uint ty, uint tw, uint th, uint sample, uint depth, PassType passtype, float[] px, int len)
		{
			if (passtype != PassType.Combined) return;
			if (sample < samples) return;
			Console.WriteLine($"C# Write Render Tile sample {sample} for session {sessionId} at ({tx},{ty}x{tw},{th}) [{depth}] {width}x{height}");

			for (var x = 0; x < (int)tw; x++)
			{
				for (var y = 0; y < (int)th; y++)
				{
					var i = y * tw * depth + x * depth;
					var ti = (ty + y) * width * depth + (tx + x) * depth;
					pixels[ti] = px[i];
					pixels[ti + 1] = px[i + 1];
					pixels[ti + 2] = px[i + 2];
					pixels[ti + 3] = px[i + 3];
				}
			}
		}
		private static CSycles.UpdateCallback g_update_callback;
		public void StatusUpdateCallback(uint sessionId)
		{
			float progress;

			CSycles.progress_get_progress(Client.Id, sessionId, out progress);
			var status = CSycles.progress_get_status(Client.Id, sessionId);
			var substatus = CSycles.progress_get_substatus(Client.Id, sessionId);
			uint samples;
			uint num_samples;

			CSycles.tilemanager_get_sample_info(Client.Id, sessionId, out samples, out num_samples);

			if (status.Equals("Finished"))
			{
				Console.WriteLine("wohoo... :D");
			}

			status = "[" + status + "]";
			if (!substatus.Equals(string.Empty)) status = status + ": " + substatus;
			Console.WriteLine("C# status update: {0} {1} {2} {3} <|> {4:P}", CSycles.progress_get_sample(Client.Id, sessionId), status, samples, num_samples, progress);
		}


		float[] pixels;
		int height;
		int width;

		public void RenderScene(string scenename)
		{
			var dev = Device.FirstGpu;
			Console.WriteLine("Using device {0} {1}", dev.Name, dev.Description);

			Size tilesize = dev.IsGpu ? new Size(512, 512) : new Size(64, 64);

			var session_params = new SessionParameters(Client, dev)
			{
				Experimental = false,
				Samples = (int) samples,
				TileSize = tilesize,
				TileOrder = TileOrder.Center,
				Threads = 0,
				ShadingSystem = ShadingSystem.SVM,
				Background = true,
				DisplayBufferLinear = false,
				ProgressiveRefine = true,
				Progressive = true,
				PixelSize = 1
			};
			var Session = new Session(Client, session_params);

			g_write_render_tile_callback = WriteRenderTileCallback;
			g_update_callback = StatusUpdateCallback;

			Session.UpdateCallback = g_update_callback;
			Session.UpdateTileCallback = null;
			Session.WriteTileCallback = g_write_render_tile_callback;

			var scene_params = new SceneParameters(Client, ShadingSystem.SVM, BvhType.Static, false, false, false);
			var scene = new Scene(Client, scene_params, Session);
			Session.Scene = scene;

			var xml = new CSyclesXmlReader(Client, scenename);
			xml.Parse(false);
			width = scene.Camera.Size.Width;
			height = scene.Camera.Size.Height;

			pixels = new float[width * height * 4];
			Session.PrepareRun();
			Session.Reset((uint)width, (uint)height, samples);

			bool stillRendering = true;
			while(stillRendering) {
				stillRendering = Session.Sample() > -1;
			}

			Session.EndRun();

			var bmp = new ed.Bitmap((int)width, (int)height, Eto.Drawing.PixelFormat.Format32bppRgba);
			for (var x = 0; x < width; x++)
			{
				for (var y = 0; y < height; y++)
				{
					var i = y * (int)width * 4 + x * 4;
					bmp.SetPixel(x, y, new ed.Color(Math.Min(pixels[i], 1.0f), Math.Min(pixels[i + 1], 1.0f), Math.Min(pixels[i + 2], 1.0f), Math.Min(pixels[i + 3], 1.0f)));
				}
			}
			bmp.Save("test.png", Eto.Drawing.ImageFormat.Png);

			Result = bmp;

			Session.Destroy();

			Console.WriteLine("Cleaning up :)");

		}
	}
	public class CSyclesForm : ef.Form
	{

		public ef.ImageView Image { get; set; }

		public string Path { get; set; }

		public CSyclesForm(string path)
		{
			ClientSize = new Eto.Drawing.Size(500, 500);
			Title = "CSycles Tester";
			Path = path;

			Image = new ef.ImageView();
			var layout = new ef.TableLayout();
			layout.Rows.Add(
				new ef.TableRow(
					Image
					)
				);

			var scenes = Directory.EnumerateFiles(path, "scene*.xml");
			Menu = new ef.MenuBar();
			var scenesmenu = Menu.Items.GetSubmenu("scenes");
			foreach(var sf in scenes)
			{
				scenesmenu.Items.Add(new RenderModalCommand(this, sf));
			}

			Content = layout;

			var m = new RendererModel();
			DataContext = m;
		}
	}
}
