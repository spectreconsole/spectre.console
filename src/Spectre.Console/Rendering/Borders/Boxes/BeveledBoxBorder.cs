namespace Spectre.Console.Rendering;

/// <summary>
/// Represents a beveled border using thin block edges and diagonal corners.
/// </summary>
public sealed class BeveledBoxBorder : BoxBorder
{
    /// <inheritdoc/>
    public override BoxBorder? SafeBorder => BoxBorder.Square;

    /// <inheritdoc/>
    public override string GetPart(BoxBorderPart part)
    {
        return part switch
        {
            BoxBorderPart.TopLeft => "╱",
            BoxBorderPart.Top => "▔",
            BoxBorderPart.TopRight => "╲",
            BoxBorderPart.Left => "▏",
            BoxBorderPart.Right => "▕",
            BoxBorderPart.BottomLeft => "╲",
            BoxBorderPart.Bottom => "▁",
            BoxBorderPart.BottomRight => "╱",
            _ => throw new InvalidOperationException("Unknown border part."),
        };
    }
}
