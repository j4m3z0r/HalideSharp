using System;
using HalideSharp;

namespace Lesson06
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            // The last lesson was quite involved, and scheduling complex
            // multi-stage pipelines is ahead of us. As an interlude, let's
            // consider something easy: evaluating funcs over rectangular
            // domains that do not start at the origin.

            // We define our familiar gradient function.
            var gradient = new HSFunc("gradient");
            var x = new HSVar("x");
            var y = new HSVar("y");
            gradient[x, y] = x + y;

            // And turn on tracing so we can see how it is being evaluated.
            gradient.TraceStores();

            // Previously we've realized gradient like so:
            //
            // gradient.realize(8, 8);
            //
            // This does three things internally:
            // 1) Generates code than can evaluate gradient over an arbitrary
            // rectangle.
            // 2) Allocates a new 8 x 8 image.
            // 3) Runs the generated code to evaluate gradient for all x, y
            // from (0, 0) to (7, 7) and puts the result into the image.
            // 4) Returns the new image as the result of the realize call.

            // What if we're managing memory carefully and don't want Halide
            // to allocate a new image for us? We can call realize another
            // way. We can pass it an image we would like it to fill in. The
            // following evaluates our Func into an existing image:
            Console.WriteLine("Evaluating gradient from (0, 0) to (7, 7)");
            var result = new HSBuffer<int>(8, 8);
            gradient.Realize(result);

            // Let's check it did what we expect:
            for (int yy = 0; yy < 8; yy++) {
                for (int xx = 0; xx < 8; xx++) {
                    if (result[xx, yy] != xx + yy) {
                        Console.WriteLine("Something went wrong!\n");
                        return -1;
                    }
                }
            }

            // Now let's evaluate gradient over a 5 x 7 rectangle that starts
            // somewhere else -- at position (100, 50). So x and y will run
            // from (100, 50) to (104, 56) inclusive.

            // We start by creating an image that represents that rectangle:
            var shifted = new HSBuffer<int>(5, 7); // In the constructor we tell it the size.
            shifted.SetMin(100, 50); // Then we tell it the top-left corner.

            Console.WriteLine("Evaluating gradient from (100, 50) to (104, 56)");

            // Note that this won't need to compile any new code, because when
            // we realized it the first time, we generated code capable of
            // evaluating gradient over an arbitrary rectangle.
            gradient.Realize(shifted);

            // From C++, we also access the image object using coordinates
            // that start at (100, 50).
            for (int yy = 50; yy < 57; yy++) {
                for (int xx = 100; xx < 105; xx++) {
                    if (shifted[xx, yy] != xx + yy) {
                        Console.WriteLine("Something went wrong!");
                        return -1;
                    }
                }
            }
            // The image 'shifted' stores the value of our Func over a domain
            // that starts at (100, 50), so asking for shifted(0, 0) would in
            // fact read out-of-bounds and probably crash.

            // What if we want to evaluate our Func over some region that
            // isn't rectangular? Too bad. Halide only does rectangles :)

            Console.WriteLine("Success!");
            return 0;
        }
    }
}