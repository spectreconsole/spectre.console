namespace Spectre.Console.ImageSharp.Sixels.Models;

/// <summary>
/// Represents the size of a cell in pixels for sixel rendering.
/// </summary>
public class CellSize
{
    /// <summary>
    /// Gets the width of a cell in pixels.
    /// </summary>
    public int PixelWidth { get; init; }

    /// <summary>
    /// Gets the height of a cell in pixels.
    /// </summary>
    public int PixelHeight { get; init; }
}