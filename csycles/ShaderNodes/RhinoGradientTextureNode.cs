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
	public class GradientTextureInputs : Inputs
	{
		public VectorSocket UVW { get; set; }
		public ColorSocket Color1 { get; set; }
		public ColorSocket Color2 { get; set; }

		public GradientTextureInputs(ShaderNode parentNode)
		{
			UVW = new VectorSocket(parentNode, "UVW");
			AddSocket(UVW);
			Color1 = new ColorSocket(parentNode, "Color1");
			AddSocket(Color1);
			Color2 = new ColorSocket(parentNode, "Color2");
			AddSocket(Color2);
		}
	}

	public class GradientTextureOutputs : Outputs
	{
		public ColorSocket Color { get; set; }

		public GradientTextureOutputs(ShaderNode parentNode)
		{
			Color = new ColorSocket(parentNode, "Color");
			AddSocket(Color);
		}
	}

	[ShaderNode("rhino_gradient_texture")]
	public class GradientTextureProceduralNode : ShaderNode
	{
		public enum GradientTypes
		{
			LINEAR,
			BOX,
			RADIAL,
			TARTAN,
			SWEEP,
			PONG,
			SPIRAL,
		};

		public GradientTextureInputs ins => (GradientTextureInputs)inputs;
		public GradientTextureOutputs outs => (GradientTextureOutputs)outputs;

		public Transform UvwTransform { get; set; } = Transform.Identity();
		public GradientTypes GradientType { get; set; }
		public bool FlipAlternate { get; set; }
		public bool UseCustomCurve { get; set; }
		public int PointWidth { get; set; }
		public int PointHeight { get; set; }

		public GradientTextureProceduralNode() : this("a gradient texture") { }
		public GradientTextureProceduralNode(string name)
			: base(ShaderNodeType.RhinoGradientTexture, name)
		{
			inputs = new GradientTextureInputs(this);
			outputs = new GradientTextureOutputs(this);
		}

		internal override void SetDirectMembers(uint clientId, uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, Type, "UvwTransform", UvwTransform[0].x, UvwTransform[0].y, UvwTransform[0].z, UvwTransform[0].w, 0);
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, Type, "UvwTransform", UvwTransform[1].x, UvwTransform[1].y, UvwTransform[1].z, UvwTransform[1].w, 1);
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, Type, "UvwTransform", UvwTransform[2].x, UvwTransform[2].y, UvwTransform[2].z, UvwTransform[2].w, 2);
			CSycles.shadernode_set_member_int(clientId, sceneId, shaderId, Id, Type, "GradientType", (int)GradientType);
			CSycles.shadernode_set_member_bool(clientId, sceneId, shaderId, Id, Type, "FlipAlternate", FlipAlternate);
			CSycles.shadernode_set_member_bool(clientId, sceneId, shaderId, Id, Type, "UseCustomCurve", UseCustomCurve);
			//CSycles.shadernode_set_member_int(clientId, sceneId, shaderId, Id, Type, "PointWidth", PointWidth);
			//CSycles.shadernode_set_member_int(clientId, sceneId, shaderId, Id, Type, "PointHeight", PointHeight);
		}
	}
}
