using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace JST.PortAudio
{
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PaStreamCallbackTimeInfo
	{
		public double inputBufferAdcTime;
		public double currentTime;
		public double outputBufferDacTime;
	}
	public enum PaStreamCallbackResult : uint
	{
		paContinue=0,   
		paComplete=1,   
		paAbort=2       
	}

	public enum PaStreamCallbackFlags : ulong
	{
		paInputUnderflow = 0x00000001,
		paInputOverflow = 0x00000002,
		paOutputUnderflow = 0x00000004,
		paOutputOverflow = 0x00000008,
		paPrimingOutput = 0x00000010
	}

	public static class PA
	{
		/// <summary>
		/// Functions of type PaStreamCallback are implemented by PortAudio clients. They consume, process or generate audio in response to requests from an active PortAudio stream.
		/// </summary>
		/// <remarks>
		///	When a stream is running, PortAudio calls the stream callback periodically.The callback function is responsible for processing buffers of audio samples passed via the input and output parameters.<para/>
		///	The PortAudio stream callback runs at very high or real-time priority. It is required to consistently meet its time deadlines.Do not allocate memory, access the file system, call library functions or call other functions from the stream callback that may block or take an unpredictable amount of time to complete.<para/>
		///	In order for a stream to maintain glitch-free operation the callback must consume and return audio data faster than it is recorded and/or played. PortAudio anticipates that each callback invocation may execute for a duration approaching the duration of frameCount audio frames at the stream sample rate. It is reasonable to expect to be able to utilise 70% or more of the available CPU time in the PortAudio callback.However, due to buffer size adaption and other factors, not all host APIs are able to guarantee audio stability under heavy CPU load with arbitrary fixed callback buffer sizes.When high callback CPU utilisation is required the most robust behavior can be achieved by using paFramesPerBufferUnspecified as the <see cref="Pa_OpenStream"/> framesPerBuffer parameter.
		/// </remarks>
		/// <param name="input">Either array of interleaved samples or; if non-interleaved samples were requested using the paNonInterleaved sample format flag, an array of buffer pointers, one non-interleaved buffer for each channel.</param>
		/// <param name="output">Either array of interleaved samples or; if non-interleaved samples were requested using the paNonInterleaved sample format flag, an array of buffer pointers, one non-interleaved buffer for each channel.</param>
		/// <param name="frameCount">The number of sample frames to be processed by the stream callback.</param>
		/// <param name="timeInfo">Timestamps indicating the ADC capture time of the first sample in the input buffer, the DAC output time of the first sample in the output buffer and the time the callback was invoked. See <see cref="PaStreamCallbackTimeInfo"/> and <see cref="GetStreamTime"/></param>
		/// <param name="statusFlags">Flags indicating whether input and/or output buffers have been inserted or will be dropped to overcome underflow or overflow conditions.</param>
		/// <param name="userData">The value of a user supplied pointer passed to <see cref="OpenStream"/> intended for storing synthesis data etc.</param>
		/// <seealso cref="Pa_OpenStream"/>
		/// <returns></returns>
		public delegate int StreamCallback(in IntPtr input, IntPtr output, ulong frameCount, ref PaStreamCallbackTimeInfo timeInfo, PaStreamCallbackFlags statusFlags, in IntPtr userData);

#if WINDOWS_64
		private const string LibraryName = "portaudio_x64";
#elif WINDOWS_32
		private const string LibraryName = "portaudio_x86";
#elif UNIX
		private const string LibraryName = "portaudio";
#else
		private const string LibraryName = "THIS_IS_A_SHIM_BUILD_YOU_ARE_REFERENCING_THE_WRONG_DLL";
#endif

		[DllImport(LibraryName)]
		private static extern int Pa_OpenStream_32();

		[DllImport(LibraryName)]
		private static extern int Pa_GetStreamTime();

		public static int OpenStream()
		{
#if IS_SHIM
			return 0;
#endif
		}

		public static int GetStreamTime()
		{
#if IS_SHIM
			return 0;
#endif
		}
	}
}
