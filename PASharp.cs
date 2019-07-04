using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using JSTF.PortAudio.Managed;
using JSTF.PortAudio.Structs;

namespace JSTF.PortAudio
{
	public static class PA
	{

#if WINDOWS_64
		private const string LibraryName = "portaudio_x64";
#elif WINDOWS_32
		private const string LibraryName = "portaudio_x86";
#elif UNIX
		private const string LibraryName = "portaudio";
#else
		private const string LibraryName = "THIS_IS_A_SHIM_BUILD_YOU_ARE_REFERENCING_THE_WRONG_DLL";
#endif

		public const int PaNoDevice = -1;
		public const int PaUseHostApiSpecificDeviceSpecification = -2;

		[DllImport(LibraryName)]
		private static extern int Pa_GetVersion();

		[DllImport(LibraryName)]
		private static extern IntPtr Pa_GetVersionText();

		/* Somehow not exported in windows DLL
		[DllImport(LibraryName)]
		private static extern IntPtr Pa_GetVersionInfo();
		*/

		[DllImport(LibraryName)]
		private static extern IntPtr Pa_GetErrorText(int c);

		[DllImport(LibraryName)]
		private static extern int Pa_Initialize();

		[DllImport(LibraryName)]
		private static extern int Pa_Terminate();

		[DllImport(LibraryName)]
		private static extern int Pa_GetHostApiCount();

		[DllImport(LibraryName)]
		private static extern int Pa_GetDefaultHostApi();

		[DllImport(LibraryName)]
		private static extern IntPtr Pa_GetHostApiInfo(int hostApi);

		[DllImport(LibraryName)]
		private static extern int Pa_HostApiTypeIdToHostApiIndex(PaHostApiTypeId type);

		[DllImport(LibraryName)]
		private static extern int Pa_HostApiDeviceIndexToDeviceIndex(int hostApi, int hostApiDeviceIndex);

		[DllImport(LibraryName)]
		private static extern IntPtr Pa_GetLastHostErrorInfo();

		[DllImport(LibraryName)]
		private static extern int Pa_GetDeviceCount();

		[DllImport(LibraryName)]
		private static extern int Pa_GetDefaultInputDevice();

		[DllImport(LibraryName)]
		private static extern int Pa_GetDefaultOutputDevice();

		[DllImport(LibraryName)]
		private static extern IntPtr Pa_GetDeviceInfo(int device);

		[DllImport(LibraryName)]
		private static extern int Pa_IsFormatSupported(IntPtr inputParameters, IntPtr outputParameters, double sampleRate);

		[DllImport(LibraryName)]
		private static extern int Pa_OpenStream(IntPtr stream,
			IntPtr inputParameters,
			IntPtr outputParameters,
			double sampleRate,
			ulong framesPerBuffer,
			PaStreamFlags streamFlags,
			PaStreamCallback streamCallback,
			IntPtr userData );

		[DllImport(LibraryName)]
		private static extern int Pa_OpenDefaultStream(IntPtr stream,
			int numInputChannels,
			int numOutputChannels,
			PaSampleFormat sampleFormat,
			double sampleRate,
			ulong framesPerBuffer,
			PaStreamCallback streamCallback,
			IntPtr userData);

		[DllImport(LibraryName)]
		private static extern int Pa_CloseStream(IntPtr stream);

		[DllImport(LibraryName)]
		private static extern int Pa_SetStreamFinishedCallback(IntPtr stream, PaStreamFinishedCallback streamFinishedCallback);


		[DllImport(LibraryName)]
		private static extern int Pa_StartStream(IntPtr stream);

		[DllImport(LibraryName)]
		private static extern int Pa_StopStream(IntPtr stream);

		[DllImport(LibraryName)]
		private static extern int Pa_AbortStream(IntPtr stream);

		[DllImport(LibraryName)]
		private static extern int Pa_IsStreamStopped(IntPtr stream);

		[DllImport(LibraryName)]
		private static extern int Pa_IsStreamActive(IntPtr stream);

		[DllImport(LibraryName)]
		private static extern IntPtr Pa_GetStreamInfo(IntPtr stream);

		[DllImport(LibraryName)]
		private static extern int Pa_GetStreamTime(IntPtr stream);

		[DllImport(LibraryName)]
		private static extern double Pa_GetStreamCpuLoad(IntPtr stream);

		[DllImport(LibraryName)]
		private static extern int Pa_ReadStream(IntPtr stream, IntPtr buffer, ulong frames);

		[DllImport(LibraryName)]
		private static extern int Pa_WriteStream(IntPtr stream, IntPtr buffer, ulong frames );

		[DllImport(LibraryName)]
		private static extern long Pa_GetStreamReadAvailable(IntPtr stream);

		[DllImport(LibraryName)]
		private static extern long Pa_GetStreamWriteAvailable(IntPtr stream);

		[DllImport(LibraryName)]
		private static extern int Pa_GetSampleSize(PaSampleFormat format);

		[DllImport(LibraryName)]
		private static extern void Pa_Sleep(long msec);

		public static int GetVersion()
		{
			return Pa_GetVersion();
		}

		public static string GetVersionText()
		{
			return Marshal.PtrToStringAnsi(Pa_GetVersionText());
		}

		public static string GetErrorText(ErrorCode c)
		{
			return Marshal.PtrToStringAnsi(Pa_GetErrorText((int)c));
		}

		public static ErrorCode Initialize()
		{
			return (ErrorCode)Pa_Initialize();
		}

		public static ErrorCode Terminate()
		{
			return (ErrorCode)Pa_Terminate();
		}

		public static ErrorCode GetHostApiCount(out int hostApiCount)
		{
			int apiCount = Pa_GetHostApiCount();
			if(apiCount < 0)
			{
				hostApiCount = 0;
				return (ErrorCode)apiCount;
			}
				hostApiCount = apiCount;
				return ErrorCode.NoError;
		}

		public static ErrorCode GetDefaultHostApi(out int defaultHostApi)
		{
			int def = Pa_GetDefaultHostApi();
			if(def < 0)
			{
				defaultHostApi = 0;
				return (ErrorCode)def;
			}
			defaultHostApi = def;
			return ErrorCode.NoError;
		}

		public static HostApiInfo GetHostApiInfo(int hostApi)
		{
			IntPtr native = Pa_GetHostApiInfo(hostApi);
			if (native == IntPtr.Zero)
				return null;
			return HostApiInfo.FromNative(native);
		}

		public static ErrorCode HostApiTypeIdToHostApiIndex(HostApiTypeId type, out int id)
		{
			int rvl = Pa_HostApiTypeIdToHostApiIndex((PaHostApiTypeId)type);
			if(rvl < 0)
			{
				id = 0;
				return (ErrorCode)rvl;
			}
			id = rvl;
			return ErrorCode.NoError;
		}

		public static ErrorCode HostApiDeviceIndexToDeviceIndex(int hostApi, int hostApiDeviceIndex, out int deviceIndex)
		{
			int rvl = Pa_HostApiDeviceIndexToDeviceIndex(hostApi, hostApiDeviceIndex);
			if(rvl < 0)
			{
				deviceIndex = 0;
				return (ErrorCode)rvl;
			}
			deviceIndex = rvl;
			return ErrorCode.NoError;
		}

		public static HostErrorInfo GetLastHostErrorInfo()
		{
			IntPtr native = Pa_GetLastHostErrorInfo();
			return HostErrorInfo.FromNative(native);
		}

		public static ErrorCode GetDeviceCount(out int deviceCount)
		{
			int rvl = Pa_GetDeviceCount();
			if(rvl < 0)
			{
				deviceCount = 0;
				return (ErrorCode)rvl;
			}
			deviceCount = rvl;
			return ErrorCode.NoError;
		}

		public static int GetDefaultInputDevice()
		{
			return Pa_GetDefaultInputDevice();
		}

		public static int GetDefaultOutputDevice()
		{
			return Pa_GetDefaultOutputDevice();
		}

		public static DeviceInfo GetDeviceInfo(int device)
		{
			IntPtr ptr = Pa_GetDeviceInfo(device);
			if (ptr == IntPtr.Zero)
				return null;
			return DeviceInfo.FromNative(ptr);
		}


	}
}
