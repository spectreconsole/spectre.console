using SixLabors.ImageSharp.Processing;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace CanvasExample
{
    public static class Program
    {
        public static void Main()
        {
            // Draw a mandelbrot set using a Canvas
            var mandelbrot = Mandelbrot.Generate(32, 32);
            Render(mandelbrot, "Mandelbrot");

            // Draw an image using CanvasImage powered by ImageSharp.
            // This requires the "Spectre.Console.ImageSharp" NuGet package.
            var image = new CanvasImage("cake.png");
            image.Mutate(c => c.Resize(32, 64));
            image.BilinearResampler();
            image.MaxWidth(32);
            Render(image, "Image from file (16 wide)");

            // Draw image again, but without render width
            image.NoMaxWidth();
            image.Mutate(ctx => ctx.Grayscale().Rotate(-45).EntropyCrop());
            Render(image, "Image from file (fit, greyscale, rotated)");
        }

        private static void Render(IRenderable canvas, string title)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Render(new Rule($"[yellow]{title}[/]").LeftAligned().RuleStyle("grey"));
            AnsiConsole.WriteLine();
            AnsiConsole.Render(canvas);
        }
    }
}
