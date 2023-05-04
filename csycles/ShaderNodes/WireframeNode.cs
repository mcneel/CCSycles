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
	public class WireframeInputs : Inputs
	{
		public FloatSocket Size { get; set; }

		internal WireframeInputs(ShaderNode parentNode)
		{
			Size = new FloatSocket(parentNode, "Size");
			AddSocket(Size);
		}
	}

	public class WireframeOutputs : Outputs
	{
		public FloatSocket Fac { get; set; }

		internal WireframeOutputs(ShaderNode parentNode)
		{
			Fac = new FloatSocket(parentNode, "Fac");
			AddSocket(Fac);
		}
	}

	[ShaderNode("wireframe")]
	public class WireframeNode : ShaderNode
	{
		public WireframeInputs ins => (WireframeInputs)inputs;
		public WireframeOutputs outs => (WireframeOutputs)outputs;

		public WireframeNode() : this("a wireframe node") { }
		public WireframeNode(string name)
			: base(ShaderNodeType.Wireframe, name)
		{
			inputs = new WireframeInputs(this);
			outputs = new WireframeOutputs(this);

			ins.Size.Value = 0.01f;
		}

		public bool UsePixelSize { get; set; } = false;

		internal override void SetDirectMembers(uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_member_bool(sceneId, shaderId, Id, Type, "usepixelsize", UsePixelSize);
			base.SetDirectMembers(sceneId, shaderId);
		}

		internal override void ParseXml(XmlReader xmlNode)
		{
			Utilities.Instance.get_float(ins.Size, xmlNode.GetAttribute("size"));
		}
		public override string CreateXmlAttributes()
		{
			var code = new StringBuilder($" usepixelsize=\"{UsePixelSize}\" ", 1024);

			return code.ToString();
		}

		public override string CreateCodeAttributes()
		{
			var code = new StringBuilder($"{VariableName}.UsePixelSize = {UsePixelSize};", 1024);

			return code.ToString();
		}
	}
}
