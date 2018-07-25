﻿using System;
using System.Runtime.InteropServices;

namespace HalideSharp
{
    public partial class HSRDom : HSObject
    {
        [DllImport(Constants.LibName)]
        private static extern IntPtr RDom_New_IntInt(int min, int extent);
        
        public HSRDom(int min, int extent)
        {
            _cppobj = RDom_New_IntInt(min, extent);
        }

        [DllImport(Constants.LibName)]
        private static extern void RDom_Delete(IntPtr r);

        ~HSRDom()
        {
            RDom_Delete(_cppobj);
        }
    }
}