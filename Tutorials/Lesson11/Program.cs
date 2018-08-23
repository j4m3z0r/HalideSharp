using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using HalideSharp;

namespace Lesson11
{
    internal class Program
    {
        public static int Main(string[] argv)
        {
            
            // We'll define the simple one-stage pipeline that we used in lesson 10.
            var brighter = new HSFunc("brighter");
            var x = new HSVar("x");
            var y = new HSVar("y");

            // Declare the arguments.
            var offset = new HSParam<byte>();
            var input = new HSImageParam<byte>(2);
            var args = new List<HSArgument>();
            args.Add(input);
            args.Add(offset);

            // Define the Func.
            brighter[x, y] = input[x, y] + offset;

            // Schedule it.
            brighter.Vectorize(x, 16).Parallel(y);

            // The following line is what we did in lesson 10. It compiles an
            // object file suitable for the system that you're running this
            // program on.  For example, if you compile and run this file on
            // 64-bit linux on an x86 cpu with sse4.1, then the generated code
            // will be suitable for 64-bit linux on x86 with sse4.1.
            brighter.CompileToFile("lesson_11_host", args, "brighter");

            // We can also compile object files suitable for other cpus and
            // operating systems. You do this with an optional third argument
            // to compile_to_file which specifies the target to compile for.

            // Let's use this to compile a 32-bit arm android version of this code:
            var target = new HSTarget();
            target.OS = HSOperatingSystem.Android; // The operating system
            target.Arch = HSArchitecture.ARM;   // The CPU architecture
            target.Bits = 32;            // The bit-width of the architecture
            var arm_features = new List<HSFeature>(); // A list of features to set
            target.SetFeatures(arm_features);
            // We then pass the target as the last argument to compile_to_file.
            brighter.CompileToFile("lesson_11_arm_32_android", args, "brighter", target);

            // And now a Windows object file for 64-bit x86 with AVX and SSE 4.1:
            target.OS = HSOperatingSystem.Windows;
            target.Arch = HSArchitecture.X86;
            target.Bits = 64;
            var x86_features = new List<HSFeature>();
            x86_features.Add(HSFeature.AVX);
            x86_features.Add(HSFeature.SSE41);
            target.SetFeatures(x86_features);
            brighter.CompileToFile("lesson_11_x86_64_windows", args, "brighter", target);

            // And finally an iOS mach-o object file for one of Apple's 32-bit
            // ARM processors - the A6. It's used in the iPhone 5. The A6 uses
            // a slightly modified ARM architecture called ARMv7s. We specify
            // this using the target features field.  Support for Apple's
            // 64-bit ARM processors is very new in llvm, and still somewhat
            // flaky.
            target.OS = HSOperatingSystem.IOS;
            target.Arch = HSArchitecture.ARM;
            target.Bits = 32;
            var armv7s_features = new List<HSFeature>();
            armv7s_features.Add(HSFeature.ARMv7s);
            target.SetFeatures(armv7s_features);
            brighter.CompileToFile("lesson_11_arm_32_ios", args, "brighter", target);


            // Now let's check these files are what they claim, by examining
            // their first few bytes.

            {
                // 32-arm android object files start with the magic bytes:
                byte[] arm_32_android_magic = {0x7f, (byte) 'E', (byte) 'L', (byte) 'F', // ELF format
                                                  1,       // 32-bit
                                                  1,       // 2's complement little-endian
                                                  1};      // Current version of elf

                var androidObjectFile = "lesson_11_arm_32_android.o";
                if (!File.Exists(androidObjectFile))
                {
                    Console.WriteLine("Object file not generated");
                    return -1;
                }
                
                var androidObjectData = File.ReadAllBytes(androidObjectFile);
                var header = new byte[arm_32_android_magic.Length];
                Buffer.BlockCopy(androidObjectData, 0, header, 0, arm_32_android_magic.Length);

                if(!header.SequenceEqual(arm_32_android_magic)) {
                    Console.WriteLine("Unexpected header bytes in 32-bit arm object file.");
                    return -1;
                }
            }

            {
                // 64-bit windows object files start with the magic 16-bit value 0x8664
                // (presumably referring to x86-64)
                byte[] win_64_magic = {0x64, 0x86};

                var winObjectFile = "lesson_11_x86_64_windows.obj";
                if(!File.Exists(winObjectFile))
                {
                    Console.WriteLine("Object file not generated");
                    return -1;
                }
                var windowsObjectData = File.ReadAllBytes(winObjectFile);
                var header = new byte[win_64_magic.Length];
                Buffer.BlockCopy(windowsObjectData, 0, header, 0, win_64_magic.Length);

                if(!header.SequenceEqual(win_64_magic))
                {
                    Console.WriteLine("Unexpected header bytes in 64-bit windows object file.");
                    return -1;
                }
            }

            {
                // 32-bit arm iOS mach-o files start with the following magic bytes:
                uint[] arm_32_ios_magic = {0xfeedface, // Mach-o magic bytes
                                               12,  // CPU type is ARM
                                               11,  // CPU subtype is ARMv7s
                                               1};  // It's a relocatable object file.
                var magicBytes = new byte[arm_32_ios_magic.Length * 4];
                Buffer.BlockCopy(arm_32_ios_magic, 0, magicBytes, 0, arm_32_ios_magic.Length * 4);
                
                var iosObjectFile = "lesson_11_arm_32_ios.o";
                if(!File.Exists(iosObjectFile))
                {
                    Console.WriteLine("Object file not generated");
                    return -1;
                }

                var iosObjectData = File.ReadAllBytes(iosObjectFile);
                var header = new byte[magicBytes.Length];
                Buffer.BlockCopy(iosObjectData, 0, header, 0, magicBytes.Length);
                if(!header.SequenceEqual(magicBytes))
                {
                    Console.WriteLine("Unexpected header bytes in 32-bit arm ios object file.");
                    return -1;
                }
                    
            }

            // It looks like the object files we produced are plausible for
            // those targets. We'll count that as a success for the purposes
            // of this tutorial. For a real application you'd then need to
            // figure out how to integrate Halide into your cross-compilation
            // toolchain. There are several small examples of this in the
            // Halide repository under the apps folder. See HelloAndroid and
            // HelloiOS here:
            // https://github.com/halide/Halide/tree/master/apps/
            Console.WriteLine("Success!");
            return 0;
        }
    }
}