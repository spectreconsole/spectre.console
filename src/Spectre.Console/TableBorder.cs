using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Spectre.Console.Internal;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a border.
    /// </summary>
    public abstract partial class TableBorder
    {
        private readonly Dictionary<TableBorderPart, string> _lookup;

        /// <summary>
        /// Gets a value indicating whether or not the border is visible.
        /// </summary>
        public virtual bool Visible { get; } = true;

        /// <summary>
        /// Gets the safe border for this border or <c>null</c> if none exist.
        /// </summary>
        public virtual TableBorder? SafeBorder { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableBorder"/> class.
        /// </summary>
        protected TableBorder()
        {
            _lookup = Initialize();
        }

        private Dictionary<TableBorderPart, string> Initialize()
        {
            var lookup = new Dictionary<TableBorderPart, string>();
            foreach (TableBorderPart? part in Enum.GetValues(typeof(TableBorderPart)))
            {
                if (part == null)
                {
                    continue;
                }

                var text = GetBorderPart(part.Value);
                if (text.Length > 1)
                {
                    throw new InvalidOperationException("A box part cannot contain more than one character.");
                }

                lookup.Add(part.Value, GetBorderPart(part.Value));
            }

            return lookup;
        }

        /// <summary>
        /// Gets the string representation of a specific border part.
        /// </summary>
        /// <param name="part">The part to get a string representation for.</param>
        /// <param name="count">The number of repetitions.</param>
        /// <returns>A string representation of the specified border part.</returns>
        public string GetPart(TableBorderPart part, int count)
        {
            // TODO: This need some optimization...
            return string.Join(string.Empty, Enumerable.Repeat(GetBorderPart(part)[0], count));
        }

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
                var centerWidth = padding.Left + columnWidth + padding.Right;
                builder.Append(center.Multiply(centerWidth));

                if (!lastColumn)
                {
                    builder.Append(separator);
                }
            }

            builder.Append(right);
            return builder.ToString();
        }

        /// <summary>
        /// Gets the string representation of a specific border part.
        /// </summary>
        /// <param name="part">The part to get a string representation for.</param>
        /// <returns>A string representation of the specified border part.</returns>
        public string GetPart(TableBorderPart part)
        {
            return _lookup[part].ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the character representing the specified border part.
        /// </summary>
        /// <param name="part">The part to get the character representation for.</param>
        /// <returns>A character representation of the specified border part.</returns>
        protected abstract string GetBorderPart(TableBorderPart part);

        /// <summary>
        /// Gets the table parts used to render a specific table row.
        /// </summary>
        /// <param name="part">The table part.</param>
        /// <returns>The table parts used to render the specific table row.</returns>
        protected (string Left, string Center, string Separator, string Right)
            GetTableParts(TablePart part)
        {
            return part switch
            {
                // Top part
                TablePart.Top =>
                    (GetPart(TableBorderPart.HeaderTopLeft), GetPart(TableBorderPart.HeaderTop),
                    GetPart(TableBorderPart.HeaderTopSeparator), GetPart(TableBorderPart.HeaderTopRight)),

                // Separator between header and cells
                TablePart.Separator =>
                    (GetPart(TableBorderPart.HeaderBottomLeft), GetPart(TableBorderPart.HeaderBottom),
                    GetPart(TableBorderPart.HeaderBottomSeparator), GetPart(TableBorderPart.HeaderBottomRight)),

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
