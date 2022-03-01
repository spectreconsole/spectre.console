namespace Spectre.Console.Rendering;

/// <summary>
/// Represents a rounded border.
/// </summary>
public sealed class RoundedTableBorder : TableBorder
{
    /// <inheritdoc/>
    public override TableBorder? SafeBorder => TableBorder.Square;

    /// <inheritdoc/>
    public override string GetPart(TableBorderPart part)
    {
        return part switch
        {
            TableBorderPart.HeaderTopLeft => "╭",
            TableBorderPart.HeaderTop => "─",
            TableBorderPart.HeaderTopSeparator => "┬",
            TableBorderPart.HeaderTopRight => "╮",
            TableBorderPart.HeaderLeft => "│",
            TableBorderPart.HeaderSeparator => "│",
            TableBorderPart.HeaderRight => "│",
            TableBorderPart.HeaderBottomLeft => "├",
            TableBorderPart.HeaderBottom => "─",
            TableBorderPart.HeaderBottomSeparator => "┼",
            TableBorderPart.HeaderBottomRight => "┤",
            TableBorderPart.CellLeft => "│",
            TableBorderPart.CellSeparator => "│",
            TableBorderPart.CellRight => "│",
            TableBorderPart.FooterTopLeft => "├",
            TableBorderPart.FooterTop => "─",
            TableBorderPart.FooterTopSeparator => "┼",
            TableBorderPart.FooterTopRight => "┤",
            TableBorderPart.FooterBottomLeft => "╰",
            TableBorderPart.FooterBottom => "─",
            TableBorderPart.FooterBottomSeparator => "┴",
            TableBorderPart.FooterBottomRight => "╯",
            _ => throw new InvalidOperationException("Unknown border part."),
        };
    }
}