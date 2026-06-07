namespace Spectre.Console.Rendering;

/// <summary>
/// Represents a wide-dashed border with square corners.
/// </summary>
public sealed class DashedWideBoxBorder : BoxBorder
{
    /// <inheritdoc/>
    public override BoxBorder? SafeBorder => BoxBorder.Square;

    /// <inheritdoc/>
    public override string GetPart(BoxBorderPart part)
    {
        return part switch
        {
            BoxBorderPart.TopLeft => "┌",
            BoxBorderPart.Top => "╌",
            BoxBorderPart.TopRight => "┐",
            BoxBorderPart.Left => "╎",
            BoxBorderPart.Right => "╎",
            BoxBorderPart.BottomLeft => "└",
            BoxBorderPart.Bottom => "╌",
            BoxBorderPart.BottomRight => "┘",
            _ => throw new InvalidOperationException("Unknown border part."),
        };
    }
}
