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
            DeleteVar(_cppobj);
        }

        [DllImport(Constants.LibName, EntryPoint = "var_plus_var")]
        private static extern IntPtr VarPlusVar(IntPtr v1, IntPtr v2);
        public static HSExpr operator +(HSVar v1, HSVar v2)
        {
            return new HSExpr(VarPlusVar(v1._cppobj, v2._cppobj));
        }
    }
}