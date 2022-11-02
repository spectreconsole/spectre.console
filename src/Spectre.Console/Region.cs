namespace Spectre.Console;

/// <summary>
/// Represents a region.
/// </summary>
[DebuggerDisplay("[X={X,nq}, Y={Y,nq}, W={Width,nq}, H={Height,nq}]")]
public struct Region
{
    /// <summary>
    /// Gets the x-coordinate.
    /// </summary>
    public int X { get; }

    /// <summary>
    /// Gets the y-coordinate.
    /// </summary>
    public int Y { get; }

    /// <summary>
    /// Gets the width.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Region"/> struct.
    /// </summary>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public Region(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }
}