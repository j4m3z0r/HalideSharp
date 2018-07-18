using System;
using System.Diagnostics;
using System.IO;
using HalideSharp;

namespace Lesson12
{
    public class MyPipeline
    {
        // Define some Vars to use.
        public HSVar X = new HSVar("X");
        public HSVar Y = new HSVar("Y");
        public HSVar C = new HSVar("C");
        public HSVar I = new HSVar("I");
        public HSVar II = new HSVar("II");
        public HSVar XO = new HSVar("XO");
        public HSVar YO = new HSVar("YO");
        public HSVar XI = new HSVar("XI");
        public HSVar YI = new HSVar("YI");

        // We're going to want to schedule a pipeline in several ways, so we
        // define the pipeline in a class so that we can recreate it several
        // times with different schedules.
        public HSFunc Lut = new HSFunc("Lut");
        public HSFunc Padded = new HSFunc("Padded");
        public HSFunc Padded16 = new HSFunc("Padded16");
        public HSFunc Sharpen = new HSFunc("Sharpen");
        public HSFunc Curved = new HSFunc("Curved");

        public HSBuffer<byte> Input;

        public MyPipeline(HSBuffer<byte> inBuf) {
            Input = inBuf;
            // For this lesson, we'll use a two-stage pipeline that sharpens
            // and then applies a look-up-table (LUT).

            // First we'll define the LUT. It will be a gamma curve.

            Lut[I] = HS.Cast<byte>(HS.Clamp(HSMath.Pow(I / 255.0f, 1.2f) * 255.0f, 0, 255));

            // Augment the input with a boundary condition.
            Padded[X, Y, C] = Input[HS.Clamp(X, 0, Input.Width - 1),
                HS.Clamp(Y, 0, Input.Height - 1), C];

            // Cast it to 16-bit to do the math.
            Padded16[X, Y, C] = HS.Cast<ushort>(Padded[X, Y, C]);

            // Next we sharpen it with a five-tap filter.
            Sharpen[X, Y, C] = (Padded16[X, Y, C] * 2 -
                                (Padded16[X - 1, Y, C] +
                                 Padded16[X, Y - 1, C] +
                                 Padded16[X + 1, Y, C] +
                                 Padded16[X, Y + 1, C]) / 4);

            // Then apply the LUT.
            Curved[X, Y, C] = Lut[Sharpen[X, Y, C]];
        }

        // Now we define methods that give our pipeline several different
        // schedules.
        public void ScheduleForCpu()
        {
            // Compute the look-up-table ahead of time.
            Lut.ComputeRoot();

            // Compute color channels innermost. Promise that there will
            // be three of them and unroll across them.
            Curved.Reorder(C, X, Y)
                .Bound(C, 0, 3)
                .Unroll(C);

            // Look-up-tables don't vectorize well, so just parallelize
            // curved in slices of 16 scanlines.
            var yo = new HSVar("yo");
            var yi = new HSVar("yi");
            Curved.Split(Y, yo, yi, 16)
                .Parallel(yo);

            // Compute sharpen as needed per scanline of curved.
            Sharpen.ComputeAt(Curved, yi);

            // Vectorize the sharpen. It's 16-bit so we'll vectorize it 8-wide.
            Sharpen.Vectorize(X, 8);

            // Compute the padded input as needed per scanline of curved,
            // reusing previous values computed within the same strip of
            // 16 scanlines.
            Padded.StoreAt(Curved, yo)
                .ComputeAt(Curved, yi);

            // Also vectorize the padding. It's 8-bit, so we'll vectorize
            // 16-wide.
            Padded.Vectorize(X, 16);

            // JIT-compile the pipeline for the CPU.
            Curved.CompileJit();
        }

        // Now a schedule that uses CUDA or OpenCL.
        public void ScheduleForGpu()
        {
            // We make the decision about whether to use the GPU for each
            // Func independently. If you have one Func computed on the
            // CPU, and the next computed on the GPU, Halide will do the
            // copy-to-gpu under the hood. For this pipeline, there's no
            // reason to use the CPU for any of the stages. Halide will
            // copy the input image to the GPU the first time we run the
            // pipeline, and leave it there to reuse on subsequent runs.

            // As before, we'll compute the LUT once at the start of the
            // pipeline.
            Lut.ComputeRoot();

            // Let's compute the look-up-table using the GPU in 16-wide
            // one-dimensional thread blocks. First we split the index
            // into blocks of size 16:
            var block = new HSVar("block");
            var thread = new HSVar("thread");
            Lut.Split(I, block, thread, 16);
            // Then we tell cuda that our Vars 'block' and 'thread'
            // correspond to CUDA's notions of blocks and threads, or
            // OpenCL's notions of thread groups and threads.
            Lut.GpuBlocks(block)
                .GpuThreads(thread);

            // This is a very common scheduling pattern on the GPU, so
            // there's a shorthand for it:

            // lut.gpu_tile(i, block, thread, 16);

            // Func::gpu_tile behaves the same as Func::tile, except that
            // it also specifies that the tile coordinates correspond to
            // GPU blocks, and the coordinates within each tile correspond
            // to GPU threads.

            // Compute color channels innermost. Promise that there will
            // be three of them and unroll across them.
            Curved.Reorder(C, X, Y)
                .Bound(C, 0, 3)
                .Unroll(C);

            // Compute curved in 2D 8x8 tiles using the GPU.
            Curved.GpuTile(X, Y, XO, YO, XI, YI, 8, 8);

            // This is equivalent to:
            // curved.tile(x, y, xo, yo, xi, yi, 8, 8)
            //       .gpu_blocks(xo, yo)
            //       .gpu_threads(xi, yi);

            // We'll leave sharpen as inlined into curved.

            // Compute the padded input as needed per GPU block, storing
            // the intermediate result in shared memory. In the schedule
            // above xo corresponds to GPU blocks.
            Padded.ComputeAt(Curved, XO);

            // Use the GPU threads for the x and y coordinates of the
            // padded input.
            Padded.GpuThreads(X, Y);

            // JIT-compile the pipeline for the GPU. CUDA, OpenCL, or
            // Metal are not enabled by default. We have to construct a
            // Target object, enable one of them, and then pass that
            // target object to compile_jit. Otherwise your CPU will very
            // slowly pretend it's a GPU, and use one thread per output
            // pixel.

            // Start with a target suitable for the machine you're running
            // this on.
            var target = HS.GetHostTarget();

            // Then enable OpenCL or Metal, depending on which platform
            // we're on. OS X doesn't update its OpenCL drivers, so they
            // tend to be broken. CUDA would also be a fine choice on
            // machines with NVidia GPUs.
            if (target.OS == SharedEnums.HSOperatingSystem.OSX)
            {
                target.SetFeature(SharedEnums.HSFeature.Metal);
            }
            else
            {
                target.SetFeature(SharedEnums.HSFeature.OpenCL);
            }

            // Uncomment the next line and comment out the lines above to
            // try CUDA instead.
            //target.SetFeature(SharedEnums.HSFeature.CUDA);

            // If you want to see all of the OpenCL, Metal, or CUDA API
            // calls done by the pipeline, you can also enable the Debug
            // flag. This is helpful for figuring out which stages are
            // slow, or when CPU -> GPU copies happen. It hurts
            // performance though, so we'll leave it commented out.
            // target.set_feature(Target::Debug);

            Curved.CompileJit(target);
        }

        public void TestPerformance()
        {
            // Test the performance of the scheduled MyPipeline.

            var output = new HSBuffer<byte>(Input.Width, Input.Height, Input.Channels);

            // Run the filter once to initialize any GPU runtime state.
            Curved.Realize(output);

            // Now take the best of 3 runs for timing.
            double best_time = 0.0;
            for (int i = 0; i < 3; i++)
            {

                var timer = Stopwatch.StartNew();

                // Run the filter 100 times.
                for (int j = 0; j < 100; j++)
                {
                    Curved.Realize(output);
                }

                // Force any GPU code to finish by copying the buffer back to the CPU.
                output.CopyToHost();

                timer.Stop();
                
                var elapsed = timer.ElapsedMilliseconds;
                if (i == 0 || elapsed < best_time)
                {
                    best_time = elapsed;
                }
            }

            Console.WriteLine($"{best_time} milliseconds");
        }

        public void TestCorrectness(HSBuffer<byte> reference_output)
        {
            var output = Curved.Realize<byte>(Input.Width, Input.Height, Input.Channels);

            // Check against the reference output.
            for (int c = 0; c < Input.Channels; c++)
            {
                for (int y = 0; y < Input.Height; y++)
                {
                    for (int x = 0; x < Input.Width; x++)
                    {
                        if (output[x, y, c] != reference_output[x, y, c])
                        {
                            Console.WriteLine($"Mismatch between output ({output[x,y,c]}) and reference output ({reference_output[x,y,c]}) at {x}, {y}, {c}");
                            Environment.Exit(-1);
                        }
                    }
                }
            }

        }
    }
}