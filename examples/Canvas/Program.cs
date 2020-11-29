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
            var image = new CanvasImage("cake.png", CanvasRenderMode.Interlaced);

            image.BilinearResampler();
            image.Mutate(c =>
            {
                var (width, height) = c.GetCurrentSize();
                c.Resize(width / 2, height);
            });

            image.MaxWidth(32);
            Render(image, "Image from file (32 wide)");


            var image2 = new CanvasImage("cake.png", CanvasRenderMode.Interlaced);

            // Draw image without render width
            image2.NoMaxWidth();
            image2.BilinearResampler();
            image2.Mutate(ctx => ctx.Grayscale().Rotate(-45).EntropyCrop());
            image2.Mutate(c =>
            {
                var (width, height) = c.GetCurrentSize();
                c.Resize(width / 2, height);
            });
            Render(image2, "Image from file (fit, greyscale, rotated)");
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
