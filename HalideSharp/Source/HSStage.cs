using System;
using System.Runtime.InteropServices;

namespace HalideSharp
{
    public class HSStage : HSObject
    {
        internal HSStage(IntPtr o)
        {
            _cppobj = o;
        }

        [DllImport(Constants.LibName)]
        private static extern void Stage_Delete(IntPtr self);
        ~HSStage()
        {
            Stage_Delete(HSUtil.CArg(this));
        }
    }
}