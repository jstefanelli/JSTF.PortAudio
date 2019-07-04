﻿using System;
using System.Collections.Generic;
using System.Text;

namespace JSTF.PortAudio.Managed
{
	public enum SampleFormat : ulong
	{
		Float32 = 0x00000001,
		Int32 = 0x00000002,
		Int24 = 0x00000004,
		Int16 = 0x00000008,
		Int8 = 0x00000010,
		UInt8 = 0x00000020,
		CustomFormat = 0x00010000,
		NonInterleaved = 0x80000000
	}
}
