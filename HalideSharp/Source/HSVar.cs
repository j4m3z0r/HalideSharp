using System;
using System.Runtime.InteropServices;
using System.Xml.Schema;

namespace HalideSharp
{
    public class HSVar
    {
        internal IntPtr _cppobj;

        [DllImport(Constants.LibName, EntryPoint = "new_var")]
        private static extern IntPtr NewVar([MarshalAs(Constants.StringType)] string name);
        
        public HSVar(string name)
        {
            _cppobj = NewVar(name);
        }

        internal HSVar(IntPtr cppobj)
        {
            _cppobj = cppobj;
        }
        
        [DllImport(Constants.LibName, EntryPoint = "delete_var")]
        private static extern void DeleteVar(IntPtr obj);
        
        ~HSVar() {
            if (_cppobj != IntPtr.Zero)
            {
                DeleteVar(_cppobj);
            }
        }

        [DllImport(Constants.LibName, EntryPoint = "var_plus_var")]
        private static extern IntPtr VarPlusVar(IntPtr v1, IntPtr v2);
        public static HSExpr operator +(HSVar v1, HSVar v2)
        {
            return new HSExpr(VarPlusVar(v1._cppobj, v2._cppobj));
        }

        [DllImport(Constants.LibName, EntryPoint = "var_mult_var")]
        private static extern IntPtr VarMultVar(IntPtr v1, IntPtr v2);

        public static HSExpr operator *(HSVar v1, HSVar v2)
        {
            return new HSExpr(VarMultVar(v1._cppobj, v2._cppobj));
        }

        [DllImport(Constants.LibName, EntryPoint = "var_equals_int")]
        private static extern IntPtr VarEqualsInt(IntPtr v, int i);

        public static HSExpr operator ==(HSVar v, int i)
        {
            return new HSExpr(VarEqualsInt(v._cppobj, i));
        }

        [DllImport(Constants.LibName, EntryPoint = "var_not_equals_int")]
        private static extern IntPtr VarNotEqualsInt(IntPtr v, int i);
        
        public static HSExpr operator !=(HSVar v, int i)
        {
            return new HSExpr(VarNotEqualsInt(v._cppobj, i));
        }
    }
}