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

namespace ccl.ShaderNodes
{
	public class ObjectInfoOutputs : Outputs
	{
		/// <summary>
		/// object location
		/// </summary>
		public VectorSocket Location { get; set; }
		/// <summary>
		/// Object pass id
		/// </summary>
		public FloatSocket ObjectIndex { get; set; }
		/// <summary>
		/// Material pass id
		/// </summary>
		public FloatSocket MaterialIndex { get; set; }
		/// <summary>
		/// Random number for shaded object
		/// </summary>
		public FloatSocket Random { get; set; }

		internal ObjectInfoOutputs(ShaderNode parentNode)
		{
			Location = new VectorSocket(parentNode, "Location");
			AddSocket(Location);
			ObjectIndex = new FloatSocket(parentNode, "ObjectIndex");
			AddSocket(ObjectIndex);
			MaterialIndex = new FloatSocket(parentNode, "MaterialIndex");
			AddSocket(MaterialIndex);
			Random = new FloatSocket(parentNode, "Random");
			AddSocket(Random);
		}
	}

	/// <summary>
	/// ObjectInfo input sockets. Not used, here for cast purposes.
	/// </summary>
	public class ObjectInfoInputs : Inputs
	{
	}

	/// <summary>
	/// ObjectInfo node gives information the object being shaded
	/// </summary>
	[ShaderNode("object_info")]
	public class ObjectInfoNode : ShaderNode
	{
		/// <summary>
		/// ObjectInfo node input sockets
		/// </summary>
		public ObjectInfoInputs ins => (ObjectInfoInputs)inputs;

		/// <summary>
		/// ObjectInfo node output sockets
		/// </summary>
		public ObjectInfoOutputs outs => (ObjectInfoOutputs)outputs;

		/// <summary>
		/// Create a new ObjectInfoNode
		/// </summary>
		public ObjectInfoNode()
			: this(string.Empty)
		{
		}

		public ObjectInfoNode(string name)
			: base(ShaderNodeType.ObjectInfo, name)
		{
			inputs = new ObjectInfoInputs();
			outputs = new ObjectInfoOutputs(this);
		}
	}
}

