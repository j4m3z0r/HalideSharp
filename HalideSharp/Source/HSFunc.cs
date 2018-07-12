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

        // Ok, so in C++, you're able to use either Vars or Exprs interchangeably, and
        // it works by having Var have an implicit conversion to Expr. We could do this
        // by having Var inherit from Expr, but that's not how it works in C++, and I
        // figure that using a different inheritance hierarchy in C# is likely to introduce
        // other problems elsewhere. So we go ahead and just enumerate all the
        // possible combinations of Vars and Exprs for indexers here. This is pretty
        // awful, but it's localized awfulness.
        //
        // A quick word on naming: we have pairs of methods, for setting and getting based on how many dimensions of
        // indexing we want to do:
        //
        // Setter: FuncSet<dim 1 type><dim 2 type><...>_<object type>
        // Getter: FuncGet<object type>_<dim 1 type><dim 2 type><...>
        //
        // Likewise, the name will match the order of the arguments of the wrapping method, both in C# and in the C
        // wrapper library.
        #region 2D indexers
        
        #region var var
        [DllImport(Constants.LibName, EntryPoint = "func_getexpr__var_var")]
        public static extern IntPtr FuncGetExpr_VarVar(IntPtr obj, IntPtr x, IntPtr y);
        
        [DllImport(Constants.LibName, EntryPoint = "func_set_var_var__expr")]
        private static extern void FuncSetVarVar_Expr(IntPtr func, IntPtr v1, IntPtr v2, IntPtr e);
        
        public HSExpr this[HSVar v1, HSVar v2]
        {
            get => new HSExpr(FuncGetExpr_VarVar(_cppobj, v1._cppobj, v2._cppobj));
            set => FuncSetVarVar_Expr(_cppobj, v1._cppobj, v2._cppobj, value._cppobj);
        }
        #endregion
        
        #region var expr
        [DllImport(Constants.LibName, EntryPoint = "func_getexpr__var_expr")]
        public static extern IntPtr FuncGetExpr_VarExpr(IntPtr obj, IntPtr x, IntPtr y);
        
        [DllImport(Constants.LibName, EntryPoint = "func_set_var_expr__expr")]
        private static extern void FuncSetVarExpr_Expr(IntPtr func, IntPtr v1, IntPtr v2, IntPtr e);
        
        public HSExpr this[HSVar v1, HSExpr v2]
        {
            get => new HSExpr(FuncGetExpr_VarExpr(_cppobj, v1._cppobj, v2._cppobj));
            set => FuncSetVarExpr_Expr(_cppobj, v1._cppobj, v2._cppobj, value._cppobj);
        }
        #endregion
        
        #region expr var
        [DllImport(Constants.LibName, EntryPoint = "func_getexpr__expr_var")]
        public static extern IntPtr FuncGetExpr_ExprVar(IntPtr obj, IntPtr x, IntPtr y);
        
        [DllImport(Constants.LibName, EntryPoint = "func_set_expr_var__expr")]
        private static extern void FuncSetExprVar_Expr(IntPtr func, IntPtr v1, IntPtr v2, IntPtr e);
        
        public HSExpr this[HSExpr v1, HSVar v2]
        {
            get => new HSExpr(FuncGetExpr_ExprVar(_cppobj, v1._cppobj, v2._cppobj));
            set => FuncSetExprVar_Expr(_cppobj, v1._cppobj, v2._cppobj, value._cppobj);
        }
        #endregion
        
        
        #region expr expr
        [DllImport(Constants.LibName, EntryPoint = "func_getexpr__expr_expr")]
        public static extern IntPtr FuncGetExpr_ExprExpr(IntPtr obj, IntPtr x, IntPtr y);
        
        [DllImport(Constants.LibName, EntryPoint = "func_set_expr_expr__expr")]
        private static extern void FuncSetExprExpr_Expr(IntPtr func, IntPtr v1, IntPtr v2, IntPtr e);
        
        public HSExpr this[HSExpr v1, HSExpr v2]
        {
            get => new HSExpr(FuncGetExpr_ExprExpr(_cppobj, v1._cppobj, v2._cppobj));
            set => FuncSetExprExpr_Expr(_cppobj, v1._cppobj, v2._cppobj, value._cppobj);
        }
        #endregion
        
        #endregion

        #region 3D indexers
        
        #region var var var
        [DllImport(Constants.LibName, EntryPoint = "func_getexpr__var_var_var")]
        public static extern IntPtr FuncGetExpr_VarVarVar(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        
        [DllImport(Constants.LibName, EntryPoint = "func_set_var_var_var__expr")]
        private static extern void FuncSetVarVarVar_Expr(IntPtr func, IntPtr v1, IntPtr v2, IntPtr v3, IntPtr e);
        
        public HSExpr this[HSVar v1, HSVar v2, HSVar v3]
        {
            get => new HSExpr(FuncGetExpr_VarVarVar(_cppobj, v1._cppobj, v2._cppobj, v3._cppobj));
            set => FuncSetVarVarVar_Expr(_cppobj, v1._cppobj, v2._cppobj, v3._cppobj, value._cppobj);
        }
        #endregion
        
        #region expr var var
        [DllImport(Constants.LibName, EntryPoint = "func_getexpr__expr_var_var")]
        public static extern IntPtr FuncGetExpr_ExprVarVar(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        
        [DllImport(Constants.LibName, EntryPoint = "func_set_expr_var_var__expr")]
        private static extern void FuncSetExprVarVar_Expr(IntPtr func, IntPtr v1, IntPtr v2, IntPtr v3, IntPtr e);
        
        public HSExpr this[HSExpr v1, HSVar v2, HSVar v3]
        {
            get => new HSExpr(FuncGetExpr_ExprVarVar(_cppobj, v1._cppobj, v2._cppobj, v3._cppobj));
            set => FuncSetExprVarVar_Expr(_cppobj, v1._cppobj, v2._cppobj, v3._cppobj, value._cppobj);
        }
        #endregion
        
        #region var expr var
        [DllImport(Constants.LibName, EntryPoint = "func_getexpr__var_expr_var")]
        public static extern IntPtr FuncGetExpr_VarExprVar(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        
        [DllImport(Constants.LibName, EntryPoint = "func_set_var_expr_var__expr")]
        private static extern void FuncSetVarExprVar_Expr(IntPtr func, IntPtr v1, IntPtr v2, IntPtr v3, IntPtr e);
        
        public HSExpr this[HSVar v1, HSExpr v2, HSVar v3]
        {
            get => new HSExpr(FuncGetExpr_VarExprVar(_cppobj, v1._cppobj, v2._cppobj, v3._cppobj));
            set => FuncSetVarExprVar_Expr(_cppobj, v1._cppobj, v2._cppobj, v3._cppobj, value._cppobj);
        }
        #endregion
        
        #region var var expr
        [DllImport(Constants.LibName, EntryPoint = "func_getexpr__var_var_expr")]
        public static extern IntPtr FuncGetExpr_VarVarExpr(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        
        [DllImport(Constants.LibName, EntryPoint = "func_set_var_var_expr__expr")]
        private static extern void FuncSetVarVarExpr_Expr(IntPtr func, IntPtr v1, IntPtr v2, IntPtr v3, IntPtr e);
        
        public HSExpr this[HSVar v1, HSVar v2, HSExpr v3]
        {
            get => new HSExpr(FuncGetExpr_VarVarExpr(_cppobj, v1._cppobj, v2._cppobj, v3._cppobj));
            set => FuncSetVarVarExpr_Expr(_cppobj, v1._cppobj, v2._cppobj, v3._cppobj, value._cppobj);
        }
        #endregion
        
        #region expr expr var
        [DllImport(Constants.LibName, EntryPoint = "func_getexpr__expr_expr_var")]
        public static extern IntPtr FuncGetExpr_ExprExprVar(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        
        [DllImport(Constants.LibName, EntryPoint = "func_set_expr_expr_var__expr")]
        private static extern void FuncSetExprExprVar_Expr(IntPtr func, IntPtr v1, IntPtr v2, IntPtr v3, IntPtr e);
        
        public HSExpr this[HSExpr v1, HSExpr v2, HSVar v3]
        {
            get => new HSExpr(FuncGetExpr_ExprExprVar(_cppobj, v1._cppobj, v2._cppobj, v3._cppobj));
            set => FuncSetExprExprVar_Expr(_cppobj, v1._cppobj, v2._cppobj, v3._cppobj, value._cppobj);
        }
        #endregion
        
        #region var expr expr
        [DllImport(Constants.LibName, EntryPoint = "func_getexpr__var_expr_expr")]
        public static extern IntPtr FuncGetExpr_VarExprExpr(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        
        [DllImport(Constants.LibName, EntryPoint = "func_set_var_expr_expr__expr")]
        private static extern void FuncSetVarExprExpr_Expr(IntPtr func, IntPtr v1, IntPtr v2, IntPtr v3, IntPtr e);
        
        public HSExpr this[HSVar v1, HSExpr v2, HSExpr v3]
        {
            get => new HSExpr(FuncGetExpr_VarExprExpr(_cppobj, v1._cppobj, v2._cppobj, v3._cppobj));
            set => FuncSetVarExprExpr_Expr(_cppobj, v1._cppobj, v2._cppobj, v3._cppobj, value._cppobj);
        }
        #endregion
        
        #region expr var expr
        [DllImport(Constants.LibName, EntryPoint = "func_getexpr__expr_var_expr")]
        public static extern IntPtr FuncGetExpr_ExprVarExpr(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        
        [DllImport(Constants.LibName, EntryPoint = "func_set_expr_var_expr__expr")]
        private static extern void FuncSetExprVarExpr_Expr(IntPtr func, IntPtr v1, IntPtr v2, IntPtr v3, IntPtr e);
        
        public HSExpr this[HSExpr v1, HSVar v2, HSExpr v3]
        {
            get => new HSExpr(FuncGetExpr_ExprVarExpr(_cppobj, v1._cppobj, v2._cppobj, v3._cppobj));
            set => FuncSetExprVarExpr_Expr(_cppobj, v1._cppobj, v2._cppobj, v3._cppobj, value._cppobj);
        }
        #endregion
        
        #region expr expr expr
        [DllImport(Constants.LibName, EntryPoint = "func_getexpr__expr_expr_expr")]
        public static extern IntPtr FuncGetExpr_ExprExprExpr(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        
        [DllImport(Constants.LibName, EntryPoint = "func_set_expr_expr_expr__expr")]
        private static extern void FuncSetExprExprExpr_Expr(IntPtr func, IntPtr v1, IntPtr v2, IntPtr v3, IntPtr e);
        
        public HSExpr this[HSExpr v1, HSExpr v2, HSExpr v3]
        {
            get => new HSExpr(FuncGetExpr_ExprExprExpr(_cppobj, v1._cppobj, v2._cppobj, v3._cppobj));
            set => FuncSetExprExprExpr_Expr(_cppobj, v1._cppobj, v2._cppobj, v3._cppobj, value._cppobj);
        }
        #endregion
        #endregion

        #region Realize to pre-allocated buffer
        [DllImport(Constants.LibName, EntryPoint = "func_realize_int_buffer")]
        private static extern IntPtr FuncRealizeIntBuffer(IntPtr func, IntPtr buffer);
        
        [DllImport(Constants.LibName, EntryPoint = "func_realize_float_buffer")]
        private static extern IntPtr FuncRealizeFloatBuffer(IntPtr func, IntPtr buffer);
        
        [DllImport(Constants.LibName, EntryPoint = "func_realize_byte_buffer")]
        private static extern IntPtr FuncRealizeByteBuffer(IntPtr func, IntPtr buffer);
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
        #endregion

        #region Realize to new buffer, 2D
        [DllImport(Constants.LibName, EntryPoint = "func_realize_int_2d")]
        private static extern IntPtr FuncRealizeInt(IntPtr func, int width, int height);
        
        [DllImport(Constants.LibName, EntryPoint = "func_realize_float_2d")]
        private static extern IntPtr FuncRealizeFloat(IntPtr func, int width, int height);
        
        [DllImport(Constants.LibName, EntryPoint = "func_realize_byte_2d")]
        private static extern IntPtr FuncRealizeByte(IntPtr func, int width, int height);
        
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
        #endregion
        
        #region Realize to new buffer, 3D
        [DllImport(Constants.LibName, EntryPoint = "func_realize_int_3d")]
        private static extern IntPtr FuncRealizeInt(IntPtr func, int width, int height, int channels);
        
        [DllImport(Constants.LibName, EntryPoint = "func_realize_float_3d")]
        private static extern IntPtr FuncRealizeFloat(IntPtr func, int width, int height, int channels);
        
        [DllImport(Constants.LibName, EntryPoint = "func_realize_byte_3d")]
        private static extern IntPtr FuncRealizeByte(IntPtr func, int width, int height, int channels);
        
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
        #endregion
        

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
        
        [DllImport(Constants.LibName, EntryPoint = "func_vectorize_int")]
        private static extern void FuncVectorizeInt(IntPtr func, IntPtr var1, int factor);

        public HSFunc Vectorize(HSVar var1, int factor)
        {
            FuncVectorizeInt(_cppobj, var1._cppobj, factor);
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

        [DllImport(Constants.LibName, EntryPoint = "func_compute_root")]
        private static extern void FuncComputeRoot(IntPtr func);

        public HSFunc ComputeRoot()
        {
            FuncComputeRoot(_cppobj);
            return this;
        }

        [DllImport(Constants.LibName, EntryPoint = "func_compute_at")]
        private static extern void FuncComputeAt(IntPtr self, IntPtr func, IntPtr v);

        public HSFunc ComputeAt(HSFunc f, HSVar v)
        {
            FuncComputeAt(_cppobj, f._cppobj, v._cppobj);
            return this;
        }

        [DllImport(Constants.LibName, EntryPoint = "func_store_root")]
        private static extern void FuncStoreRoot(IntPtr self);

        public HSFunc StoreRoot()
        {
            FuncStoreRoot(_cppobj);
            return this;
        }

        [DllImport(Constants.LibName, EntryPoint = "func_store_at")]
        private static extern void FuncStoreAt(IntPtr self, IntPtr func, IntPtr v);

        public HSFunc StoreAt(HSFunc f, HSVar v)
        {
            FuncStoreAt(_cppobj, f._cppobj, v._cppobj);
            return this;
        }
    }
}