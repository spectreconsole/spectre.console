namespace Spectre.Console.Cli;

/// <summary>
/// Provides an interface for configuring the settings related to auto-completion.
/// </summary>
public interface IAutoCompletionSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether or not to enable auto completion.
    /// </summary>
    bool IsEnabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not to use powershell completion.
    /// You can turn this off in case you want to use your own completion logic.
    /// </summary>
    bool IsPowershellEnabled { get; set; }
}