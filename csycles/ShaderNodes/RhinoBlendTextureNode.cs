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

using ccl.ShaderNodes.Sockets;
using ccl.Attributes;

namespace ccl.ShaderNodes
{
	public class BlendTextureInputs : TwoColorInputs
	{
		public VectorSocket UVW { get; set; }
		public ColorSocket BlendColor { get; set; }

		public BlendTextureInputs(ShaderNode parentNode) : base(parentNode)
		{
			UVW = new VectorSocket(parentNode, "UVW");
			AddSocket(UVW);
			BlendColor = new ColorSocket(parentNode, "BlendColor");
			AddSocket(BlendColor);
		}
	}

	public class BlendTextureOutputs : TwoColorOutputs
	{
		public BlendTextureOutputs(ShaderNode parentNode) : base(parentNode)
		{
		}
	}

	[ShaderNode("rhino_blend_texture")]
	public class BlendTextureProceduralNode : ShaderNode
	{
		public BlendTextureInputs ins => (BlendTextureInputs)inputs;
		public BlendTextureOutputs outs => (BlendTextureOutputs)outputs;

		public bool UseBlendColor { get; set; }
		public float BlendFactor { get; set; }

		public BlendTextureProceduralNode() : this("a blend texture") { }
		public BlendTextureProceduralNode(string name)
			: base(ShaderNodeType.RhinoBlendTexture, name)
		{
			inputs = new BlendTextureInputs(this);
			outputs = new BlendTextureOutputs(this);
		}

		internal override void SetDirectMembers(uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_member_bool(sceneId, shaderId, Id, Type, "UseBlendColor", UseBlendColor);
			CSycles.shadernode_set_member_float(sceneId, shaderId, Id, Type, "BlendFactor", BlendFactor);
		}
	}
}
