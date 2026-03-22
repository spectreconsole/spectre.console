namespace Spectre.Console.Ansi;

/// <summary>
/// Represents a parsed ANSI/VT token.
/// </summary>
public abstract record AnsiToken
{
    /// <summary>
    /// Prints a (Unicode codepoint) character to the screen.
    /// </summary>
    /// <param name="Code"></param>
    public record Print(char Code) : AnsiToken;

    /// <summary>
    /// Executes the specified function.
    /// </summary>
    /// <param name="Code"></param>
    public record Execute(char Code) : AnsiToken;

    /// <summary>
    /// Execute an ESC command.
    /// </summary>
    /// <param name="Collect"></param>
    /// <param name="Final"></param>
    public record Esc(List<char> Collect, char Final) : AnsiToken;

    /// <summary>
    /// Executes a CSI command.
    /// </summary>
    /// <param name="Collect"></param>
    /// <param name="Params"></param>
    /// <param name="Final"></param>
    /// <param name="ParamsRaw"></param>
    public record Csi(List<char> Collect, List<int> Params, char Final, string ParamsRaw) : AnsiToken;

    /// <summary>
    /// Executes an OSC command.
    /// </summary>
    /// <param name="Command">The command to execute.</param>
    public record Osc(OscCommand Command) : AnsiToken;

    /// <summary>
    /// Sets up a DCS handler.
    /// </summary>
    /// <param name="Collect"></param>
    /// <param name="Params"></param>
    /// <param name="Final"></param>
    /// <param name="ParamsRaw"></param>
    public record DcsHook(List<char> Collect, List<int> Params, char Final, string ParamsRaw) : AnsiToken;

    /// <summary>
    /// Passes a device control string (DCS) to the selected handler.
    /// </summary>
    /// <param name="Code"></param>
    public record DcsPut(char Code) : AnsiToken;

    /// <summary>
    /// Unselects the current specified DCS handler.
    /// </summary>
    public record DcsUnhook() : AnsiToken;

    /// <summary>
    /// Begins an APC sequence.
    /// </summary>
    public record ApcStart : AnsiToken;

    /// <summary>
    /// Appends the specified code to the current APC sequence.
    /// </summary>
    /// <param name="Code"></param>
    public record ApcPut(char Code) : AnsiToken;

    /// <summary>
    /// Ends an APC sequence.
    /// </summary>
    public record ApcEnd() : AnsiToken;
}