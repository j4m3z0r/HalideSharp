using System;

namespace HalideSharp
{
    /// <summary>
    /// The Dimension type appears to be internal to Halide, so rather than copy the object onto the heap in C++ and
    /// track a pointer, we just hold a reference back to the imageParam object and have methods on the imageParam to perform
    /// the various tasks you'd do on a Dimension there.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HSImageParamDimension<T> where T: struct
    {
        private HSImageParam<T> _imageParam;
        private int _dimension;
        
        internal HSImageParamDimension(HSImageParam<T> imageParam, int d)
        {
            _imageParam = imageParam;
            _dimension = d;
        }

        public HSExpr Stride()
        {
            return _imageParam.GetDimensionStride(_dimension);
        }

        public void SetStride(int s)
        {
            _imageParam.SetDimensionStride(_dimension, s);
        }

        public HSExpr Extent()
        {
            return _imageParam.GetDimensionExtent(_dimension);
        }

        public void SetExtent(int e)
        {
            _imageParam.SetDimensionExtent(_dimension, e);
        }

    }
}
