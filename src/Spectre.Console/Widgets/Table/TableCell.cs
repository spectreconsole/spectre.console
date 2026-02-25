namespace Spectre.Console;

/// <summary>
/// Represents a table cell that can span multiple columns.
/// </summary>
public sealed class TableCell : IRenderable
{
    /// <summary>
    /// Gets the cell content.
    /// </summary>
    public IRenderable Content { get; }

    /// <summary>
    /// Gets the number of columns this cell spans.
    /// </summary>
    public int ColumnSpan { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TableCell"/> class.
    /// </summary>
    /// <param name="content">The cell content.</param>
    public TableCell(IRenderable content)
    {
        Content = content ?? throw new ArgumentNullException(nameof(content));
        ColumnSpan = 1;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TableCell"/> class.
    /// </summary>
    /// <param name="markup">Markup text.</param>
    public TableCell(string markup)
        : this(new Markup(markup ?? string.Empty))
    {
    }

    /// <summary>
    /// Sets the number of columns this cell should span.
    /// </summary>
    /// <param name="span">The number of columns to span.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public TableCell Span(int span)
    {
        if (span < 1)
        {
            throw new ArgumentException("Column span must be at least 1.", nameof(span));
        }

        ColumnSpan = span;
        return this;
    }

    /// <summary>
    /// Implicitly converts a <see cref="string"/> to a <see cref="TableCell"/>.
    /// </summary>
    /// <param name="markup">The markup text to convert.</param>
    public static implicit operator TableCell(string markup)
    {
        return new TableCell(markup);
    }

    /// <inheritdoc/>
    Measurement IRenderable.Measure(RenderOptions options, int maxWidth)
    {
        return Content.Measure(options, maxWidth);
    }

    /// <inheritdoc/>
    IEnumerable<Segment> IRenderable.Render(RenderOptions options, int maxWidth)
    {
        return Content.Render(options, maxWidth);
    }
}
