using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace HalideSharp
{
    public class HSTarget : HSObject
    {
        [DllImport(Constants.LibName)]
        private static extern IntPtr Target_New();
        
        public HSTarget() : base()
        {
            _cppobj = Target_New();
        }
        
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

        #region Properties
        [DllImport(Constants.LibName)]
        private static extern void Target_SetOperatingSystem_OperatingSystem(IntPtr self, HSOperatingSystem os);
        
        [DllImport(Constants.LibName)]
        private static extern HSOperatingSystem Target_GetOperatingSystem(IntPtr target);

        public HSOperatingSystem OS
        {
            get => Target_GetOperatingSystem(HSUtil.CArg(this));
            set => Target_SetOperatingSystem_OperatingSystem(HSUtil.CArg(this), HSUtil.CArg(value));
        }
        
        
        [DllImport(Constants.LibName)]
        private static extern void Target_SetArchitecture_Architecture(IntPtr self, HSArchitecture arch);
        
        [DllImport(Constants.LibName)]
        private static extern HSArchitecture Target_GetArchitecture(IntPtr target);

        public HSArchitecture Arch
        {
            get => Target_GetArchitecture(HSUtil.CArg(this));
            set => Target_SetArchitecture_Architecture(HSUtil.CArg(this), HSUtil.CArg(value));
        }
        
        [DllImport(Constants.LibName)]
        private static extern void Target_SetBits_Int(IntPtr self, int bits);
        
        [DllImport(Constants.LibName)]
        private static extern int Target_GetBits(IntPtr target);

        public int Bits
        {
            get => Target_GetBits(HSUtil.CArg(this));
            set => Target_SetBits_Int(HSUtil.CArg(this), HSUtil.CArg(value));
        }
        #endregion

        [DllImport(Constants.LibName)]
        private static extern void Target_SetFeature_Feature(IntPtr self, HSFeature feature);

        public void SetFeature(HSFeature feature)
        {
            Target_SetFeature_Feature(HSUtil.CArg(this), HSUtil.CArg(feature));
        }
        
        public void SetFeatures(List<HSFeature> features)
        {
            foreach (var f in features)
            {
                SetFeature(f);
            }
        }
    }
}
