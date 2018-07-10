using System;
using HalideSharp;

namespace Lesson01
{
    public class Program
    {
        public static int Main()
        {
            var x = new HSVar("x");
            var y = new HSVar("y");

            HSExpr e = x + y;

            var gradient = new HSFunc("gradient");
            gradient[x, y] = e;

            HSBuffer<int> output = gradient.Realize<int>(800, 600);

            for (int j = 0; j < output.Height; j++)
            {
                for (int i = 0; i < output.Width; i++)
                {
                    if (output[i, j] != i + j)
                    {
                        Console.WriteLine($"Something went wrong!");
                        Console.WriteLine($"Pixel {i}, {j} was supposed to be {i+j}, but instead it's {output[i, j]}");
                        return -1;
                    }
                }
            }

            Console.WriteLine("Success!");
            return 0;
        }
    }
}