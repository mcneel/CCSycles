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
using System.Text;

namespace ccl.ShaderNodes
{
	public class NormalMapInputs : Inputs
	{
		public ColorSocket Color { get; set; }
		public FloatSocket Strength { get; set; }

		internal NormalMapInputs(ShaderNode parentNode)
		{
			Strength = new FloatSocket(parentNode, "Strength");
			AddSocket(Strength);
			Color = new ColorSocket(parentNode, "Color");
			AddSocket(Color);
		}
	}

	public class NormalMapOutputs : Outputs
	{
		public VectorSocket Normal { get; set; }

		internal NormalMapOutputs(ShaderNode parentNode)
		{
			Normal = new VectorSocket(parentNode, "Normal");
			AddSocket(Normal);
		}
	}

	[ShaderNode("emission")]
	public class NormalMapNode : ShaderNode
	{
		public enum Space
		{
			Tangent,
			Object,
			World,
			/*BlenderObject,
			BlenderWorld,*/
		}
		public NormalMapInputs ins => (NormalMapInputs)inputs;
		public NormalMapOutputs outs => (NormalMapOutputs)outputs;

		public NormalMapNode() : this("a normalmap node") { }
		public NormalMapNode(string name)
			: base(ShaderNodeType.NormalMap, name)
		{
			inputs = new NormalMapInputs(this);
			outputs = new NormalMapOutputs(this);

			ins.Color.Value = new float4(0.8f);
			ins.Strength.Value = 1.0f;
		}

		public Space SpaceType { get; set; } = Space.Tangent;

		internal override void SetEnums(uint clientId, uint shaderId)
		{
			CSycles.shadernode_set_enum(clientId, shaderId, Id, Type, "type", (int)SpaceType);
		}

		internal override void ParseXml(XmlReader xmlNode)
		{
			Utilities.Instance.get_float4(ins.Color, xmlNode.GetAttribute("color"));
			Utilities.Instance.get_float(ins.Strength, xmlNode.GetAttribute("strength"));
		}
		public override string CreateXmlAttributes()
		{
			var code = new StringBuilder($" type=\"{SpaceType}\" ", 1024);

			return code.ToString();
		}

		public override string CreateCodeAttributes()
		{
			var code = new StringBuilder($"{VariableName}.SpaceType = ccl.ShaderNodes.NormalMapNode.Space.{SpaceType};", 1024);

			return code.ToString();
		}
	}
}
