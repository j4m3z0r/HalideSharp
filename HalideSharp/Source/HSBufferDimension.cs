using System;

namespace HalideSharp
{
    /// <summary>
    /// The Dimension type appears to be internal to Halide, so rather than copy the object onto the heap in C++ and
    /// track a pointer, we just hold a reference back to the buffer object and have methods on the buffer to perform
    /// the various tasks you'd do on a Dimension there.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HSBufferDimension<T> where T: struct
    {
        private HSBuffer<T> _buffer;
        private int _dimension;
        
        internal HSBufferDimension(HSBuffer<T> buffer, int d)
        {
            _buffer = buffer;
            _dimension = d;
        }

        public int Stride
        {
            get => _buffer.GetDimensionStride(_dimension);
        }

        public int Extent
        {
            get => _buffer.GetDimensionExtent(_dimension);
        }
    }
}