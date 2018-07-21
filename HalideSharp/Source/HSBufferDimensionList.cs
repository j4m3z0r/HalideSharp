using System;
using System.Runtime.CompilerServices;

namespace HalideSharp
{
    /// <summary>
    /// Represents the set of dimension objects for a buffer, however it doesn't contain any dimension objects -- it
    /// just invokes Halide's method for getting dimensions.
    /// </summary>
    public class HSBufferDimensionList<T> where T : struct
    {
        private HSBuffer<T> _buffer;
        
        internal HSBufferDimensionList(HSBuffer<T> buffer)
        {
            _buffer = _buffer;
        }

        public HSBufferDimension<T> this[int i] => _buffer.GetDimension(i);
    }
}

