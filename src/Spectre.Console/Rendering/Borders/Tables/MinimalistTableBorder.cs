namespace Spectre.Console.Rendering;

/// <summary>
/// Represents a minimalist border.
/// </summary>
public sealed class MinimalistTableBorder : TableBorder
{
    /// <inheritdoc/>
    public override bool SupportsRowSeparator => false;

    /// <inheritdoc/>
    public override bool UsePadding => false;

    /// <inheritdoc/>
    public override string GetPart(TableBorderPart part)
    {
        return part switch
        {
            TableBorderPart.HeaderTopLeft => string.Empty,
            TableBorderPart.HeaderTop => string.Empty,
            TableBorderPart.HeaderTopSeparator => string.Empty,
            TableBorderPart.HeaderTopRight => string.Empty,
            TableBorderPart.HeaderLeft => string.Empty,
            TableBorderPart.HeaderSeparator => " ",
            TableBorderPart.HeaderRight => string.Empty,
            TableBorderPart.HeaderBottomLeft => string.Empty,
            TableBorderPart.HeaderBottom => "─",
            TableBorderPart.HeaderBottomSeparator => "─",
            TableBorderPart.HeaderBottomRight => string.Empty,
            TableBorderPart.CellLeft => string.Empty,
            TableBorderPart.CellSeparator => " ",
            TableBorderPart.CellRight => string.Empty,
            TableBorderPart.FooterTopLeft => string.Empty,
            TableBorderPart.FooterTop => "─",
            TableBorderPart.FooterTopSeparator => "─",
            TableBorderPart.FooterTopRight => string.Empty,
            TableBorderPart.FooterBottomLeft => string.Empty,
            TableBorderPart.FooterBottom => string.Empty,
            TableBorderPart.FooterBottomSeparator => string.Empty,
            TableBorderPart.FooterBottomRight => string.Empty,
            TableBorderPart.RowLeft => string.Empty,
            TableBorderPart.RowCenter => string.Empty,
            TableBorderPart.RowSeparator => string.Empty,
            TableBorderPart.RowRight => string.Empty,
            _ => throw new InvalidOperationException("Unknown border part."),
        };
    }

    /// <inheritdoc/>
    public override string GetColumnRow(TablePart part, IReadOnlyList<int> widths, IReadOnlyList<IColumn> columns)
    {
        ArgumentNullException.ThrowIfNull(widths);
        ArgumentNullException.ThrowIfNull(columns);

        var (_, center, separator, _) = GetTableParts(part);

        var builder = new StringBuilder();

        foreach (var (columnIndex, _, lastColumn, columnWidth) in widths.Enumerate())
        {
            builder.Append(center.Repeat(columnWidth));

            if (!lastColumn)
            {
                builder.Append(separator);
            }
        }

        return builder.ToString();
    }
}
