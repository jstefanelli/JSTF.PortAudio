using System;
using System.Collections.Generic;
using System.Text;

namespace JSTF.PortAudio.Structs
{
	internal enum PaSampleFormat : ulong
	{
		paFloat32 = 0x00000001,
		paInt32 = 0x00000002,
		paInt24 = 0x00000004,
		paInt16 = 0x00000008,
		paInt8 = 0x00000010,
		paUInt8 = 0x00000020,
		paCustomFormat = 0x00010000,
		paNonInterleaved = 0x80000000
	}
}
