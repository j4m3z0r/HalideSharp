using System;
using System.Runtime.InteropServices;

namespace HalideSharp
{
    public class HSExpr
    {
        internal IntPtr _cppobj;

        internal HSExpr(IntPtr cppobj)
        {
            _cppobj = cppobj;
        }

        [DllImport(Constants.LibName, EntryPoint = "delete_expr")]
        private static extern void DeleteExpr(IntPtr obj);
        
        ~HSExpr()
        {
            DeleteExpr(_cppobj);
        }
    }
}