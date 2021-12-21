/*
Ported from: https://rosettacode.org/wiki/Mandelbrot_set#C.23
Licensed under GNU Free Documentation License 1.2
*/

using System;

namespace Spectre.Console.Examples;

public static class Mandelbrot
{
    private const double MaxValueExtent = 2.0;

    private struct ComplexNumber
    {
        public double Real { get; }
        public double Imaginary { get; }

        public ComplexNumber(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public static ComplexNumber operator +(ComplexNumber x, ComplexNumber y)
        {
            return new ComplexNumber(x.Real + y.Real, x.Imaginary + y.Imaginary);
        }

        public static ComplexNumber operator *(ComplexNumber x, ComplexNumber y)
        {
            return new ComplexNumber(x.Real * y.Real - x.Imaginary * y.Imaginary,
                x.Real * y.Imaginary + x.Imaginary * y.Real);
        }

        public double Abs()
        {
            return Real * Real + Imaginary * Imaginary;
        }
    }

    public static Canvas Generate(int width, int height)
    {
        var canvas = new Canvas(width, height);

        var scale = 2 * MaxValueExtent / Math.Min(canvas.Width, canvas.Height);
        for (var i = 0; i < canvas.Height; i++)
        {
            var y = (canvas.Height / 2 - i) * scale;
            for (var j = 0; j < canvas.Width; j++)
            {
                var x = (j - canvas.Width / 2) * scale;
                var value = Calculate(new ComplexNumber(x, y));
                canvas.SetPixel(j, i, GetColor(value));
            }
        }

        return canvas;
    }

    private static double Calculate(ComplexNumber c)
    {
        const int MaxIterations = 1000;
        const double MaxNorm = MaxValueExtent * MaxValueExtent;

        var iteration = 0;
        var z = new ComplexNumber();
        do
        {
            z = z * z + c;
            iteration++;
        } while (z.Abs() < MaxNorm && iteration < MaxIterations);

        return iteration < MaxIterations
            ? (double)iteration / MaxIterations
            : 0;
    }

    private static Color GetColor(double value)
    {
        const double MaxColor = 256;
        const double ContrastValue = 0.2;
        return new Color(0, 0, (byte)(MaxColor * Math.Pow(value, ContrastValue)));
    }
}
