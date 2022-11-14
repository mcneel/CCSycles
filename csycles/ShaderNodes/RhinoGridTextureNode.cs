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
	public class GridTextureInputs : Inputs
	{
		public VectorSocket UVW { get; set; }
		public ColorSocket Color1 { get; set; }
		public ColorSocket Color2 { get; set; }

		public GridTextureInputs(ShaderNode parentNode)
		{
			UVW = new VectorSocket(parentNode, "UVW");
			AddSocket(UVW);
			Color1 = new ColorSocket(parentNode, "Color1");
			AddSocket(Color1);
			Color2 = new ColorSocket(parentNode, "Color2");
			AddSocket(Color2);
		}
	}

	public class GridTextureOutputs : Outputs
	{
		public ColorSocket Color { get; set; }

		public GridTextureOutputs(ShaderNode parentNode)
		{
			Color = new ColorSocket(parentNode, "Color");
			AddSocket(Color);
		}
	}

	[ShaderNode("rhino_grid_texture")]
	public class GridTextureProceduralNode : ShaderNode
	{
		public GridTextureInputs ins => (GridTextureInputs)inputs;
		public GridTextureOutputs outs => (GridTextureOutputs)outputs;

		public int Cells { get; set; }
		public float FontThickness { get; set; }

		public GridTextureProceduralNode() : this("a grid texture") { }
		public GridTextureProceduralNode(string name)
			: base(ShaderNodeType.RhinoGridTexture, name)
		{
			inputs = new GridTextureInputs(this);
			outputs = new GridTextureOutputs(this);
		}

		internal override void SetDirectMembers(uint clientId, uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_member_int(clientId, sceneId, shaderId, Id, Type, "Cells", Cells);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "FontThickness", FontThickness);
		}
	}
}
