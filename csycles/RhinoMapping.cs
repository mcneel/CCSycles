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

namespace ccl
{

	/// <summary>
	/// Representation of a Cycles RhinoMapping.
	/// </summary>
	public class RhinoMapping
	{
		/// <summary>
		/// Id of the Cycles RhinoMappink.
		/// </summary>
		public System.IntPtr RhinoMappingPtr { get; }
		/// <summary>
		/// Reference to the client.
		/// </summary>
		private Session Client { get; }

		/// <summary>
		/// Create a new mesh for the given client using shader as the default shader
		/// </summary>
		/// <param name="client"></param>
		public RhinoMapping(Session client)
		{
			Client = client;

			RhinoMappingPtr = CSycles.add_rhinomapping(Client.Scene.Id);
		}

		public void SetPxyz(Transform transform)
		{
			CSycles.rhinomapping_set_pxyz(RhinoMappingPtr, transform);
		}

	}
}
