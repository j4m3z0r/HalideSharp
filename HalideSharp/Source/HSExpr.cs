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
            if (_cppobj != IntPtr.Zero)
            {
                DeleteExpr(_cppobj);
            }
        }

        [DllImport(Constants.LibName, EntryPoint = "expr_mult_float")]
        private static extern IntPtr ExprMultPloat(IntPtr expr, float f);
        
        public static HSExpr operator*(HSExpr expr, float f)
        {
            return new HSExpr(ExprMultPloat(expr._cppobj, f));
        }
    }
}