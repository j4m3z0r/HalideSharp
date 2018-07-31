using System;
using System.Runtime.InteropServices.WindowsRuntime;

namespace HalideSharp
{
    public class HSObject
    {
        internal IntPtr _cppobj;

        internal HSObject()
        {
            // Configure error handling any time we create a HS object. This is a no-op
            // if it has already been initialized.
            HSErrorHandler.ConfigureErrorHandling();
        }
    }
}