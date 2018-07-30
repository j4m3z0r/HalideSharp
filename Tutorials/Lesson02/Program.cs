using System;
using HalideSharp;

namespace Lesson02
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            // This program defines a single-stage imaging pipeline that
            // brightens an image.

            // First we'll load the input image we wish to brighten.
            var input = HSBuffer<byte>.LoadImage("rgb.png");

            // See figures/lesson_02_input.jpg for a smaller version.

            // Next we define our Func object that represents our one pipeline
            // stage.
            var brighter = new HSFunc("brighter");

            // Our Func will have three arguments, representing the position
            // in the image and the color channel. Halide treats color
            // channels as an extra dimension of the image.
            var x = new HSVar("x");
            var y = new HSVar("y");
            var c = new HSVar("c");

            // Normally we'd probably write the whole function definition on
            // one line. Here we'll break it apart so we can explain what
            // we're doing at every step.

            // For each pixel of the input image.
            var value = input[x, y, c];

            // Cast it to a floating point value.
            value = HS.Cast<float>(value);

            // Multiply it by 1.5 to brighten it. Halide represents real
            // numbers as floats, not doubles, so we stick an 'f' on the end
            // of our constant.
            value = value * 1.5f;

            // Clamp it to be less than 255, so we don't get overflow when we
            // cast it back to an 8-bit unsigned int.
            value = HSMath.Min(value, 255.0f);

            // Cast it back to an 8-bit unsigned integer.
            value = HS.Cast<byte>(value);

            // Define the function.
            brighter[x, y, c] = value;

            // The equivalent one-liner to all of the above is:
            //
            // brighter(x, y, c) = Halide::cast<uint8_t>(min(input(x, y, c) * 1.5f, 255));
            //
            // In the shorter version:
            // - I skipped the cast to float, because multiplying by 1.5f does
            //   that automatically.
            // - I also used an integer constant as the second argument in the
            //   call to min, because it gets cast to float to be compatible
            //   with the first argument.
            // - I left the Halide:: off the call to min. It's unnecessary due
            //   to Koenig lookup.

            // Remember, all we've done so far is build a representation of a
            // Halide program in memory. We haven't actually processed any
            // pixels yet. We haven't even compiled that Halide program yet.

            // So now we'll realize the Func. The size of the output image
            // should match the size of the input image. If we just wanted to
            // brighten a portion of the input image we could request a
            // smaller size. If we request a larger size Halide will throw an
            // error at runtime telling us we're trying to read out of bounds
            // on the input image.
            var output =
                brighter.Realize<byte>(input.Width, input.Height, input.Channels);

            // Save the output for inspection. It should look like a bright parrot.
            output.SaveImage("brighter.png");

            // See figures/lesson_02_output.jpg for a small version of the output.

            Console.WriteLine("Success!");
            return 0;
        }
    }
}