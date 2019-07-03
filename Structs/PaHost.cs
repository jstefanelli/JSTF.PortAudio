using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace JSTF.PortAudio.Structs
{
	public enum PaHostApiTypeId : int
	{
		paInDevelopment = 0, /* use while developing support for a new host API */
		paDirectSound = 1,
		paMME = 2,
		paASIO = 3,
		paSoundManager = 4,
		paCoreAudio = 5,
		paOSS = 7,
		paALSA = 8,
		paAL = 9,
		paBeOS = 10,
		paWDMKS = 11,
		paJACK = 12,
		paWASAPI = 13,
		paAudioScienceHPI = 14
	}

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PaHostApiInfo
	{
		/// this is struct version 1
		public int structVersion;
		/// The well known unique identifier of this host API @see PaHostApiTypeId
		public PaHostApiTypeId type;
		/// A textual description of the host API for display on user interfaces.
		public IntPtr name;

		/// The number of devices belonging to this host API. This field may be
		/// used in conjunction with Pa_HostApiDeviceIndexToDeviceIndex() to enumerate
		/// all devices for this host API.
		public int deviceCount;

		/// The default input device for this host API. The value will be a
		/// device index ranging from 0 to (Pa_GetDeviceCount()-1), or paNoDevice
		/// if no default input device is available.
		public int defaultInputDevice;

		/// The default output device for this host API. The value will be a
		/// device index ranging from 0 to (Pa_GetDeviceCount()-1), or paNoDevice
		/// if no default output device is available.
		public int defaultOutputDevice;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PaHostErrorInfo
	{
		public PaHostApiTypeId hostApiType;
		public long errorCode;
		public IntPtr errorText;
	}
}
