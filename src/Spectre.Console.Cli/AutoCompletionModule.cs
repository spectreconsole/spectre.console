namespace Spectre.Console.Cli;

/// <summary>
/// Defines the modules used for auto completion.
/// </summary>
[Flags]
public enum AutoCompletionModule
{
    /// <summary>
    /// Represents a state where no auto completion module is enabled.
    /// </summary>
    None = 0,

    /// <summary>
    /// Represents the core auto completion module. This is the basic auto completion functionality.
    /// </summary>
    Base = 1 << 0,

    /// <summary>
    /// Represents the Powershell auto completion module. This module provides auto completion features specific to Powershell.
    /// </summary>
    Powershell = 1 << 1,

    /// <summary>
    /// Represents a state where all auto completion modules are enabled. This includes both the core auto completion and Powershell auto completion modules.
    /// </summary>
    All = Base | Powershell,
}
