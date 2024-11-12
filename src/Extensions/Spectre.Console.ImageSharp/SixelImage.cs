using System;
using System.Collections.Generic;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using Spectre.Console.ImageSharp.Sixels;
using Spectre.Console.ImageSharp.Sixels.Models;
using Spectre.Console.Rendering;

namespace Spectre.Console;

/// <summary>
/// Represents a renderable image.
/// </summary>
public sealed class SixelImage : Renderable
{
    /// <summary>
    /// Gets the image width in pixels.
    /// </summary>
    public int Width => Image.Width;

    /// <summary>
    /// Gets the image height in pixels.
    /// </summary>
    public int Height => Image.Height;

    /// <summary>
    /// Gets or sets the render width of the canvas in terminal cells.
    /// </summary>
    public int? MaxWidth { get; set; }

    /// <summary>
    /// Gets the render width of the canvas. This is hard coded to 1 for sixel images.
    /// </summary>
    public int PixelWidth { get; } = 1;

    internal SixLabors.ImageSharp.Image<Rgba32> Image { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SixelImage"/> class.
    /// </summary>
    /// <param name="filename">The image filename.</param>
    public SixelImage(string filename)
    {
        Image = SixLabors.ImageSharp.Image.Load<Rgba32>(filename);
    }

    /// <inheritdoc/>
    protected override Measurement Measure(RenderOptions options, int maxWidth)
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
    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        // Got a max width smaller than the render max width?
        if (MaxWidth != null && MaxWidth < maxWidth)
        {
            maxWidth = MaxWidth.Value;
        }

        // Write the sixel data as a control segment which returns the cursor to the top left cell of the sixel after render.
        var sixel = SixelParser.ImageToSixel(Image, maxWidth);
        var segments = new List<Segment>
        {
            Segment.Control(sixel.SixelString),
        };

        // Draw a transparent renderable to take up the space the sixel is drawn in.
        // This allows Spectre.Console to render the image and not write overtop of it with space characters while padding panel borders etc.
        var canvas = new Canvas(sixel.CellWidth, sixel.CellHeight)
        {
            MaxWidth = sixel.CellWidth,
            PixelWidth = PixelWidth,
            Scale = false,
        };

        // TODO remove this, it's for drawing a red and transparent checkerboard pattern to debug sixel positioning
        if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SPECTRE_CONSOLE_DEBUG")))
        {
            for (var y = 0; y < sixel.CellHeight; y++)
            {
                for (var x = 0; x < sixel.CellWidth; x++)
                {
                    if (y % 2 == 0)
                    {
                        if (x % 2 == 0)
                        {
                            canvas.SetPixel(x, y, new Color(255, 0, 0));
                        }
                    }
                    else if (x % 2 != 0)
                    {
                        canvas.SetPixel(x, y, new Color(255, 0, 0));
                    }
                }
            }
        }

        // A combination of a zero-width control segment for sixel data and a transparent canvas.
        segments.AddRange(((IRenderable)canvas).Render(options, maxWidth));

        return segments;
    }
}