using System;
using HalideSharp;

namespace Lesson19
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            // First we'll declare some Vars to use below.
            var x = new HSVar("x");
            var y = new HSVar("y");
            var xo = new HSVar("xo");
            var yo = new HSVar("yo");
            var xi = new HSVar("xi");
            var yi = new HSVar("yi");

            // This lesson will be about "wrapping" a Func or an ImageParam using the
            // Func::in and ImageParam::in directives
            {
                {
                    // Consider a simple two-stage pipeline:
                    var f = new HSFunc("f_local");
                    var g = new HSFunc("g_local");
                    f[x, y] = x + y;
                    g[x, y] = 2 * f[x, y] + 3;

                    f.ComputeRoot();

                    // This produces the following loop nests:
                    // for y:
                    //   for x:
                    //     f(x, y) = x + y
                    // for y:
                    //   for x:
                    //     g(x, y) = 2 * f(x, y) + 3

                    // Using Func::in, we can interpose a new Func in between f
                    // and g using the schedule alone:
                    HSFunc f_in_g = f.In(g);
                    f_in_g.ComputeRoot();

                    // Equivalently, we could also chain the schedules like so:
                    // f.in(g).ComputeRoot();

                    // This produces the following three loop nests:
                    // for y:
                    //   for x:
                    //     f(x, y) = x + y
                    // for y:
                    //   for x:
                    //     f_in_g(x, y) = f(x, y)
                    // for y:
                    //   for x:
                    //     g(x, y) = 2 * f_in_g(x, y) + 3

                    g.Realize<int>(5, 5);

                    // See figures/lesson_19_wrapper_local.mp4 for a visualization.
                }

                // The schedule directive f.in(g) replaces all calls to 'f'
                // inside 'g' with a wrapper Func and then returns that
                // wrapper. Essentially, it rewrites the original pipeline
                // above into the following:
                {
                    var f_in_g = new HSFunc("f_in_g");
                    var f = new HSFunc("f");
                    var g = new HSFunc("g");
                    f[x, y] = x + y;
                    f_in_g[x, y] = f[x, y];
                    g[x, y] = 2 * f_in_g[x, y] + 3;

                    f.ComputeRoot();
                    f_in_g.ComputeRoot();
                    g.ComputeRoot();
                }

                // In isolation, such a transformation seems pointless, but it
                // can be used for a variety of scheduling tricks.
            }

            {
                // In the schedule above, only the calls to 'f' made by 'g'
                // are replaced. Other calls made to f would still call 'f'
                // directly. If we wish to globally replace all calls to 'f'
                // with a single wrapper, we simply say f.in().

                // Consider a three stage pipeline, with two consumers of f:
                var f = new HSFunc("f_global");
                var g = new HSFunc("g_global");
                var h = new HSFunc("h_global");
                f[x, y] = x + y;
                g[x, y] = 2 * f[x, y];
                h[x, y] = 3 + g[x, y] - f[x, y];
                f.ComputeRoot();
                g.ComputeRoot();
                h.ComputeRoot();

                // We will replace all calls to 'f' inside both 'g' and 'h'
                // with calls to a single wrapper:
                f.In().ComputeRoot();

                // The equivalent loop nests are:
                // for y:
                //   for x:
                //     f(x, y) = x + y
                // for y:
                //   for x:
                //     f_in(x, y) = f(x, y)
                // for y:
                //   for x:
                //     g(x, y) = 2 * f_in(x, y)
                // for y:
                //   for x:
                //     h(x, y) = 3 + g(x, y) - f_in(x, y)

                h.Realize<int>(5, 5);

                // See figures/lesson_19_wrapper_global.mp4 and for a
                // visualization of what this did.
            }

            {
                // We could also give g and h their own unique wrappers of
                // f. This time we'll schedule them each inside the loop nests
                // of the consumer, which is not something we could do with a
                // single global wrapper.

                var f = new HSFunc("f_unique");
                var g = new HSFunc("g_unique");
                var h = new HSFunc("h_unique");
                f[x, y] = x + y;
                g[x, y] = 2 * f[x, y];
                h[x, y] = 3 + g[x, y] - f[x, y];

                f.ComputeRoot();
                g.ComputeRoot();
                h.ComputeRoot();

                f.In(g).ComputeAt(g, y);
                f.In(h).ComputeAt(h, y);

                // This creates the loop nests:
                // for y:
                //   for x:
                //     f(x, y) = x + y
                // for y:
                //   for x:
                //     f_in_g(x, y) = f(x, y)
                //   for x:
                //     g(x, y) = 2 * f_in_g(x, y)
                // for y:
                //   for x:
                //     f_in_h(x, y) = f(x, y)
                //   for x:
                //     h(x, y) = 3 + g(x, y) - f_in_h(x, y)

                h.Realize<int>(5, 5);
                // See figures/lesson_19_wrapper_unique.mp4 for a visualization.
            }

            {
                // So far this may seem like a lot of pointless copying of
                // memory. Func::in can be combined with other scheduling
                // directives for a variety of purposes. The first we will
                // examine is creating distinct realizations of a Func for
                // several consumers and scheduling each differently.

                // We'll start with nearly the same pipeline.
                var f = new HSFunc("f_sched");
                var g = new HSFunc("g_sched");
                var h = new HSFunc("h_sched");
                f[x, y] = x + y;
                g[x, y] = 2 * f[x, y];
                // h will use a far-away region of f
                h[x, y] = 3 + g[x, y] - f[x + 93, y - 87];

                // This time we'll inline f.
                // f.ComputeRoot();
                g.ComputeRoot();
                h.ComputeRoot();

                f.In(g).ComputeAt(g, y);
                f.In(h).ComputeAt(h, y);

                // g and h now call f via distinct wrappers. The wrappers are
                // scheduled, but f is not, which means that f is inlined into
                // its two wrappers. They will each independently compute the
                // region of f required by their consumer. If we had scheduled
                // f ComputeRoot, we'd be computing the bounding box of the
                // region required by g and the region required by h, which
                // would mostly be unused data.

                // We can also schedule each of these wrappers
                // differently. For scheduling purposes, wrappers inherit the
                // pure vars of the Func they wrap, so we use the same x and y
                // that we used when defining f:
                f.In(g).Vectorize(x, 4);
                f.In(h).Split(x, xo, xi, 2).Reorder(xo, xi);

                // Note that calling f.in(g) a second time returns the wrapper
                // already created by the first call, it doesn't make a new one.

                h.Realize<int>(8, 8);
                // See figures/lesson_19_wrapper_vary_schedule.mp4 for a
                // visualization.

                // Note that because f is inlined into its two wrappers, it is
                // the wrappers that do the work of computing f, rather than
                // just loading from an existing computed realization.
            }

            {
                // Func::in is useful to stage loads from a Func via some
                // smaller intermediate buffer, perhaps on the stack or in
                // shared GPU memory.

                // Consider a pipeline that transposes some ComputeRoot'd Func:

                var f = new HSFunc("f_transpose");
                var g = new HSFunc("g_transpose");
                f[x, y] = HSMath.Sin(((x + y) * HSMath.Sqrt(y)) / 10);
                f.ComputeRoot();

                g[x, y] = f[y, x];

                // The execution strategy we want is to load an 4x4 tile of f
                // into registers, transpose it in-register, and then write it
                // out as an 4x4 tile of g. We will use Func::in to express this:

                HSFunc f_tile = f.In(g);

                // We now have a three stage pipeline:
                // f -> f_tile -> g

                // f_tile will load vectors of f, and store them transposed
                // into registers. g will then write this data back to main
                // memory.
                g.Tile(x, y, xo, yo, xi, yi, 4, 4)
                    .Vectorize(xi)
                    .Unroll(yi);

                // We will compute f_transpose at tiles of g, and use
                // Func::reorder_storage to state that f_transpose should be
                // stored column-major, so that the loads to it done by g can
                // be dense vector loads.
                f_tile.ComputeAt(g, xo)
                    .ReorderStorage(y, x)
                    .Vectorize(x)
                    .Unroll(y);

                // We take care to make sure f_transpose is only ever accessed
                // at constant indicies. The full unrolling/vectorization of
                // all loops that exist inside its compute_at level has this
                // effect. Allocations that are only ever accessed at constant
                // indices can be promoted into registers.

                g.Realize<float>(16, 16);
                // See figures/lesson_19_transpose.mp4 for a visualization
            }

            {
                // ImageParam::in behaves the same way as Func::in, and you
                // can use it to stage loads in similar ways. Instead of
                // transposing again, we'll use ImageParam::in to stage tiles
                // of an input image into GPU shared memory, effectively using
                // shared/local memory as an explicitly-managed cache.

                var img = new HSImageParam<int>(2);

                // We will compute a small blur of the input.
                var blur = new HSFunc("blur");
                blur[x, y] = (img[x - 1, y - 1] + img[x, y - 1] + img[x + 1, y - 1] +
                              img[x - 1, y    ] + img[x, y    ] + img[x + 1, y    ] +
                              img[x - 1, y + 1] + img[x, y + 1] + img[x + 1, y + 1]);

                blur.ComputeRoot().GpuTile(x, y, xo, yo, xi, yi, 8, 8);

                // The wrapper Func created by ImageParam::in has pure vars
                // named _0, _1, etc. Schedule it per tile of "blur", and map
                // _0 and _1 to gpu threads.
                img.In(blur).ComputeAt(blur, xo).GpuThreads(HS._0, HS._1);

                // Without Func::in, computing an 8x8 tile of blur would do
                // 8*8*9 loads to global memory. With Func::in, the wrapper
                // does 10*10 loads to global memory up front, and then blur
                // does 8*8*9 loads to shared/local memory.

                // Select an appropriate GPU API, as we did in lesson 12
                var target = HS.GetHostTarget();
                if (target.OS == HSOperatingSystem.OSX) {
                    target.SetFeature(HSFeature.Metal);
                } else {
                    target.SetFeature(HSFeature.OpenCL);
                }

                // Create an interesting input image to use.
                var input = new HSBuffer<int>(258, 258);
                input.SetMin(-1, -1);
                for (int yy = input.Top; yy <= input.Bottom; yy++) {
                    for (int xx = input.Left; xx <= input.Right; xx++) {
                        input[xx, yy] = xx * 17 + yy % 4;
                    }
                }

                img.Set(input);
                blur.CompileJit(target);
                var output = blur.Realize<int>(256, 256);

                // Check the output is what we expected
                for (int yy = output.Top; yy <= output.Bottom; yy++) {
                    for (int xx = output.Left; xx <= output.Right; xx++) {
                        int val = output[xx, yy];
                        int expected = (input[xx - 1, yy - 1] + input[xx, yy - 1] + input[xx + 1, yy - 1] +
                                        input[xx - 1, yy    ] + input[xx, yy    ] + input[xx + 1, yy    ] +
                                        input[xx - 1, yy + 1] + input[xx, yy + 1] + input[xx + 1, yy + 1]);
                        if (val != expected) {
                            Console.WriteLine($"output({xx}, {yy}) = {val} instead of {expected}\n",
                                   xx, yy, val, expected);
                            return -1;
                        }
                    }
                }
            }

            {
                // Func::in can also be used to group multiple stages of a
                // Func into the same loop nest. Consider the following
                // pipeline, which computes a value per pixel, then sweeps
                // from left to right and back across each scanline.
                var f = new HSFunc("f_group");
                var g = new HSFunc("g_group");
                var h = new HSFunc("h_group");

                // Initialize f
                f[x, y] = HSMath.Sin(x - y);
                var r = new HSRDom(1, 7);

                // Sweep from left to right
                f[r, y] = (f[r, y] + f[r - 1, y]) / 2;

                // Sweep from right to left
                f[7 - r, y] = (f[7 - r, y] + f[8 - r, y]) / 2;

                // Then we do something with a complicated access pattern: A
                // 45 degree rotation with wrap-around
                g[x, y] = f[(x + y) % 8, (x - y) % 8];

                // f should be scheduled ComputeRoot, because its consumer
                // accesses it in a complicated way. But that means all stages
                // of f are computed in separate loop nests:

                // for y:
                //   for x:
                //     f(x, y) = sin(x - y)
                // for y:
                //   for r:
                //     f(r, y) = (f(r, y) + f(r - 1, y)) / 2
                // for y:
                //   for r:
                //     f(7 - r, y) = (f(7 - r, y) + f(8 - r, y)) / 2
                // for y:
                //   for x:
                //     g(x, y) = f((x + y) % 8, (x - y) % 8);

                // We can get better locality if we schedule the work done by
                // f to share a common loop over y. We can do this by
                // computing f at scanlines of a wrapper like so:

                f.In(g).ComputeRoot();
                f.ComputeAt(f.In(g), y);

                // f has the default schedule for a Func with update stages,
                // which is to be computed at the innermost loop of its
                // consumer, which is now the wrapper f.in(g). This therefore
                // generates the following loop nest, which has better
                // locality:

                // for y:
                //   for x:
                //     f(x, y) = sin(x - y)
                //   for r:
                //     f(r, y) = (f(r, y) + f(r - 1, y)) / 2
                //   for r:
                //     f(7 - r, y) = (f(7 - r, y) + f(8 - r, y)) / 2
                //   for x:
                //     f_in_g(x, y) = f(x, y)
                // for y:
                //   for x:
                //     g(x, y) = f_in_g((x + y) % 8, (x - y) % 8);

                // We'll additionally vectorize the initialization of, and
                // then transfer of pixel values from f into its wrapper:
                f.Vectorize(x, 4);
                f.In(g).Vectorize(x, 4);

                g.Realize<float>(8, 8);
                // See figures/lesson_19_group_updates.mp4 for a visualization.
            }

            Console.WriteLine("Success!");

            return 0;

        }
    }
}