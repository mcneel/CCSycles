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
	public class FbmTextureInputs : TwoColorInputs
	{
		public VectorSocket UVW { get; set; }

		public FbmTextureInputs(ShaderNode parentNode) : base(parentNode)
		{
			UVW = new VectorSocket(parentNode, "UVW");
			AddSocket(UVW);
		}
	}

	public class FbmTextureOutputs : TwoColorOutputs
	{
		public FbmTextureOutputs(ShaderNode parentNode) : base(parentNode)
		{
		}
	}

	[ShaderNode("rhino_fbm_texture")]
	public class FbmTextureProceduralNode : ShaderNode
	{
		public FbmTextureInputs ins => (FbmTextureInputs)inputs;
		public FbmTextureOutputs outs => (FbmTextureOutputs)outputs;

		public bool IsTurbulent { get; set; }
		public int MaxOctaves { get; set; }
		public float Gain { get; set; }
		public float Roughness { get; set; }

		public FbmTextureProceduralNode() : this("an fbm texture") { }
		public FbmTextureProceduralNode(string name)
			: base(ShaderNodeType.RhinoFbmTexture, name)
		{
			inputs = new FbmTextureInputs(this);
			outputs = new FbmTextureOutputs(this);
		}

		internal override void SetDirectMembers(uint clientId, uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_member_bool(clientId, sceneId, shaderId, Id, Type, "IsTurbulent", IsTurbulent);
			CSycles.shadernode_set_member_int(clientId, sceneId, shaderId, Id, Type, "MaxOctaves", MaxOctaves);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "Gain", Gain);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "Roughness", Roughness);
		}
	}
}
