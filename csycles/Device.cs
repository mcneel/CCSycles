/**
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
using System.Collections.Generic;
using System.Linq;

namespace ccl
{
	/// <summary>
	/// Representation of a Cycles rendering device
	/// </summary>
	public class Device
	{
		/// <summary>
		/// Get the numerical ID for this device
		/// </summary>
		public uint Id { get; private set; }
		/// <summary>
		/// Get the Cycles description for the device
		/// </summary>
		public string Description { get; private set; }
		/// <summary>
		/// The name for the device
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Give a nice name for UI usage. PCI bus and other redundancy is removed.
		/// 
		/// For a multidevice the type of multi device and its subdevices is given.
		/// </summary>
		public string NiceName
		{
			get
			{
				if (IsCuda)
					return Name.Split('_')[1];
				if (IsOpenCl)
					return Name.Split('_')[2];
				if (IsMulti)
				{
					var n = string.Join(",", (from sd in Subdevices select sd.NiceName).ToList());
					var multiType = (from sd in Subdevices select sd.Type).First();
					return $"Multi ({multiType}): {n}";
				}
				return Name;
			}
		}
		/// <summary>
		/// Get the Cycles num for the device
		/// </summary>
		public uint Num { get; private set; }
		/// <summary>
		/// Get the device type
		/// </summary>
		public DeviceType Type { get; private set; }
		/// <summary>
		/// True if this device supports advanced shading
		/// </summary>
		public bool AdvancedShading { get; private set; }
		/// <summary>
		/// True if this device is used as a display device
		/// </summary>
		public bool DisplayDevice { get; private set; }
		/// <summary>
		/// True if this device supports packed images
		/// </summary>
		public bool PackImages { get; private set; }
		/// <summary>
		/// True if this is a CUDA device
		/// </summary>
		public bool IsCuda => Type == DeviceType.CUDA;

		/// <summary>
		/// True if this device is an OpenCL device
		/// </summary>
		public bool IsOpenCl => Type == DeviceType.OpenCL;

		/// <summary>
		/// True if this device is a CPU
		/// </summary>
		public bool IsCpu => Type == DeviceType.CPU;

		/// <summary>
		/// True if this device is a GPU
		/// </summary>
		public bool IsGpu => !IsCpu;

		/// <summary>
		/// True if this device is a Multi device
		/// </summary>
		public bool IsMulti => Type == DeviceType.Multi;

		/// <summary>
		/// True if this is a Multi CUDA device
		/// </summary>
		public bool IsMultiCuda => Type == DeviceType.Multi && Subdevices.Where((Device d) => d.Type == DeviceType.CUDA).Any();

		/// <summary>
		/// True if this is a Multi OpenCL device
		/// </summary>
		public bool IsMultiOpenCl => Type == DeviceType.Multi && Subdevices.Where((Device d) => d.Type == DeviceType.OpenCL).Any();

		/// <summary>
		/// String representation of this device
		/// </summary>
		/// <returns>String representation of this device</returns>
		public override string ToString() => $"{base.ToString()}: {Description} ({Type}), Id {Id} Num {Num} Name {Name} DisplayDevice {DisplayDevice} AdvancedShading {AdvancedShading} PackImages {PackImages}";

		/// <summary>
		/// Get the default device (CPU)
		/// </summary>
		/// <returns>The default device</returns>
		static public Device Default => GetDevice(0);

		/// <summary>
		/// Get capabilities of all devices that Cycles can see.
		/// </summary>
		static public string Capabilities => CSycles.device_capabilities();

		/// <summary>
		/// Get a device by using GetDevice(int idx). Constructor is private.
		/// </summary>
		private Device() { }

		/// <summary>
		/// Test if given ID is for this device
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool EqualsId(int id)
		{
			if (Type != DeviceType.Multi) return (int)Id == id;

			foreach (var sd in Subdevices)
			{
				if ((int)sd.Id == id) return true;
			}

			return false;
		}

		public bool EqualsId(uint id)
		{
			return EqualsId((int)id);
		}

		/// <summary>
		/// Get the number of available Cycles render devices
		/// </summary>
		/// <returns></returns>
		static public uint Count => CSycles.number_devices();

		/// <summary>
		/// Get the number of available Cycles render multi-devices
		/// </summary>
		static public uint MultiCount => CSycles.number_multidevices();

		/// <summary>
		/// Amount of sub-devices for this (multi-)device.
		/// </summary>
		public uint SubdeviceCount => CSycles.number_multi_subdevices((int)Id);

		/// <summary>
		/// Enumerator over subdevices of this multi devices.
		/// </summary>
		public IEnumerable<Device> Subdevices
		{
			get
			{
				for (var i = 0; i < SubdeviceCount; i++)
				{
					var j = CSycles.get_multidevice_subdevice_id((int)Id, i);
					yield return GetDevice((int)j);
				}
			}
		}

		/// <summary>
		/// Enumerate over sub-devices, returning a Tuple&lt;int, Device&gt; wher
		/// the int is index into global regular device list.
		/// </summary>
		public IEnumerable<Tuple<int, Device>> SubdevicesIndex
		{
			get
			{
				for (var i = 0; i < SubdeviceCount; i++)
				{
					var j = CSycles.get_multidevice_subdevice_id((int)Id, i);
					yield return new Tuple<int, Device>((int)j, GetDevice((int)j));
				}
			}
		}

		/// <summary>
		/// True if any of the available devices is a CUDA device
		/// </summary>
		/// <returns></returns>
		static public bool CudaAvailable()
		{
			return CSycles.number_cuda_devices() > 0;
		}

		public const int MultiOffset = 100000;
		/// <summary>
		/// Enumerate over available devices.
		/// </summary>
		static public IEnumerable<Device> Devices
		{
			get
			{
				for (var i = 0; i < Count; i++)
				{
					yield return GetDevice(i);
				}

				for (var j = 0; j < MultiCount; j++)
				{
					yield return GetDevice(j + MultiOffset);
				}
			}
		}


		/// <summary>
		/// Returns the first cuda device if it exists,
		/// the default rendering device (CPU) if not.
		/// </summary>
		static public Device FirstCuda
		{
			get
			{
				var d = (from device in Devices
								 where device.IsCuda || device.IsMultiCuda
								 select device).FirstOrDefault();
				return d ?? Default;
			}
		}

		/// <summary>
		/// Returns the first openCL device if it exists,
		/// the default rendering device (CPU) if not.
		/// </summary>
		static public Device FirstOpenCL
		{
			get
			{
				var d = (from device in Devices
								 where device.IsOpenCl || device.IsMultiOpenCl
								 select device).FirstOrDefault();
				return d ?? Default;
			}
		}

		/// <summary>
		/// Returns the first Multi OpenCL device if it exists,
		/// the default rendering device (CPU) if not.
		/// </summary>
		static public Device FirstMultiOpenCL
		{
			get
			{
				var d = (from device in Devices
								 where device.IsMultiOpenCl
								 select device).FirstOrDefault();
				return d ?? Default;
			}
		}

		/// <summary>
		/// Get the first GPU.
		/// </summary>
		static public Device FirstGpu
		{
			get
			{
				var d = (from device in Devices
								 where device.IsGpu
								 select device).FirstOrDefault();
				return d ?? Default;

			}
		}


		/// <summary>
		/// Get the device with specified index. For a multi-device the 
		/// ID starts at 100000
		/// </summary>
		/// <param name="idx"></param>
		/// <returns></returns>
		static public Device GetDevice(int idx)
		{
			return new Device
			{
				Id = (uint)idx,
				Description = CSycles.device_decription(idx),
				Name = CSycles.DeviceId(idx),
				Num = CSycles.device_num(idx),
				Type = CSycles.device_type(idx),
				AdvancedShading = CSycles.device_advanced_shading(idx),
				DisplayDevice = CSycles.device_display_device(idx),
				PackImages = CSycles.device_pack_images(idx)
			};
		}


		/// <summary>
		/// Create a multi-device from the given Device list.
		/// </summary>
		/// <param name="idx">List of Devices</param>
		/// <returns>Device representing the multi-device</returns>
		static public Device CreateMultiDevice(List<Device> idx)
		{
			idx.Sort((Device a, Device b) => {
				if (a.Id == b.Id) return 0;
				return a.Id < b.Id ? -1 : 1;
			});
			int[] sortedidx = idx.ConvertAll<int>((Device d) => ((int)d.Id)).ToArray();
			var id = CSycles.create_multidevice(sortedidx.Length, ref sortedidx);
			if (id < 0) return null;
			return GetDevice(id);
		}

		/// <summary>
		/// Parse given string into a list of integers.
		/// 
		/// The integers should correspond to indices of devices in the
		/// global list of regular render devices.
		/// </summary>
		/// <param name="res"></param>
		/// <returns>
		/// Sorted List of indices into global list of regular render devices.
		/// </returns>
		static public List<int> IdListFromString(string res)
		{
			var l = IdSetFromString(res).ToList();
			l.Sort();
			return l;
		}

		/// <summary>
		/// Generate string from device ID or IDs (in case of multidevice).
		/// 
		/// The resulting string can be used as input to DeviceFromString().
		/// </summary>
		public string DeviceString {
			get {
				if (!IsMulti) return $"{Id}";
				return string.Join(",", (from sd in Subdevices select sd.Id).ToList());
			}
		}
		/// <summary>
		/// Parse given string into a set of integers.
		/// </summary>
		/// <param name="res"></param>
		/// <returns>
		/// HashSet of indices into global list of regular render devices.
		/// </returns>
		static public HashSet<int> IdSetFromString(string res)
		{
			var cleaned = res.Trim().ToLowerInvariant().Split(',');
			HashSet<int> set = new HashSet<int>();
			foreach (var item in cleaned)
			{
				if (int.TryParse(item, out int x))
				{
					set.Add(x);
				}
			}
			return set;
		}

		/// <summary>
		/// Convert a list of id integers to a Device list.
		/// 
		/// The ids are indices into the global list of regular devices,
		/// i.e. used with GetDevice(int idx);
		/// </summary>
		/// <param name="ids"></param>
		/// <returns></returns>
		static public List<Device> DeviceListFromIntList(List<int> ids)
		{
			return ids.ConvertAll((int id) => GetDevice(id));
		}

		/// <summary>
		/// Get the device for given string. If the string doesn't parse correctly
		/// the CPU device is picked.
		/// </summary>
		/// <param name="res"></param>
		/// <returns></returns>
		static public Device DeviceFromString(string res)
		{
			var l = IdListFromString(res);

			if (l.Count == 0) return Default;
			if(l.Count == 1)
			{
				return l[0] == -1 ? FirstCuda : GetDevice(l[0]);
			}

			return CreateMultiDevice(DeviceListFromIntList(l));
		}
	}
}
