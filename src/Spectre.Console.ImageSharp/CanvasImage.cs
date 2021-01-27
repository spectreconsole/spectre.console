using System;
using System.Collections.Generic;
using System.IO;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a renderable image.
    /// </summary>
    public sealed class CanvasImage : Renderable
    {
        private static readonly IResampler _defaultResampler = KnownResamplers.Bicubic;

        /// <summary>
        /// Gets the image width.
        /// </summary>
        public int Width => Image.Width;

        /// <summary>
        /// Gets the image height.
        /// </summary>
        public int Height => Image.Height;

        /// <summary>
        /// Gets or sets the render width of the canvas.
        /// </summary>
        public int? MaxWidth { get; set; }

        /// <summary>
        /// Gets or sets the render width of the canvas.
        /// </summary>
        public int PixelWidth { get; set; } = 2;

        /// <summary>
        /// Gets or sets the <see cref="IResampler"/> that should
        /// be used when scaling the image. Defaults to bicubic sampling.
        /// </summary>
        public IResampler? Resampler { get; set; }

        internal SixLabors.ImageSharp.Image<Rgba32> Image { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasImage"/> class.
        /// </summary>
        /// <param name="filename">The image filename.</param>
        public CanvasImage(string filename)
        {
            Image = SixLabors.ImageSharp.Image.Load<Rgba32>(filename);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasImage"/> class.
        /// </summary>
        /// <param name="data">Buffer containing an image.</param>
        public CanvasImage(ReadOnlySpan<byte> data)
        {
            Image = SixLabors.ImageSharp.Image.Load<Rgba32>(data);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasImage"/> class.
        /// </summary>
        /// <param name="data">Stream containing an image.</param>
        public CanvasImage(Stream data)
        {
            Image = SixLabors.ImageSharp.Image.Load<Rgba32>(data);
        }

        /// <inheritdoc/>
        protected override Measurement Measure(RenderContext context, int maxWidth)
        {
            if (PixelWidth < 0)
            {
                throw new InvalidOperationException("Pixel width must be greater than zero.");
            }

            var width = MaxWidth ?? Width;
            if (maxWidth < width * PixelWidth)
            {
                return new Measurement(maxWidth, maxWidth);
            }

            return new Measurement(width * PixelWidth, width * PixelWidth);
        }

        /// <inheritdoc/>
        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            var image = Image;

            var width = Width;
            var height = Height;

            // Got a max width?
            if (MaxWidth != null)
            {
                height = (int)(height * ((float)MaxWidth.Value) / Width);
                width = MaxWidth.Value;
            }

            // Exceed the max width when we take pixel width into account?
            if (width * PixelWidth > maxWidth)
            {
                height = (int)(height * (maxWidth / (float)(width * PixelWidth)));
                width = maxWidth / PixelWidth;
            }

            // Need to rescale the pixel buffer?
            if (width != Width || height != Height)
            {
                var resampler = Resampler ?? _defaultResampler;
                image = image.Clone(); // Clone the original image
                image.Mutate(i => i.Resize(width, height, resampler));
            }

            var canvas = new Canvas(width, height)
            {
                MaxWidth = MaxWidth,
                PixelWidth = PixelWidth,
                Scale = false,
            };

            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    if (image[x, y].A == 0)
                    {
                        continue;
                    }

                    canvas.SetPixel(x, y, new Color(
                        image[x, y].R, image[x, y].G, image[x, y].B));
                }
            }

            return ((IRenderable)canvas).Render(context, maxWidth);
        }
    }
}
