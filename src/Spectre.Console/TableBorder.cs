using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a border.
    /// </summary>
    public abstract partial class TableBorder
    {
        /// <summary>
        /// Gets a value indicating whether or not the border is visible.
        /// </summary>
        public virtual bool Visible { get; } = true;

        /// <summary>
        /// Gets the safe border for this border or <c>null</c> if none exist.
        /// </summary>
        public virtual TableBorder? SafeBorder { get; }

        /// <summary>
        /// Gets the string representation of a specified table border part.
        /// </summary>
        /// <param name="part">The part to get the character representation for.</param>
        /// <returns>A character representation of the specified border part.</returns>
        public abstract string GetPart(TableBorderPart part);

        /// <summary>
        /// Gets a whole column row for the specific column row part.
        /// </summary>
        /// <param name="part">The column row part.</param>
        /// <param name="widths">The column widths.</param>
        /// <param name="columns">The columns.</param>
        /// <returns>A string representing the column row.</returns>
        public virtual string GetColumnRow(TablePart part, IReadOnlyList<int> widths, IReadOnlyList<IColumn> columns)
        {
            if (widths is null)
            {
                throw new ArgumentNullException(nameof(widths));
            }

            if (columns is null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            var (left, center, separator, right) = GetTableParts(part);

            var builder = new StringBuilder();
            builder.Append(left);

            foreach (var (columnIndex, _, lastColumn, columnWidth) in widths.Enumerate())
            {
                var padding = columns[columnIndex].Padding;
                var centerWidth = padding.GetLeftSafe() + columnWidth + padding.GetRightSafe();
                builder.Append(center.Repeat(centerWidth));

                if (!lastColumn)
                {
                    builder.Append(separator);
                }
            }

            builder.Append(right);
            return builder.ToString();
        }

        /// <summary>
        /// Gets the table parts used to render a specific table row.
        /// </summary>
        /// <param name="part">The table part.</param>
        /// <returns>The table parts used to render the specific table row.</returns>
        protected (string Left, string Center, string Separator, string Right) GetTableParts(TablePart part)
        {
            return part switch
            {
                // Top part
                TablePart.Top =>
                    (GetPart(TableBorderPart.HeaderTopLeft), GetPart(TableBorderPart.HeaderTop),
                    GetPart(TableBorderPart.HeaderTopSeparator), GetPart(TableBorderPart.HeaderTopRight)),

                // Separator between header and cells
                TablePart.HeaderSeparator =>
                    (GetPart(TableBorderPart.HeaderBottomLeft), GetPart(TableBorderPart.HeaderBottom),
                    GetPart(TableBorderPart.HeaderBottomSeparator), GetPart(TableBorderPart.HeaderBottomRight)),

                // Separator between footer and cells
                TablePart.FooterSeparator =>
                    (GetPart(TableBorderPart.FooterTopLeft), GetPart(TableBorderPart.FooterTop),
                    GetPart(TableBorderPart.FooterTopSeparator), GetPart(TableBorderPart.FooterTopRight)),

                // Bottom part
                TablePart.Bottom =>
                    (GetPart(TableBorderPart.FooterBottomLeft), GetPart(TableBorderPart.FooterBottom),
                    GetPart(TableBorderPart.FooterBottomSeparator), GetPart(TableBorderPart.FooterBottomRight)),

                // Unknown
                _ => throw new NotSupportedException("Unknown column row part"),
            };
        }
    }
}
