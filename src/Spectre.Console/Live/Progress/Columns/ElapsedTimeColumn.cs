namespace Spectre.Console;

/// <summary>
/// A column showing the elapsed time of a task.
/// </summary>
public sealed class ElapsedTimeColumn : ProgressColumn
{
    /// <inheritdoc/>
    protected internal override bool NoWrap => true;

    /// <summary>
    /// Gets or sets the style of the elapsed time text.
    /// </summary>
    public Style Style { get; set; } = Color.Blue;

    /// <summary>
    /// Gets or sets the format of the elapsed time text.
    /// </summary>
    public string? Format { get; set; } = @"hh\:mm\:ss";

    /// <inheritdoc/>
    public override IRenderable Render(RenderOptions options, ProgressTask task, TimeSpan deltaTime)
    {
        if (task.ElapsedTime is not TimeSpan elapsed)
        {
            return new Markup("--:--:--");
        }

        if (elapsed.TotalHours > 99)
        {
            return new Markup("**:**:**");
        }

        return new Text($"{elapsed.ToString(Format)}", Style ?? Style.Plain);
    }
}
