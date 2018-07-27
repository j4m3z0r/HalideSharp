using System;
using System.Runtime.CompilerServices;

namespace HalideSharp
{
    /// <summary>
    /// Represents the set of dimension objects for a imageParam, however it doesn't contain any dimension objects -- it
    /// just invokes Halide's method for getting dimensions.
    /// </summary>
    public class HSImageParamDimensionList<T> where T : struct
    {
        private HSImageParam<T> _imageParam;
        
        internal HSImageParamDimensionList(HSImageParam<T> imageParam)
        {
            _imageParam = imageParam;
        }

        public HSImageParamDimension<T> this[int i] => _imageParam.GetDimension(i);
    }
}

