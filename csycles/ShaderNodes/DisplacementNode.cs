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

namespace ccl.ShaderNodes
{
	public class DisplacementInputs : Inputs
	{
		public FloatSocket Height { get; set; }
		public FloatSocket Midlevel { get; set; }
		public FloatSocket Scale { get; set; }
		public VectorSocket Normal { get; set; }

		public DisplacementInputs(ShaderNode parentNode)
		{
			Height = new FloatSocket(parentNode, "Height");
			AddSocket(Height);
			Midlevel = new FloatSocket(parentNode, "Midlevel");
			AddSocket(Midlevel);
			Scale = new FloatSocket(parentNode, "Scale");
			AddSocket(Scale);
			Normal = new VectorSocket(parentNode, "Normal");
			AddSocket(Normal);
		}
	}

	public class DisplacementOutputs : Outputs
	{
		public VectorSocket Displacement { get; set; }

		public DisplacementOutputs(ShaderNode parentNode)
		{
			Displacement = new VectorSocket(parentNode, "Displacement");
			AddSocket(Displacement);
		}
	}
	
	/// <summary>
	/// A Displacement node.
	/// 
	/// There is one output <c>Displacement</c>. This needs to be
	/// connected to the OutputNode.ins.Displacement input
	/// </summary>
	[ShaderNode("displacement")]
	public class DisplacementNode : ShaderNode
	{
		public DisplacementInputs ins => (DisplacementInputs)inputs;
		public DisplacementOutputs outs => (DisplacementOutputs)outputs;

		/// <summary>
		/// Create a new Diffuse Displacement closure.
		/// </summary>
		public DisplacementNode() : this("a displacement node") { }
		public DisplacementNode(string name) :
			base(ShaderNodeType.Displacement, name)
		{
			inputs = new DisplacementInputs(this);
			outputs = new DisplacementOutputs(this);
			ins.Height.Value = 1.0f;
			ins.Midlevel.Value = 0.5f;
			ins.Scale.Value = 1.0f;
		}

		internal override void ParseXml(XmlReader xmlNode)
		{
			Utilities.Instance.get_float(ins.Height, xmlNode.GetAttribute("height"));
			Utilities.Instance.get_float(ins.Midlevel, xmlNode.GetAttribute("midlevel"));
			Utilities.Instance.get_float(ins.Scale, xmlNode.GetAttribute("scale"));
		}
	}
}
