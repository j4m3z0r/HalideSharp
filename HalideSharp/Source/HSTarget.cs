using System;
using System.Runtime.InteropServices;

namespace HalideSharp
{
    public class HSTarget : HSObject
    {
        internal HSTarget(IntPtr obj) : base()
        {
            _cppobj = obj;
        }

        [DllImport(Constants.LibName)]
        private static extern void Target_Delete(IntPtr target);
        
        ~HSTarget()
        {
            #if TRACE_DESTRUCTORS
                Console.WriteLine($"Destroying {this.GetType().Name}");
            #endif

            if (_cppobj != IntPtr.Zero)
            {
                Target_Delete(_cppobj);
            }
        }

        [DllImport(Constants.LibName)]
        private static extern HSOperatingSystem Target_GetOperatingSystem(IntPtr target);

        public HSOperatingSystem OS => Target_GetOperatingSystem(HSUtil.CArg(this));


        [DllImport(Constants.LibName)]
        private static extern void Target_SetFeature_Feature(IntPtr self, HSFeature feature);

        public void SetFeature(HSFeature feature)
        {
            Target_SetFeature_Feature(HSUtil.CArg(this), HSUtil.CArg(feature));
        }
    }
}
