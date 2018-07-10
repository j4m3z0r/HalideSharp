using System;
using HalideSharp;

namespace Lesson02
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var input = HSBuffer<byte>.LoadImage("rgb.png");
            
            var brighter = new HSFunc("brighter");
            var x = new HSVar("x");
            var y = new HSVar("y");
            var c = new HSVar("c");

            var value = input.GetExpr(x, y, c);
            value = HS.Cast<float>(value);
            value = value * 1.5f;
            value = HS.Min(value, 255.0f);
            value = HS.Cast<byte>(value);

            brighter[x, y, c] = value;

            var output = brighter.Realize<byte>(input.Width, input.Height, input.Channels);
            
            output.SaveImage("brighter.png");
            
            Console.WriteLine("Success!");
        }
    }
}