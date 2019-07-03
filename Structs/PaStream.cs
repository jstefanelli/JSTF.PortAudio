using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace JSTF.PortAudio.Structs
{
	public delegate int PaStreamCallback(in IntPtr input, IntPtr output, ulong frameCount, ref PaStreamCallbackTimeInfo timeInfo, PaStreamCallbackFlags statusFlags, in IntPtr userData);
	public delegate void PaStreamFinishedCallback(IntPtr userData);

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PaStreamCallbackTimeInfo
	{
		public double inputBufferAdcTime;
		public double currentTime;
		public double outputBufferDacTime;
	}
	public enum PaStreamCallbackResult : int
	{
		paContinue = 0,
		paComplete = 1,
		paAbort = 2
	}

	public enum PaStreamCallbackFlags : ulong
	{
		paInputUnderflow = 0x00000001,
		paInputOverflow = 0x00000002,
		paOutputUnderflow = 0x00000004,
		paOutputOverflow = 0x00000008,
		paPrimingOutput = 0x00000010
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct PaStreamParameters
	{
		public int device;
		public int channelCount;
		public PaSampleFormat sampleFormat;
		public double suggestedLatency;
		public IntPtr hostApiSpecificStreamInfo;
	}

	public enum PaStreamFlags : ulong
	{
		paNoFlag = 0,
		paClipOff = 0x00000001,
		paDitherOff = 0x00000002,
		paNeverDropInput = 0x00000004,
		paPrimeOutputBuffersUsingStreamCallback = 0x00000008,
		paPlatformSpecificFlags = 0xFFFF0000
	}

	public struct PaStreamInfo
	{
		public int structVersion;
		public double inputLatency;
		public double outputLatency;
		public double sampleRate;
	}
}
