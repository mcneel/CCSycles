﻿/**
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

namespace ccl.ShaderNodes
{
	public class GlossyInputs : Inputs
	{
		public ColorSocket Color { get; set; }
		public FloatSocket Roughness { get; set; }
		public VectorSocket Normal { get; set; }

		internal GlossyInputs(ShaderNode parentNode)
		{
			Color = new ColorSocket(parentNode, "Color");
			AddSocket(Color);
			Roughness = new FloatSocket(parentNode, "Roughness");
			AddSocket(Roughness);
			Normal = new VectorSocket(parentNode, "Normal");
			AddSocket(Normal);
		}
	}

	public class GlossyOutputs : Outputs
	{
		public ClosureSocket BSDF { get; set; }

		internal GlossyOutputs(ShaderNode parentNode)
		{
			BSDF = new ClosureSocket(parentNode, "BSDF");
			AddSocket(BSDF);
		}
	}

	[ShaderNode("glossy_bsdf")]
	public class GlossyBsdfNode : ShaderNode
	{

		public enum GlossyDistribution
		{
			Sharp = 8,
			Beckmann = 12,
			GGX = 9,
			Asihkmin_Shirley = 15,
			Multiscatter_GGX = 13
		}

		public GlossyInputs ins => (GlossyInputs)inputs;
		public GlossyOutputs outs => (GlossyOutputs)outputs;

		public GlossyBsdfNode() : this("a glossy bsdf node") { }
		public GlossyBsdfNode(string name) :
			base(ShaderNodeType.Glossy, name)
		{
			inputs = new GlossyInputs(this);
			outputs = new GlossyOutputs(this);
			Distribution = GlossyDistribution.Multiscatter_GGX;
			ins.Color.Value = new float4();
			ins.Roughness.Value = 0.0f;
		}

		public GlossyDistribution Distribution { get; set; }

		public void SetDistribution(string dist)
		{
			dist = dist.Replace("-", "_");
			Distribution = (GlossyDistribution)Enum.Parse(typeof(GlossyDistribution), dist, true);
		}

		private string GlossyToString(GlossyDistribution dist)
		{
			var str = dist.ToString();
			str = str.Replace("_", "-");
			return str;
		}

		internal override void SetEnums(uint clientId, uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_enum(clientId, sceneId, shaderId, Id, Type, "distribution", (int)Distribution);
		}

		internal override void ParseXml(XmlReader xmlNode)
		{
			Utilities.Instance.get_float4(ins.Color, xmlNode.GetAttribute("color"));
			Utilities.Instance.get_float4(ins.Normal, xmlNode.GetAttribute("normal"));
			Utilities.Instance.get_float(ins.Roughness, xmlNode.GetAttribute("roughness"));
			var glossydistribution = xmlNode.GetAttribute("distribution");
			if (!string.IsNullOrEmpty(glossydistribution))
			{
				SetDistribution(glossydistribution);
			}
		}
	}
}
