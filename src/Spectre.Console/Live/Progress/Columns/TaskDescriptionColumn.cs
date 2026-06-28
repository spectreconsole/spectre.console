namespace Spectre.Console;

/// <summary>
/// A column showing the task description.
/// </summary>
public sealed class TaskDescriptionColumn : ProgressColumn
{
    /// <inheritdoc/>
    protected internal override bool NoWrap => !Wrap;

    /// <summary>
    /// Gets or sets a value indicating whether the description should wrap
    /// onto multiple lines when it's wider than the available space.
    /// Defaults to <c>false</c>, which truncates the description with an ellipsis.
    /// </summary>
    public bool Wrap { get; set; }

    /// <summary>
    /// Gets or sets the alignment of the task description.
    /// </summary>
    public Justify Alignment { get; set; } = Justify.Right;

    /// <inheritdoc/>
    public override IRenderable Render(RenderOptions options, ProgressTask task, TimeSpan deltaTime)
    {
        var text = task.Description?.RemoveNewLines()?.Trim();
        return new Markup(text ?? string.Empty)
            .Overflow(Wrap ? Overflow.Fold : Overflow.Ellipsis)
            .Justify(Alignment);
    }
}