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
	public class WavesTextureInputs : TwoColorInputs
	{
		public VectorSocket UVW { get; set; }
		public ColorSocket Color3 { get; set; }

		public WavesTextureInputs(ShaderNode parentNode) : base(parentNode)
		{
			UVW = new VectorSocket(parentNode, "UVW");
			AddSocket(UVW);
			Color3 = new ColorSocket(parentNode, "Color3");
			AddSocket(Color3);
		}
	}

	public class WavesTextureOutputs : TwoColorOutputs
	{
		public WavesTextureOutputs(ShaderNode parentNode) : base(parentNode)
		{
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

		internal override void SetDirectMembers(uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_member_int(sceneId, shaderId, Id, Type, "WaveType", (int)WaveType);
			CSycles.shadernode_set_member_float(sceneId, shaderId, Id, Type, "WaveWidth", WaveWidth);
			CSycles.shadernode_set_member_bool(sceneId, shaderId, Id, Type, "WaveWidthTextureOn", WaveWidthTextureOn);
			CSycles.shadernode_set_member_float(sceneId, shaderId, Id, Type, "Contrast1", Contrast1);
			CSycles.shadernode_set_member_float(sceneId, shaderId, Id, Type, "Contrast2", Contrast2);
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

		public WavesTextureProceduralNode.WaveTypes WaveType { get; set; } = WavesTextureProceduralNode.WaveTypes.LINEAR;

		public WavesWidthTextureProceduralNode() : this("a waves width texture") { }
		public WavesWidthTextureProceduralNode(string name)
			: base(ShaderNodeType.RhinoWavesWidthTexture, name)
		{
			inputs = new WavesWidthTextureInputs(this);
			outputs = new WavesWidthTextureOutputs(this);
		}

		internal override void SetDirectMembers(uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_member_int(sceneId, shaderId, Id, Type, "WaveType", (int)WaveType);
		}
	}
}
