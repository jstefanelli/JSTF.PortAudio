using JSTF.PortAudio.Structs;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace JSTF.PortAudio.Managed
{
	public struct VersionInfo
	{
		public int VersionMajor;
		public int VersionMinor;
		public int VersionSubMinor;
		public string VersionControlRevision;
		public string VersionText;

		public static VersionInfo FromNative(IntPtr native)
		{
			VersionInfo i = new VersionInfo();
			PaVersionInfo ni = Marshal.PtrToStructure<PaVersionInfo>(native);
			i.VersionMajor = ni.versionMajor;
			i.VersionMinor = ni.versionSubMinor;
			i.VersionSubMinor = ni.versionSubMinor;
			i.VersionControlRevision = Marshal.PtrToStringAnsi(ni.versionControlRevision);
			i.VersionText = Marshal.PtrToStringAnsi(ni.versionText);
			return i;
		}
	}

}
