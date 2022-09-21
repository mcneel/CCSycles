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
	public class TextureAdjustmentTextureInputs : Inputs
	{
		public ColorSocket Color { get; set; }

		public TextureAdjustmentTextureInputs(ShaderNode parentNode)
		{
			Color = new ColorSocket(parentNode, "Color");
			AddSocket(Color);
		}
	}

	public class TextureAdjustmentTextureOutputs : Outputs
	{
		public ColorSocket Color { get; set; }

		public TextureAdjustmentTextureOutputs(ShaderNode parentNode)
		{
			Color = new ColorSocket(parentNode, "Color");
			AddSocket(Color);
		}
	}

	[ShaderNode("rhino_texture_adjustment_texture")]
	public class TextureAdjustmentTextureProceduralNode : ShaderNode
	{
		public TextureAdjustmentTextureInputs ins => (TextureAdjustmentTextureInputs)inputs;
		public TextureAdjustmentTextureOutputs outs => (TextureAdjustmentTextureOutputs)outputs;

		public bool Grayscale { get; set; }
		public bool Invert { get; set; }
		public bool Clamp { get; set; }
		public bool ScaleToClamp { get; set; }
		public float Multiplier { get; set; }
		public float ClampMin { get; set; }
		public float ClampMax { get; set; }
		public float Gain { get; set; }
		public float Gamma { get; set; }
		public float Saturation { get; set; }
		public float HueShift { get; set; }
		public bool IsHdr { get; set; }

		public TextureAdjustmentTextureProceduralNode() : this("a texture adjustment texture") { }
		public TextureAdjustmentTextureProceduralNode(string name)
			: base(ShaderNodeType.RhinoTextureAdjustmentTexture, name)
		{
			inputs = new TextureAdjustmentTextureInputs(this);
			outputs = new TextureAdjustmentTextureOutputs(this);
		}

		internal override void SetDirectMembers(uint clientId, uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_member_bool(clientId, sceneId, shaderId, Id, Type, "Grayscale", Grayscale);
			CSycles.shadernode_set_member_bool(clientId, sceneId, shaderId, Id, Type, "Invert", Invert);
			CSycles.shadernode_set_member_bool(clientId, sceneId, shaderId, Id, Type, "Clamp", Clamp);
			CSycles.shadernode_set_member_bool(clientId, sceneId, shaderId, Id, Type, "ScaleToClamp", ScaleToClamp);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "Multiplier", Multiplier);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "ClampMin", ClampMin);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "ClampMax", ClampMax);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "Gain", Gain);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "Gamma", Gamma);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "Saturation", Saturation);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "HueShift", HueShift);
			CSycles.shadernode_set_member_bool(clientId, sceneId, shaderId, Id, Type, "IsHdr", IsHdr);
		}
	}
}
