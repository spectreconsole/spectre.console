using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Quantization;
using Spectre.Console;
using Spectre.Console.ImageSharp.Sixels.Models;

namespace Spectre.Console.ImageSharp.Sixels;

/// <summary>
/// Contains methods for converting an image to a Sixel format.
/// </summary>
public static class SixelParser
{
    /// <summary>
    /// Converts an image to a Sixel object.
    /// This uses a copy of the c# sixel codec from @trackd and @ShaunLawrie in https://github.com/trackd/Sixel.
    /// </summary>
    /// <param name="image">The image to convert.</param>
    /// <param name="cellWidth">The width of the cell in terminal cells.</param>
    /// <param name="frame">The frame to convert for animated image formats, defaults to zero so gifs show the first frame.</param>
    /// <param name="returnCursorToTopLeft">Whether to return the cursor to the top left after rendering the image.</param>
    /// <returns>The Sixel object.</returns>
    public static Sixel ImageToSixel(Image<Rgba32> image, int cellWidth, int frame = 0, bool returnCursorToTopLeft = true)
    {
        // We're going to resize the image when it's rendered, so use a copy to leave the original untouched.
        var imageClone = image.Clone();

        // Convert to pixel sizes.
        var pixelWidth = cellWidth * Compatibility.GetCellSize().PixelWidth;
        var pixelHeight = (int)Math.Round((double)imageClone.Height / imageClone.Width * pixelWidth);

        imageClone.Mutate(ctx =>
        {
            // Resize the image to the target size
            ctx.Resize(new ResizeOptions()
            {
                Sampler = KnownResamplers.Bicubic,
                Size = new SixLabors.ImageSharp.Size(pixelWidth, pixelHeight),
                PremultiplyAlpha = false,
            });

            // Sixel supports 256 colors max
            ctx.Quantize(new OctreeQuantizer(new()
            {
                MaxColors = 256,
            }));
        });

        var targetFrame = imageClone.Frames[frame];
        var cellPixelHeight = Compatibility.GetCellSize().PixelHeight;
        var cellHeight = (int)Math.Ceiling((double)pixelHeight / cellPixelHeight);

        var sixelString = FrameToSixelString(
            targetFrame,
            cellHeight,
            cellPixelHeight,
            returnCursorToTopLeft);

        return new Sixel(
            pixelWidth,
            pixelHeight,
            cellHeight,
            cellWidth,
            sixelString);
    }

    /// <summary>
    /// Converts an image frame to a Sixel string.
    /// </summary>
    /// <param name="frame">The image frame to convert.</param>
    /// <param name="cellHeight">The height of the cell in terminal cells.</param>
    /// <param name="cellPixelHeight">The height of in individual cell in pixels.</param>
    /// <param name="returnCursorToTopLeft">Whether to return the cursor to the top left after rendering the image.</param>
    /// <returns>The Sixel string.</returns>
    private static string FrameToSixelString(ImageFrame<Rgba32> frame, int cellHeight, int cellPixelHeight, bool returnCursorToTopLeft)
    {
        var sixelBuilder = new StringBuilder();
        var palette = new Dictionary<Rgba32, int>();
        var colorCounter = 1;
        var y = 0;
        sixelBuilder.StartSixel(frame.Width, cellHeight * cellPixelHeight);
        frame.ProcessPixelRows(accessor =>
        {
            for (y = 0; y < accessor.Height; y++)
            {
                var pixelRow = accessor.GetRowSpan(y);

                // The value of 1 left-shifted by the remainder of the current row divided by 6 gives the correct sixel character offset from the empty sixel char for each row.
                // See the description of s...s for more detail on the sixel format https://vt100.net/docs/vt3xx-gp/chapter14.html#S14.2.1
                var c = (char)(Constants.SIXELEMPTY + (1 << (y % 6)));
                var lastColor = -1;
                var repeatCounter = 0;

                foreach (ref var pixel in pixelRow)
                {
                    // The colors can be added to the palette and interleaved with the sixel data so long as the color is defined before it is used.
                    if (!palette.TryGetValue(pixel, out var colorIndex))
                    {
                        colorIndex = colorCounter++;
                        palette[pixel] = colorIndex;
                        sixelBuilder.AddColorToPalette(pixel, colorIndex);
                    }

                    // Transparency is a special color index of 0 that exists in our sixel palette.
                    var colorId = pixel.A == 0 ? 0 : colorIndex;

                    // Sixel data will use a repeat entry if the color is the same as the last one.
                    // https://vt100.net/docs/vt3xx-gp/chapter14.html#S14.3.1
                    if (colorId == lastColor || repeatCounter == 0)
                    {
                        // If the color was repeated go to the next loop iteration to check the next pixel.
                        lastColor = colorId;
                        repeatCounter++;
                        continue;
                    }

                    // Every time the color is not repeated the previous color is written to the string.
                    sixelBuilder.AppendRepeatSixelEntry(lastColor, repeatCounter, c);

                    // Remember the current color and reset the repeat counter.
                    lastColor = colorId;
                    repeatCounter = 1;
                }

                // Write the last color and repeat counter to the string for the current row.
                sixelBuilder.AppendRepeatSixelEntry(lastColor, repeatCounter, c);

                // Add a carriage return at the end of each row and a new line every 6 pixel rows.
                sixelBuilder.AppendCarriageReturn();
                if (y % 6 == 5)
                {
                    sixelBuilder.AppendNextLine();
                }
            }

            // Padding to ensure the cursor finishes below the image not halfway through the rendered pixels.
            for (var padding = y; padding <= (cellHeight * cellPixelHeight); padding++)
            {
                if (padding % 6 == 5)
                {
                    sixelBuilder.AppendNextLine();
                }
            }

            // And a final newline to position the cursor under the image.
            sixelBuilder.AppendNextLine();
        });

        sixelBuilder.AppendExitSixel();

        if (returnCursorToTopLeft)
        {
            // Can't use AnsiSequences.CUU(int n) from Spectre.Console.ImageSharp because it's not public.
            sixelBuilder.Append($"{Constants.ESC}[{cellHeight}A");
        }

        return sixelBuilder.ToString();
    }

    /// <summary>
    /// Adds a color to the sixel palette.
    /// </summary>
    /// <param name="sixelBuilder">The string builder to write to.</param>
    /// <param name="pixel">The pixel to add to the palette.</param>
    /// <param name="colorIndex">The index of the color in the palette.</param>
    private static void AddColorToPalette(this StringBuilder sixelBuilder, Rgba32 pixel, int colorIndex)
    {
        var r = (int)Math.Round(pixel.R / 255.0 * 100);
        var g = (int)Math.Round(pixel.G / 255.0 * 100);
        var b = (int)Math.Round(pixel.B / 255.0 * 100);

        sixelBuilder.Append(Constants.SIXELCOLOR)
                    .Append(colorIndex)
                    .Append(";2;")
                    .Append(r)
                    .Append(';')
                    .Append(g)
                    .Append(';')
                    .Append(b);
    }

    /// <summary>
    /// Writes a repeated sixel entry to the string builder.
    /// </summary>
    /// <param name="sixelBuilder">The string builder to write to.</param>
    /// <param name="colorIndex">The index of the color in the palette.</param>
    /// <param name="repeatCounter">The number of times the color is repeated.</param>
    /// <param name="sixelDataCharacter">The sixel character to write.</param>
    private static void AppendRepeatSixelEntry(this StringBuilder sixelBuilder, int colorIndex, int repeatCounter, char sixelDataCharacter)
    {
        if (repeatCounter <= 1)
        {
            sixelBuilder.AppendSixelEntry(colorIndex, sixelDataCharacter);
        }
        else
        {
            sixelBuilder.Append(Constants.SIXELCOLOR)
                    .Append(colorIndex)
                    .Append(Constants.SIXELREPEAT)
                    .Append(repeatCounter)
                    .Append(colorIndex != 0 ? sixelDataCharacter : Constants.SIXELEMPTY);
        }
    }

    /// <summary>
    /// Writes a sixel entry to the string builder.
    /// </summary>
    /// <param name="sixelBuilder">The string builder to write to.</param>
    /// <param name="colorIndex">The index of the color in the palette.</param>
    /// <param name="sixelDataCharacter">The sixel character to write.</param>
    private static void AppendSixelEntry(this StringBuilder sixelBuilder, int colorIndex, char sixelDataCharacter)
    {
        sixelBuilder.Append(Constants.SIXELCOLOR)
                    .Append(colorIndex)
                    .Append(colorIndex != 0 ? sixelDataCharacter : Constants.SIXELEMPTY);
    }

    /// <summary>
    /// Writes the Sixel carriage return sequence to the string builder.
    /// </summary>
    /// <param name="sixelBuilder">The string builder to write to.</param>
    private static void AppendCarriageReturn(this StringBuilder sixelBuilder)
    {
        sixelBuilder.Append(Constants.SIXELDECGCR);
    }

    /// <summary>
    /// Writes the Sixel next line sequence to the string builder.
    /// </summary>
    /// <param name="sixelBuilder">The string builder to write to.</param>
    private static void AppendNextLine(this StringBuilder sixelBuilder)
    {
        sixelBuilder.Append(Constants.SIXELDECGNL);
    }

    /// <summary>
    /// Writes the Sixel exit sequence to the string builder.
    /// </summary>
    /// <param name="sixelBuilder">The string builder to write to.</param>
    private static void AppendExitSixel(this StringBuilder sixelBuilder)
    {
        sixelBuilder.Append(Constants.SIXELEND);
    }

    /// <summary>
    /// Writes the Sixel start sequence to the string builder.
    /// </summary>
    /// <param name="sixelBuilder">The string builder to write to.</param>
    /// <param name="width">The width of the image in pixels.</param>
    /// <param name="height">The height of the image in pixels.</param>
    private static void StartSixel(this StringBuilder sixelBuilder, int width, int height)
    {
        sixelBuilder.Append(Constants.SIXELSTART)
                    .Append(Constants.SIXELRASTERATTRIBUTES)
                    .Append(width)
                    .Append(';')
                    .Append(height)
                    .Append(Constants.SIXELTRANSPARENTCOLOR);
    }
}