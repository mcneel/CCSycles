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
	public class DotsTextureInputs : Inputs
	{
		public VectorSocket UVW { get; set; }
		public ColorSocket Color1 { get; set; }
		public ColorSocket Color2 { get; set; }

		public DotsTextureInputs(ShaderNode parentNode)
		{
			UVW = new VectorSocket(parentNode, "UVW");
			AddSocket(UVW);
			Color1 = new ColorSocket(parentNode, "Color1");
			AddSocket(Color1);
			Color2 = new ColorSocket(parentNode, "Color2");
			AddSocket(Color2);
		}
	}

	public class DotsTextureOutputs : Outputs
	{
		public ColorSocket Color { get; set; }

		public DotsTextureOutputs(ShaderNode parentNode)
		{
			Color = new ColorSocket(parentNode, "Color");
			AddSocket(Color);
		}
	}

	[ShaderNode("rhino_dots_texture")]
	public class DotsTextureProceduralNode : ShaderNode
	{
		public enum FalloffTypes
		{
			FLAT,
			LINEAR,
			CUBIC,
			ELLIPTIC,
		};

		public enum CompositionTypes
		{
			MAXIMUM,
			ADDITION,
			SUBTRACTION,
			MULTIPLICATION,
			AVERAGE,
			STANDARD,
		};

		public DotsTextureInputs ins => (DotsTextureInputs)inputs;
		public DotsTextureOutputs outs => (DotsTextureOutputs)outputs;

		public int DataCount { get; set; }
		public int TreeNodeCount { get; set; }
		public float SampleAreaSize { get; set; }
		public bool Rings { get; set; }
		public float RingRadius { get; set; }
		public FalloffTypes FalloffType { get; set; }
		public CompositionTypes CompositionType { get; set; }

		public DotsTextureProceduralNode() : this("a dots texture") { }
		public DotsTextureProceduralNode(string name)
			: base(ShaderNodeType.RhinoTextureAdjustmentTexture, name)
		{
			inputs = new DotsTextureInputs(this);
			outputs = new DotsTextureOutputs(this);
		}

		internal override void SetDirectMembers(uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_member_int(sceneId, shaderId, Id, Type, "DataCount", DataCount);
			CSycles.shadernode_set_member_int(sceneId, shaderId, Id, Type, "TreeNodeCount", TreeNodeCount);
			CSycles.shadernode_set_member_float(sceneId, shaderId, Id, Type, "SampleAreaSize", SampleAreaSize);
			CSycles.shadernode_set_member_bool(sceneId, shaderId, Id, Type, "Rings", Rings);
			CSycles.shadernode_set_member_float(sceneId, shaderId, Id, Type, "RingRadius", RingRadius);
			CSycles.shadernode_set_member_int(sceneId, shaderId, Id, Type, "FalloffType", (int)FalloffType);
			CSycles.shadernode_set_member_int(sceneId, shaderId, Id, Type, "CompositionType", (int)CompositionType);
		}
	}
}
