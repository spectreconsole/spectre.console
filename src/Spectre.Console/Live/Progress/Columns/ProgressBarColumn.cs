namespace Spectre.Console;

/// <summary>
/// A column showing task progress as a progress bar.
/// </summary>
public sealed class ProgressBarColumn : ProgressColumn
{
    /// <summary>
    /// Gets or sets the width of the column.
    /// </summary>
    public int? Width { get; set; } = 40;

    /// <summary>
    /// Gets or sets the style of completed portions of the progress bar.
    /// </summary>
    public Style CompletedStyle { get; set; } = Color.Yellow;

    /// <summary>
    /// Gets or sets the style of a finished progress bar.
    /// </summary>
    public Style FinishedStyle { get; set; } = Color.Green;

    /// <summary>
    /// Gets or sets the style of remaining portions of the progress bar.
    /// </summary>
    public Style RemainingStyle { get; set; } = Color.Grey;

    /// <summary>
    /// Gets or sets the style of an indeterminate progress bar.
    /// </summary>
    public Style IndeterminateStyle { get; set; } = ProgressBar.DefaultPulseStyle;

    /// <inheritdoc/>
    public override IRenderable Render(RenderOptions options, ProgressTask task, TimeSpan deltaTime)
    {
        return new ProgressBar
        {
            MaxValue = task.MaxValue,
            Value = task.Value,
            Width = Width,
            CompletedStyle = CompletedStyle,
            FinishedStyle = FinishedStyle,
            RemainingStyle = RemainingStyle,
            IndeterminateStyle = IndeterminateStyle,
            IsIndeterminate = task.IsIndeterminate,
        };
    }
}

/// <summary>
/// Contains extension methods for <see cref="ProgressBarColumn"/>.
/// </summary>
public static class ProgressBarColumnExtensions
{
    /// <summary>
    /// Sets the style of completed portions of the progress bar.
    /// </summary>
    /// <param name="column">The column.</param>
    /// <param name="style">The style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static ProgressBarColumn CompletedStyle(this ProgressBarColumn column, Style style)
    {
        ArgumentNullException.ThrowIfNull(column);

        column.CompletedStyle = style;
        return column;
    }

    /// <summary>
    /// Sets the style of a finished progress bar.
    /// </summary>
    /// <param name="column">The column.</param>
    /// <param name="style">The style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static ProgressBarColumn FinishedStyle(this ProgressBarColumn column, Style style)
    {
        ArgumentNullException.ThrowIfNull(column);

        column.FinishedStyle = style;
        return column;
    }

    /// <summary>
    /// Sets the style of remaining portions of the progress bar.
    /// </summary>
    /// <param name="column">The column.</param>
    /// <param name="style">The style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static ProgressBarColumn RemainingStyle(this ProgressBarColumn column, Style style)
    {
        ArgumentNullException.ThrowIfNull(column);

        column.RemainingStyle = style;
        return column;
    }
}