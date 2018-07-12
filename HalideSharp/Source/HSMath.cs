using System;
using System.Runtime.InteropServices;

namespace HalideSharp
{
    public class HSMath
    {
        [DllImport(Constants.LibName, EntryPoint = "sin_var")]
        private static extern IntPtr SinVar(IntPtr v);
        
        public static HSExpr Sin(HSVar v)
        {
            return new HSExpr(SinVar(v._cppobj));
        }

        [DllImport(Constants.LibName, EntryPoint = "sin_expr")]
        private static extern IntPtr SinExpr(IntPtr v);
        
        public static HSExpr Sin(HSExpr v)
        {
            return new HSExpr(SinExpr(v._cppobj));
        }
        
        [DllImport(Constants.LibName, EntryPoint = "cos_var")]
        private static extern IntPtr CosVar(IntPtr v);
        
        public static HSExpr Cos(HSVar v)
        {
            return new HSExpr(CosVar(v._cppobj));
        }
        
        [DllImport(Constants.LibName, EntryPoint = "cos_expr")]
        private static extern IntPtr CosExpr(IntPtr v);
        
        public static HSExpr Cos(HSExpr v)
        {
            return new HSExpr(CosExpr(v._cppobj));
        }
    }
}