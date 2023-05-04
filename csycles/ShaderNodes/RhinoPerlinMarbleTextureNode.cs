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
	public class PerlinMarbleTextureInputs : Inputs
	{
		public VectorSocket UVW { get; set; }
		public ColorSocket Color1 { get; set; }
		public ColorSocket Color2 { get; set; }

		public PerlinMarbleTextureInputs(ShaderNode parentNode)
		{
			UVW = new VectorSocket(parentNode, "UVW");
			AddSocket(UVW);
			Color1 = new ColorSocket(parentNode, "Color1");
			AddSocket(Color1);
			Color2 = new ColorSocket(parentNode, "Color2");
			AddSocket(Color2);
		}
	}

	public class PerlinMarlinTextureOutputs : TwoColorOutputs
	{
		public PerlinMarlinTextureOutputs(ShaderNode parentNode) : base(parentNode)
		{
		}
	}

	[ShaderNode("rhino_perlin_marble_texture")]
	public class PerlinMarbleTextureProceduralNode : ShaderNode
	{
		public PerlinMarbleTextureInputs ins => (PerlinMarbleTextureInputs)inputs;
		public PerlinMarlinTextureOutputs outs => (PerlinMarlinTextureOutputs)outputs;

		public int Levels { get; set; }
		public float Noise { get; set; }
		public float Blur { get; set; }
		public float Size { get; set; }
		public float Color1Saturation { get; set; }
		public float Color2Saturation { get; set; }

		public PerlinMarbleTextureProceduralNode() : this("a perlin marble texture") { }
		public PerlinMarbleTextureProceduralNode(string name)
			: base(ShaderNodeType.RhinoPerlinMarbleTexture, name)
		{
			inputs = new PerlinMarbleTextureInputs(this);
			outputs = new PerlinMarlinTextureOutputs(this);
		}

		internal override void SetDirectMembers(uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_member_int(sceneId, shaderId, Id, Type, "Levels", Levels);
			CSycles.shadernode_set_member_float(sceneId, shaderId, Id, Type, "Noise", Noise);
			CSycles.shadernode_set_member_float(sceneId, shaderId, Id, Type, "Blur", Blur);
			CSycles.shadernode_set_member_float(sceneId, shaderId, Id, Type, "Size", Size);
			CSycles.shadernode_set_member_float(sceneId, shaderId, Id, Type, "Color1Saturation", Color1Saturation);
			CSycles.shadernode_set_member_float(sceneId, shaderId, Id, Type, "Color2Saturation", Color2Saturation);
		}
	}
}
