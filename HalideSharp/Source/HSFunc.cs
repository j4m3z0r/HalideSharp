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

        [DllImport(Constants.LibName, EntryPoint = "func_realize_int_buffer")]
        private static extern IntPtr FuncRealizeIntBuffer(IntPtr func, IntPtr buffer);
        
        [DllImport(Constants.LibName, EntryPoint = "func_realize_float_buffer")]
        private static extern IntPtr FuncRealizeFloatBuffer(IntPtr func, IntPtr buffer);
        
        [DllImport(Constants.LibName, EntryPoint = "func_realize_byte_buffer")]
        private static extern IntPtr FuncRealizeByteBuffer(IntPtr func, IntPtr buffer);
        
        
        [DllImport(Constants.LibName, EntryPoint = "func_realize_int_2d")]
        private static extern IntPtr FuncRealizeInt(IntPtr func, int width, int height);
        
        [DllImport(Constants.LibName, EntryPoint = "func_realize_float_2d")]
        private static extern IntPtr FuncRealizeFloat(IntPtr func, int width, int height);
        
        [DllImport(Constants.LibName, EntryPoint = "func_realize_byte_2d")]
        private static extern IntPtr FuncRealizeByte(IntPtr func, int width, int height);
        
        
        [DllImport(Constants.LibName, EntryPoint = "func_realize_int_3d")]
        private static extern IntPtr FuncRealizeInt(IntPtr func, int width, int height, int channels);
        
        [DllImport(Constants.LibName, EntryPoint = "func_realize_float_3d")]
        private static extern IntPtr FuncRealizeFloat(IntPtr func, int width, int height, int channels);
        
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

        public void Realize<T>(HSBuffer<T> buffer) where T: struct
        {
            if (typeof(T) == typeof(int))
            {
                FuncRealizeIntBuffer(_cppobj, buffer._cppobj);
            }
            else if (typeof(T) == typeof(float))
            {
                FuncRealizeFloatBuffer(_cppobj, buffer._cppobj);
            }
            else if (typeof(T) == typeof(byte))
            {
                FuncRealizeByteBuffer(_cppobj, buffer._cppobj);
            }
            else
            {
                throw new NotImplementedException($"Cannot realize to buffer of type {typeof(T)}");
            }
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

        [DllImport(Constants.LibName, EntryPoint = "func_print_loop_nest")]
        private static extern void FuncPrintLoopNest(IntPtr func);

        public void PrintLoopNest()
        {
            FuncPrintLoopNest(_cppobj);
        }

        [DllImport(Constants.LibName, EntryPoint = "func_reorder")]
        private static extern void FuncReorder(IntPtr func, int nrVariables, IntPtr[] vars);

        public void Reorder(params HSVar[] vars)
        {
            var varPointers = new IntPtr[vars.Length];
            for (var i = 0; i < vars.Length; i++)
            {
                varPointers[i] = vars[i]._cppobj;
            }
            
            FuncReorder(_cppobj, vars.Length, varPointers);
        }

        [DllImport(Constants.LibName, EntryPoint = "func_split")]
        private static extern void FuncSplit(IntPtr func, IntPtr origVar, IntPtr var1, IntPtr var2, int factor);

        public void Split(HSVar origVar, HSVar v1, HSVar v2, int factor)
        {
            FuncSplit(_cppobj, origVar._cppobj, v1._cppobj, v2._cppobj, factor);
        }

        [DllImport(Constants.LibName, EntryPoint = "func_fuse")]
        private static extern void FuncFuse(IntPtr func, IntPtr var1, IntPtr var2, IntPtr fused);

        public HSFunc Fuse(HSVar var1, HSVar var2, HSVar fused)
        {
            FuncFuse(_cppobj, var1._cppobj, var2._cppobj, fused._cppobj);
            return this;
        }

        [DllImport(Constants.LibName, EntryPoint = "func_vectorize")]
        private static extern void FuncVectorize(IntPtr func, IntPtr var1);

        public HSFunc Vectorize(HSVar var1)
        {
            FuncVectorize(_cppobj, var1._cppobj);
            return this;
        }
        
        [DllImport(Constants.LibName, EntryPoint = "func_unroll")]
        private static extern void FuncUnroll(IntPtr func, IntPtr var1);

        public void Unroll(HSVar var1)
        {
            FuncUnroll(_cppobj, var1._cppobj);
        }

        [DllImport(Constants.LibName, EntryPoint = "func_tile")]
        private static extern void FuncTile(IntPtr func, IntPtr x, IntPtr y, IntPtr xo, IntPtr yo, IntPtr xi,
            IntPtr yi, int xfactor, int yfactor);

        public HSFunc Tile(HSVar x, HSVar y, HSVar xo, HSVar yo, HSVar xi, HSVar yi, int xfactor, int yfactor)
        {
            FuncTile(_cppobj, x._cppobj, y._cppobj, xo._cppobj, yo._cppobj, xi._cppobj, yi._cppobj, xfactor, yfactor);
            return this;
        }
    }
}