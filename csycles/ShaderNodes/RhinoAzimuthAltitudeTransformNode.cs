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
using System.Text;

namespace ccl.ShaderNodes
{
	public class RhinoAzimuthAltitudeTransformInputs : Inputs
	{
		public VectorSocket Vector { get; set; }

		internal RhinoAzimuthAltitudeTransformInputs(ShaderNode parentNode)
		{
			Vector = new VectorSocket(parentNode, "Vector");
			AddSocket(Vector);
		}
	}

	public class RhinoAzimuthAltitudeTransformOutputs : Outputs
	{
		public VectorSocket Vector { get; set; }

		internal RhinoAzimuthAltitudeTransformOutputs(ShaderNode parentNode)
		{
			Vector = new VectorSocket(parentNode, "Vector");
			AddSocket(Vector);
		}
	}

	[ShaderNode("rhino_azimuth_altitude_transform")]
	public class RhinoAzimuthAltitudeTransformNode : ShaderNode
	{
		public RhinoAzimuthAltitudeTransformInputs ins => (RhinoAzimuthAltitudeTransformInputs)inputs;
		public RhinoAzimuthAltitudeTransformOutputs outs => (RhinoAzimuthAltitudeTransformOutputs)outputs;

		public float Azimuth { get; set; } = 0.0f;
		public float Altitude { get; set; } = 0.0f;
		public float Threshold { get; set; } = 0.001f;

	public RhinoAzimuthAltitudeTransformNode() : this("a azimuth altitude transform node") { }
		public RhinoAzimuthAltitudeTransformNode(string name)
			: base(ShaderNodeType.RhinoAzimuthAltitudeTransform, name)
		{
			inputs = new RhinoAzimuthAltitudeTransformInputs(this);
			outputs = new RhinoAzimuthAltitudeTransformOutputs(this);

			Azimuth = 0.0f;
			Altitude = 0.0f;
			Threshold = 0.001f;
		}

		internal override void SetDirectMembers(uint clientId, uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "azimuth", Azimuth);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "altitude", Altitude);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "threshold", Threshold);
		}

	}
}
