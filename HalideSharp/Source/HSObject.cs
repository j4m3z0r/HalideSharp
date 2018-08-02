using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

namespace HalideSharp
{
    public class HSInternalException : Exception
    {
        public HSInternalException() : base()
        {
        }

        public HSInternalException(string message) : base(message)
        {
        }

        public HSInternalException(string message, Exception inner) : base(message, inner)
        {
        }
    }
    
    public class HSObject
    {
        /// <summary>
        /// HACK: the only reason we have this list is so that we can maintain a reference to the objects that make
        /// up a function. Otherwise they would be deleted when C# does its garbage collection. This isn't ideal:
        /// it means that some objects will live much longer than they should. However it should be manageable since
        /// 1. the trees of these objects aren't very big, and 2. they will eventually be reaped when the root node
        /// is destroyed. Note also that the entries here are not guaranteed to be unique: if a variable is used
        /// multiple times in the definition of a function, then there will be multiple entries for it in this list.
        /// The point is to ensure that we have at least one reference to these objects.
        /// </summary>
        protected List<HSObject> _dependentObjects;
        
        internal IntPtr _cppobj;

        internal HSObject()
        {
            _dependentObjects = new List<HSObject>();
            
            // Configure error handling any time we create a HS object. This is a no-op
            // if it has already been initialized.
            HSErrorHandler.ConfigureErrorHandling();
        }

        internal void AddRef(params HSObject[] objects)
        {
            foreach (var o in objects)
            {
                if (ReferenceEquals(o, this))
                {
                    throw new HSInternalException("Attempt to add object as dependency of itself.");
                }
                _dependentObjects.Add(o);
            }
        }

        // Add dummy AddRef implementations for primitive types. This is so that the generated operators will
        // compile. We don't need to hold references to primitive types.
        internal void AddRef(int a, int b)
        {
        }

        internal void AddRef(int a, float b)
        {
        }

        internal void AddRef(float a, int b)
        {
        }

        internal void AddRef(float a, float b)
        {
        }
        
        // And a few where only one arg is a HSObject
        internal void AddRef(HSObject o, int i)
        {
            _dependentObjects.Add(o);
        }

        internal void AddRef(HSObject o, float f)
        {
            _dependentObjects.Add(o);
        }

        internal void AddRef(int i, HSObject o)
        {
            _dependentObjects.Add(o);
        }

        internal void AddRef(float f, HSObject o)
        {
            _dependentObjects.Add(o);
        }
    }
}