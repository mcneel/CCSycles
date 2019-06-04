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

namespace ccl
{

	public static class Constants
	{
		public const string ccycles = "ccycles";
	}
	/// <summary>
	/// Device types that Cycles can support.
	/// 
	/// Note that currently focus is on CPU and CUDA
	/// and CUDA MULTI support, but others can be
	/// added when needed and possible
	/// </summary>
	public enum DeviceType : uint
	{
		None,
		CPU,
		OpenCL,
		CUDA,
		Network,
		Multi
	}

	/// <summary>
	/// Shading systems available in Cycles.
	/// 
	/// Note that currently only SVM is supported
	/// in C[CS]?ycles
	/// </summary>
	public enum ShadingSystem : uint
	{
		OSL,
		SVM
	}

	/// <summary>
	/// Integration method used for ray casting.
	/// </summary>
	public enum IntegratorMethod : int
	{
		/// <summary>
		/// On each hit rays get split up for all possible equivalents
		/// </summary>
		BranchedPath = 0,
		Path
	}

	/// <summary>
	/// Sampling patterns available in
	/// Cycles.
	/// </summary>
	public enum SamplingPattern : uint
	{
		Sobol = 0,
		CMJ
	}

	/// <summary>
	/// Available Cycles shader nodes.
	/// @note keep in sync with available Cycles nodes
	/// </summary>
	public enum ShaderNodeType: uint
	{
		Background = 0,
		Output, // automatic node, but here for completeness
		Diffuse,
		Anisotropic,
		Translucent,
		Transparent,
		Velvet,
		Toon,
		Glossy,
		Glass,
		Refraction,
		Hair,
		Emission,
		AmbientOcclusion,
		AbsorptionVolume,
		ScatterVolume,
		SubsurfaceScattering,
		Value,
		Color,
		MixClosure,
		AddClosure,
		Invert,
		Mix,
		Gamma,
		Wavelength,
		Blackbody,
		Camera,
		Fresnel,
		Math,
		ImageTexture,
		EnvironmentTexture,
		BrickTexture,
		SkyTexture,
		CheckerTexture,
		NoiseTexture,
		WaveTexture,
		MagicTexture,
		MusgraveTexture,
		TextureCoordinate,
		Bump,
		RgbToBw,
		RgbToLuminance,
		LightPath,
		LightFalloff,
		LayerWeight,
		GeometryInfo,
		VoronoiTexture,
		CombineXyz,
		SeparateXyz,
		SeparateHsv,
		CombineHsv,
		SeparateRgb,
		CombineRgb,
		Mapping,
		Holdout,
		HueSaturation,
		BrightnessContrast,
		GradientTexture,
		ColorRamp,
		VectorMath,
		MatrixMath,
		Principled,
		Attribute,
		NormalMap,
		Wireframe,
		ObjectInfo,
	}

	public enum BvhType : uint
	{
		Dynamic,
		Static
	}

	public enum TileOrder : uint
	{
		Center,
		RightToLeft,
		LeftToRight,
		TopToBottom,
		BottomToTop,
		HilbertSpiral
	}

	public enum CameraType : uint
	{
		Perspective,
		Orthographic,
		Panorama
	}

	public enum PanoramaType : uint
	{
		Equirectangular,
		FisheyeEquidistant,
		FisheyeEquisolid
	}

	public enum FilterType : uint
	{
		Box = 0,
		Gaussian
	}

	public enum LightType : uint
	{
		Point = 0,
		Distant,
		Background,
		Area,
		Spot,
		Triangle,
	}

	public enum InterpolationType : int {
		None = -1,
		Linear = 0,
		Closest = 1,
		Cubic = 2,
		Smart = 3,
	}

	[FlagsAttribute]
	public enum PathRay : uint
	{
		Hidden = 0,
		Camera = 1 << 0,
		Reflect = 1 << 1,
		Transmit = 1 << 2,
		Diffuse = 1 << 3,
		Glossy = 1 << 4,
		Singular = 1 << 5,
		Transparent = 1 << 6,

		ShadowOpaqueNonCatcher = 1 << 7,
		ShadowOpaqueCatcher = 1 << 8,
		ShadowOpaque = (ShadowOpaqueNonCatcher | ShadowOpaqueCatcher),
		ShadowTransparentNonCatcher = 1 << 9,
		ShadowTransparentCatcher = 1 << 10,
		ShadowTransparent = (ShadowTransparentNonCatcher | ShadowTransparentCatcher),
		ShadowNonCatcher = (ShadowOpaqueNonCatcher | ShadowTransparentNonCatcher),
		Shadow = (ShadowOpaque | ShadowTransparent),

		Curve = 1 << 11, /* visibility flag to define curve segments*/
		VolumeScatter = 1 << 12,

		NodeUnaligned = 1 << 13,

		/* note that these can use maximum 12 bits, the other are for layers */

		AllVisibility = ((1 << 14) -1),

		MisSkip = 1 << 15,
		DiffuseAncestor = 1 << 16,
		SinglePassDone = 1 << 17,
		ShadowCatcher = 1 << 18,
		StoreShadowInfo = 1 << 19,

		/* we need layer member flags to be the 20 upper bits */
		LayerShift = (32 - 20)
	}

	public enum PassType : int
	{
		Combined = (1 << 0),
		Depth = (1 << 1),
		Normal = (1 << 2),
		Uv = (1 << 3),
		ObjectId = (1 << 4),
		MaterialId = (1 << 5),
		DiffuseColor = (1 << 6),
		GlossyColor = (1 << 7),
		TransmissionColor = (1 << 8),
		DiffuseIndirect = (1 << 9),
		GlossyIndirect = (1 << 10),
		TransmissionIndirect = (1 << 11),
		DiffuseDirect = (1 << 12),
		GlossyDirect = (1 << 13),
		TransmissionDirect = (1 << 14),
		Emission = (1 << 15),
		Background = (1 << 16),
		Ao = (1 << 17),
		Shadow = (1 << 18),
		Motion = (1 << 19),
		MotionWeight = (1 << 20),
		Mist = (1 << 21),
		SubsurfaceDirect = (1 << 22),
		SubsurfaceIndirect = (1 << 23),
		SubsurfaceColor = (1 << 24),
	}
}
