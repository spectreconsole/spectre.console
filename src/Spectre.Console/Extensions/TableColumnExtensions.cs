namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="TableColumn"/>.
/// </summary>
public static class TableColumnExtensions
{
    /// <summary>
    /// Sets the table column header.
    /// </summary>
    /// <param name="column">The table column.</param>
    /// <param name="header">The table column header markup text.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TableColumn Header(this TableColumn column, string header)
    {
        if (column is null)
        {
            throw new ArgumentNullException(nameof(column));
        }

        if (header is null)
        {
            throw new ArgumentNullException(nameof(header));
        }

        column.Header = new Markup(header);
        return column;
    }

    /// <summary>
    /// Sets the table column header.
    /// </summary>
    /// <param name="column">The table column.</param>
    /// <param name="header">The table column header.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TableColumn Header(this TableColumn column, IRenderable header)
    {
        if (column is null)
        {
            throw new ArgumentNullException(nameof(column));
        }

        if (header is null)
        {
            throw new ArgumentNullException(nameof(header));
        }

        column.Footer = header;
        return column;
    }

    /// <summary>
    /// Sets the table column footer.
    /// </summary>
    /// <param name="column">The table column.</param>
    /// <param name="footer">The table column footer markup text.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TableColumn Footer(this TableColumn column, string footer)
    {
        if (column is null)
        {
            throw new ArgumentNullException(nameof(column));
        }

        if (footer is null)
        {
            throw new ArgumentNullException(nameof(footer));
        }

        column.Footer = new Markup(footer);
        return column;
    }

    /// <summary>
    /// Sets the table column footer.
    /// </summary>
    /// <param name="column">The table column.</param>
    /// <param name="footer">The table column footer.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TableColumn Footer(this TableColumn column, IRenderable footer)
    {
        if (column is null)
        {
            throw new ArgumentNullException(nameof(column));
        }

        if (footer is null)
        {
            throw new ArgumentNullException(nameof(footer));
        }

        column.Footer = footer;
        return column;
    }
}