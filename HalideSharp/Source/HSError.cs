using System;
using System.Runtime.InteropServices;

namespace HalideSharp
{
    public class HSError : Exception
    {
        public HSError() : base()
        {
        }

        public HSError(string message) : base(message)
        {
        }

        public HSError(string message, Exception inner) : base(message, inner)
        {
        }
    }
    
    internal class HSErrorHandler
    {
        public delegate void ErrDelegate([MarshalAs(UnmanagedType.LPStr)] string error);

        private static bool _initialized = false;
        private static ErrDelegate _errDelegate = new ErrDelegate(ThrowError);
        
        [DllImport(Constants.LibName)]
        public static extern void Global_SetErrorHandler_ErrDelegate([MarshalAs(UnmanagedType.FunctionPtr)] 
            ErrDelegate pDelegate);

        private static void ThrowError(string error)
        {
            throw new HSError(error);
        }
        
        public static void ConfigureErrorHandling()
        {
            if (_initialized)
            {
                return;
            }
            Global_SetErrorHandler_ErrDelegate(_errDelegate);
            _initialized = true;
        }
    }
}