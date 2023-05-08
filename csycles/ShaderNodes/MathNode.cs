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
	/// MathNode input sockets
	/// </summary>
	public class MathInputs : Inputs
	{
		/// <summary>
		/// MathNode Value1 input socket
		/// </summary>
		public FloatSocket Value1 { get; set; }
		/// <summary>
		/// MathNode Value2 input socket
		/// </summary>
		public FloatSocket Value2 { get; set; }

		/// <summary>
		/// Create MathNode input sockets
		/// </summary>
		/// <param name="parentNode"></param>
		internal MathInputs(ShaderNode parentNode)
		{
			Value1 = new FloatSocket(parentNode, "Value1");
			AddSocket(Value1);
			Value2 = new FloatSocket(parentNode, "Value2");
			AddSocket(Value2);
		}
	}

	/// <summary>
	/// MathNode output sockets
	/// </summary>
	public class MathOutputs : Outputs
	{
		/// <summary>
		/// The resulting value of the MathNode operation
		/// </summary>
		public FloatSocket Value { get; set; }

		/// <summary>
		/// Create MathNode output sockets
		/// </summary>
		/// <param name="parentNode"></param>
		internal MathOutputs(ShaderNode parentNode)
		{
			Value = new FloatSocket(parentNode, "Value");
			AddSocket(Value);
		}
	}

	/// <summary>
	/// Add a Math node, setting output Value with any of the following <c>Operation</c>s using Value1 and Value2
	///
	/// Note that some operations use only Value1
	/// </summary>
	[ShaderNode("math")]
	public class MathNode : ShaderNode
	{
		/// <summary>
		/// Math operations the MathNode can do.
		/// </summary>
		public enum Operations
		{
			/// <summary>
			/// Value = Value1 + Value2
			/// </summary>
			Add,
			/// <summary>
			/// Value = Value1 - Value2
			/// </summary>
			Subtract,
			/// <summary>
			/// Value = Value1 * Value2
			/// </summary>
			Multiply,
			/// <summary>
			/// Value = Value1 / Value2
			/// </summary>
			Divide,
			/// <summary>
			/// Value = sin(Value1). Value2 ignored
			/// </summary>
			Sine,
			/// <summary>
			/// Value = cos(Value1). Value2 ignored
			/// </summary>
			Cosine,
			/// <summary>
			/// Value = tan(Value1). Value2 ignored
			/// </summary>
			Tangent,
			/// <summary>
			/// Value = asin(Value1). Value2 ignored
			/// </summary>
			Arcsine,
			/// <summary>
			/// Value = acos(Value1). Value2 ignored
			/// </summary>
			Arccosine,
			/// <summary>
			/// Value = atan(Value1). Value2 ignored
			/// </summary>
			Arctangent,
			/// <summary>
			/// Value = Value1 ** Value2
			/// </summary>
			Power,
			/// <summary>
			/// Value = log(Value1) / log(Value2). 0.0f if either input is 0.0f
			/// </summary>
			Logarithm,
			/// <summary>
			/// Value = min(Value1, Value2)
			/// </summary>
			Minimum,
			/// <summary>
			/// Value = max(Value1, Value2)
			/// </summary>
			Maximum,
			/// <summary>
			/// Value = floor(Value1 + 0.5). Value2 ignored
			/// </summary>
			Round,
			/// <summary>
			/// Value = Value1 &lt; Value2
			/// </summary>
			Less_Than,
			/// <summary>
			/// Value = Value1 &gt; Value2
			/// </summary>
			Greater_Than,
			/// <summary>
			/// Value = mod(Value1, Value2)
			/// </summary>
			Modulo,
			/// <summary>
			/// Value = abs(Value1). Value2 ignored
			/// </summary>
			Absolute,
			/// <summary>
			/// Value = atan2f(Value1, Value2)
			/// </summary>
			Arctan2,
			/// <summary>
			/// Value = floorf(Value1)
			/// </summary>
			Floor,
			/// <summary>
			/// Value = ceilf(Value1)
			/// </summary>
			Ceil,
			/// <summary>
			/// Value = Value1 - floorf(Value1)
			/// </summary>
			Fract,
			/// <summary>
			/// Value = sqrtf(Value1)
			/// </summary>
			Sqrt,
		}

		/// <summary>
		/// MathNode input sockets
		/// </summary>
		public MathInputs ins => (MathInputs)inputs;

		/// <summary>
		/// MathNode output sockets
		/// </summary>
		public MathOutputs outs => (MathOutputs)outputs;

		/// <summary>
		/// Math node operates on float inputs (note, some operations use only Value1)
		/// </summary>
		public MathNode() :
			this("a mathnode")
		{

		}

		public MathNode(string name) :
			base(ShaderNodeType.Math, name)
		{
			inputs = new MathInputs(this);
			outputs = new MathOutputs(this);

			Operation = Operations.Add;
			ins.Value1.Value = 0.0f;
			ins.Value2.Value = 0.0f;
		}

		/// <summary>
		/// The operation this node does on Value1 and Value2
		/// </summary>
		public Operations Operation { get; set; }

		private void SetOperation(string op)
		{
			op = op.Replace(" ", "_");
			Operation = (Operations)Enum.Parse(typeof(Operations), op, true);
		}

		/// <summary>
		/// Set to true [IN] if math output in Value should be clamped 0.0..1.0
		/// </summary>
		public bool UseClamp { get; set; }

		internal override void SetEnums(IntPtr sessionId, IntPtr shaderId)
		{
			CSycles.shadernode_set_enum(sessionId, shaderId, Id, Type, "operation", (int)Operation);
		}

		internal override void SetDirectMembers(IntPtr sessionId, IntPtr shaderId)
		{
			CSycles.shadernode_set_member_bool(sessionId, shaderId, Id, Type, "use_clamp", UseClamp);
		}

		internal override void ParseXml(XmlReader xmlNode)
		{
			Utilities.Instance.get_float(ins.Value1, xmlNode.GetAttribute("value1"));
			Utilities.Instance.get_float(ins.Value2, xmlNode.GetAttribute("value2"));
			var operation = xmlNode.GetAttribute("type");
			if (!string.IsNullOrEmpty(operation))
			{
				SetOperation(operation);
			}
			var use_clamp = xmlNode.GetAttribute("use_clamp");
			if (!string.IsNullOrEmpty(use_clamp))
			{
				UseClamp = use_clamp.ToLowerInvariant().Equals("true");
			}
		}
		public override string CreateXmlAttributes()
		{
			var codeattr = new StringBuilder(1024);

			codeattr.Append($" type=\"{Operation}\" ");
			codeattr.Append($" use_clamp=\"{UseClamp.ToString().ToLowerInvariant()}\" ");

			return codeattr.ToString();
		}

		public override string CreateCodeAttributes()
		{
			var codeattr = new StringBuilder(1024);

			codeattr.Append($" {VariableName}.Operation = MathNode.Operations.{Operation};");
			codeattr.Append($" {VariableName}.UseClamp = {UseClamp.ToString().ToLowerInvariant()};");

			return codeattr.ToString();
		}
	}

	[ShaderNode("math_add")]
	public class MathAdd : MathNode
	{
		public MathAdd() : this("an add mathnode") {}
		public MathAdd(string name) : base(name) { Operation = Operations.Add; }
		public override string ShaderNodeTypeName => "math";
	}
	[ShaderNode("math_subtract")]
	public class MathSubtract : MathNode
	{
		public MathSubtract() : this("an subtract mathnode") {}
		public MathSubtract(string name) : base(name) { Operation = Operations.Subtract; }
		public override string ShaderNodeTypeName => "math";
	}
	[ShaderNode("math_multiply")]
	public class MathMultiply : MathNode
	{
		public MathMultiply() : this("an multiply mathnode") {}
		public MathMultiply(string name) : base(name) { Operation = Operations.Multiply; }
		public override string ShaderNodeTypeName => "math";
	}
	[ShaderNode("math_divide")]
	public class MathDivide : MathNode
	{
		public MathDivide() : this("an divide mathnode") {}
		public MathDivide(string name) : base(name) { Operation = Operations.Divide; }
		public override string ShaderNodeTypeName => "math";
	}
	[ShaderNode("math_sine")]
	public class MathSine : MathNode
	{
		public MathSine() : this("an sine mathnode") {}
		public MathSine(string name) : base(name) { Operation = Operations.Sine; }
		public override string ShaderNodeTypeName => "math";
	}
	[ShaderNode("math_cosine")]
	public class MathCosine : MathNode
	{
		public MathCosine() : this("an cosine mathnode") {}
		public MathCosine(string name) : base(name) { Operation = Operations.Cosine; }
		public override string ShaderNodeTypeName => "math";
	}
	[ShaderNode("math_tangent")]
	public class MathTangent : MathNode
	{
		public MathTangent() : this("an tangent mathnode") {}
		public MathTangent(string name) : base(name) { Operation = Operations.Tangent; }
		public override string ShaderNodeTypeName => "math";
	}
	[ShaderNode("math_arcsine")]
	public class MathArcsine : MathNode
	{
		public MathArcsine() : this("an arcsine mathnode") {}
		public MathArcsine(string name) : base(name) { Operation = Operations.Arcsine; }
		public override string ShaderNodeTypeName => "math";
	}
	[ShaderNode("math_arccosine")]
	public class MathArccosine : MathNode
	{
		public MathArccosine() : this("an arccosine mathnode") {}
		public MathArccosine(string name) : base(name) { Operation = Operations.Arccosine; }
		public override string ShaderNodeTypeName => "math";
	}
	[ShaderNode("math_arctangent")]
	public class MathArctangent : MathNode
	{
		public MathArctangent() : this("an arctangent mathnode") {}
		public MathArctangent(string name) : base(name) { Operation = Operations.Arctangent; }
		public override string ShaderNodeTypeName => "math";
	}
	[ShaderNode("math_power")]
	public class MathPower : MathNode
	{
		public MathPower() : this("an power mathnode") {}
		public MathPower(string name) : base(name) { Operation = Operations.Power; }
		public override string ShaderNodeTypeName => "math";
	}
	[ShaderNode("math_logarithm")]
	public class MathLogarithm : MathNode
	{
		public MathLogarithm() : this("an logarithm mathnode") {}
		public MathLogarithm(string name) : base(name) { Operation = Operations.Logarithm; }
		public override string ShaderNodeTypeName => "math";
	}
	[ShaderNode("math_minimum")]
	public class MathMinimum : MathNode
	{
		public MathMinimum() : this("an minimum mathnode") {}
		public MathMinimum(string name) : base(name) { Operation = Operations.Minimum; }
		public override string ShaderNodeTypeName => "math";
	}
	[ShaderNode("math_maximum")]
	public class MathMaximum : MathNode
	{
		public MathMaximum() : this("an maximum mathnode") {}
		public MathMaximum(string name) : base(name) { Operation = Operations.Maximum; }
		public override string ShaderNodeTypeName => "math";
	}
	[ShaderNode("math_round")]
	public class MathRound : MathNode
	{
		public MathRound() : this("an round mathnode") {}
		public MathRound(string name) : base(name) { Operation = Operations.Round; }
		public override string ShaderNodeTypeName => "math";
	}
	[ShaderNode("math_lessthan")]
	public class MathLess_Than : MathNode
	{
		public MathLess_Than() : this("an lessthan mathnode") {}
		public MathLess_Than(string name) : base(name) { Operation = Operations.Less_Than; }
		public override string ShaderNodeTypeName => "math";
	}
	[ShaderNode("math_greaterthan")]
	public class MathGreater_Than : MathNode
	{
		public MathGreater_Than() : this("an greaterthan mathnode") {}
		public MathGreater_Than(string name) : base(name) { Operation = Operations.Greater_Than; }
		public override string ShaderNodeTypeName => "math";
	}
	[ShaderNode("math_modulo")]
	public class MathModulo : MathNode
	{
		public MathModulo() : this("an modulo mathnode") {}
		public MathModulo(string name) : base(name) { Operation = Operations.Modulo; }
		public override string ShaderNodeTypeName => "math";
	}
	[ShaderNode("math_absolute")]
	public class MathAbsolute : MathNode
	{
		public MathAbsolute() : this("an absolute mathnode") {}
		public MathAbsolute(string name) : base(name) { Operation = Operations.Absolute; }
		public override string ShaderNodeTypeName => "math";
	}
}
