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
using ccl.ShaderNodes.Sockets;
using ccl.Attributes;
using System.Xml;
using System.Drawing;
using System.Text;

namespace ccl.ShaderNodes
{
	/// <summary>
	/// EnvironmentTextureNode input sockets
	/// </summary>
	public class EnvironmentTextureInputs : Inputs
	{
		/// <summary>
		/// EnvironmentTextureNode vector input 
		/// </summary>
		public VectorSocket Vector { get; set; }

		internal EnvironmentTextureInputs(ShaderNode parentNode)
		{
			Vector = new VectorSocket(parentNode, "Vector");
			AddSocket(Vector);
		}
	}

	/// <summary>
	/// EnvironmentTextureNode output sockets
	/// </summary>
	public class EnvironmentTextureOutputs : Outputs
	{
		/// <summary>
		/// EnvironmentTextureNode color output
		/// </summary>
		public ColorSocket Color { get; set; }
		/// <summary>
		/// EnvironmentTextureNode alpha output
		/// </summary>
		public FloatSocket Alpha { get; set; }

		internal EnvironmentTextureOutputs(ShaderNode parentNode)
		{
			Color = new ColorSocket(parentNode, "Color");
			AddSocket(Color);
			Alpha = new FloatSocket(parentNode, "Alpha");
			AddSocket(Alpha);
		}
	}

	/// <summary>
	/// EnvironmentTextureNode
	/// </summary>
	[ShaderNode("environment_texture")]
	public class EnvironmentTextureNode : TextureNode
	{
		/// <summary>
		/// EnvironmentTextureNode input sockets
		/// </summary>
		public EnvironmentTextureInputs ins => (EnvironmentTextureInputs)inputs;

		/// <summary>
		/// EnvironmentTextureNode output sockets
		/// </summary>
		public EnvironmentTextureOutputs outs => (EnvironmentTextureOutputs)outputs;

		public EnvironmentTextureNode() : this("an env texture node") { }

		/// <summary>
		/// Create an EnvironmentTextureNode
		/// </summary>
		public EnvironmentTextureNode(string name) :
			base(ShaderNodeType.EnvironmentTexture, name)
		{

			inputs = new EnvironmentTextureInputs(this);
			outputs = new EnvironmentTextureOutputs(this);

			Interpolation = InterpolationType.Linear;
			ColorSpace = TextureColorSpace.None;
			Projection = EnvironmentProjection.Equirectangular;
		}
		/// <summary>
		/// Get or set environment projection
		/// </summary>
		public EnvironmentProjection Projection { get; set; }
		private void SetProjection(string projection)
		{
			projection = projection.Replace(" ", "_");
			Projection = (EnvironmentProjection)Enum.Parse(typeof(EnvironmentProjection), projection, true);
		}

		private string GetProjectionString(EnvironmentProjection projection)
		{
			var proj = "";
			switch (projection)
			{
				case EnvironmentProjection.Equirectangular:
					proj = "Equirectangular";
					break;
				case EnvironmentProjection.MirrorBall:
					proj = "Mirror Ball";
					break;
				case EnvironmentProjection.Wallpaper:
					proj = "Wallpaper";
					break;
				default:
					proj = "Equirectangular";
					break;
			}
			return proj;
		}

		internal override void SetEnums(uint clientId, uint shaderId)
		{
			var projection = GetProjectionString(Projection);
			CSycles.shadernode_set_enum(clientId, shaderId, Id, Type, "projection", (int)Projection);
			CSycles.shadernode_set_enum(clientId, shaderId, Id, Type, "color_space", (int)ColorSpace);
			CSycles.shadernode_set_enum(clientId, shaderId, Id, Type, "interpolation", (int)Interpolation);
		}

		internal override void SetDirectMembers(uint clientId, uint shaderId)
		{
			CSycles.shadernode_set_member_bool(clientId, shaderId, Id, Type, "is_linear", IsLinear);
			if (FloatImage != null)
			{
				var flimg = FloatImage;
				CSycles.shadernode_set_member_float_img(clientId, shaderId, Id, Type, "builtin-data", Filename ?? String.Format("{0}-{0}-{0}", clientId, shaderId, Id), ref flimg, Width, Height, 1, 4);
			}
			else if (ByteImage != null)
			{
				var bimg = ByteImage;
				CSycles.shadernode_set_member_byte_img(clientId, shaderId, Id, Type, "builtin-data", Filename ?? String.Format("{0}-{0}-{0}", clientId, shaderId, Id), ref bimg, Width, Height, 1, 4);
			}
		}

		internal override void ParseXml(XmlReader xmlNode)
		{
			var cs = xmlNode.GetAttribute("color_space");
			if (!string.IsNullOrEmpty(cs))
			{
				SetColorSpace(cs);
			}
			var projection = xmlNode.GetAttribute("projection");
			if (!string.IsNullOrEmpty(projection))
			{
				SetProjection(projection);
			}
			var interpolation = xmlNode.GetAttribute("interpolation");
			if (!string.IsNullOrEmpty(interpolation))
			{
				SetInterpolation(interpolation);
			}
			ImageParseXml(xmlNode);
		}


		public override string CreateXmlAttributes()
		{
			var code = new StringBuilder($" projection=\"{Projection}\" ", 1024);
			code.Append($" color_space=\"{ColorSpace}\"");
			code.Append($" extension=\"{Extension}\"");
			code.Append($" interpolation=\"{Interpolation}\"");
			code.Append($" is_linear=\"{IsLinear}\"");

			return code.ToString();
		}
		public override string CreateCodeAttributes()
		{
			var code = $"{VariableName}.Projection = {Projection};";
			code += $"{VariableName}.ColorSpace = {ColorSpace};";
			code += $"{VariableName}.Extension = {Extension};";
			code += $"{VariableName}.Interpolation = {Interpolation};";
			code += $"{VariableName}.IsLinear = {IsLinear};";

			return code;
		}
	}
}
