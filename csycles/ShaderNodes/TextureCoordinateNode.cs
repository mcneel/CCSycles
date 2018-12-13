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
	public class TextureCoordinateOutputs : Outputs
	{
		public VectorSocket Generated { get; set; }
		public VectorSocket Normal { get; set; }
		public VectorSocket UV { get; set; }
		public VectorSocket Object { get; set; }
		public VectorSocket Camera { get; set; }
		public VectorSocket Window { get; set; }
		public VectorSocket Reflection { get; set; }
		public VectorSocket WcsBox { get; set; }
		public VectorSocket EnvSpherical { get; set; }
		public VectorSocket EnvEmap { get; set; }
		public VectorSocket EnvBox { get; set; }
		public VectorSocket EnvLightProbe { get; set; }
		public VectorSocket EnvCubemap { get; set; }
		public VectorSocket EnvCubemapVerticalCross { get; set; }
		public VectorSocket EnvCubemapHorizontalCross { get; set; }
		public VectorSocket EnvHemispherical { get; set; }

		public TextureCoordinateOutputs(ShaderNode parentNode)
		{
			Generated = new VectorSocket(parentNode, "Generated");
			AddSocket(Generated);
			Normal = new VectorSocket(parentNode, "Normal");
			AddSocket(Normal);
			UV = new VectorSocket(parentNode, "UV");
			AddSocket(UV);
			Object = new VectorSocket(parentNode, "Object");
			AddSocket(Object);
			Camera = new VectorSocket(parentNode, "Camera");
			AddSocket(Camera);
			Window = new VectorSocket(parentNode, "Window");
			AddSocket(Window);
			Reflection = new VectorSocket(parentNode, "Reflection");
			AddSocket(Reflection);
			WcsBox = new VectorSocket(parentNode, "WcsBox");
			AddSocket(WcsBox);
			EnvSpherical = new VectorSocket(parentNode, "EnvSpherical");
			AddSocket(EnvSpherical);
			EnvEmap = new VectorSocket(parentNode, "EnvEmap");
			AddSocket(EnvEmap);
			EnvBox = new VectorSocket(parentNode, "EnvBox");
			AddSocket(EnvBox);
			EnvLightProbe = new VectorSocket(parentNode, "EnvLightProbe");
			AddSocket(EnvLightProbe);
			EnvCubemap = new VectorSocket(parentNode, "EnvCubemap");
			AddSocket(EnvCubemap);
			EnvCubemapVerticalCross = new VectorSocket(parentNode, "EnvCubemapVerticalCross");
			AddSocket(EnvCubemapVerticalCross);
			EnvCubemapHorizontalCross = new VectorSocket(parentNode, "EnvCubemapHorizontalCross");
			AddSocket(EnvCubemapHorizontalCross);
			EnvHemispherical = new VectorSocket(parentNode, "EnvHemi");
			AddSocket(EnvHemispherical);
		}
	}

	public class TextureCoordinateInputs : Inputs
	{
	}

	[ShaderNode("texture_coordinate")]
	public class TextureCoordinateNode : ShaderNode
	{
		public TextureCoordinateInputs ins => (TextureCoordinateInputs)inputs;
		public TextureCoordinateOutputs outs => (TextureCoordinateOutputs)outputs;

		public TextureCoordinateNode()
			: this("a texcoord") { }
		public TextureCoordinateNode(string name)
			: base(ShaderNodeType.TextureCoordinate, name)
		{
			inputs = new TextureCoordinateInputs();
			outputs = new TextureCoordinateOutputs(this);

			UseTransform = false;
			ObjectTransform = ccl.Transform.Identity();
		}


		public ccl.Transform ObjectTransform { get; set; }
		public bool UseTransform { get; set; }

		internal override void SetDirectMembers(uint clientId, uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_member_bool(clientId, shaderId, Id, ShaderNodeType.TextureCoordinate, "use_transform", UseTransform);

			if (UseTransform)
			{
				var obt = ObjectTransform;

				CSycles.shadernode_set_member_vec4_at_index(clientId, shaderId, Id, ShaderNodeType.TextureCoordinate, "object_transform_x", obt.x.x, obt.x.y, obt.x.z, obt.x.w, 0);
				CSycles.shadernode_set_member_vec4_at_index(clientId, shaderId, Id, ShaderNodeType.TextureCoordinate, "object_transform_y", obt.y.x, obt.y.y, obt.y.z, obt.y.w, 1);
				CSycles.shadernode_set_member_vec4_at_index(clientId, shaderId, Id, ShaderNodeType.TextureCoordinate, "object_transform_z", obt.z.x, obt.z.y, obt.z.z, obt.z.w, 2);
			}
		}

		internal override void ParseXml(System.Xml.XmlReader xmlNode)
		{
		}
	}

}
