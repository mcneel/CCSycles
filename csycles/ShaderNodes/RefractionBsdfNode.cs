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
using System.Xml;
using ccl.ShaderNodes.Sockets;
using ccl.Attributes;
using System.Text;

namespace ccl.ShaderNodes
{
	public class RefractionBsdfInputs : Inputs
	{
		public FloatSocket IOR { get; set; }
		public FloatSocket Roughness { get; set; }
		public ColorSocket Color { get; set; }
		public VectorSocket Normal { get; set; }

		internal RefractionBsdfInputs(ShaderNode parentNode)
		{
			Color = new ColorSocket(parentNode, "Color");
			AddSocket(Color);
			Roughness = new FloatSocket(parentNode, "Roughness");
			AddSocket(Roughness);
			IOR = new FloatSocket(parentNode, "IOR");
			AddSocket(IOR);
			Normal = new VectorSocket(parentNode, "Normal");
			AddSocket(Normal);
		}
	}

	public class RefractionBsdfOutputs : Outputs
	{
		public ClosureSocket BSDF { get; set; }

		internal RefractionBsdfOutputs(ShaderNode parentNode)
		{
			BSDF = new ClosureSocket(parentNode, "BSDF");
			AddSocket(BSDF);
		}
	}

	[ShaderNode("refraction_bsdf")]
	public class RefractionBsdfNode : ShaderNode
	{
		public enum RefractionDistribution
		{
			Sharp = 27,
			Beckmann = 28,
			GGX = 29
		}
		public RefractionBsdfInputs ins => (RefractionBsdfInputs)inputs;
		public RefractionBsdfOutputs outs => (RefractionBsdfOutputs)outputs;

		public RefractionBsdfNode() : this("a refraction bsdf node") { }
		public RefractionBsdfNode(string name) :
			base(ShaderNodeType.Refraction, name)
		{
			inputs = new RefractionBsdfInputs(this);
			outputs = new RefractionBsdfOutputs(this);

			Distribution = RefractionDistribution.GGX;
			ins.IOR.Value = 1.45f;
			ins.Roughness.Value = 0.0f;
			ins.Color.Value = new float4(0.8f);
		}

		public RefractionDistribution Distribution { get; set; }

		public void SetDistribution(string dist)
		{
			Distribution = (RefractionDistribution) Enum.Parse(typeof (RefractionDistribution), dist, true);
		}

		internal override void SetEnums(IntPtr sessionId, IntPtr shaderId)
		{
			CSycles.shadernode_set_enum(sessionId, shaderId, Id, Type, "distribution", (int)Distribution);
		}

		internal override void ParseXml(XmlReader xmlNode)
		{
			Utilities.Instance.get_float4(ins.Color, xmlNode.GetAttribute("color"));
			Utilities.Instance.get_float(ins.IOR, xmlNode.GetAttribute("ior"));
			Utilities.Instance.get_float(ins.Roughness, xmlNode.GetAttribute("roughness"));
			var distribution = xmlNode.GetAttribute("distribution");
			if (!string.IsNullOrEmpty(distribution))
			{
				SetDistribution(distribution);
			}
		}
		public override string CreateXmlAttributes()
		{
			var codeattr = new StringBuilder(1024);

			codeattr.Append($" distribution=\"{Distribution.ToString()}\" ");

			return codeattr.ToString();
		}

		public override string CreateCodeAttributes()
		{
			var codeattr = new StringBuilder(1024);

			codeattr.Append($" {VariableName}.Distribution = RefractionBsdfNode.RefractionDistribution.{Distribution.ToString()};");

			return codeattr.ToString();
		}
	}
}
