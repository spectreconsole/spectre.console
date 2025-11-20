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

    /// <summary>
    /// Gets or sets the format of the remaining time text.
    /// </summary>
    public string? Format { get; set; } = @"hh\:mm\:ss";

    /// <inheritdoc/>
    public override IRenderable Render(RenderOptions options, ProgressTask task, TimeSpan deltaTime)
    {
        if (task.RemainingTime is not TimeSpan remaining)
        {
            return new Markup("--:--:--");
        }

        if (remaining.TotalHours > 99)
        {
            return new Markup("**:**:**");
        }

        return new Text($"{remaining.ToString(Format)}", Style ?? Style.Plain);
    }
}
