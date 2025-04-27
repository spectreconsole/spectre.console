namespace Spectre.Console;

/// <summary>
/// A column showing transfer speed.
/// </summary>
public sealed class TransferSpeedColumn : ProgressColumn
{
    /// <summary>
    /// Gets or sets the <see cref="CultureInfo"/> to use.
    /// </summary>
    public CultureInfo? Culture { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="FileSizeBase"/> to use.
    /// </summary>
    public FileSizeBase Base { get; set; } = FileSizeBase.Binary;

    /// <summary>
    /// Gets or sets a value indicating whether to display the transfer speed in bits.
    /// </summary>
    public bool ShowBits { get; set; }

    /// <inheritdoc/>
    public override IRenderable Render(RenderOptions options, ProgressTask task, TimeSpan deltaTime)
    {
        if (task.Speed == null)
        {
            return new Text("?/s");
        }

        if (task.IsFinished)
        {
            return new Markup(string.Empty, Style.Plain);
        }
        else
        {
            var size = new FileSize(task.Speed.Value, Base, ShowBits);
            return new Markup(string.Format("{0}/s", size.ToString(suffix: true, Culture)));
        }
    }
}