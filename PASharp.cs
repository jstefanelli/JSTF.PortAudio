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
		private static extern int Pa_OpenStream(ref IntPtr stream,
			IntPtr inputParameters,
			IntPtr outputParameters,
			double sampleRate,
			ulong framesPerBuffer,
			PaStreamFlags streamFlags,
			PaStreamCallback streamCallback,
			IntPtr userData );

		[DllImport(LibraryName)]
		private static extern int Pa_OpenDefaultStream(ref IntPtr stream,
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
			ErrorCode c = (ErrorCode)Pa_Terminate();
			if(c == ErrorCode.NoError)
			{
				foreach(Tuple<IntPtr, IntPtr> v in StreamParameters.Values)
				{
					NativeMemoryUtils.Free(v.Item1);
					NativeMemoryUtils.Free(v.Item2);
				}
				StreamParameters.Clear();
				StreamCallbacks.Clear();
				StreamUserDatas.Clear();
				UserDataObjects.Clear();
				NativeMemoryUtils.FreeAll();
			}
			return c;
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

		public static DeviceInfo? GetDeviceInfo(int device)
		{
			IntPtr ptr = Pa_GetDeviceInfo(device);
			if (ptr == IntPtr.Zero)
				return null;
			return DeviceInfo.FromNative(ptr);
		}

		public static bool IsFormatSupported<T1, T2>(in StreamParameters<T1> inputParameters, in StreamParameters<T2> outputParameters, double sampleRate, out ErrorCode error) where T1 : struct where T2 : struct
		{
			IntPtr input = NativeMemoryUtils.Alloc(Marshal.SizeOf<PaStreamParameters>());
			IntPtr output = NativeMemoryUtils.Alloc(Marshal.SizeOf<PaStreamParameters>());
			Marshal.StructureToPtr(inputParameters.Native, input, false);
			Marshal.StructureToPtr(outputParameters.Native, output, false);
			error = (ErrorCode)Pa_IsFormatSupported(input, output, sampleRate);
			NativeMemoryUtils.Free(input);
			NativeMemoryUtils.Free(output);
			return error == ErrorCode.NoError /* 0 */;
		}

		internal static readonly Dictionary<IntPtr, object> UserDataObjects = new Dictionary<IntPtr, object>();
		internal static int LastObjectId = -1;
		internal static readonly Dictionary<IntPtr, PaStreamCallback> StreamCallbacks = new Dictionary<IntPtr, PaStreamCallback>();
		internal static readonly Dictionary<IntPtr, Tuple<IntPtr, IntPtr>> StreamParameters = new Dictionary<IntPtr, Tuple<IntPtr, IntPtr>>();
		internal static readonly Dictionary<IntPtr, IntPtr> StreamUserDatas = new Dictionary<IntPtr, IntPtr>();

		public static ErrorCode OpenStream<T1, T2>(
			out IntPtr stream, 
			in StreamParameters<T1> inputParameters, 
			in StreamParameters<T2> outputParameters, 
			double sampleRate, 
			ulong framesPerBufer, 
			StreamFlags flags, 
			StreamCallback callback, 
			object userData) where T1 : struct where T2 : struct
		{
			PaStreamCallback cb = (IntPtr input, IntPtr output, ulong frameCount, ref PaStreamCallbackTimeInfo timeInfo, PaStreamCallbackFlags paFlags, IntPtr paUserData) =>
			{
				Console.WriteLine("Native CB");
				ManagedStreamCallbackInfo cbInfo = new ManagedStreamCallbackInfo();
				cbInfo.Input = input;
				cbInfo.Output = output;

				object managedUserData = UserDataObjects.ContainsKey(paUserData) ? UserDataObjects[paUserData] : null;
				StreamCallbackTimeInfo managedTimeInfo = new StreamCallbackTimeInfo();
				managedTimeInfo.CurrentTime = timeInfo.currentTime;
				managedTimeInfo.InputBufferAdcTime = timeInfo.inputBufferAdcTime;
				managedTimeInfo.OutputBufferDacTime = timeInfo.outputBufferDacTime;

				return callback(cbInfo, frameCount, managedTimeInfo, (StreamCallbackFlags)paFlags, managedUserData);
			};

			IntPtr nativeInput = NativeMemoryUtils.Alloc(Marshal.SizeOf<PaStreamParameters>());
			IntPtr nativeOutput = NativeMemoryUtils.Alloc(Marshal.SizeOf<PaStreamParameters>());
			Marshal.StructureToPtr(inputParameters.Native, nativeInput, false);
			Marshal.StructureToPtr(outputParameters.Native, nativeOutput, false);
			IntPtr myStream = IntPtr.Zero;

			IntPtr myUserData = new IntPtr(++LastObjectId);
			UserDataObjects.Add(myUserData, userData);

			ErrorCode c = (ErrorCode)Pa_OpenStream(ref myStream, nativeInput, nativeOutput, sampleRate, framesPerBufer, (PaStreamFlags)flags, cb, myUserData);
			if(c == ErrorCode.NoError)
			{
				StreamParameters.Add(myStream, new Tuple<IntPtr, IntPtr>(nativeInput, nativeOutput));
				StreamCallbacks.Add(myStream, cb);
				StreamUserDatas.Add(myStream, myUserData);
				stream = myStream;
			}
			else
			{
				NativeMemoryUtils.Free(nativeInput);
				NativeMemoryUtils.Free(nativeOutput);
				UserDataObjects.Remove(myUserData);
				stream = IntPtr.Zero;
			}
			return c;
		}

		public static ErrorCode OpenDefaultStream(
			out IntPtr stream,
			int numInputChannels,
			int numOutputChannels,
			SampleFormat sampleFormat,
			double sampleRate,
			ulong framesPerBuffer,
			StreamCallback callback,
			object userData)
		{
			PaStreamCallback cb = (IntPtr input, IntPtr output, ulong frameCount, ref PaStreamCallbackTimeInfo timeInfo, PaStreamCallbackFlags paFlags, IntPtr paUserData) =>
			{
				ManagedStreamCallbackInfo cbInfo = new ManagedStreamCallbackInfo();
				cbInfo.Input = input;
				cbInfo.Output = output;

				object managedUserData = UserDataObjects.ContainsKey(paUserData) ? UserDataObjects[paUserData] : null;
				StreamCallbackTimeInfo managedTimeInfo = new StreamCallbackTimeInfo();
				managedTimeInfo.CurrentTime = timeInfo.currentTime;
				managedTimeInfo.InputBufferAdcTime = timeInfo.inputBufferAdcTime;
				managedTimeInfo.OutputBufferDacTime = timeInfo.outputBufferDacTime;

				return callback(cbInfo, frameCount, managedTimeInfo, (StreamCallbackFlags)paFlags, managedUserData);
			};
			IntPtr myStream = IntPtr.Zero;

			IntPtr myUserData = new IntPtr(++LastObjectId);
			UserDataObjects.Add(myUserData, userData);

			ErrorCode c = (ErrorCode)Pa_OpenDefaultStream(ref myStream, numInputChannels, numOutputChannels, (PaSampleFormat)sampleFormat, sampleRate, framesPerBuffer, cb, myUserData);
			if(c == ErrorCode.NoError)
			{
				StreamCallbacks.Add(myStream, cb);
				StreamUserDatas.Add(myStream, myUserData);
				stream = myStream;
			}
			else
			{
				UserDataObjects.Remove(myUserData);
				stream = IntPtr.Zero;
			}
			return c;
		}

		public static ErrorCode CloseStream(IntPtr stream)
		{
			ErrorCode c = (ErrorCode)Pa_CloseStream(stream);
			if(c == ErrorCode.NoError)
			{
				if (StreamCallbacks.ContainsKey(stream))
					StreamCallbacks.Remove(stream);

				if (StreamParameters.ContainsKey(stream))
				{
					Tuple<IntPtr, IntPtr> prms = StreamParameters[stream];
					StreamParameters.Remove(stream);
					NativeMemoryUtils.Free(prms.Item1);
					NativeMemoryUtils.Free(prms.Item2);
				}

				if (StreamUserDatas.ContainsKey(stream))
				{
					IntPtr usrData = StreamUserDatas[stream];
					StreamUserDatas.Remove(stream);
					UserDataObjects.Remove(usrData);
				}
			}
			return c;
		}

		public static ErrorCode StartStream(IntPtr stream)
		{
			return (ErrorCode)Pa_StartStream(stream);
		}

		public static ErrorCode StopStream(IntPtr stream)
		{
			return (ErrorCode)Pa_StopStream(stream);
		}

		public static ErrorCode AbortStream(IntPtr stream)
		{
			return (ErrorCode)Pa_AbortStream(stream);
		}

		public static ErrorCode IsStreamActive(IntPtr stream, out bool isActive)
		{
			int c = Pa_IsStreamActive(stream);
			if(c == 0)
			{
				isActive = true;
				return ErrorCode.NoError;
			}else if(c == 1)
			{
				isActive = false;
				return ErrorCode.NoError;
			}
			else
			{
				isActive = false;
				return (ErrorCode)c;
			}
		}

		public static StreamInfo? GetStreamInfo(IntPtr stream)
		{
			IntPtr inf = Pa_GetStreamInfo(stream);
			if (inf == IntPtr.Zero)
				return null;
			return StreamInfo.FromNative(inf);
		}

		public static double GetStreamTime(IntPtr stream)
		{
			return Pa_GetStreamTime(stream);
		}

		public static double GetStreamCpuLoad(IntPtr stream)
		{
			return Pa_GetStreamCpuLoad(stream);
		}

		//TODO: Implement ReadStream(IntPtr) and WriteStream(IntPtr)
	}
}
