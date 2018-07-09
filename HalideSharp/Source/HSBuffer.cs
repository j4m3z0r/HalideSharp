using System;
using System.Runtime.InteropServices;

namespace HalideSharp
{
    internal class CppBuffer
    {
        [DllImport(Constants.LibName, EntryPoint = "delete_int_buffer")]
        public static extern void DeleteIntBuffer(IntPtr obj);

        [DllImport(Constants.LibName, EntryPoint = "buffer_int_getval")]
        public static extern void BufferIntGetVal(IntPtr obj, int x, int y, IntPtr result);
        
        [DllImport(Constants.LibName, EntryPoint = "buffer_int_width")]
        public static extern int BufferIntWidth(IntPtr obj);

        [DllImport(Constants.LibName, EntryPoint = "buffer_int_height")]
        public static extern int BufferIntHeight(IntPtr obj);
    }
    
    public class HSBuffer<T> where T: struct
    {
        internal IntPtr _cppobj;

        private void CheckType()
        {
            if (typeof(T) != typeof(int))
            {
                throw new NotImplementedException($"Buffer type {typeof(T)} unsupported");
            }
        }
        
        internal HSBuffer(IntPtr cppobj)
        {
            CheckType();
            _cppobj = cppobj;
        }

        ~HSBuffer()
        {
            CheckType();
            if(typeof(T) == typeof(int))
            {
                CppBuffer.DeleteIntBuffer(_cppobj);
            }
        }

        public T this[int x, int y]
        {
            get
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
        }

        public int Width
        {
            get
            {
                CheckType();
                return CppBuffer.BufferIntWidth(_cppobj);      
            }
        } 
        

        public int Height
        {
            get
            {
                CheckType();
                return CppBuffer.BufferIntHeight(_cppobj);      
            }
        } 
    }
}