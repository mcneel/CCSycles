/**
Copyright 2014-2019 Robert McNeel and Associates

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

using System;
using ccl.ShaderNodes.Sockets;
using ccl.Attributes;

/**
 * TODO:
 * - expose attribute, currently hardcoded to uvmap
 * - expose tangent types (radial, uvmap), for now default to uvmap
 */
namespace ccl.ShaderNodes
{
	public class TangentOutputs : Outputs
	{
		/// <summary>
		/// Tangent
		/// </summary>
		public VectorSocket Tangent { get; set; }

		internal TangentOutputs(ShaderNode parentNode)
		{
			Tangent = new VectorSocket(parentNode, "Tangent");
			AddSocket(Tangent);
		}
	}

	/// <summary>
	/// Tangent input sockets. Not used, here for cast purposes.
	/// </summary>
	public class TangentInputs : Inputs
	{
	}

	/// <summary>
	/// Tangent node allows to link to the tangent map of a mesh
	/// </summary>
	[ShaderNode("tangent")]
	public class TangentNode : ShaderNode
	{
		/// <summary>
		/// Tangent node input sockets
		/// </summary>
		public TangentInputs ins => (TangentInputs)inputs;

		/// <summary>
		/// Tangent node output sockets
		/// </summary>
		public TangentOutputs outs => (TangentOutputs)outputs;

		/// <summary>
		/// Create a new TangentNode
		/// </summary>
		public TangentNode(Shader shader)
			: this(shader, "a tangent node")
		{
		}

		public TangentNode(Shader shader, string name)
			: base(shader, true)
		{
			inputs = new TangentInputs();
			outputs = new TangentOutputs(this);
		}
	}
}

