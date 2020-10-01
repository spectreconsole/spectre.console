using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console.Internal;

namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Represents a Markdown border.
    /// </summary>
    public sealed class MarkdownTableBorder : TableBorder
    {
        /// <inheritdoc/>
        public override string GetPart(TableBorderPart part)
        {
            return part switch
            {
                TableBorderPart.HeaderTopLeft => " ",
                TableBorderPart.HeaderTop => " ",
                TableBorderPart.HeaderTopSeparator => " ",
                TableBorderPart.HeaderTopRight => " ",
                TableBorderPart.HeaderLeft => "|",
                TableBorderPart.HeaderSeparator => "|",
                TableBorderPart.HeaderRight => "|",
                TableBorderPart.HeaderBottomLeft => "|",
                TableBorderPart.HeaderBottom => "-",
                TableBorderPart.HeaderBottomSeparator => "|",
                TableBorderPart.HeaderBottomRight => "|",
                TableBorderPart.CellLeft => "|",
                TableBorderPart.CellSeparator => "|",
                TableBorderPart.CellRight => "|",
                TableBorderPart.FooterBottomLeft => " ",
                TableBorderPart.FooterBottom => " ",
                TableBorderPart.FooterBottomSeparator => " ",
                TableBorderPart.FooterBottomRight => " ",
                _ => throw new InvalidOperationException("Unknown border part."),
            };
        }

        /// <inheritdoc/>
        public override string GetColumnRow(TablePart part, IReadOnlyList<int> widths, IReadOnlyList<IColumn> columns)
        {
            if (part != TablePart.Separator)
            {
                return base.GetColumnRow(part, widths, columns);
            }

            var (left, center, separator, right) = GetTableParts(part);

            var builder = new StringBuilder();
            builder.Append(left);

            foreach (var (columnIndex, _, lastColumn, columnWidth) in widths.Enumerate())
            {
                var padding = columns[columnIndex].Padding;

                if (padding.Left > 0)
                {
                    // Left padding
                    builder.Append(" ".Repeat(padding.Left));
                }

                var justification = columns[columnIndex].Alignment;
                if (justification == null)
                {
                    // No alignment
                    builder.Append(center.Repeat(columnWidth));
                }
                else if (justification.Value == Justify.Left)
                {
                    // Left
                    builder.Append(':');
                    builder.Append(center.Repeat(columnWidth - 1));
                }
                else if (justification.Value == Justify.Center)
                {
                    // Centered
                    builder.Append(':');
                    builder.Append(center.Repeat(columnWidth - 2));
                    builder.Append(':');
                }
                else if (justification.Value == Justify.Right)
                {
                    // Right
                    builder.Append(center.Repeat(columnWidth - 1));
                    builder.Append(':');
                }

                // Right padding
                if (padding.Right > 0)
                {
                    builder.Append(" ".Repeat(padding.Right));
                }

                if (!lastColumn)
                {
                    builder.Append(separator);
                }
            }

            builder.Append(right);
            return builder.ToString();
        }
    }
}
