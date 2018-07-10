using System;
using System.Diagnostics;
using HalideSharp;

namespace Lesson05
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            // We're going to define and schedule our gradient function in
            // several different ways, and see what order pixels are computed
            // in.

            var x = new HSVar("x");
            var y = new HSVar("y");

            // First we observe the default ordering.
            {
                var gradient = new HSFunc("gradient");
                gradient[x, y] = x + y;
                gradient.TraceStores();

                // By default we walk along the rows and then down the
                // columns. This means x varies quickly, and y varies
                // slowly. x is the column and y is the row, so this is a
                // row-major traversal.
                Console.WriteLine("Evaluating gradient row-major");
                var output = gradient.Realize<int>(4, 4);

                // See figures/lesson_05_row_major.gif for a visualization of
                // what this did.

                // The equivalent C is:
                Console.WriteLine("Equivalent C:");
                for (int yy = 0; yy < 4; yy++) {
                    for (int xx = 0; xx < 4; xx++)
                    {
                        Console.WriteLine($"Evaluating at x = {xx}, y = {yy}: {xx + yy}");
                    }
                }
                Console.WriteLine("\n");

                // Tracing is one useful way to understand what a schedule is
                // doing. You can also ask Halide to print out pseudocode
                // showing what loops Halide is generating:
                Console.WriteLine("Pseudo-code for the schedule:");
                gradient.PrintLoopNest();
                Console.WriteLine();

                // Because we're using the default ordering, it should print:
                // compute gradient:
                //   for y:
                //     for x:
                //       gradient(...) = ...
            }

            // Reorder variables.
            {
                var gradient = new HSFunc("gradient_col_major");
                gradient[x, y] = x + y;
                gradient.TraceStores();

                // If we reorder x and y, we can walk down the columns
                // instead. The reorder call takes the arguments of the func,
                // and sets a new nesting order for the for loops that are
                // generated. The arguments are specified from the innermost
                // loop out, so the following call puts y in the inner loop:
                gradient.Reorder(y, x);

                // This means y (the row) will vary quickly, and x (the
                // column) will vary slowly, so this is a column-major
                // traversal.

                Console.WriteLine("Evaluating gradient column-major");
                var output = gradient.Realize<int>(4, 4);

                // See figures/lesson_05_col_major.gif for a visualization of
                // what this did.

                Console.WriteLine("Equivalent C:");
                for (int xx = 0; xx < 4; xx++) {
                    for (int yy = 0; yy < 4; yy++)
                    {
                        Console.WriteLine($"Evaluating at x = {xx}, y = {yy}: {xx + yy}");
                    }
                }
                Console.WriteLine();

                // If we print pseudo-code for this schedule, we'll see that
                // the loop over y is now inside the loop over x.
                Console.WriteLine("Pseudo-code for the schedule:");
                gradient.PrintLoopNest();
                Console.WriteLine();
            }

            // Split a variable into two.
            {
                var gradient = new HSFunc("gradient_split");
                gradient[x, y] = x + y;
                gradient.TraceStores();

                // The most powerful primitive scheduling operation you can do
                // to a var is to split it into inner and outer sub-variables:
                var x_outer = new HSVar("x_outer");
                var x_inner = new HSVar("x_inner");
                gradient.Split(x, x_outer, x_inner, 2);

                // This breaks the loop over x into two nested loops: an outer
                // one over x_outer, and an inner one over x_inner. The last
                // argument to split was the "split factor". The inner loop
                // runs from zero to the split factor. The outer loop runs
                // from zero to the extent required of x (4 in this case)
                // divided by the split factor. Within the loops, the old
                // variable is defined to be outer * factor + inner. If the
                // old loop started at a value other than zero, then that is
                // also added within the loops.

                Console.WriteLine("Evaluating gradient with x split into x_outer and x_inner ");
                var output = gradient.Realize<int>(4, 4);

                Console.WriteLine("Equivalent C:");
                for (int yy = 0; yy < 4; yy++) {
                    for (int xOuter = 0; xOuter < 2; xOuter++) {
                        for (int xInner = 0; xInner < 2; xInner++) {
                            int xx = xOuter * 2 + xInner;
                            Console.WriteLine($"Evaluating at x = {xx}, y = {yy}: {xx + yy}");
                        }
                    }
                }
                Console.WriteLine();

                Console.WriteLine("Pseudo-code for the schedule:");
                gradient.PrintLoopNest();
                Console.WriteLine();

                // Note that the order of evaluation of pixels didn't actually
                // change! Splitting by itself does nothing, but it does open
                // up all of the scheduling possibilities that we will explore
                // below.
            }

            // Fuse two variables into one.
            {
                var gradient = new HSFunc("gradient_fused");
                gradient[x, y] = x + y;

                // The opposite of splitting is 'fusing'. Fusing two variables
                // merges the two loops into a single for loop over the
                // product of the extents. Fusing is less important than
                // splitting, but it also sees use (as we'll see later in this
                // lesson). Like splitting, fusing by itself doesn't change
                // the order of evaluation.
                var fused = new HSVar("fused");
                gradient.Fuse(x, y, fused);

                Console.WriteLine("Evaluating gradient with x and y fused");
                var output = gradient.Realize<int>(4, 4);

                Console.WriteLine("Equivalent C:");
                for (int f = 0; f < 4*4; f++) {
                    int yy = f / 4;
                    int xx = f % 4;
                    Console.WriteLine($"Evaluating at x = {xx}, y = {yy}: {xx + yy}");
                }
                Console.WriteLine();

                Console.WriteLine("Pseudo-code for the schedule:");
                gradient.PrintLoopNest();
                Console.WriteLine();
            }

            // Evaluating in tiles.
            {
                var gradient = new HSFunc("gradient_tiled");
                gradient[x, y] = x + y;
                gradient.TraceStores();

                // Now that we can both split and reorder, we can do tiled
                // evaluation. Let's split both x and y by a factor of four,
                // and then reorder the vars to express a tiled traversal.
                //
                // A tiled traversal splits the domain into small rectangular
                // tiles, and outermost iterates over the tiles, and within
                // that iterates over the points within each tile. It can be
                // good for performance if neighboring pixels use overlapping
                // input data, for example in a blur. We can express a tiled
                // traversal like so:
                var x_outer = new HSVar("x_outer");
                var x_inner = new HSVar("x_inner");
                var y_outer = new HSVar("y_outer");
                var y_inner = new HSVar("y_inner");
                gradient.Split(x, x_outer, x_inner, 4);
                gradient.Split(y, y_outer, y_inner, 4);
                gradient.Reorder(x_inner, y_inner, x_outer, y_outer);

                // This pattern is common enough that there's a shorthand for it:
                // gradient.tile(x, y, x_outer, y_outer, x_inner, y_inner, 4, 4);

                Console.WriteLine("Evaluating gradient in 4x4 tiles");
                var output = gradient.Realize<int>(8, 8);

                // See figures/lesson_05_tiled.gif for a visualization of this
                // schedule.

                Console.WriteLine("Equivalent C:");
                for (int yOuter = 0; yOuter < 2; yOuter++) {
                    for (int xOuter = 0; xOuter < 2; xOuter++) {
                        for (int yInner = 0; yInner < 4; yInner++) {
                            for (int xInner = 0; xInner < 4; xInner++) {
                                int xx = xOuter * 4 + xInner;
                                int yy = yOuter * 4 + yInner;
                                Console.WriteLine($"Evaluating at x = {xx}, y = {yy}: {xx + yy}");
                            }
                        }
                    }
                }
                Console.WriteLine();

                Console.WriteLine("Pseudo-code for the schedule:");
                gradient.PrintLoopNest();
                Console.WriteLine();
            }

            // Evaluating in vectors.
            {
                var gradient = new HSFunc("gradient_in_vectors");
                gradient[x, y] = x + y;
                gradient.TraceStores();

                // The nice thing about splitting is that it guarantees the
                // inner variable runs from zero to the split factor. Most of
                // the time the split-factor will be a compile-time constant,
                // so we can replace the loop over the inner variable with a
                // single vectorized computation. This time we'll split by a
                // factor of four, because on X86 we can use SSE to compute in
                // 4-wide vectors.
                var x_outer = new HSVar("x_outer");
                var x_inner = new HSVar("x_inner");
                gradient.Split(x, x_outer, x_inner, 4);
                gradient.Vectorize(x_inner);

                // Splitting and then vectorizing the inner variable is common
                // enough that there's a short-hand for it. We could have also
                // said:
                //
                // gradient.vectorize(x, 4);
                //
                // which is equivalent to:
                //
                // gradient.split(x, x, x_inner, 4);
                // gradient.vectorize(x_inner);
                //
                // Note that in this case we reused the name 'x' as the new
                // outer variable. Later scheduling calls that refer to x
                // will refer to this new outer variable named x.

                // This time we'll evaluate over an 8x4 box, so that we have
                // more than one vector of work per scanline.
                Console.WriteLine("Evaluating gradient with x_inner vectorized ");
                var output = gradient.Realize<int>(8, 4);

                // See figures/lesson_05_vectors.gif for a visualization.

                Console.WriteLine("Equivalent C:");
                for (int yy = 0; yy < 4; yy++) {
                    for (int xOuter = 0; xOuter < 2; xOuter++) {
                        // The loop over x_inner has gone away, and has been
                        // replaced by a vectorized version of the
                        // expression. On x86 processors, Halide generates SSE
                        // for all of this.
                        int[] x_vec = {xOuter * 4 + 0,
                                       xOuter * 4 + 1,
                                       xOuter * 4 + 2,
                                       xOuter * 4 + 3};
                        int[] val = {x_vec[0] + yy,
                                     x_vec[1] + yy,
                                     x_vec[2] + yy,
                                     x_vec[3] + yy};
                        Console.WriteLine($"Evaluating at " + 
                                          $"<{x_vec[0]}, {x_vec[1]}, {x_vec[2]}, {x_vec[3]}>, " +
                                          $"<{yy}, {yy}, {yy}, {yy}>: " +
                                          $"<{val[0]}, {val[1]}, {val[2]}, {val[3]}>");
                    }
                }
                Console.WriteLine();

                Console.WriteLine("Pseudo-code for the schedule:");
                gradient.PrintLoopNest();
                Console.WriteLine();
            }

            // Unrolling a loop.
            {
                var gradient = new HSFunc("gradient_unroll");
                gradient[x, y] = x + y;
                gradient.TraceStores();

                // If multiple pixels share overlapping data, it can make
                // sense to unroll a computation so that shared values are
                // only computed or loaded once. We do this similarly to how
                // we expressed vectorizing. We split a dimension and then
                // fully unroll the loop of the inner variable. Unrolling
                // doesn't change the order in which things are evaluated.
                var x_outer = new HSVar("x_outer");
                var x_inner = new HSVar("x_inner");
                gradient.Split(x, x_outer, x_inner, 2);
                gradient.Unroll(x_inner);

                // The shorthand for this is:
                // gradient.unroll(x, 2);

                Console.WriteLine("Evaluating gradient unrolled by a factor of two");
                var result = gradient.Realize<int>(4, 4);

                Console.WriteLine("Equivalent C:");
                for (int yy = 0; yy < 4; yy++) {
                    for (int xOuter = 0; xOuter < 2; xOuter++) {
                        // Instead of a for loop over x_inner, we get two
                        // copies of the innermost statement.
                        {
                            int xInner = 0;
                            int xx = xOuter * 2 + xInner;
                            Console.WriteLine($"Evaluating at x = {xx}, y = {yy}: {xx + yy}");
                        }
                        {
                            int xInner = 1;
                            int xx = xOuter * 2 + xInner;
                            Console.WriteLine($"Evaluating at x = {xx}, y = {yy}: {xx + yy}");
                        }
                    }
                }
                Console.WriteLine();

                Console.WriteLine("Pseudo-code for the schedule:");
                gradient.PrintLoopNest();
                Console.WriteLine();
            }

            // Splitting by factors that don't divide the extent.
            {
                var gradient = new HSFunc("gradient_split_7x2");
                gradient[x, y] = x + y;
                gradient.TraceStores();

                // Splitting guarantees that the inner loop runs from zero to
                // the split factor, which is important for the uses we saw
                // above. So what happens when the total extent we wish to
                // evaluate x over isn't a multiple of the split factor? We'll
                // split by a factor three, and we'll evaluate gradient over a
                // 7x2 box instead of the 4x4 box we've been using.
                var x_outer = new HSVar("x_outer");
                var x_inner = new HSVar("x_inner");
                gradient.Split(x, x_outer, x_inner, 3);

                Console.WriteLine("Evaluating gradient over a 7x2 box with x split by three ");
                var output = gradient.Realize<int>(7, 2);

                // See figures/lesson_05_split_7_by_3.gif for a visualization
                // of what happened. Note that some points get evaluated more
                // than once!

                Console.WriteLine("Equivalent C:");
                for (int yy = 0; yy < 2; yy++) {
                    for (int xOuter = 0; xOuter < 3; xOuter++) { // Now runs from 0 to 2
                        for (int xInner = 0; xInner < 3; xInner++) {
                            int xx = xOuter * 3;
                            // Before we add x_inner, make sure we don't
                            // evaluate points outside of the 7x2 box. We'll
                            // clamp x to be at most 4 (7 minus the split
                            // factor).
                            if (xx > 4) xx = 4;
                            xx += xInner;
                            Console.WriteLine($"Evaluating at x = {xx}, y = {yy}: {xx + yy}");
                        }
                    }
                }
                Console.WriteLine();

                Console.WriteLine("Pseudo-code for the schedule:");
                gradient.PrintLoopNest();
                Console.WriteLine();

                // If you read the output, you'll see that some coordinates
                // were evaluated more than once. That's generally OK, because
                // pure Halide functions have no side-effects, so it's safe to
                // evaluate the same point multiple times. If you're calling
                // out to C functions like we are, it's your responsibility to
                // make sure you can handle the same point being evaluated
                // multiple times.

                // The general rule is: If we require x from x_min to x_min + x_extent, and
                // we split by a factor 'factor', then:
                //
                // x_outer runs from 0 to (x_extent + factor - 1)/factor
                // x_inner runs from 0 to factor
                // x = min(x_outer * factor, x_extent - factor) + x_inner + x_min
                //
                // In our example, x_min was 0, x_extent was 7, and factor was 3.

                // However, if you write a Halide function with an update
                // definition (see lesson 9), then it is not safe to evaluate
                // the same point multiple times, so we won't apply this
                // trick. Instead the range of values computed will be rounded
                // up to the next multiple of the split factor.
            }

            // Fusing, tiling, and parallelizing.
            {
                // We saw in the previous lesson that we can parallelize
                // across a variable. Here we combine it with fusing and
                // tiling to express a useful pattern - processing tiles in
                // parallel.

                // This is where fusing shines. Fusing helps when you want to
                // parallelize across multiple dimensions without introducing
                // nested parallelism. Nested parallelism (parallel for loops
                // within parallel for loops) is supported by Halide, but
                // often gives poor performance compared to fusing the
                // parallel variables into a single parallel for loop.

                var gradient = new HSFunc("gradient_fused_tiles");
                gradient[x, y] = x + y;
                gradient.TraceStores();

                // First we'll tile, then we'll fuse the tile indices and
                // parallelize across the combination.
                var x_outer = new HSVar("x_outer");
                var y_outer = new HSVar("y_outer");
                var x_inner = new HSVar("x_inner");
                var y_inner  = new HSVar("y_inner");
                var tile_index = new HSVar("tile_index");
                gradient.Tile(x, y, x_outer, y_outer, x_inner, y_inner, 4, 4);
                gradient.Fuse(x_outer, y_outer, tile_index);
                gradient.Parallel(tile_index);

                // The scheduling calls all return a reference to the Func, so
                // you can also chain them together into a single statement to
                // make things slightly clearer:
                //
                // gradient
                //     .tile(x, y, x_outer, y_outer, x_inner, y_inner, 2, 2)
                //     .fuse(x_outer, y_outer, tile_index)
                //     .parallel(tile_index);


                Console.WriteLine("Evaluating gradient tiles in parallel");
                var output = gradient.Realize<int>(8, 8);

                // The tiles should occur in arbitrary order, but within each
                // tile the pixels will be traversed in row-major order. See
                // figures/lesson_05_parallel_tiles.gif for a visualization.

                Console.WriteLine("Equivalent (serial) C:\n");
                // This outermost loop should be a parallel for loop, but that's hard in C.
                for (int ti = 0; ti < 4; ti++) {
                    int yOuter = ti / 2;
                    int xOuter = ti % 2;
                    for (int j_inner = 0; j_inner < 4; j_inner++) {
                        for (int i_inner = 0; i_inner < 4; i_inner++) {
                            int j = yOuter * 4 + j_inner;
                            int i = xOuter * 4 + i_inner;
                            Console.WriteLine($"Evaluating at x = {i}, y = {j}: {i + j}");
                        }
                    }
                }

                Console.WriteLine();

                Console.WriteLine("Pseudo-code for the schedule:");
                gradient.PrintLoopNest();
                Console.WriteLine();
            }

            // Putting it all together.
            {
                // Are you ready? We're going to use all of the features above now.
                var gradient_fast = new HSFunc("gradient_fast");
                gradient_fast[x, y] = x + y;

                // We'll process 64x64 tiles in parallel.
                var x_outer = new HSVar("x_outer");
                var y_outer = new HSVar("y_outer");
                var x_inner = new HSVar("x_inner");
                var y_inner = new HSVar("y_inner");
                var tile_index = new HSVar("tile_index");
                gradient_fast
                    .Tile(x, y, x_outer, y_outer, x_inner, y_inner, 64, 64)
                    .Fuse(x_outer, y_outer, tile_index)
                    .Parallel(tile_index);

                // We'll compute two scanlines at once while we walk across
                // each tile. We'll also vectorize in x. The easiest way to
                // express this is to recursively tile again within each tile
                // into 4x2 subtiles, then vectorize the subtiles across x and
                // unroll them across y:
                var x_inner_outer = new HSVar("x_inner_outer");
                var y_inner_outer = new HSVar("y_inner_outer");
                var x_vectors = new HSVar("x_vectors");
                var y_pairs = new HSVar("y_pairs");
                gradient_fast
                    .Tile(x_inner, y_inner, x_inner_outer, y_inner_outer, x_vectors, y_pairs, 4, 2)
                    .Vectorize(x_vectors)
                    .Unroll(y_pairs);

                // Note that we didn't do any explicit splitting or
                // reordering. Those are the most important primitive
                // operations, but mostly they are buried underneath tiling,
                // vectorizing, or unrolling calls.

                // Now let's evaluate this over a range which is not a
                // multiple of the tile size.

                // If you like you can turn on tracing, but it's going to
                // produce a lot of printfs. Instead we'll compute the answer
                // both in C and Halide and see if the answers match.
                var result = gradient_fast.Realize<int>(350, 250);

                // See figures/lesson_05_fast.mp4 for a visualization.

                Console.WriteLine("Checking Halide result against equivalent C...");
                for (int tileIndex = 0; tileIndex < 6 * 4; tileIndex++) {
                    int yOuter = tileIndex / 4;
                    int xOuter = tileIndex % 4;
                    for (int yInnerOuter = 0; yInnerOuter < 64/2; yInnerOuter++) {
                        for (int xInnerOuter = 0; xInnerOuter < 64/4; xInnerOuter++) {
                            // We're vectorized across x
                            int xx = Math.Min(xOuter * 64, 350-64) + xInnerOuter*4;
                            int[] xVec = {xx + 0,
                                          xx + 1,
                                          xx + 2,
                                          xx + 3};

                            // And we unrolled across y
                            int yBase = Math.Min(yOuter * 64, 250-64) + yInnerOuter*2;
                            {
                                // y_pairs = 0
                                int yy = yBase + 0;
                                int[] yVec = {yy, yy, yy, yy};
                                int[] val = {xVec[0] + yVec[0],
                                             xVec[1] + yVec[1],
                                             xVec[2] + yVec[2],
                                             xVec[3] + yVec[3]};

                                // Check the result.
                                for (int i = 0; i < 4; i++) {
                                    if (result[xVec[i], yVec[i]] != val[i])
                                    {
                                        Console.WriteLine($"There was an error at {xVec[i]} {yVec[i]}!");
                                        return -1;
                                    }
                                }
                            }
                            {
                                // y_pairs = 1
                                int yy = yBase + 1;
                                int[] yVec = {yy, yy, yy, yy};
                                int[] val = {xVec[0] + yVec[0],
                                             xVec[1] + yVec[1],
                                             xVec[2] + yVec[2],
                                             xVec[3] + yVec[3]};

                                // Check the result.
                                for (int i = 0; i < 4; i++) {
                                    if (result[xVec[i], yVec[i]] != val[i])
                                    {
                                        Console.WriteLine($"There was an error at {xVec[i]} {yVec[i]}!");
                                        return -1;
                                    }
                                }
                            }
                        }
                    }
                }
                Console.WriteLine();

                Console.WriteLine("Pseudo-code for the schedule:");
                gradient_fast.PrintLoopNest();
                Console.WriteLine();

                // Note that in the Halide version, the algorithm is specified
                // once at the top, separately from the optimizations, and there
                // aren't that many lines of code total. Compare this to the C
                // version. There's more code (and it isn't even parallelized or
                // vectorized properly). More annoyingly, the statement of the
                // algorithm (the result is x plus y) is buried in multiple places
                // within the mess. This C code is hard to write, hard to read,
                // hard to debug, and hard to optimize further. This is why Halide
                // exists.
            }


            Console.WriteLine("Success!");
            return 0;
        }
    }
}