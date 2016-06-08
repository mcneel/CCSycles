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

using ccl.Attributes;
using System;

namespace ccl.ShaderNodes
{
	/// <summary>
	/// Base texture node
	/// </summary>
	[ShaderNode("texture_node_base", true)]
	public class TextureNode : ShaderNode
	{
		public enum TextureColorSpace
		{
			None,
			Color,
		}

		public enum TextureProjection
		{
			Flat,
			Box,
			Sphere,
			Tube,
		}

		public enum TextureExtension
		{
			Repeat,
			Extend,
			Clip,
		}

		public enum EnvironmentProjection
		{
			Equirectangular,
			MirrorBall,
			Wallpaper
		}

		internal TextureNode(ShaderNodeType type) :
			base(type) { }

		internal TextureNode(ShaderNodeType type, string name) :
			base(type, name) { }

		/// <summary>
		/// Color space to operate in
		/// </summary>
		public TextureColorSpace ColorSpace { get; set; }
		/// <summary>
		/// EnvironmentTexture texture interpolation
		/// </summary>
		public InterpolationType Interpolation { get; set; }
		/// <summary>
		/// texture extension type.
		/// </summary>
		public TextureExtension Extension { get; set; }
		/// <summary>
		/// Set to true if image data is to be interpreted as linear.
		/// </summary>
		public bool IsLinear { get; set; }
		/// <summary>
		/// Get or set image name
		/// </summary>
		public string Filename { get; set; }
		/// <summary>
		/// Get or set the float data for image. Use for HDR images
		/// </summary>
		public float[] FloatImage { set; get; }
		/// <summary>
		/// Get or set the byte data for image
		/// </summary>
		public byte[] ByteImage { set; get; }
		/// <summary>
		/// Get or set image resolution width
		/// </summary>
		public uint Width { get; set; }
		/// <summary>
		/// Get or set image resolution height
		/// </summary>
		public uint Height { get; set; }

		protected void SetInterpolation(string interp)
		{
			interp = interp.Replace(" ", "_");
			Interpolation = (InterpolationType)Enum.Parse(typeof(InterpolationType), interp, true);
		}
		protected void SetExtension(string extension)
		{
			extension = extension.Replace(" ", "_");
			Extension = (TextureExtension)Enum.Parse(typeof(TextureExtension), extension, true);
		}
		protected void SetColorSpace(string cs)
		{
			ColorSpace = (TextureColorSpace)Enum.Parse(typeof(TextureColorSpace), cs, true);
		}

		protected void ImageParseXml(System.Xml.XmlReader xmlNode)
		{
			var imgsrc = xmlNode.GetAttribute("src");
			if (!string.IsNullOrEmpty(imgsrc) && System.IO.File.Exists(imgsrc))
			{
				using (var bmp = new System.Drawing.Bitmap(imgsrc))
				{
					var l = bmp.Width * bmp.Height * 4;
					var bmpdata = new byte[l];
					for (var x = 0; x < bmp.Width; x++)
					{
						for (var y = 0; y < bmp.Height; y++)
						{
							var pos = y * bmp.Width * 4 + x * 4;
							var pixel = bmp.GetPixel(x, y);
							bmpdata[pos] = pixel.R;
							bmpdata[pos + 1] = pixel.G;
							bmpdata[pos + 2] = pixel.B;
							bmpdata[pos + 3] = pixel.A;
						}
					}
					ByteImage = bmpdata;
					Width = (uint)bmp.Width;
					Height = (uint)bmp.Height;
					Filename = imgsrc;
				}
			}

		}
	}
}
