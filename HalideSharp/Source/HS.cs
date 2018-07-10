using System;
using System.Runtime.InteropServices;

namespace HalideSharp
{
    public class HS
    {
        [DllImport(Constants.LibName, EntryPoint = "cast_to_float")]
        private static extern IntPtr CastToFloat(IntPtr expr);
        
        [DllImport(Constants.LibName, EntryPoint = "cast_to_byte")]
        private static extern IntPtr CastToByte(IntPtr expr);
        
        public static HSExpr Cast<T>(HSExpr expr)
        {
            // Cast uses move semantics.
            IntPtr newExpr;
            if (typeof(T) == typeof(float))
            {
                newExpr = CastToFloat(expr._cppobj);
            }
            else if (typeof(T) == typeof(byte))
            {
                newExpr = CastToByte(expr._cppobj);
            }
            else
            {
                throw new NotImplementedException($"Casting not implemented for type {typeof(T)}");
            }
            
            expr._cppobj = IntPtr.Zero;
            return new HSExpr(newExpr);
        }

        [DllImport(Constants.LibName, EntryPoint = "min_expr_float")]
        private static extern IntPtr MinExprFloat(IntPtr expr, float f);

        public static HSExpr Min(HSExpr expr, float f)
        {
            // Min uses move semantics.
            var newExpr = MinExprFloat(expr._cppobj, f);
            expr._cppobj = IntPtr.Zero;
            return new HSExpr(newExpr);
        }
    }
}