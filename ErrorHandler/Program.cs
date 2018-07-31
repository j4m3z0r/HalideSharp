using System.Security.Cryptography.X509Certificates;
using HalideSharp;

namespace ErrorHandler
{
    public class Program
    {
        public static void Main()
        {
            // Now make a small program that will trigger an error
            var buffer = new HSBuffer<int>(2, 2);
            for (var j = 0; j < 2; j++)
            {
                for (var i = 0; i < 2; i++)
                {
                    buffer[j, i] = i * j;
                }
            }
            var errilicious = new HSFunc("EISFORERROR");
            var x = new HSVar("x");
            var y = new HSVar("y");
            errilicious[x, y] = buffer[x, y] * buffer[x, y];
            
            // Now realize over a domain that is larger than the buffer, which should trigger
            // an error
            var result = errilicious.Realize<int>(100, 100);
        }
    }
}