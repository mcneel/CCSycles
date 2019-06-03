using System.Runtime.InteropServices;

namespace ccl
{
	public partial class CSycles
	{
#region mesh
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private unsafe static extern void cycles_mesh_set_verts(uint clientId, uint sceneId, uint meshId, float* verts, uint vcount);
		public static void mesh_set_verts(uint clientId, uint sceneId, uint meshId, ref float[] verts, uint vcount)
		{
			unsafe
			{
				fixed (float* pverts = verts)
				{
					cycles_mesh_set_verts(clientId, sceneId, meshId, pverts, vcount);
				}
			}
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private unsafe static extern void cycles_mesh_set_uvs(uint clientId, uint sceneId, uint meshId, float* uvs, uint uvcount);
		public static void mesh_set_uvs(uint clientId, uint sceneId, uint meshId, ref float[] uvs, uint uvcount)
		{
			unsafe
			{
				fixed (float* puvs = uvs)
				{
					cycles_mesh_set_uvs(clientId, sceneId, meshId, puvs, uvcount);
				}
			}
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private unsafe static extern void cycles_mesh_set_vertex_normals(uint clientId, uint sceneId, uint meshId, float* vertex_normals, uint vncount);
		public static void mesh_set_vertex_normals(uint clientId, uint sceneId, uint meshId, ref float[] vertex_normals, uint vncount)
		{
			unsafe
			{
				fixed (float* pvertex_normals = vertex_normals)
				{
					cycles_mesh_set_vertex_normals(clientId, sceneId, meshId, pvertex_normals, vncount);
				}
			}
		}
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private unsafe static extern void cycles_mesh_set_vertex_colors(uint clientId, uint sceneId, uint meshId, float* vertex_colors, uint vccount);
		public static void mesh_set_vertex_colors(uint clientId, uint sceneId, uint meshId, ref float[] vertex_colors, uint vccount)
		{
			unsafe
			{
				fixed (float* pvertex_colors = vertex_colors)
				{
					cycles_mesh_set_vertex_colors(clientId, sceneId, meshId, pvertex_colors, vccount);
				}
			}
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private unsafe static extern void cycles_mesh_set_tris(uint clientId, uint sceneId, uint meshId, int* faces, uint fcount, uint shaderId, uint smooth);
		public static void mesh_set_tris(uint clientId, uint sceneId, uint meshId, ref int[] tris, uint fcount, uint shaderId, bool smooth)
		{
			unsafe
			{
				fixed (int* ptris = tris)
				{
					cycles_mesh_set_tris(clientId, sceneId, meshId, ptris, fcount, shaderId, (uint)(smooth ? 1 : 0));
				}
			}
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_mesh_set_triangle(uint clientId, uint sceneId, uint meshId, uint tri_idx, uint v0, uint v1, uint v2, uint shaderId, uint smooth);

		public static void mesh_set_triangle(uint clientId, uint sceneId, uint meshId, uint tri_idx, uint v0, uint v1, uint v2,
			uint shaderId, bool smooth)
		{
			cycles_mesh_set_triangle(clientId, sceneId, meshId, tri_idx, v0, v1, v2, shaderId, (uint)(smooth ? 1 : 0));
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_mesh_add_triangle(uint clientId, uint sceneId, uint meshId, uint v0, uint v1, uint v2, uint shaderId, uint smooth);

		public static void mesh_add_triangle(uint clientId, uint sceneId, uint meshId, uint v0, uint v1, uint v2,
			uint shaderId, bool smooth)
		{
			cycles_mesh_add_triangle(clientId, sceneId, meshId, v0, v1, v2, shaderId, (uint)(smooth ? 1 : 0));
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_mesh_set_smooth(uint clientId, uint sceneId, uint meshId, uint smooth);

		public static void mesh_set_smooth(uint clientId, uint sceneId, uint meshId, bool smooth)
		{
			cycles_mesh_set_smooth(clientId, sceneId, meshId, (uint)(smooth ? 1 : 0));
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_mesh_set_shader(uint clientId, uint sceneId, uint meshId, uint shader);

		public static void mesh_set_shader(uint clientId, uint sceneId, uint meshId, uint shaderId)
		{
			cycles_mesh_set_shader(clientId, sceneId, meshId, shaderId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_mesh_clear(uint clientId, uint sceneId, uint meshId);

		public static void mesh_clear(uint clientId, uint sceneId, uint meshId)
		{
			cycles_mesh_clear(clientId, sceneId, meshId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_mesh_resize(uint clientId, uint sceneId, uint meshId, uint vcount, uint fcount);

		public static void mesh_resize(uint clientId, uint sceneId, uint meshId, uint vcount, uint fcount)
		{
			cycles_mesh_resize(clientId, sceneId, meshId, vcount, fcount);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_mesh_reserve(uint clientId, uint sceneId, uint meshId, uint vcount, uint fcount);

		public static void mesh_reserve(uint clientId, uint sceneId, uint meshId, uint vcount, uint fcount)
		{
			cycles_mesh_reserve(clientId, sceneId, meshId, vcount, fcount);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_mesh_tag_rebuild(uint clientId, uint sceneId, uint meshId);

		public static void mesh_tag_rebuild(uint clientId, uint sceneId, uint meshId)
		{
			cycles_mesh_tag_rebuild(clientId, sceneId, meshId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_mesh_attr_tangentspace(uint clientId, uint sceneId, uint meshId);

		public static void mesh_attr_tangentspace(uint clientId, uint sceneId, uint meshId)
		{
			cycles_mesh_attr_tangentspace(clientId, sceneId, meshId);
		}

#endregion

	}
}
