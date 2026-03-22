namespace Spectre.Console.Ansi;

/// <summary>
/// Represents an OSC command.
/// </summary>
public record OscCommand
{
    /// <summary>
    /// Represents a hyperlink start.
    /// </summary>
    /// <param name="Id">The hyperlink ID.</param>
    /// <param name="Uri">The hyperlink URI.</param>
    public record HyperLinkStart(string? Id, string Uri) : OscCommand;

    /// <summary>
    /// Represents a hyperlink end.
    /// </summary>
    public record HyperLinkEnd : OscCommand;
}