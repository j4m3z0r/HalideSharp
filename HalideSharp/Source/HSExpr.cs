using System;
using System.CodeDom;
using System.Runtime.InteropServices;

namespace HalideSharp
{
    public class HSExpr : HSObject
    {
        public const string CppType = "Expr";
        
        internal HSExpr(IntPtr cppobj)
        {
            _cppobj = cppobj;
        }

        [DllImport(Constants.LibName, EntryPoint = "new_expr_int")]
        private static extern IntPtr NewExprInt(int i);
        
        public HSExpr(int i)
        {
            _cppobj = NewExprInt(i);
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

        #region expr <op> float
        [DllImport(Constants.LibName, EntryPoint = "expr_mult_float")]
        private static extern IntPtr ExprMultFloat(IntPtr expr, float f);
        
        public static HSExpr operator*(HSExpr expr, float f)
        {
            return new HSExpr(ExprMultFloat(HSUtil.CArg(expr), HSUtil.CArg(f)));
        }
        #endregion

        #region expr <op> int
        [DllImport(Constants.LibName, EntryPoint = "expr_plus_int")]
        private static extern IntPtr ExprPlusInt(IntPtr exp, int i);

        public static HSExpr operator +(HSExpr exp, int i)
        {
            return new HSExpr(ExprPlusInt(HSUtil.CArg(exp), HSUtil.CArg(i)));
        }
        
        [DllImport(Constants.LibName, EntryPoint = "expr_div_int")]
        private static extern IntPtr ExprDivInt(IntPtr exp, int i);

        public static HSExpr operator /(HSExpr exp, int i)
        {
            return new HSExpr(ExprDivInt(HSUtil.CArg(exp), HSUtil.CArg(i)));
        }
        
        [DllImport(Constants.LibName, EntryPoint = "expr_lt_int")]
        private static extern IntPtr ExprLtInt(IntPtr exp, int i);
        
        public static HSExpr operator <(HSExpr exp, int i)
        {
            return new HSExpr(ExprLtInt(HSUtil.CArg(exp), HSUtil.CArg(i)));
        }
        
        [DllImport(Constants.LibName, EntryPoint = "expr_gt_int")]
        private static extern IntPtr ExprGtInt(IntPtr exp, int i);
        
        public static HSExpr operator >(HSExpr exp, int i)
        {
            return new HSExpr(ExprGtInt(HSUtil.CArg(exp), HSUtil.CArg(i)));
        }
        #endregion
        
        #region expr <op> expr
        [DllImport(Constants.LibName, EntryPoint = "expr_plus_expr")]
        private static extern IntPtr ExprPlusExpr(IntPtr exp1, IntPtr exp2);

        public static HSExpr operator +(HSExpr exp1, HSExpr exp2)
        {
            return new HSExpr(ExprPlusExpr(HSUtil.CArg(exp1), HSUtil.CArg(exp2)));
        }
        
        [DllImport(Constants.LibName, EntryPoint = "expr_minus_expr")]
        private static extern IntPtr ExprMinusExpr(IntPtr exp1, IntPtr exp2);

        public static HSExpr operator -(HSExpr exp1, HSExpr exp2)
        {
            return new HSExpr(ExprMinusExpr(HSUtil.CArg(exp1), HSUtil.CArg(exp2)));
        }
        
        [DllImport(Constants.LibName, EntryPoint = "expr_and_expr")]
        private static extern IntPtr ExprAndExpr(IntPtr e1, IntPtr e2);
        
        public static HSExpr operator& (HSExpr e1, HSExpr e2)
        {
            return new HSExpr(ExprAndExpr(HSUtil.CArg(e1), HSUtil.CArg(e2)));
        }
        #endregion

        #region expr <op> var
        [DllImport(Constants.LibName, EntryPoint = "expr_plus_var")]
        private static extern IntPtr ExprPlusVar(IntPtr exp, IntPtr v);

        public static HSExpr operator +(HSExpr exp, HSVar v)
        {
            return new HSExpr(ExprPlusVar(HSUtil.CArg(exp), HSUtil.CArg(v)));
        }
        #endregion
        
        #region int <op> expr
        [DllImport(Constants.LibName, EntryPoint = "int_mult_expr")]
        private static extern IntPtr IntMultExpr(int i, IntPtr exp);

        public static HSExpr operator *(int i, HSExpr exp)
        {
            return new HSExpr(IntMultExpr(HSUtil.CArg(i), HSUtil.CArg(exp)));
        }
        #endregion
        

        // HACK: returning false from both operator true and operator false should force us to never short-circuit,
        // meaning that && will always invoke operator&, which calls C++'s operator&& in the wrapper. In short, this
        // allows for the common (x && y) style conditionals.
        public static bool operator true(HSExpr e)
        {
            return false;
        }

        public static bool operator false(HSExpr e)
        {
            return false;
        }

        [DllImport(Constants.LibName, EntryPoint = "expr_to_string")]
        private static extern IntPtr ExprToString(IntPtr exp);

        [DllImport(Constants.LibC, EntryPoint = "free")]
        private static extern void Free(IntPtr p);
        
        public override string ToString()
        {
            var stringPtr = ExprToString(_cppobj);
            var result = Marshal.PtrToStringAnsi(stringPtr);
            Free(stringPtr);

            return result;
        }
    }
}
