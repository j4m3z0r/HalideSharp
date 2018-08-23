using System;
using System.Runtime.InteropServices;

namespace HalideSharp
{
    internal class HSUtil
    {
        // Argument overloads to convert objects into the appropriate type for our C API: the _cppobj IntPtr for
        // objects, and the value of primitive types (int, float, etc) for everything else.
        internal static int CArg(int i)
        {
            return i;
        }

        internal static float CArg(float f)
        {
            return f;
        }

        internal static IntPtr CArg(HSObject o)
        {
            if (o == null)
            {
                return IntPtr.Zero;
            }
            
            return o._cppobj;
        }
        
        internal static string CArg(string s)
        {
            return s;
        }

        internal static IntPtr CArg(GCHandle h)
        {
            return h.AddrOfPinnedObject();
        }

        internal static HSOutputFormat CArg(HSOutputFormat f)
        {
            return f;
        }

        internal static HSFeature CArg(HSFeature f)
        {
            return f;
        }

        public static HSOperatingSystem CArg(HSOperatingSystem os)
        {
            return os;
        }

        public static HSArchitecture CArg(HSArchitecture arch)
        {
            return arch;
        }
    }
}