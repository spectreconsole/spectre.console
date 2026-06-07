namespace Spectre.Console.Rendering;

/// <summary>
/// Represents a heavy dashed border.
/// </summary>
public sealed class HeavyDashedBoxBorder : BoxBorder
{
    /// <inheritdoc/>
    public override BoxBorder? SafeBorder => BoxBorder.Heavy;

    /// <inheritdoc/>
    public override string GetPart(BoxBorderPart part)
    {
        return part switch
        {
            BoxBorderPart.TopLeft => "┏",
            BoxBorderPart.Top => "┅",
            BoxBorderPart.TopRight => "┓",
            BoxBorderPart.Left => "┇",
            BoxBorderPart.Right => "┇",
            BoxBorderPart.BottomLeft => "┗",
            BoxBorderPart.Bottom => "┅",
            BoxBorderPart.BottomRight => "┛",
            _ => throw new InvalidOperationException("Unknown border part."),
        };
    }
}
