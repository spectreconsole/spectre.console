namespace Spectre.Console;

/// <summary>
/// A column showing task progress in percentage.
/// </summary>
public sealed class PercentageColumn : ProgressColumn
{
    /// <summary>
    /// Gets or sets the style for a non-complete task.
    /// </summary>
    public Style Style { get; set; } = Style.Plain;

    /// <summary>
    /// Gets or sets the style for a completed task.
    /// </summary>
    public Style CompletedStyle { get; set; } = Color.Green;

    /// <inheritdoc/>
    public override IRenderable Render(RenderOptions options, ProgressTask task, TimeSpan deltaTime)
    {
        var percentage = (int)task.Percentage;
        var style = percentage == 100 ? CompletedStyle : Style ?? Style.Plain;
        return new Text($"{percentage}%", style).RightJustified();
    }

    /// <inheritdoc/>
    public override int? GetColumnWidth(RenderOptions options)
    {
        return 4;
    }
}

/// <summary>
/// Contains extension methods for <see cref="PercentageColumn"/>.
/// </summary>
public static class PercentageColumnExtensions
{
    /// <param name="column">The column.</param>
    extension(PercentageColumn column)
    {
        /// <summary>
        /// Sets the style for a non-complete task.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public PercentageColumn Style(Style style)
        {
            ArgumentNullException.ThrowIfNull(column);

            ArgumentNullException.ThrowIfNull(style);

            column.Style = style;
            return column;
        }

        /// <summary>
        /// Sets the style for a completed task.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public PercentageColumn CompletedStyle(Style style)
        {
            ArgumentNullException.ThrowIfNull(column);

            ArgumentNullException.ThrowIfNull(style);

            column.CompletedStyle = style;
            return column;
        }
    }
}