using System;
using System.Runtime.InteropServices;

namespace HalideSharp
{
    internal class CppBuffer
    {
        [DllImport(Constants.LibName, EntryPoint = "delete_int_buffer")]
        public static extern void DeleteIntBuffer(IntPtr obj);
        
        [DllImport(Constants.LibName, EntryPoint = "delete_float_buffer")]
        public static extern void DeleteFloatBuffer(IntPtr obj);
        
        [DllImport(Constants.LibName, EntryPoint = "delete_byte_buffer")]
        public static extern void DeleteByteBuffer(IntPtr obj);

        
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_getval_2d")]
        public static extern void BufferIntGetVal(IntPtr obj, int x, int y, IntPtr result);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_float_getval_2d")]
        public static extern void BufferFloatGetVal(IntPtr obj, int x, int y, IntPtr result);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_getval_2d")]
        public static extern void BufferByteGetVal(IntPtr obj, int x, int y, IntPtr result);


        [DllImport(Constants.LibName, EntryPoint = "buffer_int_getexpr_2d_var_var")]
        public static extern IntPtr BufferIntGetExprVarVar(IntPtr obj, IntPtr x, IntPtr y);

        [DllImport(Constants.LibName, EntryPoint = "buffer_float_getexpr_2d_var_var")]
        public static extern IntPtr BufferFloatGetExprVarVar(IntPtr obj, IntPtr x, IntPtr y);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_getexpr_2d_var_var")]
        public static extern IntPtr BufferByteGetExprVarVar(IntPtr obj, IntPtr x, IntPtr y);
       
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_getval_3d")]
        public static extern void BufferIntGetVal(IntPtr obj, int x, int y, int z, IntPtr result);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_float_getval_3d")]
        public static extern void BufferFloatGetVal(IntPtr obj, int x, int y, int z, IntPtr result);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_getval_3d")]
        public static extern void BufferByteGetVal(IntPtr obj, int x, int y, int z, IntPtr result);


        [DllImport(Constants.LibName, EntryPoint = "buffer_int_getexpr_3d_var_var_var")]
        public static extern IntPtr BufferIntGetExprVarVarVar(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);

        [DllImport(Constants.LibName, EntryPoint = "buffer_float_getexpr_3d_var_var_var")]
        public static extern IntPtr BufferFloatGetExprVarVarVar(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_getexpr_3d_var_var_var")]
        public static extern IntPtr BufferByteGetExprVarVarVar(IntPtr obj, IntPtr x, IntPtr y, IntPtr z);
        
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_width")]
        public static extern int BufferIntWidth(IntPtr obj);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_float_width")]
        public static extern int BufferFloatWidth(IntPtr obj);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_width")]
        public static extern int BufferByteWidth(IntPtr obj);
        

        [DllImport(Constants.LibName, EntryPoint = "buffer_int_height")]
        public static extern int BufferIntHeight(IntPtr obj);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_float_height")]
        public static extern int BufferFloatHeight(IntPtr obj);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_height")]
        public static extern int BufferByteHeight(IntPtr obj);

        
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_channels")]
        public static extern int BufferIntChannels(IntPtr obj);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_float_channels")]
        public static extern int BufferFloatChannels(IntPtr obj);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_byte_channels")]
        public static extern int BufferByteChannels(IntPtr obj);

        
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

        public HSExpr GetExpr(HSVar x, HSVar y)
        {
            if(typeof(T) == typeof(int))
            {
                return new HSExpr(CppBuffer.BufferIntGetExprVarVar(_cppobj, x._cppobj, y._cppobj));
            }
            else if(typeof(T) == typeof(float))
            {
                return new HSExpr(CppBuffer.BufferFloatGetExprVarVar(_cppobj, x._cppobj, y._cppobj));
            }
            else if (typeof(T) == typeof(byte))
            {
                return new HSExpr(CppBuffer.BufferByteGetExprVarVar(_cppobj, x._cppobj, y._cppobj));
            }
            else
            {
                throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
            }
        }
        
        public HSExpr GetExpr(HSVar x, HSVar y, HSVar z)
        {
            if(typeof(T) == typeof(int))
            {
                return new HSExpr(CppBuffer.BufferIntGetExprVarVarVar(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            if(typeof(T) == typeof(float))
            {
                return new HSExpr(CppBuffer.BufferFloatGetExprVarVarVar(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else if (typeof(T) == typeof(byte))
            {
                return new HSExpr(CppBuffer.BufferByteGetExprVarVarVar(_cppobj, x._cppobj, y._cppobj, z._cppobj));
            }
            else
            {
                throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
            }
        }

        public HSExpr this[HSVar x, HSVar y] => GetExpr(x, y);
        public HSExpr this[HSVar x, HSVar y, HSVar z] => GetExpr(x, y, z);
        

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