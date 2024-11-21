namespace Spectre.Console.ImageSharp.Sixels.Models;

/// <summary>
/// Represents the size of a cell in pixels for sixel rendering.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Sixel"/> class.
/// </remarks>
/// <param name="pixelWidth">The width of a sixel image in pixels.</param>
/// <param name="pixelHeight">The height of a sixel image in pixels.</param>
/// <param name="cellHeight">The height of a sixel image in terminal cells.</param>
/// <param name="cellWidth">The width of a sixel image in terminal cells.</param>
/// <param name="sixelStrings">The Sixel strings representing each frame of the image.</param>
public class Sixel(int pixelWidth, int pixelHeight, int cellHeight, int cellWidth, string[] sixelStrings)
{
    /// <summary>
    /// Gets the width of a sixel image in pixels.
    /// </summary>
    public int PixelWidth { get; init; } = pixelWidth;

    /// <summary>
    /// Gets the height of a sixel image in pixels.
    /// </summary>
    public int PixelHeight { get; init; } = pixelHeight;

    /// <summary>
    /// Gets the height of a sixel image in terminal cells.
    /// </summary>
    public int CellHeight { get; init; } = cellHeight;

    /// <summary>
    /// Gets the width of a sixel image in terminal cells.
    /// </summary>
    public int CellWidth { get; init; } = cellWidth;

    /// <summary>
    /// Gets the Sixel string.
    /// </summary>
    /// <returns>The Sixel string.</returns>
    public string[] SixelStrings { get; init; } = sixelStrings;
}