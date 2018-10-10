using System;
using System.Runtime.InteropServices;

namespace HalideSharp
{
    public class HSOutputImageParam : HSObject
    {
        private HSOutputImageParamDimensionList _dimensions;
        public HSOutputImageParamDimensionList Dim => _dimensions;
        
        internal HSOutputImageParam(IntPtr obj) : base()
        {
            _dimensions = new HSOutputImageParamDimensionList(this);
            _cppobj = obj;
        }

        [DllImport(Constants.LibName)]
        private static extern void OutputImageParam_Delete(IntPtr self);

        ~HSOutputImageParam()
        {
            OutputImageParam_Delete(_cppobj);
        }

        [DllImport(Constants.LibName)]
        private static extern IntPtr OutputImageParam_GetDimensionStride_Int(IntPtr self, int d);
        public HSExpr GetDimensionStride(int dimension)
        {
            var result = new HSExpr(OutputImageParam_GetDimensionStride_Int(HSUtil.CArg(this), HSUtil.CArg(dimension)));
            result.AddRef(this);
            return result;
        }

        [DllImport(Constants.LibName)]
        private static extern IntPtr OutputImageParam_SetDimensionStride_IntExpr(IntPtr self, int d, IntPtr s);
        public void SetDimensionStride(int dimension, HSExpr s)
        {
            OutputImageParam_SetDimensionStride_IntExpr(HSUtil.CArg(this), HSUtil.CArg(dimension), HSUtil.CArg(s));
            AddRef(s);
        }

        [DllImport(Constants.LibName)]
        private static extern IntPtr OutputImageParam_GetDimensionExtent_Int(IntPtr self, int d);
        public HSExpr GetDimensionExtent(int dimension)
        {
            var result =  new HSExpr(OutputImageParam_GetDimensionExtent_Int(HSUtil.CArg(this), HSUtil.CArg(dimension)));
            result.AddRef(this);
            return result;
        }

        [DllImport(Constants.LibName)]
        private static extern IntPtr OutputImageParam_SetDimensionExtent_IntExpr(IntPtr self, int d, IntPtr e);
        public void SetDimensionExtent(int dimension, HSExpr e)
        {
            OutputImageParam_SetDimensionExtent_IntExpr(HSUtil.CArg(this), HSUtil.CArg(dimension), HSUtil.CArg(e));
            AddRef(e);
        }

        internal HSOutputImageParamDimension GetDimension(int d)
        {
            return new HSOutputImageParamDimension(this, d);
        }
    }
}