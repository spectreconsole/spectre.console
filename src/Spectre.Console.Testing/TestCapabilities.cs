namespace Spectre.Console.Testing;

/// <summary>
/// Represents fake capabilities useful in tests.
/// </summary>
public sealed class TestCapabilities : IReadOnlyCapabilities
{
    /// <inheritdoc/>
    public ColorSystem ColorSystem { get; set; } = ColorSystem.TrueColor;

    /// <inheritdoc/>
    public bool Ansi { get; set; }

    /// <inheritdoc/>
    public bool Links { get; set; }

    /// <inheritdoc/>
    public bool Legacy { get; set; }

    /// <inheritdoc/>
    public bool Interactive { get; set; }

    /// <inheritdoc/>
    public bool Unicode { get; set; }

    /// <inheritdoc/>
    public bool AlternateBuffer { get; set; }

    /// <summary>
    /// Creates a <see cref="RenderOptions"/> with the same capabilities as this instance.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <returns>A <see cref="RenderOptions"/> with the same capabilities as this instance.</returns>
    public RenderOptions CreateRenderContext(IAnsiConsole console)
    {
        ArgumentNullException.ThrowIfNull(console);

        return RenderOptions.Create(console, this);
    }
}