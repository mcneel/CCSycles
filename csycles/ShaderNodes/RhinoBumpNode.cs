/**
Copyright 2014-2026 Robert McNeel and Associates

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

using ccl.Attributes;
using ccl.ShaderNodes.Sockets;
using System;

namespace ccl.ShaderNodes
{
	public class RhinoBumpInputs : Inputs
	{
		/// <summary>
		/// Height of the bump, 0 to 1
		/// </summary>
		public FloatSocket Height { get; set; }
		/// <summary>
		/// Texture coordinates the height is sampled with
		/// </summary>
		public VectorSocket UVW { get; set; }
		/// <summary>
		/// Input normal. If not connected, uses the shading normal
		/// </summary>
		public VectorSocket Normal { get; set; }
		/// <summary>
		/// Signed bump amount
		/// </summary>
		public FloatSocket Strength { get; set; }
		/// <summary>
		/// False: Strength scales the tilted normal and saturates (PBR). True: Strength scales the slope (Custom material)
		/// </summary>
		public BoolSocket Linear { get; set; }
		/// <summary>
		/// Rows of the linear map from UVW to texels, and its translation
		/// </summary>
		public VectorSocket TexelU { get; set; }
		public VectorSocket TexelV { get; set; }
		public VectorSocket TexelW { get; set; }
		public VectorSocket TexelOrigin { get; set; }
		/// <summary>
		/// Sample the neighbours at texel centres, for images with Filter off
		/// </summary>
		public BoolSocket Snap { get; set; }

		internal RhinoBumpInputs(ShaderNode parentNode)
		{
			Height = new FloatSocket(parentNode, "Height", "height");
			AddSocket(Height);
			UVW = new VectorSocket(parentNode, "UVW", "uvw");
			AddSocket(UVW);
			Normal = new VectorSocket(parentNode, "Normal", "normal");
			AddSocket(Normal);
			Strength = new FloatSocket(parentNode, "Strength", "strength");
			AddSocket(Strength);
			Linear = new BoolSocket(parentNode, "Linear", "linear");
			AddSocket(Linear);
			TexelU = new VectorSocket(parentNode, "TexelU", "texel_u");
			AddSocket(TexelU);
			TexelV = new VectorSocket(parentNode, "TexelV", "texel_v");
			AddSocket(TexelV);
			TexelW = new VectorSocket(parentNode, "TexelW", "texel_w");
			AddSocket(TexelW);
			TexelOrigin = new VectorSocket(parentNode, "TexelOrigin", "texel_origin");
			AddSocket(TexelOrigin);
			Snap = new BoolSocket(parentNode, "Snap", "snap");
			AddSocket(Snap);
		}
	}

	public class RhinoBumpOutputs : Outputs
	{
		public VectorSocket Normal { get; set; }

		internal RhinoBumpOutputs(ShaderNode parentNode)
		{
			Normal = new VectorSocket(parentNode, "Normal", "normal");
			AddSocket(Normal);
		}
	}

	/// <summary>
	/// Bump with the slope measured per texel, as in Rhino's display, so it does not depend on
	/// object size or units.
	/// </summary>
	[ShaderNode("rhino_bump")]
	public class RhinoBumpNode : ShaderNode
	{
		public RhinoBumpInputs ins => (RhinoBumpInputs)inputs;
		public RhinoBumpOutputs outs => (RhinoBumpOutputs)outputs;

		public RhinoBumpNode(Shader shader) : this(shader, "a rhino bump node") { }
		public RhinoBumpNode(Shader shader, string name) : base(shader, name)
		{
			FinalizeConstructor();
		}

		internal RhinoBumpNode(Shader shader, IntPtr intPtr) : base(shader, intPtr)
		{
			FinalizeConstructor();
		}

		private void FinalizeConstructor()
		{
			inputs = new RhinoBumpInputs(this);
			outputs = new RhinoBumpOutputs(this);

			ins.Strength.Value = 1.0f;
			ins.Linear.Value = false;
			ins.TexelU.Value = new float4(1.0f, 0.0f, 0.0f);
			ins.TexelV.Value = new float4(0.0f, 1.0f, 0.0f);
			ins.TexelW.Value = new float4(0.0f, 0.0f, 1.0f);
			ins.TexelOrigin.Value = new float4(0.0f, 0.0f, 0.0f);
			ins.Snap.Value = false;
		}

		/// <summary>
		/// Set the UVW-to-texel map from a transform.
		/// </summary>
		public void SetTexelTransform(Transform t)
		{
			ins.TexelU.Value = new float4(t.x.x, t.x.y, t.x.z);
			ins.TexelV.Value = new float4(t.y.x, t.y.y, t.y.z);
			ins.TexelW.Value = new float4(t.z.x, t.z.y, t.z.z);
			ins.TexelOrigin.Value = new float4(t.x.w, t.y.w, t.z.w);
		}

		internal override void SetDirectMembers()
		{
		}
	}
}
