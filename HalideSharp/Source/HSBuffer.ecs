using System;
using System.Runtime.InteropServices;

namespace HalideSharp
{
    internal class CppBuffer
    {
        #region 2D constructors
        [DllImport(Constants.LibName, EntryPoint = "new_int_buffer_int_int")]
        public static extern IntPtr NewIntBufferIntInt(int width, int height);
        
        [DllImport(Constants.LibName, EntryPoint = "new_float_buffer_int_int")]
        public static extern IntPtr NewFloatBufferIntInt(int width, int height);
        
        [DllImport(Constants.LibName, EntryPoint = "new_byte_buffer_int_int")]
        public static extern IntPtr NewByteBufferIntInt(int width, int height);
        #endregion

        #region 3D constructors
        [DllImport(Constants.LibName, EntryPoint = "new_int_buffer_int_int_int")]
        public static extern IntPtr NewIntBufferIntIntInt(int width, int height, int channels);
        
        [DllImport(Constants.LibName, EntryPoint = "new_float_buffer_int_int_int")]
        public static extern IntPtr NewFloatBufferIntIntInt(int width, int height, int channels);
        
        [DllImport(Constants.LibName, EntryPoint = "new_byte_buffer_int_int_int")]
        public static extern IntPtr NewByteBufferIntIntInt(int width, int height, int channels);
        #endregion

        #region destructors
        [DllImport(Constants.LibName, EntryPoint = "delete_int_buffer")]
        public static extern void DeleteIntBuffer(IntPtr obj);
        
        [DllImport(Constants.LibName, EntryPoint = "delete_float_buffer")]
        public static extern void DeleteFloatBuffer(IntPtr obj);
        
        [DllImport(Constants.LibName, EntryPoint = "delete_byte_buffer")]
        public static extern void DeleteByteBuffer(IntPtr obj);
        #endregion

        
        #region 2D inderxers
        #region int indexers
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_getval__int_int")]
        public static extern void BufferIntGetVal(IntPtr obj, int x, int y, IntPtr result);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_float_getval__int_int")]
        public static extern void BufferFloatGetVal(IntPtr obj, int x, int y, IntPtr result);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_getval__int_int")]
        public static extern void BufferByteGetVal(IntPtr obj, int x, int y, IntPtr result);
        #endregion

        #region expr expr
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_getexpr__Expr_Expr")]
        public static extern IntPtr BufferIntGetExpr_ExprExpr(IntPtr obj, IntPtr x, IntPtr y);

        [DllImport(Constants.LibName, EntryPoint = "buffer_float_getexpr__Expr_Expr")]
        public static extern IntPtr BufferFloatGetExpr_ExprExpr(IntPtr obj, IntPtr x, IntPtr y);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_getexpr__Expr_Expr")]
        public static extern IntPtr BufferByteGetExpr_ExprExpr(IntPtr obj, IntPtr x, IntPtr y);
        #endregion

        #region expr var
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_getexpr__Expr_Var")]
        public static extern IntPtr BufferIntGetExpr_ExprVar(IntPtr obj, IntPtr x, IntPtr y);

        [DllImport(Constants.LibName, EntryPoint = "buffer_float_getexpr__Expr_Var")]
        public static extern IntPtr BufferFloatGetExpr_ExprVar(IntPtr obj, IntPtr x, IntPtr y);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_getexpr__Expr_Var")]
        public static extern IntPtr BufferByteGetExpr_ExprVar(IntPtr obj, IntPtr x, IntPtr y);
        #endregion
        
        #region var expr
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_getexpr__Var_Expr")]
        public static extern IntPtr BufferIntGetExpr_VarExpr(IntPtr obj, IntPtr x, IntPtr y);

        [DllImport(Constants.LibName, EntryPoint = "buffer_float_getexpr__Var_Expr")]
        public static extern IntPtr BufferFloatGetExpr_VarExpr(IntPtr obj, IntPtr x, IntPtr y);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_getexpr__Var_Expr")]
        public static extern IntPtr BufferByteGetExpr_VarExpr(IntPtr obj, IntPtr x, IntPtr y);
        #endregion
        
        #region var var
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_getexpr__Var_Var")]
        public static extern IntPtr BufferIntGetExpr_VarVar(IntPtr obj, IntPtr x, IntPtr y);

        [DllImport(Constants.LibName, EntryPoint = "buffer_float_getexpr__Var_Var")]
        public static extern IntPtr BufferFloatGetExpr_VarVar(IntPtr obj, IntPtr x, IntPtr y);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_getexpr__Var_Var")]
        public static extern IntPtr BufferByteGetExpr_VarVar(IntPtr obj, IntPtr x, IntPtr y);
        #endregion
        #endregion
       
        
        #region 3D indexers
        #region int indexers
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_getval__int_int_int")]
        public static extern void BufferIntGetVal(IntPtr obj, int x, int y, int z, IntPtr result);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_float_getval__int_int_int")]
        public static extern void BufferFloatGetVal(IntPtr obj, int x, int y, int z, IntPtr result);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_getval__int_int_int")]
        public static extern void BufferByteGetVal(IntPtr obj, int x, int y, int z, IntPtr result);
        #endregion


        #region expr expr expr
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_getexpr__Expr_Expr_Expr")]
        public static extern IntPtr BufferIntGetExpr_ExprExprExpr(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);

        [DllImport(Constants.LibName, EntryPoint = "buffer_float_getexpr__Expr_Expr_Expr")]
        public static extern IntPtr BufferFloatGetExpr_ExprExprExpr(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_getexpr__Expr_Expr_Expr")]
        public static extern IntPtr BufferByteGetExpr_ExprExprExpr(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        #endregion
        
        #region expr expr var
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_getexpr__Expr_Expr_Var")]
        public static extern IntPtr BufferIntGetExpr_ExprExprVar(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);

        [DllImport(Constants.LibName, EntryPoint = "buffer_float_getexpr__Expr_Expr_Var")]
        public static extern IntPtr BufferFloatGetExpr_ExprExprVar(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_getexpr__Expr_Expr_Var")]
        public static extern IntPtr BufferByteGetExpr_ExprExprVar(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        #endregion
        
        #region expr var expr
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_getexpr__Expr_Var_Expr")]
        public static extern IntPtr BufferIntGetExpr_ExprVarExpr(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);

        [DllImport(Constants.LibName, EntryPoint = "buffer_float_getexpr__Expr_Var_Expr")]
        public static extern IntPtr BufferFloatGetExpr_ExprVarExpr(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_getexpr__Expr_Var_Expr")]
        public static extern IntPtr BufferByteGetExpr_ExprVarExpr(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        #endregion
        
        #region expr var var
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_getexpr__Expr_Var_Var")]
        public static extern IntPtr BufferIntGetExpr_ExprVarVar(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);

        [DllImport(Constants.LibName, EntryPoint = "buffer_float_getexpr__Expr_Var_Var")]
        public static extern IntPtr BufferFloatGetExpr_ExprVarVar(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_getexpr__Expr_Var_Var")]
        public static extern IntPtr BufferByteGetExpr_ExprVarVar(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        #endregion
        
        #region var expr expr
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_getexpr__Var_Expr_Expr")]
        public static extern IntPtr BufferIntGetExpr_VarExprExpr(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);

        [DllImport(Constants.LibName, EntryPoint = "buffer_float_getexpr__Var_Expr_Expr")]
        public static extern IntPtr BufferFloatGetExpr_VarExprExpr(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_getexpr__Var_Expr_Expr")]
        public static extern IntPtr BufferByteGetExpr_VarExprExpr(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        #endregion
        
        #region var expr var
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_getexpr__Var_Expr_Var")]
        public static extern IntPtr BufferIntGetExpr_VarExprVar(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);

        [DllImport(Constants.LibName, EntryPoint = "buffer_float_getexpr__Var_Expr_Var")]
        public static extern IntPtr BufferFloatGetExpr_VarExprVar(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_getexpr__Var_Expr_Var")]
        public static extern IntPtr BufferByteGetExpr_VarExprVar(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        #endregion
        
        #region var var expr
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_getexpr__Var_Var_Expr")]
        public static extern IntPtr BufferIntGetExpr_VarVarExpr(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);

        [DllImport(Constants.LibName, EntryPoint = "buffer_float_getexpr__Var_Var_Expr")]
        public static extern IntPtr BufferFloatGetExpr_VarVarExpr(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_getexpr__Var_Var_Expr")]
        public static extern IntPtr BufferByteGetExpr_VarVarExpr(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        #endregion
        
        #region var var var
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_getexpr__Var_Var_Var")]
        public static extern IntPtr BufferIntGetExpr_VarVarVar(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);

        [DllImport(Constants.LibName, EntryPoint = "buffer_float_getexpr__Var_Var_Var")]
        public static extern IntPtr BufferFloatGetExpr_VarVarVar(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_getexpr__Var_Var_Var")]
        public static extern IntPtr BufferByteGetExpr_VarVarVar(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        #endregion
        
        #endregion
        
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_setmin")]
        public static extern int BufferIntSetMin(IntPtr obj, int x, int y);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_float_setmin")]
        public static extern int BufferFloatSetMin(IntPtr obj, int x, int y);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_setmin")]
        public static extern int BufferByteSetMin(IntPtr obj, int x, int y);


        #region Dimension size accessors
        #region Width accessors
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_width")]
        public static extern int BufferIntWidth(IntPtr obj);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_float_width")]
        public static extern int BufferFloatWidth(IntPtr obj);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_width")]
        public static extern int BufferByteWidth(IntPtr obj);
        #endregion
        
        #region Height accessors
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_height")]
        public static extern int BufferIntHeight(IntPtr obj);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_float_height")]
        public static extern int BufferFloatHeight(IntPtr obj);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_height")]
        public static extern int BufferByteHeight(IntPtr obj);
        #endregion

        #region Channel accessors
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_channels")]
        public static extern int BufferIntChannels(IntPtr obj);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_float_channels")]
        public static extern int BufferFloatChannels(IntPtr obj);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_channels")]
        public static extern int BufferByteChannels(IntPtr obj);
        #endregion
        #endregion

        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_load_image")]
        public static extern IntPtr BufferByteLoadImage([MarshalAs(Constants.StringType)] string filename);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_save_image")]
        public static extern IntPtr BufferByteSaveImage(IntPtr buffer, [MarshalAs(Constants.StringType)] string filename);
    }
    
    public class HSBuffer<T> where T: struct
    {
        internal IntPtr _cppobj;

        private void CheckType()
        {
            if (typeof(T) != typeof(int) && typeof(T) != typeof(float) && typeof(T) != typeof(byte))
            {
                throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
            }
        }
        
        public HSBuffer(int width, int height)
        {
            if (typeof(T) == typeof(int))
            {
                _cppobj = CppBuffer.NewIntBufferIntInt(width, height);
            }
            else if (typeof(T) == typeof(float))
            {
                _cppobj = CppBuffer.NewFloatBufferIntInt(width, height);
            }
            else if (typeof(T) == typeof(byte))
            {
                _cppobj = CppBuffer.NewByteBufferIntInt(width, height);
            }
            else
            {
                throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
            }
        }
        
        public HSBuffer(int width, int height, int channels)
        {
            if (typeof(T) == typeof(int))
            {
                _cppobj = CppBuffer.NewIntBufferIntIntInt(width, height, channels);
            }
            else if (typeof(T) == typeof(float))
            {
                _cppobj = CppBuffer.NewFloatBufferIntIntInt(width, height, channels);
            }
            else if (typeof(T) == typeof(byte))
            {
                _cppobj = CppBuffer.NewByteBufferIntIntInt(width, height, channels);
            }
            else
            {
                throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
            }
        }
        
        internal HSBuffer(IntPtr cppobj)
        {
            CheckType();
            _cppobj = cppobj;
        }

        public static HSBuffer<T> LoadImage(string filename)
        {
            if (typeof(T) == typeof(byte))
            {
                return new HSBuffer<T>(CppBuffer.BufferByteLoadImage(filename));
            }
            else
            {
                throw new NotImplementedException($"Can't load images to buffer of type {typeof(T)}");
            }
        }

        public void SaveImage(string filename)
        {
            if (typeof(T) == typeof(byte))
            {
                CppBuffer.BufferByteSaveImage(_cppobj, filename);
            }
            else
            {
                throw new NotImplementedException($"Can't save images to buffer of type {typeof(T)}");
            }
        }

        ~HSBuffer()
        {
            CheckType();
            if (_cppobj == IntPtr.Zero)
            {
                return;
            }
            if(typeof(T) == typeof(int))
            {
                CppBuffer.DeleteIntBuffer(_cppobj);
            }
            else if(typeof(T) == typeof(float))
            {
                CppBuffer.DeleteFloatBuffer(_cppobj);
            }
            else if (typeof(T) == typeof(byte))
            {
                CppBuffer.DeleteByteBuffer(_cppobj);
            }
            else
            {
                throw new NotImplementedException($"No destructor for type {typeof(T)}");
            }
        }

        public T GetVal(int x, int y)
        {
            // Here's what's going on with this piece of pure artisinal C#: over in C++ land, Halide::Buffer is a
            // templated type. However, since we need to route calls to that templated C++ code via a C interface,
            // we have a different version of the C wrapper function for each type we're interested in -- hence the
            // test against typeof(T). However, the real trick is in fooling C# into letting C/C++ fill in the
            // result value. We can't use unsafe & fixed, since T is a generic, and you can't have a generic type
            // for fixed(). GCHandle will do the trick, but there's one final wrinkle: if you use GCHandle on a
            // non-object type (which we have constrained T to be), it won't pass the address of the original value
            // but will instead make a copy of the value and give you the address of that copy. At that point, C++
            // will fill in the correct value, and C# will promptly throw it away. To workaround this, we create
            // a temporary array of Ts of length 1, take the address of the array, then return the first element.
            var result = new T[1];
            var resultHandle = GCHandle.Alloc(result, GCHandleType.Pinned);

            try
            {
                if (typeof(T) == typeof(int))
                {
                    CppBuffer.BufferIntGetVal(_cppobj, x, y, resultHandle.AddrOfPinnedObject());
                }
                else if (typeof(T) == typeof(float))
                {
                    CppBuffer.BufferFloatGetVal(_cppobj, x, y, resultHandle.AddrOfPinnedObject());
                }
                else if (typeof(T) == typeof(byte))
                {
                    CppBuffer.BufferByteGetVal(_cppobj, x, y, resultHandle.AddrOfPinnedObject());
                }
                else
                {
                    throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
                }
            }
            finally
            {
                resultHandle.Free();
            }
            
            return result[0];
        }

        public T GetVal(int x, int y, int z)
        {
            // See above
            var result = new T[1];
            var resultHandle = GCHandle.Alloc(result, GCHandleType.Pinned);

            try
            {
                if (typeof(T) == typeof(int))
                {
                    CppBuffer.BufferIntGetVal(_cppobj, x, y, z, resultHandle.AddrOfPinnedObject());
                }
                else if (typeof(T) == typeof(float))
                {
                    CppBuffer.BufferFloatGetVal(_cppobj, x, y, z, resultHandle.AddrOfPinnedObject());
                }
                else if (typeof(T) == typeof(byte))
                {
                    CppBuffer.BufferByteGetVal(_cppobj, x, y, z, resultHandle.AddrOfPinnedObject());
                }
                else
                {
                    throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
                }
            }
            finally
            {
                resultHandle.Free();
            }
            
            return result[0];
        }

        public T this[int x, int y] => GetVal(x, y);
        public T this[int x, int y, int z] => GetVal(x, y, z);

        #region 2D indexers
        #region expr expr
        public HSExpr GetExpr(HSExpr x, HSExpr y)
        {
            if(typeof(T) == typeof(int))
            {
                return new HSExpr(CppBuffer.BufferIntGetExpr_ExprExpr(_cppobj, x._cppobj, y._cppobj));
            }
            else if(typeof(T) == typeof(float))
            {
                return new HSExpr(CppBuffer.BufferFloatGetExpr_ExprExpr(_cppobj, x._cppobj, y._cppobj));
            }
            else if (typeof(T) == typeof(byte))
            {
                return new HSExpr(CppBuffer.BufferByteGetExpr_ExprExpr(_cppobj, x._cppobj, y._cppobj));
            }
            else
            {
                throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
            }
        }
        public HSExpr this[HSExpr x, HSExpr y] => GetExpr(x, y);
        #endregion
        
        #region expr var
        public HSExpr GetExpr(HSExpr x, HSVar y)
        {
            if(typeof(T) == typeof(int))
            {
                return new HSExpr(CppBuffer.BufferIntGetExpr_ExprVar(_cppobj, x._cppobj, y._cppobj));
            }
            else if(typeof(T) == typeof(float))
            {
                return new HSExpr(CppBuffer.BufferFloatGetExpr_ExprVar(_cppobj, x._cppobj, y._cppobj));
            }
            else if (typeof(T) == typeof(byte))
            {
                return new HSExpr(CppBuffer.BufferByteGetExpr_ExprVar(_cppobj, x._cppobj, y._cppobj));
            }
            else
            {
                throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
            }
        }
        public HSExpr this[HSExpr x, HSVar y] => GetExpr(x, y);
        #endregion
        
        #region var expr
        public HSExpr GetExpr(HSVar x, HSExpr y)
        {
            if(typeof(T) == typeof(int))
            {
                return new HSExpr(CppBuffer.BufferIntGetExpr_VarExpr(_cppobj, x._cppobj, y._cppobj));
            }
            else if(typeof(T) == typeof(float))
            {
                return new HSExpr(CppBuffer.BufferFloatGetExpr_VarExpr(_cppobj, x._cppobj, y._cppobj));
            }
            else if (typeof(T) == typeof(byte))
            {
                return new HSExpr(CppBuffer.BufferByteGetExpr_VarExpr(_cppobj, x._cppobj, y._cppobj));
            }
            else
            {
                throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
            }
        }
        public HSExpr this[HSVar x, HSExpr y] => GetExpr(x, y);
        #endregion
        
        #region var var
        public HSExpr GetExpr(HSVar x, HSVar y)
        {
            if(typeof(T) == typeof(int))
            {
                return new HSExpr(CppBuffer.BufferIntGetExpr_VarVar(_cppobj, x._cppobj, y._cppobj));
            }
            else if(typeof(T) == typeof(float))
            {
                return new HSExpr(CppBuffer.BufferFloatGetExpr_VarVar(_cppobj, x._cppobj, y._cppobj));
            }
            else if (typeof(T) == typeof(byte))
            {
                return new HSExpr(CppBuffer.BufferByteGetExpr_VarVar(_cppobj, x._cppobj, y._cppobj));
            }
            else
            {
                throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
            }
        }
        public HSExpr this[HSVar x, HSVar y] => GetExpr(x, y);
        #endregion
        #endregion

        #region 3D indexers
        
        #region expr expr expr
        public HSExpr GetExpr(HSExpr x, HSExpr y, HSExpr z)
        {
            if(typeof(T) == typeof(int))
            {
                return new HSExpr(CppBuffer.BufferIntGetExpr_ExprExprExpr(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else if(typeof(T) == typeof(float))
            {
                return new HSExpr(CppBuffer.BufferFloatGetExpr_ExprExprExpr(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else if (typeof(T) == typeof(byte))
            {
                return new HSExpr(CppBuffer.BufferByteGetExpr_ExprExprExpr(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else
            {
                throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
            }
        }
        public HSExpr this[HSExpr x, HSExpr y, HSExpr z] => GetExpr(x, y, z);
        #endregion
        
        #region expr expr var
        public HSExpr GetExpr(HSExpr x, HSExpr y, HSVar z)
        {
            if(typeof(T) == typeof(int))
            {
                return new HSExpr(CppBuffer.BufferIntGetExpr_ExprExprVar(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else if(typeof(T) == typeof(float))
            {
                return new HSExpr(CppBuffer.BufferFloatGetExpr_ExprExprVar(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else if (typeof(T) == typeof(byte))
            {
                return new HSExpr(CppBuffer.BufferByteGetExpr_ExprExprVar(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else
            {
                throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
            }
        }
        public HSExpr this[HSExpr x, HSExpr y, HSVar z] => GetExpr(x, y, z);
        #endregion
        
        #region expr var expr
        public HSExpr GetExpr(HSExpr x, HSVar y, HSExpr z)
        {
            if(typeof(T) == typeof(int))
            {
                return new HSExpr(CppBuffer.BufferIntGetExpr_ExprVarExpr(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else if(typeof(T) == typeof(float))
            {
                return new HSExpr(CppBuffer.BufferFloatGetExpr_ExprVarExpr(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else if (typeof(T) == typeof(byte))
            {
                return new HSExpr(CppBuffer.BufferByteGetExpr_ExprVarExpr(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else
            {
                throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
            }
        }
        public HSExpr this[HSExpr x, HSVar y, HSExpr z] => GetExpr(x, y, z);
        #endregion
        
        #region expr var var
        public HSExpr GetExpr(HSExpr x, HSVar y, HSVar z)
        {
            if(typeof(T) == typeof(int))
            {
                return new HSExpr(CppBuffer.BufferIntGetExpr_ExprVarVar(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else if(typeof(T) == typeof(float))
            {
                return new HSExpr(CppBuffer.BufferFloatGetExpr_ExprVarVar(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else if (typeof(T) == typeof(byte))
            {
                return new HSExpr(CppBuffer.BufferByteGetExpr_ExprVarVar(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else
            {
                throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
            }
        }
        public HSExpr this[HSExpr x, HSVar y, HSVar z] => GetExpr(x, y, z);
        #endregion
        
        #region var expr expr
        public HSExpr GetExpr(HSVar x, HSExpr y, HSExpr z)
        {
            if(typeof(T) == typeof(int))
            {
                return new HSExpr(CppBuffer.BufferIntGetExpr_VarExprExpr(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else if(typeof(T) == typeof(float))
            {
                return new HSExpr(CppBuffer.BufferFloatGetExpr_VarExprExpr(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else if (typeof(T) == typeof(byte))
            {
                return new HSExpr(CppBuffer.BufferByteGetExpr_VarExprExpr(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else
            {
                throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
            }
        }
        public HSExpr this[HSVar x, HSExpr y, HSExpr z] => GetExpr(x, y, z);
        #endregion
        
        #region var expr var
        public HSExpr GetExpr(HSVar x, HSExpr y, HSVar z)
        {
            if(typeof(T) == typeof(int))
            {
                return new HSExpr(CppBuffer.BufferIntGetExpr_VarExprVar(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else if(typeof(T) == typeof(float))
            {
                return new HSExpr(CppBuffer.BufferFloatGetExpr_VarExprVar(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else if (typeof(T) == typeof(byte))
            {
                return new HSExpr(CppBuffer.BufferByteGetExpr_VarExprVar(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else
            {
                throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
            }
        }
        public HSExpr this[HSVar x, HSExpr y, HSVar z] => GetExpr(x, y, z);
        #endregion
        
        #region var var expr
        public HSExpr GetExpr(HSVar x, HSVar y, HSExpr z)
        {
            if(typeof(T) == typeof(int))
            {
                return new HSExpr(CppBuffer.BufferIntGetExpr_VarVarExpr(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else if(typeof(T) == typeof(float))
            {
                return new HSExpr(CppBuffer.BufferFloatGetExpr_VarVarExpr(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else if (typeof(T) == typeof(byte))
            {
                return new HSExpr(CppBuffer.BufferByteGetExpr_VarVarExpr(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else
            {
                throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
            }
        }
        public HSExpr this[HSVar x, HSVar y, HSExpr z] => GetExpr(x, y, z);
        #endregion
        
        #region var var var
        public HSExpr GetExpr(HSVar x, HSVar y, HSVar z)
        {
            if(typeof(T) == typeof(int))
            {
                return new HSExpr(CppBuffer.BufferIntGetExpr_VarVarVar(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else if(typeof(T) == typeof(float))
            {
                return new HSExpr(CppBuffer.BufferFloatGetExpr_VarVarVar(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else if (typeof(T) == typeof(byte))
            {
                return new HSExpr(CppBuffer.BufferByteGetExpr_VarVarVar(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else
            {
                throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
            }
        }
        public HSExpr this[HSVar x, HSVar y, HSVar z] => GetExpr(x, y, z);
        #endregion
        #endregion
        
        public void SetMin(int x, int y)
        {
            if(typeof(T) == typeof(int))
            {
                CppBuffer.BufferIntSetMin(_cppobj, x, y);
            }
            else if(typeof(T) == typeof(float))
            {
                CppBuffer.BufferFloatSetMin(_cppobj, x, y);
            }
            else if (typeof(T) == typeof(byte))
            {
                CppBuffer.BufferByteSetMin(_cppobj, x, y);
            }
            else
            {
                throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
            }
        }

        public int Width
        {
            get
            {
                if (typeof(T) == typeof(int))
                {
                    return CppBuffer.BufferIntWidth(_cppobj);
                }
                else if (typeof(T) == typeof(float))
                {
                    return CppBuffer.BufferFloatWidth(_cppobj);
                }
                else if (typeof(T) == typeof(byte))
                {
                    return CppBuffer.BufferByteWidth(_cppobj);
                }
                else
                {
                    throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
                }
            }
        } 
        

        public int Height
        {
            get
            {
                if (typeof(T) == typeof(int))
                {
                    return CppBuffer.BufferIntHeight(_cppobj);      
                }
                else if (typeof(T) == typeof(float))
                {
                    return CppBuffer.BufferFloatHeight(_cppobj);      
                }
                else if(typeof(T) == typeof(byte))
                {
                    return CppBuffer.BufferByteHeight(_cppobj);      
                }
                else
                {
                    throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
                }
            }
        } 
        
        public int Channels
        {
            get
            {
                if (typeof(T) == typeof(int))
                {
                    return CppBuffer.BufferIntChannels(_cppobj);      
                }
                if (typeof(T) == typeof(float))
                {
                    return CppBuffer.BufferFloatChannels(_cppobj);      
                }
                else if(typeof(T) == typeof(byte))
                {
                    return CppBuffer.BufferByteChannels(_cppobj);      
                }
                else
                {
                    throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
                }
            }
        } 
    }
}