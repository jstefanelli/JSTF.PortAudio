using JSTF.PortAudio.Structs;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace JSTF.PortAudio.Managed
{
	public enum HostApiTypeId : int
	{
		InDevelopment = 0, /* use while developing support for a new host API */
		DirectSound = 1,
		MME = 2,
		ASIO = 3,
		SoundManager = 4,
		CoreAudio = 5,
		OSS = 7,
		ALSA = 8,
		AL = 9,
		BeOS = 10,
		WDMKS = 11,
		JACK = 12,
		WASAPI = 13,
		AudioScienceHPI = 14
	}

	public sealed class HostApiInfo
	{
		public int StructVersion;
		public HostApiTypeId Type;
		public string Name;
		public int DeviceCount;
		public int DefaultInputDevice;
		public int DefaultOutputDevice;

		internal HostApiInfo()
		{

		}

		public static HostApiInfo FromNative(IntPtr ptr)
		{
			PaHostApiInfo native = Marshal.PtrToStructure<PaHostApiInfo>(ptr);
			HostApiInfo managed = new HostApiInfo();
			managed.StructVersion = native.structVersion;
			managed.Type = (HostApiTypeId)native.type;
			managed.Name = Marshal.PtrToStringAnsi(native.name);
			managed.DeviceCount = native.deviceCount;
			managed.DefaultInputDevice = native.defaultInputDevice;
			managed.DefaultOutputDevice = native.defaultOutputDevice;
			return managed;
		}
	}

	public struct HostErrorInfo
	{
		public HostApiTypeId HostApiType;
		public long ErrorCode;
		public string ErrorText;

		public static HostErrorInfo FromNative(IntPtr ptr)
		{
			PaHostErrorInfo native = Marshal.PtrToStructure<PaHostErrorInfo>(ptr);
			HostErrorInfo managed = new HostErrorInfo();
			managed.HostApiType = (HostApiTypeId)native.hostApiType;
			managed.ErrorCode = native.errorCode;
			managed.ErrorText = Marshal.PtrToStringAnsi(native.errorText);
			return managed;
		}
	}
}
