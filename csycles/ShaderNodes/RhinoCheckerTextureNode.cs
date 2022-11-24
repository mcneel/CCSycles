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
	public class CheckerTextureInputs : Inputs
	{
		public VectorSocket UVW { get; set; }
		public ColorSocket Color1 { get; set; }
		public ColorSocket Color2 { get; set; }

		public CheckerTextureInputs(ShaderNode parentNode)
		{
			UVW = new VectorSocket(parentNode, "UVW");
			AddSocket(UVW);
			Color1 = new ColorSocket(parentNode, "Color1");
			AddSocket(Color1);
			Color2 = new ColorSocket(parentNode, "Color2");
			AddSocket(Color2);
		}
	}

	public class CheckerTextureOutputs : Outputs
	{
		public ColorSocket Color { get; set; }

		public CheckerTextureOutputs(ShaderNode parentNode)
		{
			Color = new ColorSocket(parentNode, "Color");
			AddSocket(Color);
		}
	}

	[ShaderNode("rhino_checker_texture")]
	public class CheckerTextureProceduralNode : ShaderNode
	{
		public CheckerTextureInputs ins => (CheckerTextureInputs)inputs;
		public CheckerTextureOutputs outs => (CheckerTextureOutputs)outputs;

		public CheckerTextureProceduralNode() : this("a checker texture") { }
		public CheckerTextureProceduralNode(string name)
			: base(ShaderNodeType.RhinoCheckerTexture, name)
		{
			inputs = new CheckerTextureInputs(this);
			outputs = new CheckerTextureOutputs(this);
			ins.Color1.Value = new float4(0.0f, 0.0f, 0.0f);
			ins.Color2.Value = new float4(1.0f, 1.0f, 1.0f);
		}

		internal override void SetDirectMembers(uint clientId, uint sceneId, uint shaderId)
		{
		}
	}
}
