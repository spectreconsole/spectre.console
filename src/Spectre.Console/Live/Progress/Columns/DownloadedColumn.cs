namespace Spectre.Console;

/// <summary>
/// A column showing download progress.
/// </summary>
public sealed class DownloadedColumn : ProgressColumn
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
    public bool DisplayBits { get; set; }

    /// <inheritdoc/>
    public override IRenderable Render(RenderOptions options, ProgressTask task, TimeSpan deltaTime)
    {
        var total = new FileSize(task.MaxValue, Base, DisplayBits);

        if (task.IsFinished)
        {
            return new Markup(string.Format(
                "[green]{0} {1}[/]",
                total.Format(Culture),
                total.Suffix));
        }
        else
        {
            var downloaded = new FileSize(task.Value, total.Prefix);

            return new Markup(string.Format(
                "{0}[grey]/[/]{1} [grey]{2}[/]",
                downloaded.Format(Culture),
                total.Format(Culture),
                total.Suffix));
        }
    }
}