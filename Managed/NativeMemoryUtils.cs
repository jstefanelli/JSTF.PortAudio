using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace JSTF.PortAudio.Managed
{
	internal static class NativeMemoryUtils
	{
		internal static readonly List<IntPtr> AllocatedMemoryBlocks = new List<IntPtr>();

		internal static IntPtr Alloc(int size)
		{
			IntPtr mem = Marshal.AllocHGlobal(size);
			lock(AllocatedMemoryBlocks) {
				AllocatedMemoryBlocks.Add(mem);
			}
			return mem;

		}

		internal static bool Free(IntPtr mem)
		{
			lock (AllocatedMemoryBlocks)
			{
				if (!AllocatedMemoryBlocks.Contains(mem))
					return false;
				AllocatedMemoryBlocks.Remove(mem);
			}
			Marshal.FreeHGlobal(mem);
			return true;
		}

		internal static void FreeAll()
		{
			lock (AllocatedMemoryBlocks)
			{
				foreach(IntPtr t in AllocatedMemoryBlocks)
				{
					Marshal.FreeHGlobal(t);
				}
				AllocatedMemoryBlocks.Clear();
			}
		}
	}
}
