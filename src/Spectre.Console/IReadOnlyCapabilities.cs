namespace Spectre.Console;

/// <summary>
/// Represents (read-only) terminal capabilities.
/// </summary>
public interface IReadOnlyCapabilities : IReadOnlyAnsiCapabilities
{
    /// <summary>
    /// Gets a value indicating whether or not
    /// this is a legacy console (cmd.exe) on an OS
    /// prior to Windows 10.
    /// </summary>
    /// <remarks>
    /// Only relevant when running on Microsoft Windows.
    /// </remarks>
    [Obsolete("This property will be removed in a future version")]
    bool Legacy { get; }

    /// <summary>
    /// Gets a value indicating whether
    /// or not the console supports interaction.
    /// </summary>
    bool Interactive { get; }

    /// <summary>
    /// Gets a value indicating whether
    /// or not the console supports Unicode.
    /// </summary>
    bool Unicode { get; }
}