using System;
using System.Collections.Generic;
using System.Text;

namespace JSTF.PortAudio.Managed
{
	public enum ErrorCode : int
	{
		NoError = 0,

		NotInitialized = -10000,
		UnanticipatedHostError,
		InvalidChannelCount,
		InvalidSampleRate,
		InvalidDevice,
		InvalidFlag,
		SampleFormatNotSupported,
		BadIODeviceCombination,
		InsufficientMemory,
		BufferTooBig,
		BufferTooSmall,
		NullCallback,
		BadStreamPtr,
		TimedOut,
		InternalError,
		DeviceUnavailable,
		IncompatibleHostApiSpecificStreamInfo,
		StreamIsStopped,
		StreamIsNotStopped,
		InputOverflowed,
		OutputUnderflowed,
		HostApiNotFound,
		InvalidHostApi,
		CanNotReadFromACallbackStream,
		CanNotWriteToACallbackStream,
		CanNotReadFromAnOutputOnlyStream,
		CanNotWriteToAnInputOnlyStream,
		IncompatibleStreamHostApi,
		BadBufferPtr
	}
}
