using SixLabors.ImageSharp.Processing;
using Spectre.Console;

namespace Generator.Commands.Samples
{
    internal class CanvasImageSample : BaseSample
    {
        public override void Run(IAnsiConsole console)
        {
            var image = new CanvasImage("../../../examples/Console/Canvas/cake.png");
            image.MaxWidth(16);
            console.Write(image);
        }
    }

    internal class CanvasImageManipulationSample : BaseSample
    {
        public override void Run(IAnsiConsole console)
        {
            var image = new CanvasImage("../../../examples/Console/Canvas/cake.png");
            image.MaxWidth(24);
            image.BilinearResampler();
            image.Mutate(ctx => ctx.Grayscale().Rotate(-45).EntropyCrop());
            console.Write(image);
        }
    }
}