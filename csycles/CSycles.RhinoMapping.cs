/**
Copyright 2014-2024 Robert McNeel and Associates

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
using System.Runtime.InteropServices;

namespace ccl
{
	public partial class CSycles
	{
		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_rhinomapping_set_pxyz(IntPtr rhinoMappingId,
			float a, float b, float c, float d,
			float e, float f, float g, float h,
			float i, float j, float k, float l);
		public static void rhinomapping_set_pxyz(IntPtr rhinoMappingId, Transform t)
		{
			cycles_rhinomapping_set_pxyz(rhinoMappingId,
				t.x.x, t.x.y, t.x.z, t.x.w,
				t.y.x, t.y.y, t.y.z, t.y.w,
				t.z.x, t.z.y, t.z.z, t.z.w);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_rhinomapping_set_nxyz(IntPtr rhinoMappingId,
			float a, float b, float c, float d,
			float e, float f, float g, float h,
			float i, float j, float k, float l);
		public static void rhinomapping_set_nxyz(IntPtr rhinoMappingId, Transform t)
		{
			cycles_rhinomapping_set_nxyz(rhinoMappingId,
				t.x.x, t.x.y, t.x.z, t.x.w,
				t.y.x, t.y.y, t.y.z, t.y.w,
				t.z.x, t.z.y, t.z.z, t.z.w);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern void cycles_rhinomapping_set_uvw(IntPtr rhinoMappingId,
			float a, float b, float c, float d,
			float e, float f, float g, float h,
			float i, float j, float k, float l);
		public static void rhinomapping_set_uvw(IntPtr rhinoMappingId, Transform t)
		{
			cycles_rhinomapping_set_uvw(rhinoMappingId,
				t.x.x, t.x.y, t.x.z, t.x.w,
				t.y.x, t.y.y, t.y.z, t.y.w,
				t.z.x, t.z.y, t.z.z, t.z.w);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr cycles_scene_add_rhinomapping(IntPtr sessionId);
		public static IntPtr add_rhinomapping(IntPtr sessionId)
		{
			return cycles_scene_add_rhinomapping(sessionId);
		}

		[DllImport(Constants.ccycles, SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr cycles_delete_rhinomapping(IntPtr sessionId, IntPtr rhinoMappingId);
		public static IntPtr delete_rhinomapping(IntPtr sessionId, IntPtr rhinoMappingId)
		{
			return cycles_delete_rhinomapping(sessionId, rhinoMappingId);
		}

	}
}
