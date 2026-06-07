namespace Spectre.Console.Rendering;

/// <summary>
/// Represents the vertical variant of the McGugan border, which draws thin
/// block edges inside the content's bounds. Named after Will McGugan.
/// </summary>
public sealed class McGuganVerticalBoxBorder : BoxBorder
{
    /// <inheritdoc/>
    public override BoxBorder? SafeBorder => BoxBorder.Square;

    /// <inheritdoc/>
    public override string GetPart(BoxBorderPart part)
    {
        return part switch
        {
            BoxBorderPart.TopLeft => "▕",
            BoxBorderPart.Top => "▔",
            BoxBorderPart.TopRight => "▏",
            BoxBorderPart.Left => "▕",
            BoxBorderPart.Right => "▏",
            BoxBorderPart.BottomLeft => "▕",
            BoxBorderPart.Bottom => "▁",
            BoxBorderPart.BottomRight => "▏",
            _ => throw new InvalidOperationException("Unknown border part."),
        };
    }
}
