namespace Spectre.Console;

/// <summary>
/// Represents ANSI capabilities.
/// </summary>
public class AnsiCapabilities : IReadOnlyAnsiCapabilities
{
    /// <summary>
    /// Gets or sets the color system.
    /// </summary>
    public ColorSystem ColorSystem { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not
    /// the console supports VT/ANSI control codes.
    /// </summary>
    public bool Ansi { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not
    /// the console support links.
    /// </summary>
    public bool Links { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether
    /// or not the console supports alternate buffers.
    /// </summary>
    public bool AlternateBuffer { get; set; }

    /// <summary>
    /// Creates a <see cref="AnsiCapabilities"/> instance from the provided arguments.
    /// </summary>
    /// <param name="writer">The text writer to use.</param>
    /// <returns>A <see cref="AnsiCapabilities"/> instance.</returns>
    public static AnsiCapabilities Create(TextWriter writer)
    {
        return Create(writer, new AnsiWriterSettings());
    }

    /// <summary>
    /// Creates a <see cref="AnsiCapabilities"/> instance from the provided arguments.
    /// </summary>
    /// <param name="writer">The text writer to use.</param>
    /// <param name="settings">The settings to use.</param>
    /// <returns>A <see cref="AnsiCapabilities"/> instance.</returns>
    public static AnsiCapabilities Create(TextWriter writer, AnsiWriterSettings settings)
    {
        ArgumentNullException.ThrowIfNull(writer);

        // Detect if the terminal support ANSI or not
        var (supportsAnsi, legacyConsole) = AnsiDetector.Detect(writer, settings.Ansi);

        // Get the color system
        var colorSystem = settings.ColorSystem == ColorSystemSupport.Detect
            ? ColorSystemDetector.Detect(supportsAnsi)
            : (ColorSystem)settings.ColorSystem;

        return new AnsiCapabilities
        {
            ColorSystem = colorSystem,
            Ansi = supportsAnsi,
            Links = supportsAnsi && !legacyConsole,
            AlternateBuffer = supportsAnsi && !legacyConsole,
        };
    }
}

/// <summary>
/// Represents read-only ANSI capabilities.
/// </summary>
public interface IReadOnlyAnsiCapabilities
{
    /// <summary>
    /// Gets or sets the color system.
    /// </summary>
    public ColorSystem ColorSystem { get; }

    /// <summary>
    /// Gets or sets a value indicating whether or not
    /// the console supports VT/ANSI control codes.
    /// </summary>
    public bool Ansi { get; }

    /// <summary>
    /// Gets or sets a value indicating whether or not
    /// the console support links.
    /// </summary>
    public bool Links { get; }

    /// <summary>
    /// Gets or sets a value indicating whether
    /// or not the console supports alternate buffers.
    /// </summary>
    public bool AlternateBuffer { get; }
}