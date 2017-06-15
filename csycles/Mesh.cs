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

namespace ccl
{
	/// <summary>
	/// Representation of a Cycles mesh.
	/// </summary>
	public class Mesh
	{
		/// <summary>
		/// Id of mesh in scene.
		/// </summary>
		public uint Id { get; }
		/// <summary>
		/// Reference to client.
		/// </summary>
		private Client Client { get; }
		/// <summary>
		/// Shader used for this mesh.
		/// </summary>
		private Shader Shader { get; set; }

		/// <summary>
		/// Create a new mesh for the given client using shader as the default shader
		/// </summary>
		/// <param name="client"></param>
		/// <param name="shader"></param>
		public Mesh(Client client, Shader shader)
		{
			Client = client;
			Shader = shader;

			Id = CSycles.scene_add_mesh(Client.Id, Client.Scene.Id, Client.Scene.GetShaderSceneId(shader));
			System.Diagnostics.Trace.WriteLine($"Created mesh {Id} with shader {shader.Id}\n");
		}

		/// <summary>
		/// Constructor to use when a mesh that already exists in Cycles needs to be represented.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="id"></param>
		/// <param name="shader"></param>
		internal Mesh(Client client, uint id, Shader shader)
		{
			Client = client;
			Shader = shader;

			Id = id;
		}

		/// <summary>
		/// Clears out any pushed data
		/// </summary>
		public void ClearData()
		{
			CSycles.mesh_clear(Client.Id, Client.Scene.Id, Id);
		}

		/// <summary>
		/// Use given <c>shader</c> as new shader.
		/// </summary>
		/// <param name="shader"></param>
		public void ReplaceShader(Shader shader)
		{
			System.Diagnostics.Trace.WriteLine($"on mesh {Id} replacing {Shader.Id} with {shader.Id}\n");
			Shader = shader;
			CSycles.mesh_set_shader(Client.Id, Client.Scene.Id, Id, Client.Scene.GetShaderSceneId(Shader));
			TagRebuild();
		}

		/// <summary>
		/// Tag for update and rebuild
		/// </summary>
		public void TagRebuild()
		{
			CSycles.mesh_tag_rebuild(Client.Id, Client.Scene.Id, Id);
		}

		/// <summary>
		/// Reserve memory for mesh data
		/// </summary>
		/// <param name="vcount"></param>
		/// <param name="fcount"></param>
		public void Reserve(uint vcount, uint fcount)
		{
			CSycles.mesh_reserve(Client.Id, Client.Scene.Id, Id, vcount, fcount);
		}

		/// <summary>
		/// Resize mesh data to given counts
		/// </summary>
		/// <param name="vcount"></param>
		/// <param name="fcount"></param>
		public void Resize(uint vcount, uint fcount)
		{
			CSycles.mesh_resize(Client.Id, Client.Scene.Id, Id, vcount, fcount);
		}

		/// <summary>
		/// Set vertex coordinates
		/// </summary>
		/// <param name="verts"></param>
		public void SetVerts(ref float[] verts)
		{
			CSycles.mesh_set_verts(Client.Id, Client.Scene.Id, Id, ref verts, (uint) (verts.Length/3));
		}

		/// <summary>
		/// Set trifaces
		/// </summary>
		/// <param name="faces"></param>
		/// <param name="smooth"></param>
		public void SetVertTris(ref int[] faces, bool smooth)
		{
			CSycles.mesh_set_tris(Client.Id, Client.Scene.Id, Id, ref faces, (uint) (faces.Length/3), Client.Scene.GetShaderSceneId(Shader), smooth);
		}

		/// <summary>
		/// Set vertex normals
		/// </summary>
		/// <param name="vertex_normals"></param>
		public void SetVertNormals(ref float[] vertex_normals)
		{
			CSycles.mesh_set_vertex_normals(Client.Id, Client.Scene.Id, Id, ref vertex_normals, (uint) (vertex_normals.Length/3));
		}

		/// <summary>
		/// Set UVs
		/// </summary>
		/// <param name="uvs"></param>
		public void SetUvs(ref float[] uvs)
		{
			CSycles.mesh_set_uvs(Client.Id, Client.Scene.Id, Id, ref uvs, (uint) (uvs.Length/2));
		}

		/// <summary>
		/// Set vertex colors
		/// </summary>
		/// <param name="uvs"></param>
		public void SetVertexColors(ref float[] vertexcolors)
		{
			CSycles.mesh_set_vertex_colors(Client.Id, Client.Scene.Id, Id, ref vertexcolors, (uint) (vertexcolors.Length/3));
		}

		/// <summary>
		/// Add a triangle to the mesh using the given vertex coordinates, shader and smooth flag
		/// </summary>
		/// <param name="v0"></param>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <param name="shader"></param>
		/// <param name="smooth"></param>
		public void AddTri(uint v0, uint v1, uint v2, Shader shader, bool smooth)
		{
			CSycles.mesh_add_triangle(Client.Id, Client.Scene.Id, Id, v0, v1, v2, Client.Scene.GetShaderSceneId(shader), smooth);
		}

		/// <summary>
		/// Set a triangle to the mesh at given triangle idx using the given vertex coordinates, shader and smooth flag
		/// </summary>
		/// <param name="idx"></param>
		/// <param name="v0"></param>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <param name="shader"></param>
		/// <param name="smooth"></param>
		public void SetTri(uint idx, uint v0, uint v1, uint v2, Shader shader, bool smooth)
		{
			CSycles.mesh_set_triangle(Client.Id, Client.Scene.Id, Id, idx, v0, v1, v2, Client.Scene.GetShaderSceneId(shader), smooth);
		}
	}
}
