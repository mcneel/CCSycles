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
	public class NoiseTextureInputs : Inputs
	{
		public VectorSocket UVW { get; set; }
		public ColorSocket Color1 { get; set; }
		public ColorSocket Color2 { get; set; }

		public NoiseTextureInputs(ShaderNode parentNode)
		{
			UVW = new VectorSocket(parentNode, "UVW");
			AddSocket(UVW);
			Color1 = new ColorSocket(parentNode, "Color1");
			AddSocket(Color1);
			Color2 = new ColorSocket(parentNode, "Color2");
			AddSocket(Color2);
		}
	}

	public class NoiseTextureOutputs : Outputs
	{
		public VectorSocket Color { get; set; }

		public NoiseTextureOutputs(ShaderNode parentNode)
		{
			Color = new VectorSocket(parentNode, "Color");
			AddSocket(Color);
		}
	}

	[ShaderNode("rhino_noise_texture")]
	public class NoiseTextureProceduralNode : ShaderNode
	{
		public enum NoiseTypes
		{
			PERLIN,
			VALUE_NOISE,
			PERLIN_PLUS_VALUE,
			SIMPLEX,
			SPARSE_CONVOLUTION,
			LATTICE_CONVOLUTION,
			WARDS_HERMITE,
			AALTONEN,
		};

		public enum SpecSynthTypes
		{
			FRACTAL_SUM,
			TURBULENCE,
		}

		public NoiseTextureInputs ins => (NoiseTextureInputs)inputs;
		public NoiseTextureOutputs outs => (NoiseTextureOutputs)outputs;

		public Transform UvwTransform { get; set; } = Transform.Identity();
		public NoiseTypes NoiseType { get; set; } = NoiseTypes.PERLIN;
		public SpecSynthTypes SpecSynthType { get; set; } = SpecSynthTypes.FRACTAL_SUM;
		public int OctaveCount { get; set; } = 0;
		public float FrequencyMultiplier { get; set; } = 1.0f;
		public float AmplitudeMultiplier { get; set; } = 1.0f;
		public float ClampMin { get; set; } = 0.0f;
		public float ClampMax { get; set; } = 1.0f;
		public bool ScaleToClamp { get; set; } = false;
		public bool Inverse { get; set; } = false;
		public float Gain { get; set; } = 1.0f;

		public NoiseTextureProceduralNode() : this("a noise texture") { }
		public NoiseTextureProceduralNode(string name)
			: base(ShaderNodeType.RhinoNoiseTexture, name)
		{
			inputs = new NoiseTextureInputs(this);
			outputs = new NoiseTextureOutputs(this);
			ins.Color1.Value = new float4(0.0f, 0.0f, 0.0f);
			ins.Color2.Value = new float4(1.0f, 1.0f, 1.0f);
		}

		internal override void SetDirectMembers(uint clientId, uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, Type, "UvwTransform", UvwTransform[0].x, UvwTransform[0].y, UvwTransform[0].z, UvwTransform[0].w, 0);
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, Type, "UvwTransform", UvwTransform[1].x, UvwTransform[1].y, UvwTransform[1].z, UvwTransform[1].w, 1);
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, Type, "UvwTransform", UvwTransform[2].x, UvwTransform[2].y, UvwTransform[2].z, UvwTransform[2].w, 2);
			CSycles.shadernode_set_member_int(clientId, sceneId, shaderId, Id, Type, "NoiseType", (int)NoiseType);
			CSycles.shadernode_set_member_int(clientId, sceneId, shaderId, Id, Type, "SpecSynthType", (int)SpecSynthType);
			CSycles.shadernode_set_member_int(clientId, sceneId, shaderId, Id, Type, "OctaveCount", OctaveCount);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "FrequencyMultiplier", FrequencyMultiplier);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "AmplitudeMultiplier", AmplitudeMultiplier);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "ClampMin", ClampMin);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "ClampMax", ClampMax);
			CSycles.shadernode_set_member_bool(clientId, sceneId, shaderId, Id, Type, "ScaleToClamp", ScaleToClamp);
			CSycles.shadernode_set_member_bool(clientId, sceneId, shaderId, Id, Type, "Inverse", Inverse);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "Gain", Gain);
		}
	}
}
