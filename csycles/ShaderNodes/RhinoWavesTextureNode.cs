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
	public class WavesTextureInputs : Inputs
	{
		public VectorSocket UVW { get; set; }
		public ColorSocket Color1 { get; set; }
		public ColorSocket Color2 { get; set; }
		public ColorSocket Color3 { get; set; }

		public WavesTextureInputs(ShaderNode parentNode)
		{
			UVW = new VectorSocket(parentNode, "UVW");
			AddSocket(UVW);
			Color1 = new ColorSocket(parentNode, "Color1");
			AddSocket(Color1);
			Color2 = new ColorSocket(parentNode, "Color2");
			AddSocket(Color2);
			Color3 = new ColorSocket(parentNode, "Color3");
			AddSocket(Color3);
		}
	}

	public class WavesTextureOutputs : Outputs
	{
		public ColorSocket Color { get; set; }

		public WavesTextureOutputs(ShaderNode parentNode)
		{
			Color = new ColorSocket(parentNode, "Color");
			AddSocket(Color);
		}
	}

	[ShaderNode("rhino_waves_texture")]
	public class WavesTextureProceduralNode : ShaderNode
	{
		public enum WaveTypes
		{
			LINEAR,
			RADIAL,
		};

		public WavesTextureInputs ins => (WavesTextureInputs)inputs;
		public WavesTextureOutputs outs => (WavesTextureOutputs)outputs;

		public Transform UvwTransform { get; set; } = Transform.Identity();
		public WaveTypes WaveType { get; set; } = WaveTypes.LINEAR;
		public float WaveWidth { get; set; } = 0.5f;
		public bool WaveWidthTextureOn { get; set; } = false;
		public float Contrast1 { get; set; } = 1.0f;
		public float Contrast2 { get; set; } = 0.5f;

		public WavesTextureProceduralNode() : this("a waves texture") { }
		public WavesTextureProceduralNode(string name)
			: base(ShaderNodeType.RhinoWavesTexture, name)
		{
			inputs = new WavesTextureInputs(this);
			outputs = new WavesTextureOutputs(this);
			ins.Color1.Value = new float4(0.0f, 0.0f, 0.0f);
			ins.Color2.Value = new float4(1.0f, 1.0f, 1.0f);
			ins.Color3.Value = new float4(1.0f, 1.0f, 1.0f);
		}

		internal override void SetDirectMembers(uint clientId, uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, Type, "UvwTransform", UvwTransform[0].x, UvwTransform[0].y, UvwTransform[0].z, UvwTransform[0].w, 0);
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, Type, "UvwTransform", UvwTransform[1].x, UvwTransform[1].y, UvwTransform[1].z, UvwTransform[1].w, 1);
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, Type, "UvwTransform", UvwTransform[2].x, UvwTransform[2].y, UvwTransform[2].z, UvwTransform[2].w, 2);
			CSycles.shadernode_set_member_int(clientId, sceneId, shaderId, Id, Type, "WaveType", (int)WaveType);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "WaveWidth", WaveWidth);
			CSycles.shadernode_set_member_bool(clientId, sceneId, shaderId, Id, Type, "WaveWidthTextureOn", WaveWidthTextureOn);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "Contrast1", Contrast1);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "Contrast2", Contrast2);
		}
	}

	public class WavesWidthTextureInputs : Inputs
	{
		public VectorSocket UVW { get; set; }

		public WavesWidthTextureInputs(ShaderNode parentNode)
		{
			UVW = new VectorSocket(parentNode, "UVW");
			AddSocket(UVW);
		}
	}

	public class WavesWidthTextureOutputs : Outputs
	{
		public VectorSocket UVW { get; set; }

		public WavesWidthTextureOutputs(ShaderNode parentNode)
		{
			UVW = new VectorSocket(parentNode, "UVW");
			AddSocket(UVW);
		}
	}

	[ShaderNode("rhino_waves_width_texture")]
	public class WavesWidthTextureProceduralNode : ShaderNode
	{
		public WavesWidthTextureInputs ins => (WavesWidthTextureInputs)inputs;
		public WavesWidthTextureOutputs outs => (WavesWidthTextureOutputs)outputs;

		public Transform UvwTransform { get; set; } = Transform.Identity();
		public WavesTextureProceduralNode.WaveTypes WaveType { get; set; } = WavesTextureProceduralNode.WaveTypes.LINEAR;

		public WavesWidthTextureProceduralNode() : this("a waves width texture") { }
		public WavesWidthTextureProceduralNode(string name)
			: base(ShaderNodeType.RhinoWavesWidthTexture, name)
		{
			inputs = new WavesWidthTextureInputs(this);
			outputs = new WavesWidthTextureOutputs(this);
		}

		internal override void SetDirectMembers(uint clientId, uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, Type, "UvwTransform", UvwTransform[0].x, UvwTransform[0].y, UvwTransform[0].z, UvwTransform[0].w, 0);
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, Type, "UvwTransform", UvwTransform[1].x, UvwTransform[1].y, UvwTransform[1].z, UvwTransform[1].w, 1);
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, Type, "UvwTransform", UvwTransform[2].x, UvwTransform[2].y, UvwTransform[2].z, UvwTransform[2].w, 2);
			CSycles.shadernode_set_member_int(clientId, sceneId, shaderId, Id, Type, "WaveType", (int)WaveType);
		}
	}

	public class PerturbingPart1TextureInputs : Inputs
	{
		public VectorSocket UVW { get; set; }

		public PerturbingPart1TextureInputs(ShaderNode parentNode)
		{
			UVW = new VectorSocket(parentNode, "UVW");
			AddSocket(UVW);
		}
	}

	public class PerturbingPart1TextureOutputs : Outputs
	{
		public VectorSocket UVW0 { get; set; }
		public VectorSocket UVW1 { get; set; }
		public VectorSocket UVW2 { get; set; }

		public PerturbingPart1TextureOutputs(ShaderNode parentNode)
		{
			UVW0 = new VectorSocket(parentNode, "UVW0");
			AddSocket(UVW0);
			UVW1 = new VectorSocket(parentNode, "UVW1");
			AddSocket(UVW1);
			UVW2 = new VectorSocket(parentNode, "UVW2");
			AddSocket(UVW2);
		}
	}

	[ShaderNode("rhino_perturbing_part1_texture")]
	public class PerturbingPart1TextureProceduralNode : ShaderNode
	{
		public PerturbingPart1TextureInputs ins => (PerturbingPart1TextureInputs)inputs;
		public PerturbingPart1TextureOutputs outs => (PerturbingPart1TextureOutputs)outputs;

		public Transform UvwTransform { get; set; } = Transform.Identity();

		public PerturbingPart1TextureProceduralNode() : this("a pertrubing part1 texture") { }
		public PerturbingPart1TextureProceduralNode(string name)
			: base(ShaderNodeType.RhinoPerturbingPart1Texture, name)
		{
			inputs = new PerturbingPart1TextureInputs(this);
			outputs = new PerturbingPart1TextureOutputs(this);
		}

		internal override void SetDirectMembers(uint clientId, uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, Type, "UvwTransform", UvwTransform[0].x, UvwTransform[0].y, UvwTransform[0].z, UvwTransform[0].w, 0);
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, Type, "UvwTransform", UvwTransform[1].x, UvwTransform[1].y, UvwTransform[1].z, UvwTransform[1].w, 1);
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, Type, "UvwTransform", UvwTransform[2].x, UvwTransform[2].y, UvwTransform[2].z, UvwTransform[2].w, 2);
		}
	}

	public class PerturbingPart2TextureInputs : Inputs
	{
		public ColorSocket Color0 { get; set; }
		public ColorSocket Color1 { get; set; }
		public ColorSocket Color2 { get; set; }

		public PerturbingPart2TextureInputs(ShaderNode parentNode)
		{
			Color0 = new ColorSocket(parentNode, "Color0");
			AddSocket(Color0);
			Color1 = new ColorSocket(parentNode, "Color1");
			AddSocket(Color1);
			Color2 = new ColorSocket(parentNode, "Color2");
			AddSocket(Color2);
		}
	}

	public class PerturbingPart2TextureOutputs : Outputs
	{
		public ColorSocket Color { get; set; }

		public PerturbingPart2TextureOutputs(ShaderNode parentNode)
		{
			Color = new ColorSocket(parentNode, "Color");
			AddSocket(Color);
		}
	}

	[ShaderNode("rhino_perturbing_part2_texture")]
	public class PerturbingPart2TextureProceduralNode : ShaderNode
	{
		public PerturbingPart2TextureInputs ins => (PerturbingPart2TextureInputs)inputs;
		public PerturbingPart2TextureOutputs outs => (PerturbingPart2TextureOutputs)outputs;

		public float Amount { get; set; } = 0.1f;

		public PerturbingPart2TextureProceduralNode() : this("a pertrubing part2 texture") { }
		public PerturbingPart2TextureProceduralNode(string name)
			: base(ShaderNodeType.RhinoPerturbingPart2Texture, name)
		{
			inputs = new PerturbingPart2TextureInputs(this);
			outputs = new PerturbingPart2TextureOutputs(this);
		}

		internal override void SetDirectMembers(uint clientId, uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "Amount", Amount);
		}
	}
}
