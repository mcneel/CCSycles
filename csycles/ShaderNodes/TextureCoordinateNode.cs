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
		public VectorSocket DecalUv { get; set; }
		public VectorSocket DecalPlanar { get; set; }
		public VectorSocket DecalSpherical { get; set; }
		public VectorSocket DecalCylindrical { get; set; }
		public FloatSocket DecalForward { get; set; }
		public FloatSocket DecalUsage { get; set; }

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
			DecalUv = new VectorSocket(parentNode, "DecalUv");
			AddSocket(DecalUv);
			DecalPlanar = new VectorSocket(parentNode, "DecalPlanar");
			AddSocket(DecalPlanar);
			DecalSpherical = new VectorSocket(parentNode, "DecalSpherical");
			AddSocket(DecalSpherical);
			DecalCylindrical = new VectorSocket(parentNode, "DecalCylindrical");
			AddSocket(DecalCylindrical);
			DecalForward = new FloatSocket(parentNode, "DecalForward");
			AddSocket(DecalForward);
			DecalUsage = new FloatSocket(parentNode, "DecalUsage");
			AddSocket(DecalUsage);
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

			HorizontalSweepStart = 0.0f;
			HorizontalSweepEnd = 1.0f;
			VerticalSweepStart = 0.0f;
			VerticalSweepEnd = 1.0f;
			DecalHeight = 1.0f;
			DecalRadius = 1.0f;
			DecalOrigin = new float4(0.0f);
			Across = new float4(0.0f);
			Up = new float4(0.0f);

			DecalPxyz = ccl.Transform.Identity();
			DecalNxyz = ccl.Transform.Identity();
			DecalUvw = ccl.Transform.Identity();
		}


		public ccl.Transform ObjectTransform { get; set; }
		public bool UseTransform { get; set; }

		/// <summary>
		/// Give the start for the horizontal sweep of a spherical or cylindrical decal projection
		/// </summary>
		public float HorizontalSweepStart { get; set; }

		/// <summary>
		/// Give the end for the horizontal sweep of a spherical or cylindrical decal projection
		/// </summary>
		public float HorizontalSweepEnd { get; set; }

		/// <summary>
		/// Give the start for the vertical sweep of a spherical or cylindrical decal projection
		/// </summary>
		public float VerticalSweepStart { get; set; }

		/// <summary>
		/// Give the end for the vertical sweep of a spherical or cylindrical decal projection
		/// </summary>
		public float VerticalSweepEnd { get; set; }

		/// <summary>
		/// Height for cylindrical projection of a decal
		/// </summary>
		public float DecalHeight { get; set; }

		/// <summary>
		/// Radius for cylindrical or spherical decal projection
		/// </summary>
		public float DecalRadius { get; set; }

		/// <summary>
		/// Direction of the decal: forward, backward
		/// </summary>
		public DecalDirection Direction { get; set; }

		/// <summary>
		/// use the x, y and z properties for the origin vector. The w property is ignored.
		/// </summary>
		public ccl.float4 DecalOrigin { get; set; }
		/// <summary>
		/// use the x, y and z properties for the across vector. The w property is ignored.
		/// </summary>
		public ccl.float4 Across { get; set; }

		/// <summary>
		/// use the x, y and z properties for the up vector. The w property is ignored.
		/// </summary>
		public ccl.float4 Up { get; set; }

		/// <summary>
		/// Transform used to map point P to normalized mapping primitive
		/// </summary>
		public ccl.Transform DecalPxyz { get; set; }

		/// <summary>
		/// Transform used to map normal N to normalized mapping primitive
		/// </summary>
		public ccl.Transform DecalNxyz { get; set; }

		/// <summary>
		/// Transform used to map point to UV(W) space
		/// </summary>
		public ccl.Transform DecalUvw { get; set; }

	public string UvMap { get; set; } = "uvmap1";

	internal override void SetDirectMembers(uint clientId, uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_member_bool(clientId, sceneId, shaderId, Id, ShaderNodeType.TextureCoordinate, "use_transform", UseTransform);

			if (UseTransform)
			{
				var obt = ObjectTransform;

				CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, ShaderNodeType.TextureCoordinate, "object_transform", obt.x.x, obt.x.y, obt.x.z, obt.x.w, 0);
				CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, ShaderNodeType.TextureCoordinate, "object_transform", obt.y.x, obt.y.y, obt.y.z, obt.y.w, 1);
				CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, ShaderNodeType.TextureCoordinate, "object_transform", obt.z.x, obt.z.y, obt.z.z, obt.z.w, 2);
			}
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "decal_height", DecalHeight);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "decal_radius", DecalRadius);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "decal_hor_start", HorizontalSweepStart);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "decal_hor_end", HorizontalSweepEnd);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "decal_ver_start", VerticalSweepStart);
			CSycles.shadernode_set_member_float(clientId, sceneId, shaderId, Id, Type, "decal_ver_end", VerticalSweepEnd);
			CSycles.shadernode_set_member_vec(clientId, sceneId, shaderId, Id, Type, "origin", DecalOrigin.x, DecalOrigin.y, DecalOrigin.z);
			CSycles.shadernode_set_member_vec(clientId, sceneId, shaderId, Id, Type, "across", Across.x, Across.y, Across.z);
			CSycles.shadernode_set_member_vec(clientId, sceneId, shaderId, Id, Type, "up", Up.x, Up.y, Up.z);

			var pxyz = DecalPxyz;

			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, ShaderNodeType.TextureCoordinate, "pxyz", pxyz.x.x, pxyz.x.y, pxyz.x.z, pxyz.x.w, 0);
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, ShaderNodeType.TextureCoordinate, "pxyz", pxyz.y.x, pxyz.y.y, pxyz.y.z, pxyz.y.w, 1);
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, ShaderNodeType.TextureCoordinate, "pxyz", pxyz.z.x, pxyz.z.y, pxyz.z.z, pxyz.z.w, 2);

			var nxyz = DecalNxyz;

			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, ShaderNodeType.TextureCoordinate, "nxyz", nxyz.x.x, nxyz.x.y, nxyz.x.z, nxyz.x.w, 0);
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, ShaderNodeType.TextureCoordinate, "nxyz", nxyz.y.x, nxyz.y.y, nxyz.y.z, nxyz.y.w, 1);
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, ShaderNodeType.TextureCoordinate, "nxyz", nxyz.z.x, nxyz.z.y, nxyz.z.z, nxyz.z.w, 2);

			var uvw = DecalUvw;

			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, ShaderNodeType.TextureCoordinate, "uvw", uvw.x.x, uvw.x.y, uvw.x.z, uvw.x.w, 0);
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, ShaderNodeType.TextureCoordinate, "uvw", uvw.y.x, uvw.y.y, uvw.y.z, uvw.y.w, 1);
			CSycles.shadernode_set_member_vec4_at_index(clientId, sceneId, shaderId, Id, ShaderNodeType.TextureCoordinate, "uvw", uvw.z.x, uvw.z.y, uvw.z.z, uvw.z.w, 2);

			CSycles.shadernode_set_member_string(clientId, sceneId, shaderId, Id, ShaderNodeType.TextureCoordinate, "uvmap", UvMap);
		}

		internal override void SetEnums(uint clientId, uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_enum(clientId, sceneId, shaderId, Id, Type, "decal_projection", (int)Direction);
		}

		internal override void ParseXml(System.Xml.XmlReader xmlNode)
		{
		}
	}

}
