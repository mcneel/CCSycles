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
	public class TileTextureInputs : Inputs
	{
		public VectorSocket UVW { get; set; }
		public ColorSocket Color1 { get; set; }
		public ColorSocket Color2 { get; set; }

		public TileTextureInputs(ShaderNode parentNode)
		{
			UVW = new VectorSocket(parentNode, "UVW");
			AddSocket(UVW);
			Color1 = new ColorSocket(parentNode, "Color1");
			AddSocket(Color1);
			Color2 = new ColorSocket(parentNode, "Color2");
			AddSocket(Color2);
		}
	}

	public class TileTextureOutputs : Outputs
	{
		public ColorSocket Color { get; set; }

		public TileTextureOutputs(ShaderNode parentNode)
		{
			Color = new ColorSocket(parentNode, "Color");
			AddSocket(Color);
		}
	}

	[ShaderNode("rhino_tile_texture")]
	public class TileTextureProceduralNode : ShaderNode
	{
		public enum TileTypes
		{
			RECTANGULAR_3D,
			RECTANGULAR_2D,
			HEXAGONAL_2D,
			TRIANGULAR_2D,
			OCTAGONAL_2D,
		};

		public TileTextureInputs ins => (TileTextureInputs)inputs;
		public TileTextureOutputs outs => (TileTextureOutputs)outputs;

		public TileTypes TileType { get; set; }
		public float PhaseX { get; set; }
		public float PhaseY { get; set; }
		public float PhaseZ { get; set; }
		public float JoinWidthX { get; set; }
		public float JoinWidthY { get; set; }
		public float JoinWidthZ { get; set; }

		public TileTextureProceduralNode() : this("a tile texture") { }
		public TileTextureProceduralNode(string name)
			: base(ShaderNodeType.RhinoTileTexture, name)
		{
			inputs = new TileTextureInputs(this);
			outputs = new TileTextureOutputs(this);
		}

		internal override void SetDirectMembers(uint clientId, uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_member_int(clientId, sceneId, shaderId, Id, Type, "Type", (int)TileType);
			CSycles.shadernode_set_member_vec(clientId, sceneId, shaderId, Id, Type, "Phase", PhaseX, PhaseY, PhaseZ);
			CSycles.shadernode_set_member_vec(clientId, sceneId, shaderId, Id, Type, "JoinWidth", JoinWidthX, JoinWidthY, JoinWidthZ);
		}
	}
}
