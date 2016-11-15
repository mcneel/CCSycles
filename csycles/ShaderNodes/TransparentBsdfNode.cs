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

using System.Xml;
using ccl.ShaderNodes.Sockets;
using ccl.Attributes;

namespace ccl.ShaderNodes
{
	public class TransparentBsdfInputs : Inputs
	{
		public ColorSocket Color { get; set; }

		internal TransparentBsdfInputs(ShaderNode parentNode)
		{
			Color = new ColorSocket(parentNode, "Color");
			AddSocket(Color);
		}
	}

	public class TransparentBsdfOutputs : Outputs
	{
		public ClosureSocket BSDF { get; set; }

		internal TransparentBsdfOutputs(ShaderNode parentNode)
		{
			BSDF = new ClosureSocket(parentNode, "BSDF");
			AddSocket(BSDF);
		}
	}
	
	/// <summary>
	/// A Transparent BSDF closure.

	/// There is one output <c>BSDF</c>
	/// </summary>
	[ShaderNode("transparent_bsdf")]
	public class TransparentBsdfNode : ShaderNode
	{
		public TransparentBsdfInputs ins => (TransparentBsdfInputs)inputs;
		public TransparentBsdfOutputs outs => (TransparentBsdfOutputs)outputs;

		/// <summary>
		/// Create a new Transparent BSDF closure. Default Color is white
		/// </summary>
		public TransparentBsdfNode() : this("a transparent bsdf node") { }
		/// <summary>
		/// Create a new Transparent BSDF closure. Default Color is white
		/// </summary>
		public TransparentBsdfNode(string name) :
			base(ShaderNodeType.Transparent, name)
		{
			inputs = new TransparentBsdfInputs(this);
			outputs = new TransparentBsdfOutputs(this);
			ins.Color.Value = new float4(1.0f, 1.0f, 1.0f, 1.0f);
		}

		internal override void ParseXml(XmlReader xmlNode)
		{
			Utilities.Instance.get_float4(ins.Color, xmlNode.GetAttribute("color"));
		}
	}
}
