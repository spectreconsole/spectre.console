namespace Spectre.Console.Testing;

/// <summary>
/// Represents the configuration settings for the <see cref="CommandAppTester"/> class.
/// </summary>
public sealed class CommandAppTesterSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether whitespace should be trimmed from the console output.
    /// </summary>
    /// <remarks>
    /// When enabled, leading and trailing whitespace from the console output and trailing whitespace from each line will be trimmed.
    /// </remarks>
    public bool TrimConsoleOutput { get; set; } = true;
}