using System;
using System.Runtime.InteropServices;

namespace HalideSharp
{
    public partial class HSRDom : HSObject
    {
        private HSRVar _x = null;
        private HSRVar _y = null;
        private HSRVar _z = null;
        private HSRVar _w = null;

        [DllImport(Constants.LibName)]
        private static extern IntPtr RDom_GetX(IntPtr self);
        
        [DllImport(Constants.LibName)]
        private static extern IntPtr RDom_GetY(IntPtr self);
        
        [DllImport(Constants.LibName)]
        private static extern IntPtr RDom_GetZ(IntPtr self);
        
        [DllImport(Constants.LibName)]
        private static extern IntPtr RDom_GetW(IntPtr self);
        
        public HSRVar X
        {
            get
            {
                if (_x != null)
                {
                    return _x;
                }
                _x = new HSRVar(RDom_GetX(HSUtil.CArg(this)));
                _x.AddRef(this);
                return _x;
            }
        }
        
        public HSRVar Y
        {
            get
            {
                if (_y != null)
                {
                    return _y;
                }
                _y = new HSRVar(RDom_GetX(HSUtil.CArg(this)));
                _y.AddRef(this);
                return _y;
            }
        }
        
        public HSRVar Z
        {
            get
            {
                if (_z != null)
                {
                    return _z;
                }
                _z = new HSRVar(RDom_GetX(HSUtil.CArg(this)));
                _z.AddRef(this);
                return _z;
            }
        }
        
        public HSRVar W
        {
            get
            {
                if (_w != null)
                {
                    return _w;
                }
                _w = new HSRVar(RDom_GetX(HSUtil.CArg(this)));
                _w.AddRef(this);
                return _w;
            }
        }
        
        [DllImport(Constants.LibName)]
        private static extern IntPtr RDom_New_IntInt(int min, int extent);
        
        public HSRDom(int min, int extent) : base()
        {
            _cppobj = RDom_New_IntInt(min, extent);
        }

        [DllImport(Constants.LibName)]
        private static extern void RDom_Delete(IntPtr r);

        ~HSRDom()
        {
            #if TRACE_DESTRUCTORS
                Console.WriteLine($"Destroying {this.GetType().Name}");
            #endif

            RDom_Delete(_cppobj);
        }
    }
}
