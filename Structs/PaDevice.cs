using System;
using System.Collections.Generic;
using System.Text;

namespace JSTF.PortAudio.Structs
{
	public struct PaDeviceInfo
	{
		public int structVersion;  /* this is struct version 2 */
		public IntPtr name;
		public int hostApi; /**< note this is a host API index, not a type id*/

		public int maxInputChannels;
		public int maxOutputChannels;

		/** Default latency values for interactive performance. */
		public double defaultLowInputLatency;
		public double defaultLowOutputLatency;
		/** Default latency values for robust non-interactive applications (eg. playing sound files). */
		public double defaultHighInputLatency;
		public double defaultHighOutputLatency;

		public double defaultSampleRate;
	}
}
