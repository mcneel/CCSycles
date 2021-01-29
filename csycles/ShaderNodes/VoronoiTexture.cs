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
using System.Text;

namespace ccl.ShaderNodes
{
	public class VoronoiInputs : Inputs
	{
		public VectorSocket Vector { get; set; }
		public FloatSocket Scale { get; set; }

		public VoronoiInputs(ShaderNode parentNode)
		{
			Vector = new VectorSocket(parentNode, "Vector");
			AddSocket(Vector);
			Scale = new FloatSocket(parentNode, "Scale");
			AddSocket(Scale);
		}
	}

	public class VoronoiOutputs : Outputs
	{
		public ColorSocket Color { get; set; }
		public FloatSocket Fac { get; set; }

		public VoronoiOutputs(ShaderNode parentNode)
		{
			Color = new ColorSocket(parentNode, "Color");
			AddSocket(Color);
			Fac = new FloatSocket(parentNode, "Fac");
			AddSocket(Fac);
		}
	}

	[ShaderNode("voronoi_texture")]
	public class VoronoiTexture : TextureNode
	{

		public enum ColoringTypes
		{
			Intensity,
			Cells,
		}

		public enum Metrics
		{
			Distance,
			Manhattan,
			Chebychev,
			Minkowski,
		}

		public enum Features
		{
			F1,
			F2,
			F3,
			F4,
			F2F1,
		}
		public VoronoiInputs ins => (VoronoiInputs)inputs;
		public VoronoiOutputs outs => (VoronoiOutputs)outputs;

		public VoronoiTexture() : this("a voronoi texture") { }
		public VoronoiTexture(string name)
			: base(ShaderNodeType.VoronoiTexture, name)
		{
			inputs = new VoronoiInputs(this);
			outputs = new VoronoiOutputs(this);

			ins.Scale.Value = 1.0f;
		}

		/// <summary>
		/// One of:
		/// - Cells
		/// - Intensity
		/// </summary>
		public ColoringTypes Coloring { get; set; } = ColoringTypes.Cells;
		public Metrics Metric { get; set; } = Metrics.Distance;
		public Features Feature { get; set; } = Features.F1;

		internal override void SetEnums(uint clientId, uint sceneId, uint shaderId)
		{
			CSycles.shadernode_set_enum(clientId, sceneId, shaderId, Id, Type, "coloring", (int)Coloring);
			CSycles.shadernode_set_enum(clientId, sceneId, shaderId, Id, Type, "metric", (int)Metric);
			CSycles.shadernode_set_enum(clientId, sceneId, shaderId, Id, Type, "feature", (int)Feature);
		}

		internal override void ParseXml(XmlReader xmlNode)
		{
			Utilities.Instance.get_float4(ins.Vector, xmlNode.GetAttribute("vector"));
			Utilities.Instance.get_float(ins.Scale, xmlNode.GetAttribute("scale"));
			var coloring = xmlNode.GetAttribute("coloring");
			if (!string.IsNullOrEmpty(coloring))
			{
				if (Enum.TryParse(coloring, out ColoringTypes ct))
					Coloring = ct;
			}
			var metric = xmlNode.GetAttribute("metric");
			if (!string.IsNullOrEmpty(metric))
			{
				if (Enum.TryParse(metric, out Metrics ct))
					Metric = ct;
			}
			var feature = xmlNode.GetAttribute("feature");
			if (!string.IsNullOrEmpty(feature))
			{
				if (Enum.TryParse(feature, out Features ct))
					Feature = ct;
			}
		}
		public override string CreateXmlAttributes()
		{
			var code = new StringBuilder($" coloring=\"{Coloring}\" ", 1024);
			code.Append($" metric=\"{Metric}\" ");
			code.Append($" feature=\"{Feature}\" ");

			return code.ToString();
		}
		public override string CreateCodeAttributes()
		{
			var code = new StringBuilder($"{VariableName}.Coloring = {Coloring};", 1024);
			code.Append( $"{VariableName}.Metric = {Metric};");
			code.Append($"{VariableName}.Feature = {Feature};");

			return code.ToString();
		}
	}
}
