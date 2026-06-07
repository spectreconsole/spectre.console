namespace Spectre.Console.Rendering;

/// <summary>
/// Represents a border with heavy vertical edges and light horizontal edges.
/// </summary>
public sealed class HeavyVerticalBoxBorder : BoxBorder
{
    /// <inheritdoc/>
    public override BoxBorder? SafeBorder => BoxBorder.Square;

    /// <inheritdoc/>
    public override string GetPart(BoxBorderPart part)
    {
        return part switch
        {
            BoxBorderPart.TopLeft => "┎",
            BoxBorderPart.Top => "─",
            BoxBorderPart.TopRight => "┒",
            BoxBorderPart.Left => "┃",
            BoxBorderPart.Right => "┃",
            BoxBorderPart.BottomLeft => "┖",
            BoxBorderPart.Bottom => "─",
            BoxBorderPart.BottomRight => "┚",
            _ => throw new InvalidOperationException("Unknown border part."),
        };
    }
}
