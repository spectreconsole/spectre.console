namespace Spectre.Console;

/// <summary>
/// Represents settings for <see cref="AnsiWriter"/>.
/// </summary>
public sealed class AnsiWriterSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether or
    /// not ANSI escape sequences are supported.
    /// </summary>
    /// <remarks>Defaults to <see cref="AnsiSupport.Detect"/></remarks>
    public AnsiSupport Ansi { get; init; } = AnsiSupport.Detect;

    /// <summary>
    /// Gets or sets the color system to use.
    /// </summary>
    /// <remarks>Defaults to <see cref="ColorSystemSupport.Detect"/></remarks>
    public ColorSystemSupport ColorSystem { get; init; } = ColorSystemSupport.Detect;
}