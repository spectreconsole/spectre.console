namespace Spectre.Console.Ansi;

/// <summary>
/// Represents the kind of stream an <see cref="AnsiParser"/> reads.
/// </summary>
[Experimental(Constants.ExperimentalParserId)]
public enum AnsiParserMode
{
    /// <summary>
    /// What an application writes to a terminal,
    /// parsed as a VT500-series terminal does.
    /// </summary>
    Output = 0,

    /// <summary>
    /// What a terminal sends to an application:
    /// keys, mouse reports and replies to queries.
    /// </summary>
    Input = 1,
}