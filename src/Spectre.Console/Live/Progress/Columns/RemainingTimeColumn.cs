namespace Spectre.Console;

/// <summary>
/// A column showing the remaining time of a task.
/// </summary>
public sealed class RemainingTimeColumn : ProgressColumn
{
    /// <inheritdoc/>
    protected internal override bool NoWrap => true;

    /// <summary>
    /// Gets or sets the style of the remaining time text.
    /// </summary>
    public Style Style { get; set; } = Color.Blue;

    /// <inheritdoc/>
    public override IRenderable Render(RenderOptions options, ProgressTask task, TimeSpan deltaTime)
    {
        var remaining = task.RemainingTime;
        if (remaining == null)
        {
            return new Markup("--:--:--");
        }

        if (remaining.Value.TotalHours > 99)
        {
            return new Markup("**:**:**");
        }

        return new Text($"{remaining.Value:hh\\:mm\\:ss}", Style ?? Style.Plain);
    }

    /// <inheritdoc/>
    public override int? GetColumnWidth(RenderOptions options)
    {
        return 8;
    }
}

/// <summary>
/// Contains extension methods for <see cref="RemainingTimeColumn"/>.
/// </summary>
public static class RemainingTimeColumnExtensions
{
    /// <param name="column">The column.</param>
    extension(RemainingTimeColumn column)
    {
        /// <summary>
        /// Sets the style of the remaining time text.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public RemainingTimeColumn Style(Style style)
        {
            ArgumentNullException.ThrowIfNull(column);

            ArgumentNullException.ThrowIfNull(style);

            column.Style = style;
            return column;
        }
    }
}