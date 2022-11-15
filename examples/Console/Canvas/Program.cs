using System.Diagnostics;
using System.Reflection;
using SixLabors.ImageSharp.Processing;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Canvas;

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
        image.BilinearResampler();
        image.MaxWidth(16);
        Render(image, "Image from file (16 wide)");

        // Draw image again, but without render width
        image.NoMaxWidth();
        image.Mutate(ctx => ctx.Grayscale().Rotate(-45).EntropyCrop());
        Render(image, "Image from file (fit, greyscale, rotated)");

        // Draw image again, but load from embedded resource rather than file
        using (var fileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Canvas.cake.png"))
        {
            Debug.Assert(fileStream != null);
            var embeddedImage = new CanvasImage(fileStream);
            embeddedImage.BilinearResampler();
            embeddedImage.MaxWidth(16);
            Render(embeddedImage, "Image from embedded resource (16 wide)");
        }
    }

    private static void Render(IRenderable canvas, string title)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Rule($"[yellow]{title}[/]").LeftJustified().RuleStyle("grey"));
        AnsiConsole.WriteLine();
        AnsiConsole.Write(canvas);
    }
}
