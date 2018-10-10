using System;

namespace HalideSharp
{
    public class HSOutputImageParamDimension
    {
        private HSOutputImageParam _param;
        private int _dimension;

        internal HSOutputImageParamDimension(HSOutputImageParam param, int d)
        {
            _param = param;
            _dimension = d;
        }

        public HSExpr Stride()
        {
            return _param.GetDimensionStride(_dimension);
        }

        public void SetStride(int s)
        {
            _param.SetDimensionStride(_dimension, s);
        }

        public HSExpr Extent()
        {
            return _param.GetDimensionExtent(_dimension);
        }

        public void SetExtent(int e)
        {
            _param.SetDimensionExtent(_dimension, e);
        }
    }
}