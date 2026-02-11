using ccl.Attributes;
using ccl.ShaderNodes.Sockets;
using System;

namespace ccl.ShaderNodes
{
	/// <summary>
	/// BevelNode input sockets
	/// </summary>
	public class BevelNodeInputs : Inputs
	{
		public VectorSocket Normal { get; set; }
		public FloatSocket Radius { get; private set; }
		public IntSocket Samples { get; private set; }

		public BevelNodeInputs(ShaderNode parentNode)
		{
			Normal = new VectorSocket(parentNode, "Normal", "normal");
			AddSocket(Normal);
			Radius = new FloatSocket(parentNode, "Radius", "radius");
			AddSocket(Radius);
			Samples = new IntSocket(parentNode, "Samples", "samples");
			AddSocket(Samples);
		}
	}

	/// <summary>
	/// BevelNode output sockets
	/// </summary>
	public class BevelNodeOutputs : Outputs
	{
		/// <summary>
		/// BevelNode new Normal
		/// </summary>
		public VectorSocket Normal { get; set; }

		internal BevelNodeOutputs(ShaderNode parentNode)
		{
			Normal = new VectorSocket(parentNode, "Normal", "normal");
			AddSocket(Normal);
		}
	}

	/// <summary>
	/// BevelNode
	/// </summary>
	[ShaderNode("bevel")]
	public class BevelNode : ShaderNode
	{
		/// <summary>
		/// BevelNode input sockets
		/// </summary>
		public BevelNodeInputs ins => (BevelNodeInputs)inputs;

		/// <summary>
		/// BevelNode output sockets
		/// </summary>
		public BevelNodeOutputs outs => (BevelNodeOutputs)outputs;

		/// <summary>
		/// Create new BevelNode with blend type Bump.
		/// </summary>
		public BevelNode(Shader shader) : this(shader, "a bevel node") { }
		public BevelNode(Shader shader, string name) : base(shader, name)
		{
			FinalizeConstructor();
		}

		internal BevelNode(Shader shader, IntPtr intPtr) : base(shader, intPtr)
		{
			FinalizeConstructor();
		}

		private void FinalizeConstructor()
		{
			inputs = new BevelNodeInputs(this);
			outputs = new BevelNodeOutputs(this);

			ins.Radius.Value = 0.1f;
			ins.Samples.Value = 4;
		}

		internal override void ParseXml(System.Xml.XmlReader xmlNode)
		{
			Utilities.Instance.get_float(ins.Radius, xmlNode.GetAttribute("radius"));
			Utilities.Instance.get_int(ins.Samples, xmlNode.GetAttribute("samples"));
		}
	}
}
