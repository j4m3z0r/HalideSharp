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
            if (_cppobj != IntPtr.Zero)
            {
                DeleteFunc(_cppobj);
            }
        }

        [DllImport(Constants.LibName, EntryPoint = "func_set_var_var_expr")]
        private static extern void FuncSetVarVarExpr(IntPtr func, IntPtr v1, IntPtr v2, IntPtr e);
        
        public HSExpr this[HSVar v1, HSVar v2]
        {
            set => FuncSetVarVarExpr(_cppobj, v1._cppobj, v2._cppobj, value._cppobj);
        }

        [DllImport(Constants.LibName, EntryPoint = "func_set_var_var_var_expr")]
        private static extern void FuncSetVarVarVarExpr(IntPtr func, IntPtr v1, IntPtr v2, IntPtr v3, IntPtr e);
        
        public HSExpr this[HSVar v1, HSVar v2, HSVar v3]
        {
            set => FuncSetVarVarVarExpr(_cppobj, v1._cppobj, v2._cppobj, v3._cppobj, value._cppobj);
        }
        
        [DllImport(Constants.LibName, EntryPoint = "func_realize_int_2d")]
        private static extern IntPtr FuncRealizeInt(IntPtr func, int width, int height);
        
        [DllImport(Constants.LibName, EntryPoint = "func_realize_int_3d")]
        private static extern IntPtr FuncRealizeInt(IntPtr func, int width, int height, int channels);
        
        [DllImport(Constants.LibName, EntryPoint = "func_realize_float_2d")]
        private static extern IntPtr FuncRealizeFloat(IntPtr func, int width, int height);
        
        [DllImport(Constants.LibName, EntryPoint = "func_realize_float_3d")]
        private static extern IntPtr FuncRealizeFloat(IntPtr func, int width, int height, int channels);
        
        [DllImport(Constants.LibName, EntryPoint = "func_realize_byte_2d")]
        private static extern IntPtr FuncRealizeByte(IntPtr func, int width, int height);
        
        [DllImport(Constants.LibName, EntryPoint = "func_realize_byte_3d")]
        private static extern IntPtr FuncRealizeByte(IntPtr func, int width, int height, int channels);
        
        public HSBuffer<T> Realize<T>(int width, int height) where T : struct
        {
            IntPtr result;
            if (typeof(T) == typeof(int))
            {
                result = FuncRealizeInt(_cppobj, width, height);
            }
            else if (typeof(T) == typeof(float))
            {
                result = FuncRealizeFloat(_cppobj, width, height);
            }
            else if (typeof(T) == typeof(byte))
            {
                result = FuncRealizeByte(_cppobj, width, height);
            }
            else
            {
                throw new NotImplementedException($"Cannot realize to buffer of type {typeof(T)}");
            }

            return new HSBuffer<T>(result);
        }

        public HSBuffer<T> Realize<T>(int width, int height, int channels) where T : struct
        {
            IntPtr result;
            if (typeof(T) == typeof(int))
            {
                result = FuncRealizeInt(_cppobj, width, height, channels);
            }
            else if (typeof(T) == typeof(float))
            {
                result = FuncRealizeFloat(_cppobj, width, height, channels);
            }
            else if (typeof(T) == typeof(byte))
            {
                result = FuncRealizeByte(_cppobj, width, height, channels);
            }
            else
            {
                throw new NotImplementedException($"Cannot realize to buffer of type {typeof(T)}");
            }

            return new HSBuffer<T>(result);
        }

        [DllImport(Constants.LibName, EntryPoint = "func_trace_stores")]
        private static extern void FuncTraceStores(IntPtr func);
        
        public void TraceStores()
        {
            FuncTraceStores(_cppobj);
        }

        [DllImport(Constants.LibName, EntryPoint = "func_parallel_var")]
        private static extern void FuncParallelVar(IntPtr func, IntPtr v);
        
        public void Parallel(HSVar v)
        {
            FuncParallelVar(_cppobj, v._cppobj);
        }

        [DllImport(Constants.LibName, EntryPoint = "func_compile_to_lowered_stmt")]
        private static extern void FuncCompileToLoweredStmt(IntPtr func,
            [MarshalAs(Constants.StringType)] string filename, SharedEnums.HSOutputFormat format);
        
        public void CompileToLoweredStmt(string filename, SharedEnums.HSOutputFormat format)
        {
            FuncCompileToLoweredStmt(_cppobj, filename, format);
        }
    }
}