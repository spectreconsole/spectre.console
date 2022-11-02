namespace Spectre.Console;

/// <summary>
/// Represents a size.
/// </summary>
[DebuggerDisplay("{Width,nq}x{Height,nq}")]
public struct Size
{
    /// <summary>
    /// Gets the width.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Size"/> struct.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public Size(int width, int height)
    {
        Width = width;
        Height = height;
    }
}
