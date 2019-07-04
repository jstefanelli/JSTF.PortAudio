using JSTF.PortAudio.Structs;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace JSTF.PortAudio.Managed
{
	public sealed class DeviceInfo
	{
		public int StructVersion;  /* this is struct version 2 */
		public string Name;
		public int HostApi; /**< note this is a host API index, not a type id*/

		public int MaxInputChannels;
		public int MaxOutputChannels;

		/** Default latency values for interactive performance. */
		public double DefaultLowInputLatency;
		public double DefaultLowOutputLatency;
		/** Default latency values for robust non-interactive applications (eg. playing sound files). */
		public double DefaultHighInputLatency;
		public double DefaultHighOutputLatency;

		public double DefaultSampleRate;

		internal DeviceInfo(){
			
		}

		public static DeviceInfo FromNative(IntPtr ptr)
		{
			PaDeviceInfo native = Marshal.PtrToStructure<PaDeviceInfo>(ptr);
			DeviceInfo managed = new DeviceInfo();
			managed.StructVersion = native.structVersion;
			managed.Name = Marshal.PtrToStringAnsi(native.name);
			managed.HostApi = native.hostApi;
			managed.DefaultLowInputLatency = native.defaultLowInputLatency;
			managed.DefaultLowOutputLatency = native.defaultLowOutputLatency;
			managed.DefaultHighInputLatency = native.defaultHighInputLatency;
			managed.DefaultHighOutputLatency = native.defaultHighOutputLatency;
			managed.DefaultSampleRate = native.defaultSampleRate;
			return managed;
		}
	}
}
