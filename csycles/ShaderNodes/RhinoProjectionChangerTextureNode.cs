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
	public class ProjectionChangerTextureInputs : Inputs
	{
		public VectorSocket UVW { get; set; }

		public ProjectionChangerTextureInputs(ShaderNode parentNode)
		{
			UVW = new VectorSocket(parentNode, "UVW");
			AddSocket(UVW);
		}
	}

	public class ProjectionChangerTextureOutputs : Outputs
	{
		public VectorSocket OutputUVW { get; set; }

		public ProjectionChangerTextureOutputs(ShaderNode parentNode)
		{
			OutputUVW = new VectorSocket(parentNode, "Output UVW");
			AddSocket(OutputUVW);
		}
	}

	[ShaderNode("rhino_projection_changer_texture")]
	public class ProjectionChangerTextureProceduralNode : ShaderNode
	{
		public enum ProjectionTypes
		{
			NONE,
			PLANAR,
			LIGHTPROBE,
			EQUIRECT,
			CUBEMAP,
			VERTICAL_CROSS_CUBEMAP,
			HORIZONTAL_CROSS_CUBEMAP,
			EMAP,
			SAME_AS_INPUT,
			HEMISPHERICAL,
		};

		public ProjectionChangerTextureInputs ins => (ProjectionChangerTextureInputs)inputs;
		public ProjectionChangerTextureOutputs outs => (ProjectionChangerTextureOutputs)outputs;

		public ProjectionTypes InputProjectionType { get; set; }
		public ProjectionTypes OutputProjectionType { get; set; }
		public float Azimuth { get; set; }
		public float Altitude { get; set; }

		public ProjectionChangerTextureProceduralNode() : this("a projection changer texture") { }
		public ProjectionChangerTextureProceduralNode(string name)
			: base(ShaderNodeType.RhinoProjectionChangerTexture, name)
		{
			inputs = new ProjectionChangerTextureInputs(this);
			outputs = new ProjectionChangerTextureOutputs(this);
		}

		internal override void SetDirectMembers(uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_member_int(sceneId, shaderId, Id, Type, "InputProjectionType", (int)InputProjectionType);
			CSycles.shadernode_set_member_int(sceneId, shaderId, Id, Type, "OutputProjectionType", (int)OutputProjectionType);
			CSycles.shadernode_set_member_float(sceneId, shaderId, Id, Type, "Azimuth", Azimuth);
			CSycles.shadernode_set_member_float(sceneId, shaderId, Id, Type, "Altitude", Altitude);
		}
	}
}
