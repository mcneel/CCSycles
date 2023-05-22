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
	public class CameraDataInputs : Inputs
	{
		public CameraDataInputs(ShaderNode parentNode)
		{
		}
	}

	public class CameraDataOutputs : Outputs
	{
		public VectorSocket ViewVector { get; set; }
		public FloatSocket ViewDepth { get; set; }
		public FloatSocket ViewDistance { get; set; }

		public CameraDataOutputs(ShaderNode parentNode)
		{
			ViewVector = new VectorSocket(parentNode, "View Vector");
			AddSocket(ViewVector);
			ViewDepth = new FloatSocket(parentNode, "View Z Depth");
			AddSocket(ViewDepth);
			ViewDistance = new FloatSocket(parentNode, "View Distance");
			AddSocket(ViewDistance);
		}
	}

	[ShaderNode("camera_info")]
	public class CameraDataNode : ShaderNode
	{
		public CameraDataInputs ins => (CameraDataInputs)inputs;
		public CameraDataOutputs outs => (CameraDataOutputs)outputs;

		public CameraDataNode(Shader shader) : this(shader, "a camera data node") {}
		public CameraDataNode(Shader shader, string name)
			: base(shader, true)
		{
			inputs = new CameraDataInputs(this);
			outputs = new CameraDataOutputs(this);
		}
	}
}
