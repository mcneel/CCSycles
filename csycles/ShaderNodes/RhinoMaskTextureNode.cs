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

using ccl.ShaderNodes.Sockets;
using ccl.Attributes;

namespace ccl.ShaderNodes
{
	public class MaskTextureInputs : Inputs
	{
		public ColorSocket Color { get; set; }
		public FloatSocket Alpha { get; set; }

		public MaskTextureInputs(ShaderNode parentNode)
		{
			Color = new ColorSocket(parentNode, "Color");
			AddSocket(Color);
			Alpha = new FloatSocket(parentNode, "Alpha");
			AddSocket(Alpha);
		}
	}

	public class MaskTextureOutputs : TwoColorOutputs
	{
		public MaskTextureOutputs(ShaderNode parentNode) : base(parentNode)
		{
		}
	}

	[ShaderNode("rhino_mask_texture")]
	public class MaskTextureProceduralNode : ShaderNode
	{
		public enum MaskTypes
		{
			LUMINANCE,
			RED,
			GREEN,
			BLUE,
			ALPHA,
		};

		public MaskTextureInputs ins => (MaskTextureInputs)inputs;
		public MaskTextureOutputs outs => (MaskTextureOutputs)outputs;

		public MaskTypes MaskType { get; set; }

		public MaskTextureProceduralNode() : this("a mask texture") { }
		public MaskTextureProceduralNode(string name)
			: base(ShaderNodeType.RhinoMaskTexture, name)
		{
			inputs = new MaskTextureInputs(this);
			outputs = new MaskTextureOutputs(this);
		}

		internal override void SetDirectMembers(uint clientId, uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_member_int(clientId, sceneId, shaderId, Id, Type, "MaskType", (int)MaskType);
		}
	}
}
