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
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using ccl;
using ccl.ShaderNodes;
using ccl.ShaderNodes.Sockets;

namespace ccl
{
	/// <summary>
	/// Representation of a Cycles shader
	/// </summary>
	public class Shader : IDisposable
	{
		public enum ShaderType
		{
			Material,
			World
		}
		/// <summary>
		/// Get the ID for this shader. This ID is given by CCycles
		/// </summary>
		public uint Id { get; }
		private Session Client { get; }
		public ShaderType Type { get; set; }

		private bool CreatedInCycles { get; set; }

		/// <summary>
		/// Print out debug information if set to true. Default false.
		/// </summary>
		public bool Verbose { get; set; }

		/// <summary>
		/// Get the output node for this shader.
		/// </summary>
		public OutputNode Output { get; internal set; }

		/// <summary>
		/// Create a new shader for client.
		/// </summary>
		/// <param name="client">Client ID for C[CS]ycles API.</param>
		/// <param name="type">The type of shader to create</param>
		public Shader(Session client, ShaderType type)
		{
			Client = client;
			Id = CSycles.create_shader(Client.Scene.Id);
			CommonConstructor(type, false);
		}

		/// <summary>
		/// Create front for existing shader. Note that
		/// functionality through this can be limited.
		/// Generally used only for interal matters
		/// </summary>
		/// <param name="client"></param>
		/// <param name="type"></param>
		/// <param name="id"></param>
		internal Shader(Session client, ShaderType type, uint id)
		{
			Client = client;
			Id = id;
			CommonConstructor(type, true);
		}

		private void CommonConstructor(ShaderType type, bool createdInCycles)
		{
			Type = type;
			Output = new OutputNode();
			AddNode(Output);
			CreatedInCycles = createdInCycles;
			Verbose = false;
		}

		/// <summary>
		/// Create a shader outside of the Cycles system. Can be used to set up a
		/// shader graph for serialisation purposes.
		/// </summary>
		/// <param name="type"></param>
		public Shader(ShaderType type)
		{
			Type = type;
			Output = new OutputNode();
		}

		/// <summary>
		/// Clear the shader graph for this node, so it can be repopulated.
		/// </summary>
		public virtual void Recreate()
		{
			CSycles.shader_new_graph(Client.Scene.Id, Id);

			CreatedInCycles = false;

			m_nodes.Clear();

			Output = new OutputNode();
			AddNode(Output);
		}

		/// <summary>
		/// Tag shader for device update
		/// </summary>
		public virtual void Tag()
		{
			Tag(true);
		}
		public virtual void Tag(bool use)
		{
			CSycles.scene_tag_shader(Client.Scene.Id, Id, use);
		}

		/// <summary>
		/// Static constructor for wrapping default surface shader created by Cycles shader manager.
		/// </summary>
		/// <param name="client"></param>
		/// <returns></returns>
		static public Shader WrapDefaultSurfaceShader(Session client)
		{
			var shader = new Shader(client, ShaderType.Material, CSycles.DEFAULT_SURFACE_SHADER) {Name = "default_surface"};

			// just add nodes so we have local node presentation, but no need to actually finalise
			// since it already exists in Cycles.
			var diffuse_bsdf = new DiffuseBsdfNode();
			diffuse_bsdf.ins.Color.Value = new float4(0.8f);

			shader.AddNode(diffuse_bsdf);

			diffuse_bsdf.outs.BSDF.Connect(shader.Output.ins.Surface);

			return shader;
		}

		/// <summary>
		/// Static constructor for wrapping default light shader created by Cycles shader manager.
		/// </summary>
		/// <param name="client"></param>
		/// <returns></returns>
		static public Shader WrapDefaultLightShader(Session client)
		{
			var shader = new Shader(client, ShaderType.Material, CSycles.DEFAULT_LIGHT_SHADER) {Name = "default_light"};

			// just add nodes so we have local node presentation, but no need to actually finalise
			// since it already exists in Cycles.
			var emission_node = new EmissionNode();
			emission_node.ins.Color.Value = new float4(0.8f);
			emission_node.ins.Strength.Value = 0.0f;

			shader.AddNode(emission_node);

			emission_node.outs.Emission.Connect(shader.Output.ins.Surface);

			return shader;
		}

		/// <summary>
		/// Static constructor for wrapping default background shader created by Cycles shader manager.
		/// </summary>
		/// <param name="client"></param>
		/// <returns></returns>
		static public Shader WrapDefaultBackgroundShader(Session client)
		{
			var shader = new Shader(client, ShaderType.World, CSycles.DEFAULT_BACKGROUND_SHADER) {Name = "default_background"};

			return shader;
		}

		/// <summary>
		/// Static constructor for wrapping default empty shader created by Cycles shader manager.
		/// </summary>
		/// <param name="client"></param>
		/// <returns></returns>
		static public Shader WrapDefaultEmptyShader(Session client)
		{
			var shader = new Shader(client, ShaderType.Material, CSycles.DEFAULT_EMPTY_SHADER) {Name = "default_empty"};

			return shader;
		}

		readonly internal List<ShaderNode> m_nodes = new List<ShaderNode>();
		/// <summary>
		/// Add a ShaderNode to the shader. This will create the node in Cycles, set
		/// any values for sockets and direct members.
		/// </summary>
		/// <param name="node">ShaderNode to add</param>
		public virtual void AddNode(ShaderNode node)
		{
			if (node is OutputNode)
			{
				node.Id = CSycles.OUTPUT_SHADERNODE_ID;
				m_nodes.Add(node);
				return;
			}

			if (CreatedInCycles)
			{
				m_nodes.Add(node);
				return;
			}

			var nodeid = CSycles.add_shader_node(Client.Scene.Id, Id, node.Type);
			node.Id = nodeid;
			m_nodes.Add(node);
		}

		/// <summary>
		/// Finalizes the graph by connecting all sockets in Cycles as specified
		/// through code.
		///
		/// This step also commits any values set to input sockets, enumerations
		/// and direct member variables.
		/// </summary>
		public virtual void FinalizeGraph()
		{
			if (Verbose)
			{
				Utilities.ConsoleWrite($"Finalizing {Name}");
			}
			foreach (var node in m_nodes)
			{
				/* set enumerations */
				node.SetEnums(Client.Scene.Id, Id);

				/* set direct member variables */
				node.SetDirectMembers(Client.Scene.Id, Id);

				if (node.inputs == null) continue;

				node.SetSockets(Client.Scene.Id, Id);

				foreach (var socket in node.inputs.Sockets)
				{
					var from = socket.ConnectionFrom;
					if (from == null) continue;
					if (Verbose)
					{
						Utilities.ConsoleWrite($"Shader {Name}: Connecting {from.Path} to {socket.Path}\n");
					}
					Connect(from.Parent, from.Name, node, socket.Name);
				}
			}
		}

		/// <summary>
		/// Make the actual connection between nodes.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="fromout"></param>
		/// <param name="to"></param>
		/// <param name="toin"></param>
		private void Connect(ShaderNode from, string fromout, ShaderNode to, string toin)
		{
			if (m_nodes.Contains(from) && m_nodes.Contains(to))
			{
				CSycles.shader_connect_nodes(Client.Scene.Id, Id, from.Id, fromout, to.Id, toin);
			}
			else
			{
				throw new ArgumentException($"Cannot connect {@from} to {to}");
			}
		}

		private string m_name;
	private bool disposedValue;

	/// <summary>
	/// Set the name of the Shader
	/// </summary>
	public string Name
		{
			set
			{
				m_name = value;
				if(!CreatedInCycles && Client!=null) CSycles.shader_set_name(Client.Scene.Id, Id, m_name);
			}
			get
			{
				return m_name;
			}
		}

		/// <summary>
		/// Set to true if multiple importance sampling is to be used
		/// </summary>
		public bool UseMis
		{
			set
			{
				if(!CreatedInCycles && Client!=null) CSycles.shader_set_use_mis(Client.Scene.Id, Id, value);
			}
		}

		/// <summary>
		/// Set to true if this shader supports transparent shadows.
		/// </summary>
		public bool UseTransparentShadow
		{
			set
			{
				if(!CreatedInCycles && Client!=null) CSycles.shader_set_use_transparent_shadow(Client.Scene.Id, Id, value);
			}
		}

		/// <summary>
		/// Set to true if this shader is used for a heterogeneous volume.
		/// </summary>
		public bool HeterogeneousVolume
		{
			set
			{
				if(!CreatedInCycles && Client!=null) CSycles.shader_set_heterogeneous_volume(Client.Scene.Id, Id, value);
			}
		}

		/// <summary>
		/// Create node graph in the given shader from the passed XML.
		///
		/// Note that you should call FinalizeGraph on shader if you are not further
		/// changing the graph.
		/// </summary>
		/// <param name="shader">Shader to populate with nodes from the XML representation.</param>
		/// <param name="shaderXml">The XML representation for the shader.</param>
		/// <param name="finalize">Set to true if the shader should be finalized.</param>
		public static void ShaderFromXml(Shader shader, string shaderXml, bool finalize)
		{
			var xmlmem = Encoding.UTF8.GetBytes(shaderXml);
			using (var xmlstream = new MemoryStream(xmlmem))
			{
				var settings = new XmlReaderSettings
				{
					ConformanceLevel = ConformanceLevel.Fragment,
					IgnoreComments = true,
					IgnoreProcessingInstructions = true,
					IgnoreWhitespace = true
				};
				var reader = XmlReader.Create(xmlstream, settings);
				Utilities.Instance.ReadNodeGraph(shader, reader, finalize);
			}
		}

	protected virtual void Dispose(bool disposing)
	{
	  if (!disposedValue)
	  {
		if (disposing)
		{
		  m_nodes.Clear();
		}
		disposedValue = true;
	  }
	}

	public void Dispose()
	{
	  // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
	  Dispose(disposing: true);
	  GC.SuppressFinalize(this);
	}
  }
}
