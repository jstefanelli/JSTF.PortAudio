using JSTF.PortAudio.Structs;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace JSTF.PortAudio.Managed
{
	public struct StreamCallbackTimeInfo
	{
		public double InputBufferAdcTime;
		public double CurrentTime;
		public double OutputBufferDacTime;

		public static StreamCallbackTimeInfo FromNative(IntPtr ptr)
		{
			PaStreamCallbackTimeInfo native = Marshal.PtrToStructure<PaStreamCallbackTimeInfo>(ptr);
			StreamCallbackTimeInfo managed = new StreamCallbackTimeInfo();
			managed.InputBufferAdcTime = native.inputBufferAdcTime;
			managed.CurrentTime = native.currentTime;
			managed.OutputBufferDacTime = native.outputBufferDacTime;
			return managed;
		}
	}
	public enum StreamCallbackResult : int
	{
		Continue = 0,
		Complete = 1,
		Abort = 2
	}

	public enum StreamCallbackFlags : ulong
	{
		InputUnderflow = 0x00000001,
		InputOverflow = 0x00000002,
		OutputUnderflow = 0x00000004,
		OutputOverflow = 0x00000008,
		PrimingOutput = 0x00000010
	}

	public struct StreamParameters<T> where T : struct
	{
		public int Device;
		public int ChannelCount;
		public SampleFormat SampleFormat;
		public double SuggestedLatency;
		public T? HostApiSpecificStreamInfo;
	}

	internal enum StreamFlags : ulong
	{
		NoFlag = 0,
		ClipOff = 0x00000001,
		DitherOff = 0x00000002,
		NeverDropInput = 0x00000004,
		PrimeOutputBuffersUsingStreamCallback = 0x00000008,
		PlatformSpecificFlags = 0xFFFF0000
	}

	public struct StreamInfo
	{
		public int StructVersion;
		public double InputLatency;
		public double OutputLatency;
		public double SampleRate;
	}
}
