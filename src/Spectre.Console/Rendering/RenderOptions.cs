namespace Spectre.Console.Rendering;

/// <summary>
/// Represents render options.
/// </summary>
/// <param name="Capabilities">The capabilities.</param>
/// <param name="ConsoleSize">The console size.</param>
public record class RenderOptions(IReadOnlyCapabilities Capabilities, Size ConsoleSize)
{
    /// <summary>
    /// Gets the current color system.
    /// </summary>
    public ColorSystem ColorSystem => Capabilities.ColorSystem;

    /// <summary>
    /// Gets a value indicating whether or not VT/Ansi codes are supported.
    /// </summary>
    public bool Ansi => Capabilities.Ansi;

    /// <summary>
    /// Gets a value indicating whether or not unicode is supported.
    /// </summary>
    public bool Unicode => Capabilities.Unicode;

    /// <summary>
    /// Gets the current justification.
    /// </summary>
    public Justify? Justification { get; init; }

    /// <summary>
    /// Gets the requested height.
    /// </summary>
    public int? Height { get; init; }

    /// <summary>
    /// Gets a value indicating whether the context want items to render without
    /// line breaks and return a single line where applicable.
    /// </summary>
    internal bool SingleLine { get; init; }

    /// <summary>
    /// Creates a <see cref="RenderOptions"/> instance from a <see cref="IAnsiConsole"/>.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <param name="capabilities">The capabilities, or <c>null</c> to use the provided console's capabilities.</param>
    /// <returns>A <see cref="RenderOptions"/> representing the provided <see cref="IAnsiConsole"/>.</returns>
    public static RenderOptions Create(IAnsiConsole console, IReadOnlyCapabilities? capabilities = null)
    {
        if (console is null)
        {
            throw new ArgumentNullException(nameof(console));
        }

        return new RenderOptions(
            capabilities ?? console.Profile.Capabilities,
            new Size(console.Profile.Width, console.Profile.Height))
        {
            Justification = null,
            Height = null,
            SingleLine = false,
        };
    }
}
