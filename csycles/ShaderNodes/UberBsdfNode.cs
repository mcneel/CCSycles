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

using System;
using System.Xml;
using ccl.ShaderNodes.Sockets;
using ccl.Attributes;

namespace ccl.ShaderNodes
{
	public class UberBsdfInputs : Inputs
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
		public FloatSocket Transparency { get; set; }
		public FloatSocket RefractionRoughness { get; set; }
		public FloatSocket AnisotropicRotation { get; set; }
		public VectorSocket Normal { get; set; }
		public VectorSocket ClearcoatNormal { get; set; }
		public VectorSocket Tangent { get; set; }

		public UberBsdfInputs(ShaderNode parentNode)
		{

			BaseColor = new ColorSocket(parentNode, "Base Color");
			AddSocket(BaseColor);
			//SpecularColor = new ColorSocket(parentNode, "Specular Color");
			//AddSocket(SpecularColor);
			SubsurfaceColor = new ColorSocket(parentNode, "Subsurface Color");
			AddSocket(SubsurfaceColor);
			Metallic = new FloatSocket(parentNode, "Metallic");
			AddSocket(Metallic);
			Subsurface = new FloatSocket(parentNode, "Subsurface");
			AddSocket(Subsurface);
			SubsurfaceRadius = new VectorSocket(parentNode, "Subsurface Radius");
			AddSocket(SubsurfaceRadius);
			Specular = new FloatSocket(parentNode, "Specular");
			AddSocket(Specular);
			Roughness = new FloatSocket(parentNode, "Roughness");
			AddSocket(Roughness);
			SpecularTint = new FloatSocket(parentNode, "Specular Tint");
			AddSocket(SpecularTint);
			Anisotropic = new FloatSocket(parentNode, "Anisotropic");
			AddSocket(Anisotropic);
			Sheen = new FloatSocket(parentNode, "Sheen");
			AddSocket(Sheen);
			SheenTint = new FloatSocket(parentNode, "Sheen Tint");
			AddSocket(SheenTint);
			Clearcoat = new FloatSocket(parentNode, "Clearcoat");
			AddSocket(Clearcoat);
			ClearcoatGloss = new FloatSocket(parentNode, "Clearcoat Gloss");
			AddSocket(ClearcoatGloss);
			IOR = new FloatSocket(parentNode, "IOR");
			AddSocket(IOR);
			Transparency = new FloatSocket(parentNode, "Transparency");
			AddSocket(Transparency);
			RefractionRoughness = new FloatSocket(parentNode, "Refraction Roughness");
			AddSocket(RefractionRoughness);
			AnisotropicRotation = new FloatSocket(parentNode, "Anisotropic Rotation");
			AddSocket(AnisotropicRotation);
			Normal = new VectorSocket(parentNode, "Normal");
			AddSocket(Normal);
			ClearcoatNormal = new VectorSocket(parentNode, "Clearcoat Normal");
			AddSocket(ClearcoatNormal);
			Tangent = new VectorSocket(parentNode, "Tangent");
			AddSocket(Tangent);
		}
	}

	public class UberBsdfOutputs : Outputs
	{
		public ClosureSocket BSDF { get; set; }

		public UberBsdfOutputs(ShaderNode parentNode)
		{
			BSDF = new ClosureSocket(parentNode, "BSDF");
			AddSocket(BSDF);
		}
	}
	
	/// <summary>
	/// A Uber BSDF closure.
	/// This closure takes two inputs, <c>Color</c> and <c>Roughness</c>. The result
	/// will be a regular diffuse shading.
	/// 
	/// There is one output <c>Closure</c>
	/// </summary>
	[ShaderNode("uber_bsdf")]
	public class UberBsdfNode : ShaderNode
	{
		public enum Distributions
		{
			GGX = 33,
			Multiscatter_GGX = 34
		}

		public UberBsdfInputs ins => (UberBsdfInputs)inputs;
		public UberBsdfOutputs outs => (UberBsdfOutputs)outputs;

		/// <summary>
		/// Create a new Uber BSDF closure.
		/// </summary>
		public UberBsdfNode() : this("a disney bsdf node") { }
		public UberBsdfNode(string name) :
			base(ShaderNodeType.Uber, name)
		{
			inputs = new UberBsdfInputs(this);
			outputs = new UberBsdfOutputs(this);
			ins.BaseColor.Value = new float4(0.7f, 0.6f, 0.5f, 1.0f);
			ins.Metallic.Value = 0.0f;
			ins.Specular.Value = 0.5f;
			ins.SpecularTint.Value = 0.0f;
			ins.SubsurfaceRadius.Value = new float4(0.7f, 1.0f, 1.0f, 1.0f);
			ins.Roughness.Value = 0.0f;
			ins.Anisotropic.Value = 0.0f;
			ins.AnisotropicRotation.Value = 0.0f;
			ins.Sheen.Value = 0.0f;
			ins.SheenTint.Value = 0.5f;
			ins.Clearcoat.Value = 0.0f;
			ins.ClearcoatGloss.Value = 1.0f;
			ins.IOR.Value = 1.45f;
			ins.Transparency.Value = 0.0f;
			ins.RefractionRoughness.Value = 0.0f;
			Distribution = Distributions.Multiscatter_GGX;
		}

		public Distributions Distribution { get; set; }

		internal override void SetEnums(uint clientId, uint shaderId)
		{
			CSycles.shadernode_set_enum(clientId, shaderId, Id, Type, "distribution", (int)Distribution);
		}

		internal override void ParseXml(XmlReader xmlNode)
		{
			Utilities.Instance.get_float4(ins.BaseColor, xmlNode);
			//Utilities.Instance.get_float4(ins.SpecularColor, xmlNode);
			Utilities.Instance.get_float4(ins.SubsurfaceColor, xmlNode);
			Utilities.Instance.get_float(ins.Metallic, xmlNode);
			Utilities.Instance.get_float(ins.Subsurface, xmlNode);
			Utilities.Instance.get_float4(ins.SubsurfaceRadius, xmlNode);
			Utilities.Instance.get_float(ins.Specular, xmlNode);
			Utilities.Instance.get_float(ins.Roughness, xmlNode);
			Utilities.Instance.get_float(ins.SpecularTint, xmlNode);
			Utilities.Instance.get_float(ins.Anisotropic, xmlNode);
			Utilities.Instance.get_float(ins.Sheen, xmlNode);
			Utilities.Instance.get_float(ins.SheenTint, xmlNode);
			Utilities.Instance.get_float(ins.Clearcoat, xmlNode);
			Utilities.Instance.get_float(ins.ClearcoatGloss, xmlNode);
			Utilities.Instance.get_float(ins.IOR, xmlNode);
			Utilities.Instance.get_float(ins.Transparency, xmlNode);
			Utilities.Instance.get_float(ins.RefractionRoughness, xmlNode);
			Utilities.Instance.get_float(ins.AnisotropicRotation, xmlNode);
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
		}
	}
}
