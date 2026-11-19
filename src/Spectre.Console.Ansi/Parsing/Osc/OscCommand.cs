namespace Spectre.Console.Ansi;

/// <summary>
/// Represents an OSC command.
/// </summary>
[Experimental(Constants.ExperimentalParserId)]
public abstract record OscCommand
{
    // Only the nested command types derive from this
    private OscCommand()
    {
    }

    /// <summary>
    /// Represents a hyperlink start.
    /// </summary>
    /// <param name="Id">The hyperlink ID.</param>
    /// <param name="Uri">The hyperlink URI.</param>
    public sealed record HyperLinkStart(string? Id, string Uri) : OscCommand;

    /// <summary>
    /// Represents a hyperlink end.
    /// </summary>
    public sealed record HyperLinkEnd : OscCommand;

    /// <summary>
    /// Represents an unknown OSC command.
    /// </summary>
    /// <param name="Data">The OSC command data.</param>
    public sealed record Unknown(string Data) : OscCommand;
}