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
	public class CheckerTexture2dInputs : Inputs
	{
		public VectorSocket UVW { get; set; }
		public ColorSocket Color1 { get; set; }
		public ColorSocket Color2 { get; set; }

		public CheckerTexture2dInputs(ShaderNode parentNode)
		{
			UVW = new VectorSocket(parentNode, "UVW");
			AddSocket(UVW);
			Color1 = new ColorSocket(parentNode, "Color1");
			AddSocket(Color1);
			Color2 = new ColorSocket(parentNode, "Color2");
			AddSocket(Color2);
		}
	}

	public class CheckerTexture2dOutputs : Outputs
	{
		public VectorSocket Color { get; set; }

		public CheckerTexture2dOutputs(ShaderNode parentNode)
		{
			Color = new VectorSocket(parentNode, "Color");
			AddSocket(Color);
		}
	}

	[ShaderNode("rhino_checker_texture_2d")]
	public class CheckerTexture2dProceduralNode : ShaderNode
	{
		public CheckerTexture2dInputs ins => (CheckerTexture2dInputs)inputs;
		public CheckerTexture2dOutputs outs => (CheckerTexture2dOutputs)outputs;

		public Transform UvwTransform { get; set; } = Transform.Identity();

		public CheckerTexture2dProceduralNode() : this("a checker texture 2d") { }
		public CheckerTexture2dProceduralNode(string name)
			: base(ShaderNodeType.RhinoCheckerTexture2d, name)
		{
			inputs = new CheckerTexture2dInputs(this);
			outputs = new CheckerTexture2dOutputs(this);
			ins.Color1.Value = new float4(0.0f, 0.0f, 0.0f);
			ins.Color2.Value = new float4(1.0f, 1.0f, 1.0f);
		}

		internal override void SetDirectMembers(uint clientId, uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, Type, "UvwTransform", UvwTransform[0].x, UvwTransform[0].y, UvwTransform[0].z, UvwTransform[0].w, 0);
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, Type, "UvwTransform", UvwTransform[1].x, UvwTransform[1].y, UvwTransform[1].z, UvwTransform[1].w, 1);
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, Type, "UvwTransform", UvwTransform[2].x, UvwTransform[2].y, UvwTransform[2].z, UvwTransform[2].w, 2);
		}
	}
}
