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
	public class NormalPart1TextureInputs : Inputs
	{
		public VectorSocket UVW { get; set; }

		public NormalPart1TextureInputs(ShaderNode parentNode)
		{
			UVW = new VectorSocket(parentNode, "UVW");
			AddSocket(UVW);
		}
	}

	public class NormalPart1TextureOutputs : Outputs
	{
		public VectorSocket UVW1 { get; set; }
		public VectorSocket UVW2 { get; set; }
		public VectorSocket UVW3 { get; set; }
		public VectorSocket UVW4 { get; set; }
		public VectorSocket UVW5 { get; set; }
		public VectorSocket UVW6 { get; set; }
		public VectorSocket UVW7 { get; set; }
		public VectorSocket UVW8 { get; set; }

		public NormalPart1TextureOutputs(ShaderNode parentNode)
		{
			UVW1 = new VectorSocket(parentNode, "UVW1");
			AddSocket(UVW1);
			UVW2 = new VectorSocket(parentNode, "UVW2");
			AddSocket(UVW2);
			UVW3 = new VectorSocket(parentNode, "UVW3");
			AddSocket(UVW3);
			UVW4 = new VectorSocket(parentNode, "UVW4");
			AddSocket(UVW4);
			UVW5 = new VectorSocket(parentNode, "UVW5");
			AddSocket(UVW5);
			UVW6 = new VectorSocket(parentNode, "UVW6");
			AddSocket(UVW6);
			UVW7 = new VectorSocket(parentNode, "UVW7");
			AddSocket(UVW7);
			UVW8 = new VectorSocket(parentNode, "UVW8");
			AddSocket(UVW8);
		}
	}

	[ShaderNode("rhino_normal_part1_texture")]
	public class NormalPart1TextureProceduralNode : ShaderNode
	{
		public NormalPart1TextureInputs ins => (NormalPart1TextureInputs)inputs;
		public NormalPart1TextureOutputs outs => (NormalPart1TextureOutputs)outputs;

		public NormalPart1TextureProceduralNode() : this("a normal part 1 texture") { }
		public NormalPart1TextureProceduralNode(string name)
			: base(ShaderNodeType.RhinoNormalPart1Texture, name)
		{
			inputs = new NormalPart1TextureInputs(this);
			outputs = new NormalPart1TextureOutputs(this);
		}

		internal override void SetDirectMembers(uint clientId, uint sceneId, uint shaderId)
		{
		}
	}

	public class NormalPart2TextureInputs : Inputs
	{
		public ColorSocket Color1 { get; set; }
		public ColorSocket Color2 { get; set; }
		public ColorSocket Color3 { get; set; }
		public ColorSocket Color4 { get; set; }
		public ColorSocket Color5 { get; set; }
		public ColorSocket Color6 { get; set; }
		public ColorSocket Color7 { get; set; }
		public ColorSocket Color8 { get; set; }

		public NormalPart2TextureInputs(ShaderNode parentNode)
		{
			Color1 = new ColorSocket(parentNode, "Color1");
			AddSocket(Color1);
			Color2 = new ColorSocket(parentNode, "Color2");
			AddSocket(Color2);
			Color3 = new ColorSocket(parentNode, "Color3");
			AddSocket(Color3);
			Color4 = new ColorSocket(parentNode, "Color4");
			AddSocket(Color4);
			Color5 = new ColorSocket(parentNode, "Color5");
			AddSocket(Color5);
			Color6 = new ColorSocket(parentNode, "Color6");
			AddSocket(Color6);
			Color7 = new ColorSocket(parentNode, "Color7");
			AddSocket(Color7);
			Color8 = new ColorSocket(parentNode, "Color8");
			AddSocket(Color8);
		}
	}

	public class NormalPart2TextureOutputs : Outputs
	{
		public ColorSocket Color { get; set; }

		public NormalPart2TextureOutputs(ShaderNode parentNode)
		{
			Color = new ColorSocket(parentNode, "Color");
			AddSocket(Color);
		}
	}

	[ShaderNode("rhino_normal_part2_texture")]
	public class NormalPart2TextureProceduralNode : ShaderNode
	{
		public NormalPart2TextureInputs ins => (NormalPart2TextureInputs)inputs;
		public NormalPart2TextureOutputs outs => (NormalPart2TextureOutputs)outputs;

		public NormalPart2TextureProceduralNode() : this("a normal part 2 texture") { }
		public NormalPart2TextureProceduralNode(string name)
			: base(ShaderNodeType.RhinoNormalPart2Texture, name)
		{
			inputs = new NormalPart2TextureInputs(this);
			outputs = new NormalPart2TextureOutputs(this);
		}

		internal override void SetDirectMembers(uint clientId, uint sceneId, uint shaderId)
		{
		}
	}
}
