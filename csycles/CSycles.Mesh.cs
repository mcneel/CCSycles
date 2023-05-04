using System.Runtime.InteropServices;

namespace ccl
{
	public partial class CSycles
	{
#region mesh
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private unsafe static extern void cycles_mesh_set_verts(uint sceneId, uint meshId, float* verts, uint vcount);
		public static void mesh_set_verts(uint sceneId, uint meshId, ref float[] verts, uint vcount)
		{
			unsafe
			{
				fixed (float* pverts = verts)
				{
					cycles_mesh_set_verts(sceneId, meshId, pverts, vcount);
				}
			}
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private unsafe static extern void cycles_mesh_set_uvs(uint sceneId, uint meshId, float* uvs, uint uvcount, [MarshalAs(UnmanagedType.LPStr)] string uvmap_name);
		public static void mesh_set_uvs(uint sceneId, uint meshId, ref float[] uvs, uint uvcount, string uvmap_name)
		{
			unsafe
			{
				fixed (float* puvs = uvs)
				{
					cycles_mesh_set_uvs(sceneId, meshId, puvs, uvcount, uvmap_name);
				}
			}
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private unsafe static extern void cycles_mesh_set_vertex_normals(uint sceneId, uint meshId, float* vertex_normals, uint vncount);
		public static void mesh_set_vertex_normals(uint sceneId, uint meshId, ref float[] vertex_normals, uint vncount)
		{
			unsafe
			{
				fixed (float* pvertex_normals = vertex_normals)
				{
					cycles_mesh_set_vertex_normals(sceneId, meshId, pvertex_normals, vncount);
				}
			}
		}
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private unsafe static extern void cycles_mesh_set_vertex_colors(uint sceneId, uint meshId, float* vertex_colors, uint vccount);
		public static void mesh_set_vertex_colors(uint sceneId, uint meshId, ref float[] vertex_colors, uint vccount)
		{
			unsafe
			{
				fixed (float* pvertex_colors = vertex_colors)
				{
					cycles_mesh_set_vertex_colors(sceneId, meshId, pvertex_colors, vccount);
				}
			}
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private unsafe static extern void cycles_mesh_set_tris(uint sceneId, uint meshId, int* faces, uint fcount, uint shaderId, uint smooth);
		public static void mesh_set_tris(uint sceneId, uint meshId, ref int[] tris, uint fcount, uint shaderId, bool smooth)
		{
			unsafe
			{
				fixed (int* ptris = tris)
				{
					cycles_mesh_set_tris(sceneId, meshId, ptris, fcount, shaderId, (uint)(smooth ? 1 : 0));
				}
			}
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_mesh_set_triangle(uint sceneId, uint meshId, uint tri_idx, uint v0, uint v1, uint v2, uint shaderId, uint smooth);

		public static void mesh_set_triangle(uint sceneId, uint meshId, uint tri_idx, uint v0, uint v1, uint v2,
			uint shaderId, bool smooth)
		{
			cycles_mesh_set_triangle(sceneId, meshId, tri_idx, v0, v1, v2, shaderId, (uint)(smooth ? 1 : 0));
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_mesh_add_triangle(uint sceneId, uint meshId, uint v0, uint v1, uint v2, uint shaderId, uint smooth);

		public static void mesh_add_triangle(uint sceneId, uint meshId, uint v0, uint v1, uint v2,
			uint shaderId, bool smooth)
		{
			cycles_mesh_add_triangle(sceneId, meshId, v0, v1, v2, shaderId, (uint)(smooth ? 1 : 0));
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_mesh_set_smooth(uint sceneId, uint meshId, uint smooth);

		public static void mesh_set_smooth(uint sceneId, uint meshId, bool smooth)
		{
			cycles_mesh_set_smooth(sceneId, meshId, (uint)(smooth ? 1 : 0));
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_mesh_set_shader(uint sceneId, uint meshId, uint shader);

		public static void mesh_set_shader(uint sceneId, uint meshId, uint shaderId)
		{
			cycles_mesh_set_shader(sceneId, meshId, shaderId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_mesh_clear(uint sceneId, uint meshId);

		public static void mesh_clear(uint sceneId, uint meshId)
		{
			cycles_mesh_clear(sceneId, meshId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_mesh_resize(uint sceneId, uint meshId, uint vcount, uint fcount);

		public static void mesh_resize(uint sceneId, uint meshId, uint vcount, uint fcount)
		{
			cycles_mesh_resize(sceneId, meshId, vcount, fcount);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_mesh_reserve(uint sceneId, uint meshId, uint vcount, uint fcount);

		public static void mesh_reserve(uint sceneId, uint meshId, uint vcount, uint fcount)
		{
			cycles_mesh_reserve(sceneId, meshId, vcount, fcount);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_mesh_tag_rebuild(uint sceneId, uint meshId);

		public static void mesh_tag_rebuild(uint sceneId, uint meshId)
		{
			cycles_mesh_tag_rebuild(sceneId, meshId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_mesh_attr_tangentspace(uint sceneId, uint meshId, [MarshalAs(UnmanagedType.LPStr)] string uvmap_name);

		public static void mesh_attr_tangentspace(uint sceneId, uint meshId, string uvmap_name)
		{
			cycles_mesh_attr_tangentspace(sceneId, meshId, uvmap_name);
		}

#endregion

	}
}
