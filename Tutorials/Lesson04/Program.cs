using System;
using HalideSharp;

namespace Lesson04
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            var x = new HSVar("x");
            var y = new HSVar("y");

            // Printing out the value of Funcs as they are computed.
            {
                // We'll define our gradient function as before.
                var gradient = new HSFunc("gradient");
                gradient[x, y] = x + y;

                // And tell Halide that we'd like to be notified of all
                // evaluations.
                gradient.TraceStores();

                // Realize the function over an 8x8 region.
                Console.WriteLine("Evaluating gradient");
                var output = gradient.Realize<int>(8, 8);
                // Click to show output ...

                // This will print out all the times gradient(x, y) gets
                // evaluated.

                // Now that we can snoop on what Halide is doing, let's try our
                // first scheduling primitive. We'll make a new version of
                // gradient that processes each scanline in parallel.
                var parallel_gradient = new HSFunc("parallel_gradient");
                parallel_gradient[x, y] = x + y;

                // We'll also trace this function.
                parallel_gradient.TraceStores();

                // Things are the same so far. We've defined the algorithm, but
                // haven't said anything about how to schedule it. In general,
                // exploring different scheduling decisions doesn't change the code
                // that describes the algorithm.

                // Now we tell Halide to use a parallel for loop over the y
                // coordinate. On Linux we run this using a thread pool and a task
                // queue. On OS X we call into grand central dispatch, which does
                // the same thing for us.
                parallel_gradient.Parallel(y);

                // This time the printfs should come out of order, because each
                // scanline is potentially being processed in a different
                // thread. The number of threads should adapt to your system, but
                // on linux you can control it manually using the environment
                // variable HL_NUM_THREADS.
                Console.WriteLine("\nEvaluating parallel_gradient");
                parallel_gradient.Realize<int>(8, 8);
                // Click to show output ...
            }

            // Printing individual Exprs.
            {
                // trace_stores() can only print the value of a
                // Func. Sometimes you want to inspect the value of
                // sub-expressions rather than the entire Func. The built-in
                // function 'print' can be wrapped around any Expr to print
                // the value of that Expr every time it is evaluated.

                // For example, say we have some Func that is the sum of two terms:
                var f = new HSFunc("f");
                f[x, y] = HSMath.Sin(x) + HSMath.Cos(y);

                // If we want to inspect just one of the terms, we can wrap
                // 'print' around it like so:
                var g = new HSFunc("g");
                g[x, y] = HSMath.Sin(x) + HS.Print(HSMath.Cos(y));

                Console.WriteLine("\nEvaluating sin(x) + cos(y), and just printing cos(y)");
                g.Realize<float>(4, 4);
                // Click to show output ...
            }

            // Printing additional context.
            {
                // print can take multiple arguments. It prints all of them
                // and evaluates to the first one. The arguments can be Exprs
                // or constant strings. This can be used to print additional
                // context alongside the value:
                var f = new HSFunc("f");
                f[x, y] = HSMath.Sin(x) + HS.Print(HSMath.Cos(y), "<- this is cos(", y, ") when x =", x);

                Console.WriteLine("\nEvaluating sin(x) + cos(y), and printing cos(y) with more context");
                f.Realize<float>(4, 4);
                // Click to show output ...

                // It can be useful to split expressions like the one above
                // across multiple lines to make it easier to turn on and off
                // printing certain values while debugging.
                HSExpr e = HSMath.Cos(y);
                // Uncomment the following line to print the value of cos(y)
                // e = print(e, "<- this is cos(", y, ") when x =", x);
                var g = new HSFunc("g");
                g[x, y] = HSMath.Sin(x) + e;
                g.Realize<float>(4, 4);
            }

            // Conditional printing
            {
                // Both print and trace_stores can produce a lot of output. If
                // you're looking for a rare event, or just want to see what
                // happens at a single pixel, this amount of output can be
                // difficult to dig through. Instead, the function print_when
                // can be used to conditionally print an Expr. The first
                // argument to print_when is a boolean Expr. If the Expr
                // evaluates to true, it returns the second argument and
                // prints all of the arguments. If the Expr evaluates to false
                // it just returns the second argument and does not print.

                var f = new HSFunc("f");
                HSExpr e = HSMath.Cos(y);
                e = HS.PrintWhen(x == 37 && y == 42, e, "<- this is cos(y) at x, y == (37, 42)");
                f[x, y] = HSMath.Sin(x) + e;
                Console.WriteLine("\nEvaluating sin(x) + cos(y), and printing cos(y) at a single pixel");
                f.Realize<float>(640, 480);
                // Click to show output ...

                // print_when can also be used to check for values you're not expecting:
                var g = new HSFunc("g");
                e = HSMath.Cos(y);
                e = HS.PrintWhen(e < 0, e, "cos(y) < 0 at y ==", y);
                g[x, y] = HSMath.Sin(x) + e;
                Console.WriteLine("\nEvaluating sin(x) + cos(y), and printing whenever cos(y) < 0");
                g.Realize<float>(4, 4);
                // Click to show output ...
            }

            // Printing expressions at compile-time.
            {
                // The code above builds up a Halide Expr across several lines
                // of code. If you're programmatically constructing a complex
                // expression, and you want to check the Expr you've created
                // is what you think it is, you can also print out the
                // expression itself using C++ streams:
                var fizz = new HSVar("fizz");
                var buzz = new HSVar("buzz");
                var e = new HSExpr(1);
                for (int i = 2; i < 100; i++) {
                    if (i % 3 == 0 && i % 5 == 0) e += fizz*buzz;
                    else if (i % 3 == 0) e += fizz;
                    else if (i % 5 == 0) e += buzz;
                    else e += i;
                }

                Console.WriteLine($"Printing a complex Expr: {e}");
                // Click to show output ...
            }

            Console.WriteLine("Success!");
            return 0;
        }
    }
}