namespace Spectre.Console.Rendering;

/// <summary>
/// Represents a border with double vertical edges and single horizontal edges.
/// </summary>
public sealed class DoubleVerticalBoxBorder : BoxBorder
{
    /// <inheritdoc/>
    public override BoxBorder? SafeBorder => BoxBorder.Double;

    /// <inheritdoc/>
    public override string GetPart(BoxBorderPart part)
    {
        return part switch
        {
            BoxBorderPart.TopLeft => "╓",
            BoxBorderPart.Top => "─",
            BoxBorderPart.TopRight => "╖",
            BoxBorderPart.Left => "║",
            BoxBorderPart.Right => "║",
            BoxBorderPart.BottomLeft => "╙",
            BoxBorderPart.Bottom => "─",
            BoxBorderPart.BottomRight => "╜",
            _ => throw new InvalidOperationException("Unknown border part."),
        };
    }
}
