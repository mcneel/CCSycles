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

using System.Xml;
using ccl.ShaderNodes.Sockets;
using ccl.Attributes;
using System;

namespace ccl.ShaderNodes
{
	public class SubsurfaceScatteringInputs : Inputs
	{
		public ColorSocket Color { get; set; }
		public FloatSocket Scale { get; set; }
		public FloatSocket Sharpness { get; set; }
		public FloatSocket TextureBlur { get; set; }
		public VectorSocket Radius { get; set; }
		public VectorSocket Normal { get; set; }

		public SubsurfaceScatteringInputs(ShaderNode parentNode)
		{
			Color = new ColorSocket(parentNode, "Color");
			AddSocket(Color);
			Scale = new FloatSocket(parentNode, "Scale");
			AddSocket(Scale);
			Sharpness = new FloatSocket(parentNode, "Sharpness");
			AddSocket(Sharpness);
			TextureBlur = new FloatSocket(parentNode, "Texture Blur");
			AddSocket(TextureBlur);
			Radius = new VectorSocket(parentNode, "Radius");
			AddSocket(Radius);
			Normal = new VectorSocket(parentNode, "Normal");
			AddSocket(Normal);
		}
	}

	public class SubsurfaceScatteringOutputs : Outputs
	{
		public ClosureSocket BSSRDF { get; set; }

		public SubsurfaceScatteringOutputs(ShaderNode parentNode)
		{
			BSSRDF = new ClosureSocket(parentNode, "BSSRDF");
			AddSocket(BSSRDF);
		}
	}

	/// <summary>
	/// A scatter volume node.
	/// </summary>
	[ShaderNode("subsurface_scattering")]
	public class SubsurfaceScatteringNode : ShaderNode
	{
		public enum FalloffTypes
		{
			Cubic = 40,
			Gaussian = 41,
			Principled = 42,
			Burley = 43,
			RandomWalk = 44,
			PrincipledRandomWalk = 45
		}


		static public int IntFromSssMethod(string m)
		{
			if (m == "Cubic") return 40;
			if (m == "Gaussian") return 41;
			if (m == "Principled") return 42;
			if (m == "Burley") return 43;
			if (m == "RandomWalk") return 44;
			if (m == "PrincipledRandomWalk") return 45;

			return 43;
		}

		static public string SssMethodFromInt(int m)
		{
			if (m == 40) return "Cubic";
			if (m == 41) return "Gaussian";
			if (m == 42) return "Principled";
			if (m == 43) return "Burley";
			if (m == 44) return "RandomWalk";
			if (m == 45) return "PrincipledRandomWalk";

			return "Burley";
		}

		static public FalloffTypes SssEnumFromInt(int m)
		{
			var falloff = FalloffTypes.Burley;
			var falloffstr = SssMethodFromInt(m);
			if (!string.IsNullOrEmpty(falloffstr)) {
				if (Enum.TryParse(falloffstr, out FalloffTypes ft)) {
					falloff = ft;
				}
			}
			return falloff;
		}

		public SubsurfaceScatteringInputs ins => (SubsurfaceScatteringInputs)inputs;
		public SubsurfaceScatteringOutputs outs => (SubsurfaceScatteringOutputs)outputs;

		/// <summary>
		/// Create a new Scatter volume node
		/// </summary>
		public SubsurfaceScatteringNode() : this("a subsurface scattering node") { }
		public SubsurfaceScatteringNode(string name) :
			base(ShaderNodeType.SubsurfaceScattering, name)
		{
			inputs = new SubsurfaceScatteringInputs(this);
			outputs = new SubsurfaceScatteringOutputs(this);
			ins.Color.Value = new float4(1.0f);
			ins.Scale.Value = 0.01f;
			ins.Sharpness.Value = 0.0f;
			ins.TextureBlur.Value = 0.0f;
			ins.Radius.Value = new float4(1.0f);
			ins.Normal.Value = new float4(0.0f);
			Falloff = FalloffTypes.Burley;
		}

		public FalloffTypes Falloff { get; set; }

		internal override void SetEnums(uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_enum(sceneId, shaderId, Id, Type, "falloff", (int)Falloff);
		}

		internal override void ParseXml(XmlReader xmlNode)
		{
			Utilities.Instance.get_float4(ins.Color, xmlNode.GetAttribute("color"));
			Utilities.Instance.get_float(ins.Scale, xmlNode.GetAttribute("scale"));
			Utilities.Instance.get_float(ins.Sharpness, xmlNode.GetAttribute("sharpness"));
			Utilities.Instance.get_float(ins.TextureBlur, xmlNode.GetAttribute("texture_blur"));

			var falloff = xmlNode.GetAttribute("falloff");
			if(!string.IsNullOrEmpty(falloff))
			{
				FalloffTypes ft = FalloffTypes.Cubic;
				if(Enum.TryParse(falloff, out ft))
				{
					Falloff = ft;
				}
			}
		}
	}
}
