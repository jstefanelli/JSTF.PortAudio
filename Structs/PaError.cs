using System;
using System.Collections.Generic;
using System.Text;

namespace JSTF.PortAudio.Structs
{
	internal enum PaErrorCode : int
	{
		paNoError = 0,

		paNotInitialized = -10000,
		paUnanticipatedHostError,
		paInvalidChannelCount,
		paInvalidSampleRate,
		paInvalidDevice,
		paInvalidFlag,
		paSampleFormatNotSupported,
		paBadIODeviceCombination,
		paInsufficientMemory,
		paBufferTooBig,
		paBufferTooSmall,
		paNullCallback,
		paBadStreamPtr,
		paTimedOut,
		paInternalError,
		paDeviceUnavailable,
		paIncompatibleHostApiSpecificStreamInfo,
		paStreamIsStopped,
		paStreamIsNotStopped,
		paInputOverflowed,
		paOutputUnderflowed,
		paHostApiNotFound,
		paInvalidHostApi,
		paCanNotReadFromACallbackStream,
		paCanNotWriteToACallbackStream,
		paCanNotReadFromAnOutputOnlyStream,
		paCanNotWriteToAnInputOnlyStream,
		paIncompatibleStreamHostApi,
		paBadBufferPtr
	}
}
