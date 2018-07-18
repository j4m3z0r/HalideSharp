using System;
using System.Runtime.InteropServices;

namespace HalideSharp
{
    public class HSTarget : HSObject
    {
        internal HSTarget(IntPtr obj)
        {
            _cppobj = obj;
        }

        [DllImport(Constants.LibName)]
        private static extern void Target_Delete(IntPtr target);
        
        ~HSTarget()
        {
            if (_cppobj != IntPtr.Zero)
            {
                Target_Delete(_cppobj);
            }
        }

        [DllImport(Constants.LibName)]
        private static extern SharedEnums.HSOperatingSystem Target_GetOperatingSystem(IntPtr target);

        public SharedEnums.HSOperatingSystem OS => Target_GetOperatingSystem(HSUtil.CArg(this));


        [DllImport(Constants.LibName)]
        private static extern void Target_SetFeature_Feature(IntPtr self, SharedEnums.HSFeature feature);

        public void SetFeature(SharedEnums.HSFeature feature)
        {
            Target_SetFeature_Feature(HSUtil.CArg(this), HSUtil.CArg(feature));
        }
    }
}