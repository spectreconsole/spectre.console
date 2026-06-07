namespace Spectre.Console.Rendering;

/// <summary>
/// Represents a border with double horizontal edges and single vertical edges.
/// </summary>
public sealed class DoubleHorizontalBoxBorder : BoxBorder
{
    /// <inheritdoc/>
    public override BoxBorder? SafeBorder => BoxBorder.Double;

    /// <inheritdoc/>
    public override string GetPart(BoxBorderPart part)
    {
        return part switch
        {
            BoxBorderPart.TopLeft => "╒",
            BoxBorderPart.Top => "═",
            BoxBorderPart.TopRight => "╕",
            BoxBorderPart.Left => "│",
            BoxBorderPart.Right => "│",
            BoxBorderPart.BottomLeft => "╘",
            BoxBorderPart.Bottom => "═",
            BoxBorderPart.BottomRight => "╛",
            _ => throw new InvalidOperationException("Unknown border part."),
        };
    }
}
