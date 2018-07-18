using System;
using System.Runtime.InteropServices;
using HalideSharp;

namespace Lesson12
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            // Load an input image.
            var input = HSBuffer<byte>.LoadImage("rgb.png");

            // Allocated an image that will store the correct output
            var reference_output = new HSBuffer<byte>(input.Width, input.Height, input.Channels);

            Console.WriteLine("Testing performance on CPU:");
            var p1 = new MyPipeline(input);
            p1.ScheduleForCpu();
            p1.TestPerformance();
            p1.Curved.Realize(reference_output);

            if (HaveOpenclOrMetal()) {
                Console.WriteLine("Testing performance on GPU:");
                var p2 = new MyPipeline(input);
                p2.ScheduleForGpu();
                p2.TestPerformance();
                p2.TestCorrectness(reference_output);
            } else {
                Console.WriteLine("Not testing performance on GPU, because I can't find the opencl library");
            }

            return 0;
        }

        
#if __APPLE__
        // FIXME: We're just going to assume that Apple targets will have Metal available for the time being.
        //private const string _metalLib = "/System/Library/Frameworks/Metal.framework/Versions/Current/Metal";
#else
    #if _WIN32
        private const string _oclLib = "OpenCL.dll";
    #else
        private const string _oclLib = "libOpenCL.so";
    #endif
        [DllImport(_oclLib)]
        private static extern int clGetPlatformIDs(uint numEntries, IntPtr platforms, out uint numPlatforms);
#endif
        
        private static bool HaveOpenclOrMetal()
        {
#if __APPLE__
            // FIXME
            return true;
#else
            try
            {
                var result = clGetPlatformIDs(0, IntPtr.Zero, out uint _);
                return result == 0;
            }
            catch (DllNotFoundException)
            {
                return false;
            }
#endif
        }
    }
}