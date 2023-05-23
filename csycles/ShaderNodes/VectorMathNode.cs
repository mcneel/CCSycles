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
using System.Text;
using System.Xml;
using ccl.ShaderNodes.Sockets;
using ccl.Attributes;

namespace ccl.ShaderNodes
{
	/// <summary>
	/// VectorMathNode input sockets
	/// </summary>
	public class VectorMathInputs : Inputs
	{
		/// <summary>
		/// VectorMathNode Vector1 input socket
		/// </summary>
		public VectorSocket Vector1 { get; set; }
		/// <summary>
		/// VectorMathNode Vector2 input socket
		/// </summary>
		public VectorSocket Vector2 { get; set; }

		/// <summary>
		/// Create VectorMathNode input sockets
		/// </summary>
		/// <param name="parentNode"></param>
		internal VectorMathInputs(ShaderNode parentNode)
		{
			Vector1 = new VectorSocket(parentNode, "Vector1");
			AddSocket(Vector1);
			Vector2 = new VectorSocket(parentNode, "Vector2");
			AddSocket(Vector2);
		}
	}

	/// <summary>
	/// VectorMathNode output sockets
	/// </summary>
	public class VectorMathOutputs : Outputs
	{
		/// <summary>
		/// The resulting vector of the VectorMathNode operation
		/// </summary>
		public FloatSocket Value { get; set; }

		public VectorSocket Vector { get; set; }

		/// <summary>
		/// Create VectorMathNode output sockets
		/// </summary>
		/// <param name="parentNode"></param>
		internal VectorMathOutputs(ShaderNode parentNode)
		{
			Value = new FloatSocket(parentNode, "Value");
			AddSocket(Value);
			Vector = new VectorSocket(parentNode, "Vector");
			AddSocket(Vector);
		}
	}

	/// <summary>
	/// Add a VectorMath node, setting output Value with any of the following <c>Operation</c>s using Vector1 and Vector2
	///
	/// Note that some operations use only Vector1
	/// </summary>
	[ShaderNode("vector_math")]
	public class VectorMathNode : ShaderNode
	{
		/// <summary>
		/// VectorMath operations the VectorMathNode can do.
		/// </summary>
		public enum Operations
		{
			/// <summary>
			/// Vector = Vector1 + Vector2
			/// Value = average_fac(Vector)
			/// </summary>
			Add,
			/// <summary>
			/// Vector = Vector1 - Vector2
			/// Value = average_fac(Vector)
			/// </summary>
			Subtract,
			/// <summary>
			/// Vector = len(Vector1 + Vector2)
			/// Value = average_fac(Vector)
			/// </summary>
			Average,
			/// <summary>
			/// Value = dot(Vector1, Vector2)
			/// Vector = 0.0, 0.0, 0.0
			/// </summary>
			Dot_Product,
			/// <summary>
			/// Vector = normalize(cross(Vector1, Vector2))
			/// Value = len(cross(Vector1, Vector2))
			/// </summary>
			Cross_Product,
			/// <summary>
			/// Vector = normalize(Vector1)
			/// Value = len(Vector1)
			///
			/// Note, Vector2 unused
			/// </summary>
			Normalize
		}

		/// <summary>
		/// VectorMathNode input sockets
		/// </summary>
		public VectorMathInputs ins => (VectorMathInputs)inputs;

		/// <summary>
		/// VectorMathNode output sockets
		/// </summary>
		public VectorMathOutputs outs => (VectorMathOutputs)outputs;

		/// <summary>
		/// VectorMath node operates on float inputs (note, some operations use only Vector1)
		/// </summary>
		public VectorMathNode(Shader shader) : this(shader, "a vector math node")
		{

		}

		public VectorMathNode(Shader shader, string name) :
			base(shader, true)
		{
			FinalizeConstructor();
		}

		internal VectorMathNode(Shader shader, IntPtr intPtr) : base(shader, intPtr)
		{
			FinalizeConstructor();
		}

		private void FinalizeConstructor()
		{
			inputs = new VectorMathInputs(this);
			outputs = new VectorMathOutputs(this);

			Operation = Operations.Add;
			ins.Vector1.Value = new float4(0.0f);
			ins.Vector2.Value = new float4(0.0f);
		}

		/// <summary>
		/// The operation this node does on Vector1 and Vector2
		/// </summary>
		public Operations Operation { get; set; }

		public void SetOperation(string op)
		{
			op = op.Replace(" ", "_");
			Operation = (Operations)Enum.Parse(typeof(Operations), op, true);
		}

		internal override void SetEnums(IntPtr sessionId, IntPtr shaderId)
		{
			CSycles.shadernode_set_enum(Id, "operation", (int)Operation);
		}

		internal override void ParseXml(XmlReader xmlNode)
		{
			Utilities.Instance.get_float4(ins.Vector1, xmlNode.GetAttribute("vector1"));
			Utilities.Instance.get_float4(ins.Vector2, xmlNode.GetAttribute("vector2"));
			var operation = xmlNode.GetAttribute("type");
			if (!string.IsNullOrEmpty(operation))
			{
				SetOperation(operation);
			}
		}
		public override string CreateXmlAttributes()
		{
			var codeattr = new StringBuilder(1024);

			codeattr.Append($" type=\"{Operation}\" ");

			return codeattr.ToString();
		}

		public override string CreateCodeAttributes()
		{
			var codeattr = new StringBuilder(1024);

			codeattr.Append($" {VariableName}.Operation = VectorMathNode.Operations.{Operation};");

			return codeattr.ToString();
		}
	}
	[ShaderNode("vector_add")]
	public class VectorAdd: VectorMathNode
	{

		public VectorAdd(Shader shader) : this(shader, "a vector add node") {}
		public VectorAdd(Shader shader, string name) : base(shader, name) { Operation = Operations.Add; }
		internal VectorAdd(Shader shader, IntPtr intPtr) : base(shader, intPtr) { Operation = Operations.Add; }
		public override string ShaderNodeTypeName => "vector_math";
	}
	[ShaderNode("vector_subtract")]
	public class VectorSubtract: VectorMathNode
	{

		public VectorSubtract(Shader shader) : this(shader, "a vector subtract node") {}
		public VectorSubtract(Shader shader, string name) : base(shader, name) { Operation = Operations.Subtract; }
		internal VectorSubtract(Shader shader, IntPtr intPtr) : base(shader, intPtr) { Operation = Operations.Subtract; }
		public override string ShaderNodeTypeName => "vector_math";
	}
	[ShaderNode("vector_average")]
	public class VectorAverage: VectorMathNode
	{

		public VectorAverage(Shader shader) : this(shader, "a vector average node") {}
		public VectorAverage(Shader shader, string name) : base(shader, name) { Operation = Operations.Average; }
		internal VectorAverage(Shader shader, IntPtr intPtr) : base(shader, intPtr) { Operation = Operations.Average; }
		public override string ShaderNodeTypeName => "vector_math";
	}
	[ShaderNode("vector_cross")]
	public class VectorCross_Product: VectorMathNode
	{

		public VectorCross_Product(Shader shader) : this(shader, "a vector cross node") {}
		public VectorCross_Product(Shader shader, string name) : base(shader, name) { Operation = Operations.Cross_Product; }
		internal VectorCross_Product(Shader shader, IntPtr intPtr) : base(shader, intPtr) { Operation = Operations.Cross_Product; }
		public override string ShaderNodeTypeName => "vector_math";
	}
	[ShaderNode("vector_dot")]
	public class VectorDot_Product: VectorMathNode
	{

		public VectorDot_Product(Shader shader) : this(shader, "a vector dot node") {}
		public VectorDot_Product(Shader shader, string name) : base(shader, name) { Operation = Operations.Dot_Product; }
		internal VectorDot_Product(Shader shader, IntPtr intPtr) : base(shader, intPtr) { Operation = Operations.Dot_Product; }
		public override string ShaderNodeTypeName => "vector_math";
	}
	[ShaderNode("vector_normalize")]
	public class VectorNormalize: VectorMathNode
	{

		public VectorNormalize(Shader shader) : this(shader, "a vector normalize node") {}
		public VectorNormalize(Shader shader, string name) : base(shader, name) { Operation = Operations.Normalize; }
		internal VectorNormalize(Shader shader, IntPtr intPtr) : base(shader, intPtr) { Operation = Operations.Normalize; }
		public override string ShaderNodeTypeName => "vector_math";
	}
}
