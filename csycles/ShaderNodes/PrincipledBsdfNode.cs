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

using System;
using System.Xml;
using ccl.ShaderNodes.Sockets;
using ccl.Attributes;

namespace ccl.ShaderNodes
{
	public class PrincipledBsdfInputs : Inputs
	{
		public ColorSocket BaseColor { get; set; }
		//public ColorSocket SpecularColor { get; set; }
		public ColorSocket SubsurfaceColor { get; set; }
		public FloatSocket Metallic { get; set; }
		public FloatSocket Subsurface { get; set; }
		public VectorSocket SubsurfaceRadius { get; set; }
		public FloatSocket Specular { get; set; }
		public FloatSocket Roughness { get; set; }
		public FloatSocket SpecularTint { get; set; }
		public FloatSocket Anisotropic { get; set; }
		public FloatSocket Sheen { get; set; }
		public FloatSocket SheenTint { get; set; }
		public FloatSocket Clearcoat { get; set; }
		public FloatSocket ClearcoatGloss { get; set; }
		public FloatSocket IOR { get; set; }
		public FloatSocket Transmission { get; set; }
		public FloatSocket TransmissionRoughness { get; set; }
		public FloatSocket AnisotropicRotation { get; set; }
		public VectorSocket Normal { get; set; }
		public VectorSocket ClearcoatNormal { get; set; }
		public VectorSocket Tangent { get; set; }

		public PrincipledBsdfInputs(ShaderNode parentNode)
		{

			BaseColor = new ColorSocket(parentNode, "Base Color");
			//SpecularColor          = new ColorSocket(parentNode, "Specular Color");
			Subsurface             = new FloatSocket(parentNode, "Subsurface");
			SubsurfaceRadius       = new VectorSocket(parentNode, "Subsurface Radius");
			SubsurfaceColor        = new ColorSocket(parentNode, "Subsurface Color");
			Metallic               = new FloatSocket(parentNode, "Metallic");
			Specular               = new FloatSocket(parentNode, "Specular");
			SpecularTint           = new FloatSocket(parentNode, "Specular Tint");
			Roughness              = new FloatSocket(parentNode, "Roughness");
			Anisotropic            = new FloatSocket(parentNode, "Anisotropic");
			Sheen                  = new FloatSocket(parentNode, "Sheen");
			SheenTint              = new FloatSocket(parentNode, "Sheen Tint");
			Clearcoat              = new FloatSocket(parentNode, "Clearcoat");
			ClearcoatGloss         = new FloatSocket(parentNode, "Clearcoat Roughness");
			IOR                    = new FloatSocket(parentNode, "IOR");
			Transmission           = new FloatSocket(parentNode, "Transmission");
			TransmissionRoughness  = new FloatSocket(parentNode, "Transmission Roughness");
			AnisotropicRotation    = new FloatSocket(parentNode, "Anisotropic Rotation");
			Normal                 = new VectorSocket(parentNode, "Normal");
			ClearcoatNormal        = new VectorSocket(parentNode, "Clearcoat Normal");
			Tangent                = new VectorSocket(parentNode, "Tangent");

			AddSocket(BaseColor);
			//AddSocket(SpecularColor);
			AddSocket(Subsurface);
			AddSocket(SubsurfaceRadius);
			AddSocket(SubsurfaceColor);
			AddSocket(Metallic);
			AddSocket(Specular);
			AddSocket(SpecularTint);
			AddSocket(Roughness);
			AddSocket(Anisotropic);
			AddSocket(AnisotropicRotation);
			AddSocket(Sheen);
			AddSocket(SheenTint);
			AddSocket(Clearcoat);
			AddSocket(ClearcoatGloss);
			AddSocket(IOR);
			AddSocket(Transmission);
			AddSocket(TransmissionRoughness);
			AddSocket(Normal);
			AddSocket(ClearcoatNormal);
			AddSocket(Tangent);
		}
	}

	public class PrincipledBsdfOutputs : Outputs
	{
		public ClosureSocket BSDF { get; set; }

		public PrincipledBsdfOutputs(ShaderNode parentNode)
		{
			BSDF = new ClosureSocket(parentNode, "BSDF");
			AddSocket(BSDF);
		}
	}

	/// <summary>
	/// A Principled BSDF closure.
	/// This closure takes two inputs, <c>Color</c> and <c>Roughness</c>. The result
	/// will be a regular diffuse shading.
	///
	/// There is one output <c>Closure</c>
	/// </summary>
	[ShaderNode("principled_bsdf")]
	public class PrincipledBsdfNode : ShaderNode
	{
		public enum Distributions
		{
			GGX = 32,
			Multiscatter_GGX = 30
		}

		public enum ScatterMethod
		{
			Burley = 42,
			RandomWalk = 45,
		}

		public PrincipledBsdfInputs ins => (PrincipledBsdfInputs)inputs;
		public PrincipledBsdfOutputs outs => (PrincipledBsdfOutputs)outputs;

		/// <summary>
		/// Create a new Principled BSDF closure.
		/// </summary>
		public PrincipledBsdfNode() : this("a principled bsdf node") { }
		public PrincipledBsdfNode(string name) :
			base(ShaderNodeType.Principled, name)
		{
			/* TODO: Add scatter method property */
			inputs = new PrincipledBsdfInputs(this);
			outputs = new PrincipledBsdfOutputs(this);
			ins.BaseColor.Value = new float4(0.7f, 0.6f, 0.5f, 1.0f);
			ins.Metallic.Value = 0.0f;
			ins.Specular.Value = 0.5f;
			ins.SpecularTint.Value = 0.0f;
			ins.Subsurface.Value = 0.0f;
			ins.SubsurfaceColor.Value = new float4(0.7f, 0.1f, 0.1f);
			ins.SubsurfaceRadius.Value = new float4(0.7f, 1.0f, 1.0f, 1.0f);
			ins.Roughness.Value = 0.0f;
			ins.Anisotropic.Value = 0.0f;
			ins.AnisotropicRotation.Value = 0.0f;
			ins.Sheen.Value = 0.0f;
			ins.SheenTint.Value = 0.5f;
			ins.Clearcoat.Value = 0.0f;
			ins.ClearcoatGloss.Value = 1.0f;
			ins.IOR.Value = 1.45f;
			ins.Transmission.Value = 0.0f;
			ins.TransmissionRoughness.Value = 0.0f;
			Distribution = Distributions.GGX;
		}

		public Distributions Distribution { get; set; }
		public SubsurfaceScatteringNode.FalloffTypes Sss {get; set; }

		internal override void SetEnums(uint clientId, uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_enum(clientId, sceneId, shaderId, Id, Type, "distribution", (int)Distribution);
			CSycles.shadernode_set_enum(clientId, sceneId, shaderId, Id, Type, "sss", (int)Sss);
		}

		internal override void ParseXml(XmlReader xmlNode)
		{
			Utilities.Instance.get_float4(ins.BaseColor, xmlNode);
			//Utilities.Instance.get_float4(ins.SpecularColor, xmlNode);
			Utilities.Instance.get_float(ins.Subsurface, xmlNode);
			Utilities.Instance.get_float4(ins.SubsurfaceRadius, xmlNode);
			Utilities.Instance.get_float4(ins.SubsurfaceColor, xmlNode);
			Utilities.Instance.get_float(ins.Metallic, xmlNode);
			Utilities.Instance.get_float(ins.Specular, xmlNode);
			Utilities.Instance.get_float(ins.SpecularTint, xmlNode);
			Utilities.Instance.get_float(ins.Roughness, xmlNode);
			Utilities.Instance.get_float(ins.Anisotropic, xmlNode);
			Utilities.Instance.get_float(ins.AnisotropicRotation, xmlNode);
			Utilities.Instance.get_float(ins.Sheen, xmlNode);
			Utilities.Instance.get_float(ins.SheenTint, xmlNode);
			Utilities.Instance.get_float(ins.Clearcoat, xmlNode);
			Utilities.Instance.get_float(ins.ClearcoatGloss, xmlNode);
			Utilities.Instance.get_float(ins.IOR, xmlNode);
			Utilities.Instance.get_float(ins.Transmission, xmlNode);
			Utilities.Instance.get_float(ins.TransmissionRoughness, xmlNode);
			Utilities.Instance.get_float4(ins.Normal, xmlNode);
			Utilities.Instance.get_float4(ins.ClearcoatNormal, xmlNode);
			Utilities.Instance.get_float4(ins.Tangent, xmlNode);
			var str = "";
			Utilities.Instance.read_string(ref str, xmlNode.GetAttribute("distribution"));
			if (!string.IsNullOrEmpty(str))
			{
				Distributions d;
				if (Enum.TryParse(str, true, out d)) Distribution = d;
			}
			str = "";
			Utilities.Instance.read_string(ref str, xmlNode.GetAttribute("sss"));
			if (!string.IsNullOrEmpty(str)) {
				SubsurfaceScatteringNode.FalloffTypes sss;
				if (Enum.TryParse(str, true, out sss)) Sss = sss;
			}
		}

		public override ClosureSocket GetClosureSocket()
		{
			return outs.BSDF;
		}
	}
}
