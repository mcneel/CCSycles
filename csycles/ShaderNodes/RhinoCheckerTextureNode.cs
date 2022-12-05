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

using System.Xml;
using ccl.ShaderNodes.Sockets;
using ccl.Attributes;

namespace ccl.ShaderNodes
{
	public class CheckerTextureInputs : TwoColorInputs
	{
		public VectorSocket UVW { get; set; }

		public CheckerTextureInputs(ShaderNode parentNode) : base(parentNode)
		{
			UVW = new VectorSocket(parentNode, "UVW");
			AddSocket(UVW);
		}
	}

	public class CheckerTextureOutputs : TwoColorOutputs
	{
		public CheckerTextureOutputs(ShaderNode parentNode) : base(parentNode)
		{
		}
	}

	[ShaderNode("rhino_checker_texture")]
	public class CheckerTextureProceduralNode : ShaderNode
	{
		public CheckerTextureInputs ins => (CheckerTextureInputs)inputs;
		public CheckerTextureOutputs outs => (CheckerTextureOutputs)outputs;

		public CheckerTextureProceduralNode() : this("a checker texture") { }
		public CheckerTextureProceduralNode(string name)
			: base(ShaderNodeType.RhinoCheckerTexture, name)
		{
			inputs = new CheckerTextureInputs(this);
			outputs = new CheckerTextureOutputs(this);
		}

		internal override void SetDirectMembers(uint clientId, uint sceneId, uint shaderId)
		{
		}
	}
}
