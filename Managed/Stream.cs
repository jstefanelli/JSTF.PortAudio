using JSTF.PortAudio.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace JSTF.PortAudio.Managed
{

	public sealed class ManagedStreamCallbackInfo
	{

		internal IntPtr Input { get; set; }
		internal IntPtr Output { get; set; }

		internal ManagedStreamCallbackInfo()
		{

		}

		public byte[] ReadInputInterleaved(int frameAmount, int frameSize)
		{
			byte[] res = new byte[frameAmount * frameSize];
			Marshal.Copy(Input, res, 0, res.Length);
			return res;
		}

		public byte[][] ReadInputNotInterleaved(int frameAmount, int frameSize, int channelAmount)
		{
			byte[][] res = new byte[channelAmount][];
			for(int i = 0; i < channelAmount; i++)
			{
				IntPtr pointer = Marshal.ReadIntPtr(Input, i * IntPtr.Size);
				res[i] = new byte[frameAmount * frameSize];
				Marshal.Copy(pointer, res[i], 0, res.Length);
			}
			return res;
		}

		public void WriteOutputInterleaved(byte[] data, int offset, int length)
		{
			Marshal.Copy(data, offset, Output, length);
		}

		public void WriteOutputNotInterleaved(byte[][] data, int channelOffset, int channelLength)
		{
			for(int i = 0; i < data.Length; i++)
			{
				byte[] myData = data[i];
				IntPtr myPointer = Marshal.ReadIntPtr(Output, i * IntPtr.Size);
				Marshal.Copy(myData, channelOffset, myPointer, channelLength);
			}
		}

		public unsafe byte* GetOutputDirect()
		{
			return (byte*)Output;
		}
	}

	public delegate int StreamCallback(ManagedStreamCallbackInfo info, ulong frameCount, StreamCallbackTimeInfo timeInfo, StreamCallbackFlags statusFlags, in object userData);

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

	public sealed class StreamParameters<T> where T : struct
	{
		public int Device { get => Native.device; set => Native.device = value; }
		public int ChannelCount { get => Native.channelCount; set => Native.channelCount = value; }
		public SampleFormat SampleFormat { get => (SampleFormat)Native.sampleFormat; set => Native.sampleFormat = (PaSampleFormat)value; }
		public double SuggestedLatency { get => Native.suggestedLatency; set => Native.suggestedLatency = value; }
		private T? _HostApiSpecificStreamIno;
		public T? HostApiSpecificStreamInfo { get => _HostApiSpecificStreamIno;
			set
			{
				if (Native.hostApiSpecificStreamInfo != IntPtr.Zero) {
					NativeMemoryUtils.Free(Native.hostApiSpecificStreamInfo);
					Native.hostApiSpecificStreamInfo = IntPtr.Zero;
				}
				_HostApiSpecificStreamIno = value;
				if(_HostApiSpecificStreamIno == null)
					Native.hostApiSpecificStreamInfo = IntPtr.Zero;
				else
				{
					Native.hostApiSpecificStreamInfo = NativeMemoryUtils.Alloc(Marshal.SizeOf<T>());
					Marshal.StructureToPtr((T)_HostApiSpecificStreamIno, Native.hostApiSpecificStreamInfo, false);
				}
			}
		}

		public StreamParameters()
		{
			Native = new PaStreamParameters();
		}

		internal PaStreamParameters Native;

		internal PaStreamParameters ToNative()
		{
			return Native;
		}

		internal static StreamParameters<T> FromNative(IntPtr ptr)
		{
			PaStreamParameters native = Marshal.PtrToStructure<PaStreamParameters>(ptr);
			StreamParameters<T> managed = new StreamParameters<T>();
			managed.Device = native.device;
			managed.ChannelCount = native.channelCount;
			managed.SampleFormat = (SampleFormat)native.sampleFormat;
			managed.SuggestedLatency = native.suggestedLatency;
			if (native.hostApiSpecificStreamInfo == IntPtr.Zero)
				managed.HostApiSpecificStreamInfo = null;
			else
			{
				managed.HostApiSpecificStreamInfo = Marshal.PtrToStructure<T>(native.hostApiSpecificStreamInfo);
			}
			return managed;
		}
		
	}

	public enum StreamFlags : ulong
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

		internal static StreamInfo? FromNative(IntPtr ptr)
		{
			PaStreamInfo native = Marshal.PtrToStructure<PaStreamInfo>(ptr);
			StreamInfo managed = new StreamInfo();
			managed.InputLatency = native.inputLatency;
			managed.OutputLatency = native.outputLatency;
			managed.SampleRate = native.sampleRate;
			managed.StructVersion = native.structVersion;
			return managed;
		}
	}
}
