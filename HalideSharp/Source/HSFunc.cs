using System;
using System.Runtime.InteropServices;

namespace HalideSharp
{
    public class HSFunc
    {
        internal IntPtr _cppobj;
        
        [DllImport(Constants.LibName, EntryPoint = "new_func")]
        private static extern IntPtr NewFunc([MarshalAs(Constants.StringType)] string name);
        
        public HSFunc(string name)
        {
            _cppobj = NewFunc(name);
        }
        
        [DllImport(Constants.LibName, EntryPoint = "delete_func")]
        private static extern void DeleteFunc(IntPtr obj);
        
        ~HSFunc() {
            DeleteFunc(_cppobj);
        }

        [DllImport(Constants.LibName, EntryPoint = "func_set_var_var_expr")]
        private static extern void FuncSetVarVarExpr(IntPtr func, IntPtr v1, IntPtr v2, IntPtr e);
        
        public HSExpr this[HSVar v1, HSVar v2]
        {
            set => FuncSetVarVarExpr(_cppobj, v1._cppobj, v2._cppobj, value._cppobj);
        }

        [DllImport(Constants.LibName, EntryPoint = "func_realize_int")]
        private static extern IntPtr FuncRealizeInt(IntPtr func, int width, int height);
        
        public HSBuffer<T> Realize<T>(int width, int height) where T : struct
        {
            IntPtr result;
            if (typeof(T) == typeof(int))
            {
                result = FuncRealizeInt(_cppobj, width, height);
            }
            else
            {
                throw new NotImplementedException($"Cannot realize to buffer of type {typeof(T)}");
            }

            return new HSBuffer<T>(result);
        }
    }
}