﻿/**
Copyright 2014-2017 Robert McNeel and Associates

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
using ccl.ShaderNodes;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace ccl
{
	/// <summary>
	/// Representation of a Cycles shader outside of the Cycles backend. This
	/// class is used for creating a graph to serialise to code.
	/// </summary>
	public class CodeShader : Shader
	{
		public CodeShader(ShaderType type) : base(type)
		{
			Xml = "";
			Code = "";
		}

		public override void Recreate()
		{
			Code = "";
			Xml = "";
			m_nodes.Clear();

			Output = new OutputNode();
			AddNode(Output);
		}

		/// <summary>
		/// Tag shader for device update. No-op for CodeShader
		/// </summary>
		public override void Tag()
		{
		}


		public override void AddNode(ShaderNode node)
		{
			m_nodes.Add(node);
		}

		/// <summary>
		/// Iterate over nodes in shader
		/// </summary>
		public IEnumerable<ShaderNode> Nodes
		{
			get
			{
				foreach(var n in m_nodes)
				{
					yield return n;
				}
			}
		}

		/// <summary>
		/// C# code after call to FinalizeGraph()
		/// </summary>
		public string Code { get; private set; }

		/// <summary>
		/// XML code after call to FinalizeGraph()
		/// </summary>
		public string Xml { get; private set; }

		/// <summary>
		/// Finalizes the graph by connecting all sockets in Cycles as specified
		/// through code and generating both XML and C# strings that represent
		/// the node graph
		/// </summary>
		public override void FinalizeGraph()
		{
			var code = new StringBuilder($"var shader = new Shader(ccl.Shader.ShaderType.{Type});", 10240);
			var xml = new StringBuilder(1024);

			if (Verbose) System.Diagnostics.Debug.WriteLine($"Finalizing XML and Code {Name}");

			foreach (var node in m_nodes)
			{
				code.Append(node.CreateCode());
				xml.Append(node.CreateXml());
			}
			foreach (var node in m_nodes)
			{
				code.Append(node.CreateAddToShaderCode("shader"));
			}
			foreach (var node in m_nodes)
			{
				code.Append(node.CreateConnectCode());
				xml.Append(node.CreateConnectXml());
			}

			// clean up unnecessary new lines
			var regex = new Regex("(\n)(?<=\\1\\1)", RegexOptions.Compiled);

			Code = regex.Replace(code.ToString(), string.Empty);
			Xml = xml.ToString();
		}

	}
}
