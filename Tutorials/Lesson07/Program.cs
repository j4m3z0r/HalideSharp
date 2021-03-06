﻿using System;
using HalideSharp;

namespace Lesson07
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            // First we'll declare some Vars to use below.
            var x = new HSVar("x");
            var y = new HSVar("y");
            var c = new HSVar("c");

            // Now we'll express a multi-stage pipeline that blurs an image
            // first horizontally, and then vertically.
            {
                // Take a color 8-bit input
                var input = HSBuffer<byte>.LoadImage("rgb.png");

                // Upgrade it to 16-bit, so we can do math without it overflowing.
                var input_16 = new HSFunc("input_16");
                input_16[x, y, c] = HS.Cast<ushort>(input[x, y, c]);

                // Blur it horizontally:
                var blur_x = new HSFunc("blur_x");
                blur_x[x, y, c] = (input_16[x-1, y, c] +
                                   2 * input_16[x, y, c] +
                                   input_16[x+1, y, c]) / 4;

                // Blur it vertically:
                var blur_y = new HSFunc("blur_y");
                blur_y[x, y, c] = (blur_x[x, y-1, c] +
                                   2 * blur_x[x, y, c] +
                                   blur_x[x, y+1, c]) / 4;

                // Convert back to 8-bit.
                var output = new HSFunc("output");
                output[x, y, c] = HS.Cast<byte>(blur_y[x, y, c]);

                // Each Func in this pipeline calls a previous one using
                // familiar function call syntax (we've overloaded operator()
                // on Func objects). A Func may call any other Func that has
                // been given a definition. This restriction prevents
                // pipelines with loops in them. Halide pipelines are always
                // feed-forward graphs of Funcs.

                // Now let's realize it...

                // Buffer<byte> result = output.realize(input.width(), input.height(), 3);

                // Except that the line above is not going to work. Uncomment
                // it to see what happens.

                // Realizing this pipeline over the same domain as the input
                // image requires reading pixels out of bounds in the input,
                // because the blur_x stage reaches outwards horizontally, and
                // the blur_y stage reaches outwards vertically. Halide
                // detects this by injecting a piece of code at the top of the
                // pipeline that computes the region over which the input will
                // be read. When it starts to run the pipeline it first runs
                // this code, determines that the input will be read out of
                // bounds, and refuses to continue. No actual bounds checks
                // occur in the inner loop; that would be slow.
                //
                // So what do we do? There are a few options. If we realize
                // over a domain shifted inwards by one pixel, we won't be
                // asking the Halide routine to read out of bounds. We saw how
                // to do this in the previous lesson:
                var result = new HSBuffer<byte>(input.Width-2, input.Height-2, 3);
                result.SetMin(1, 1);
                output.Realize(result);

                // Save the result. It should look like a slightly blurry
                // parrot, and it should be two pixels narrower and two pixels
                // shorter than the input image.
                result.SaveImage("blurry_parrot_1.png");

                // This is usually the fastest way to deal with boundaries:
                // don't write code that reads out of bounds :) The more
                // general solution is our next example.
            }

            // The same pipeline, with a boundary condition on the input.
            {
                // Take a color 8-bit input
                var input = HSBuffer<byte>.LoadImage("rgb.png");

                // This time, we'll wrap the input in a Func that prevents
                // reading out of bounds:
                var clamped = new HSFunc("clamped");

                // Define an expression that clamps x to lie within the
                // range [0, input.width()-1].
                var clamped_x = HS.Clamp(x, 0, input.Width-1);
                // clamp(x, a, b) is equivalent to max(min(x, b), a).

                // Similarly clamp y.
                var clamped_y = HS.Clamp(y, 0, input.Height-1);
                // Load from input at the clamped coordinates. This means that
                // no matter how we evaluated the Func 'clamped', we'll never
                // read out of bounds on the input. This is a clamp-to-edge
                // style boundary condition, and is the simplest boundary
                // condition to express in Halide.
                clamped[x, y, c] = input[clamped_x, clamped_y, c];

                // Defining 'clamped' in that way can be done more concisely
                // using a helper function from the BoundaryConditions
                // namespace like so:
                //
                // clamped = BoundaryConditions::repeat_edge(input);
                //
                // These are important to use for other boundary conditions,
                // because they are expressed in the way that Halide can best
                // understand and optimize. When used correctly they are as
                // cheap as having no boundary condition at all.

                // Upgrade it to 16-bit, so we can do math without it
                // overflowing. This time we'll refer to our new Func
                // 'clamped', instead of referring to the input image
                // directly.
                var input_16 = new HSFunc("input_16");
                input_16[x, y, c] = HS.Cast<ushort>(clamped[x, y, c]);

                // The rest of the pipeline will be the same...

                // Blur it horizontally:
                var blur_x = new HSFunc("blur_x");
                blur_x[x, y, c] = (input_16[x-1, y, c] +
                                   2 * input_16[x, y, c] +
                                   input_16[x+1, y, c]) / 4;

                // Blur it vertically:
                var blur_y = new HSFunc("blur_y");
                blur_y[x, y, c] = (blur_x[x, y-1, c] +
                                   2 * blur_x[x, y, c] +
                                   blur_x[x, y+1, c]) / 4;

                // Convert back to 8-bit.
                var  output = new HSFunc("output");
                output[x, y, c] = HS.Cast<byte>(blur_y[x, y, c]);

                // This time it's safe to evaluate the output over the some
                // domain as the input, because we have a boundary condition.
                var result = output.Realize<byte>(input.Width, input.Height, 3);

                // Save the result. It should look like a slightly blurry
                // parrot, but this time it will be the same size as the
                // input.
                result.SaveImage("blurry_parrot_2.png");
            }

            Console.WriteLine("Success!");
            return 0;

        }
    }
}