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
        var style = percentage == 100 ? CompletedStyle : Style;
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
    /// <summary>
    /// Sets the style for a non-complete task.
    /// </summary>
    /// <param name="column">The column.</param>
    /// <param name="style">The style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static PercentageColumn Style(this PercentageColumn column, Style style)
    {
        ArgumentNullException.ThrowIfNull(column);

        column.Style = style;
        return column;
    }

    /// <summary>
    /// Sets the style for a completed task.
    /// </summary>
    /// <param name="column">The column.</param>
    /// <param name="style">The style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static PercentageColumn CompletedStyle(this PercentageColumn column, Style style)
    {
        ArgumentNullException.ThrowIfNull(column);

        column.CompletedStyle = style;
        return column;
    }
}