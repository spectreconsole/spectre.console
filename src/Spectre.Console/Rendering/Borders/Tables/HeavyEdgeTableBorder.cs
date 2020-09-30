using System;

namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Represents a border with a heavy edge.
    /// </summary>
    public sealed class HeavyEdgeTableBorder : TableBorder
    {
        /// <inheritdoc/>
        public override TableBorder? SafeBorder => TableBorder.Square;

        /// <inheritdoc/>
        protected override string GetBorderPart(TableBorderPart part)
        {
            return part switch
            {
                TableBorderPart.HeaderTopLeft => "┏",
                TableBorderPart.HeaderTop => "━",
                TableBorderPart.HeaderTopSeparator => "┯",
                TableBorderPart.HeaderTopRight => "┓",
                TableBorderPart.HeaderLeft => "┃",
                TableBorderPart.HeaderSeparator => "│",
                TableBorderPart.HeaderRight => "┃",
                TableBorderPart.HeaderBottomLeft => "┠",
                TableBorderPart.HeaderBottom => "─",
                TableBorderPart.HeaderBottomSeparator => "┼",
                TableBorderPart.HeaderBottomRight => "┨",
                TableBorderPart.CellLeft => "┃",
                TableBorderPart.CellSeparator => "│",
                TableBorderPart.CellRight => "┃",
                TableBorderPart.FooterBottomLeft => "┗",
                TableBorderPart.FooterBottom => "━",
                TableBorderPart.FooterBottomSeparator => "┷",
                TableBorderPart.FooterBottomRight => "┛",
                _ => throw new InvalidOperationException("Unknown border part."),
            };
        }
    }
}
