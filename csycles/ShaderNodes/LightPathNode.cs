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
using ccl.ShaderNodes.Sockets;
using ccl.Attributes;

namespace ccl.ShaderNodes
{
	/// <summary>
	/// Output sockets for different ray type information.
	/// 
	/// See http://www.blender.org/manual/render/cycles/settings/light_paths.html for more information
	/// </summary>
	public class LightPathOutputs : Outputs
	{
		/// <summary>
		/// Value is 1.0f when the handled ray is a camera ray.
		/// 
		/// The ray comes straight from the camera.
		/// </summary>
		public FloatSocket IsCameraRay { get; set; }
		/// <summary>
		/// Value is 1.0f when the handled ray is a shadow ray.
		/// </summary>
		public FloatSocket IsShadowRay { get; set; }
		/// <summary>
		/// Value is 1.0f when the handled ray is a diffuse ray.
		///
		/// The ray is generated by a diffuse reflection or transmission (translucency).
		/// </summary>
		public FloatSocket IsDiffuseRay { get; set; }
		/// <summary>
		/// Value is 1.0f when the handled ray is a glossy ray.
		///
		/// The ray is generated by a glossy specular reflection or transmission.
		/// </summary>
		public FloatSocket IsGlossyRay { get; set; }
		/// <summary>
		/// Value is 1.0f when the handled ray is a singular ray.
		///
		/// The ray is generated by a perfectly sharp reflection or transmission.
		/// </summary>
		public FloatSocket IsSingularRay { get; set; }
		/// <summary>
		/// Value is 1.0f when the handled ray is a reflection ray.
		///
		/// The ray is generated by a reflection off a surface
		/// </summary>
		public FloatSocket IsReflectionRay { get; set; }
		/// <summary>
		/// Value is 1.0f when the handled ray is a transmission ray.
		///
		/// The ray is generated by a transmission through a surface.
		/// </summary>
		public FloatSocket IsTransmissionRay { get; set; }
		/// <summary>
		/// Value is 1.0f when the handled ray is a ray generated through volume scatter.
		/// </summary>
		public FloatSocket IsVolumeScatterRay { get; set; }
		/// <summary>
		/// Distance traveled by the light ray from the last bounce or camera.
		/// </summary>
		public FloatSocket RayLength { get; set; }
		/// <summary>
		/// Ray depth. Amount of ray bounces.
		/// </summary>
		public FloatSocket RayDepth { get; set; }
		/// <summary>
		/// Diffuse depth. Amount of diffuse bounces.
		/// </summary>
		public FloatSocket DiffuseDepth { get; set; }
		/// <summary>
		/// Glossy depth. Amount of glossy bounces.
		/// </summary>
		public FloatSocket GlossyDepth { get; set; }
		/// <summary>
		/// Transparent depth. Amount of transparent bounces.
		/// </summary>
		public FloatSocket TransparentDepth { get; set; }
		/// <summary>
		/// Transmission depth. Amount of transmission bounces.
		/// </summary>
		public FloatSocket TransmissionDepth { get; set; }
		/// <summary>
		/// No Ray set. Will be 1.0 if no path type ray has been set.
		/// </summary>
		public FloatSocket NoRaySet { get; set; }

		internal LightPathOutputs(ShaderNode parentNode)
		{
			IsCameraRay = new FloatSocket(parentNode, "Is Camera Ray");
			AddSocket(IsCameraRay);
			IsShadowRay = new FloatSocket(parentNode, "Is Shadow Ray");
			AddSocket(IsShadowRay);
			IsDiffuseRay = new FloatSocket(parentNode, "Is Diffuse Ray");
			AddSocket(IsDiffuseRay);
			IsGlossyRay = new FloatSocket(parentNode, "Is Glossy Ray");
			AddSocket(IsGlossyRay);
			IsSingularRay = new FloatSocket(parentNode, "Is Singular Ray");
			AddSocket(IsSingularRay);
			IsReflectionRay = new FloatSocket(parentNode, "Is Reflection Ray");
			AddSocket(IsReflectionRay);
			IsTransmissionRay = new FloatSocket(parentNode, "Is Transmission Ray");
			AddSocket(IsTransmissionRay);
			IsVolumeScatterRay = new FloatSocket(parentNode, "Is Volume Scatter Ray");
			AddSocket(IsVolumeScatterRay);
			RayLength = new FloatSocket(parentNode, "Ray Length");
			AddSocket(RayLength);
			RayDepth = new FloatSocket(parentNode, "Ray Depth");
			AddSocket(RayDepth);
			DiffuseDepth = new FloatSocket(parentNode, "Diffuse Depth");
			AddSocket(DiffuseDepth);
			GlossyDepth = new FloatSocket(parentNode, "Glossy Depth");
			AddSocket(GlossyDepth);
			TransparentDepth = new FloatSocket(parentNode, "Transparent Depth");
			AddSocket(TransparentDepth);
			TransmissionDepth = new FloatSocket(parentNode, "Transmission Depth");
			AddSocket(TransmissionDepth);
			NoRaySet = new FloatSocket(parentNode, "No Ray Set");
			AddSocket(NoRaySet);
		}
	}

	/// <summary>
	/// LightPath input sockets. Not used, here for cast purposes.
	/// </summary>
	public class LightPathInputs : Inputs
	{
	}

	/// <summary>
	/// LightPath node gives information about rays Cycles is handling.
	/// </summary>
	[ShaderNode("light_path")]
	public class LightPathNode : ShaderNode
	{
		/// <summary>
		/// LightPath node input sockets
		/// </summary>
		public LightPathInputs ins => (LightPathInputs)inputs;

		/// <summary>
		/// LightPath node output sockets
		/// </summary>
		public LightPathOutputs outs => (LightPathOutputs)outputs;

		/// <summary>
		/// Create a new LightPathNode
		/// </summary>
		public LightPathNode()
			: this(String.Empty)
		{
		}

		public LightPathNode(string name)
			: base(ShaderNodeType.LightPath, name)
		{
			inputs = new LightPathInputs();
			outputs = new LightPathOutputs(this);
		}
	}
}
