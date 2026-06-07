namespace Spectre.Console.Rendering;

/// <summary>
/// Represents a border with heavy horizontal edges and light vertical edges.
/// </summary>
public sealed class HeavyHorizontalBoxBorder : BoxBorder
{
    /// <inheritdoc/>
    public override BoxBorder? SafeBorder => BoxBorder.Square;

    /// <inheritdoc/>
    public override string GetPart(BoxBorderPart part)
    {
        return part switch
        {
            BoxBorderPart.TopLeft => "┍",
            BoxBorderPart.Top => "━",
            BoxBorderPart.TopRight => "┑",
            BoxBorderPart.Left => "│",
            BoxBorderPart.Right => "│",
            BoxBorderPart.BottomLeft => "┕",
            BoxBorderPart.Bottom => "━",
            BoxBorderPart.BottomRight => "┙",
            _ => throw new InvalidOperationException("Unknown border part."),
        };
    }
}
