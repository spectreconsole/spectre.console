namespace Spectre.Console;

/// <summary>
/// Represents terminal capabilities.
/// </summary>
public sealed class Capabilities : AnsiCapabilities, IReadOnlyCapabilities
{
    /// <summary>
    /// Gets or sets a value indicating whether or not
    /// this is a legacy console (cmd.exe) on an OS
    /// prior to Windows 10.
    /// </summary>
    /// <remarks>
    /// Only relevant when running on Microsoft Windows.
    /// </remarks>
    [Obsolete("This property will be removed in a future version")]
    public bool Legacy { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether
    /// or not the console supports interaction.
    /// </summary>
    public bool Interactive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether
    /// or not the console supports Unicode.
    /// </summary>
    public bool Unicode { get; set; }

    /// <summary>
    /// Creates a <see cref="Capabilities"/> instance from the provided arguments.
    /// </summary>
    /// <param name="writer">The text writer to use.</param>
    /// <param name="settings">The settings to use.</param>
    /// <param name="encoding">The detected encoding.</param>
    /// <returns>A <see cref="Capabilities"/> instance.</returns>
    public static Capabilities Create(TextWriter writer, AnsiConsoleSettings settings, out Encoding encoding)
    {
        ArgumentNullException.ThrowIfNull(writer);

        var ansiCaps = AnsiCapabilities.Create(writer, new AnsiWriterSettings
        {
            Ansi = settings.Ansi,
            ColorSystem = settings.ColorSystem,
        });

        // Use console encoding or fall back to provided encoding
        encoding = writer.IsStandardOut() || writer.IsStandardError()
            ? System.Console.OutputEncoding : writer.Encoding;

        return new Capabilities
        {
            ColorSystem = ansiCaps.ColorSystem,
            Ansi = ansiCaps.Ansi,
            Links = ansiCaps.Links,
#pragma warning disable CS0618 // Type or member is obsolete
            Legacy = false,
#pragma warning restore CS0618 // Type or member is obsolete
            Interactive = InteractionDetector.IsInteractive(settings.Interactive),
            Unicode = encoding.EncodingName.ContainsExact("Unicode"),
            AlternateBuffer = ansiCaps.AlternateBuffer,
        };
    }
}