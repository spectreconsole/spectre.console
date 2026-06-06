namespace Spectre.Console;

/// <summary>
/// Represents the different Figlet layout modes.
/// </summary>
public enum FigletLayoutMode
{
    /// <summary>
    /// Represents each Figlet character occupying the full width or
    /// height of its arrangement of sub-characters as designed.
    /// </summary>
    FullSize = 0,

    /// <summary>
    /// Characters are rendered touching each other by removing
    /// trailing and leading spaces at character boundaries (kerning).
    /// </summary>
    Fitted = 1,

    /// <summary>
    /// Boundary characters are merged according to the standard Figlet
    /// smushing rules. Smushing implies fitting.
    /// </summary>
    Smushed = 2,
}