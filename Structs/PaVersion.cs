using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace JSTF.PortAudio.Structs
{
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PaVersionInfo
	{
		public int versionMajor;
		public int versionMinor;
		public int versionSubMinor;
		public IntPtr versionControlRevision;
		public IntPtr versionText;
	}
}
