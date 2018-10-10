namespace HalideSharp
{
    public class HSOutputImageParamDimensionList
    {
        private HSOutputImageParam _param;

        internal HSOutputImageParamDimensionList(HSOutputImageParam param)
        {
            _param = param;
        }

        public HSOutputImageParamDimension this[int i] => _param.GetDimension(i);
    }
}