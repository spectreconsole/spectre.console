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
    /// Executes the C0 or C1 function.
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

    public record Osc(OscCommand command) : AnsiToken;

    public record OscOld(char Code, List<char> Data) : AnsiToken;

    public record DcsHook(List<char> Collect, List<int> Params, char Final, string ParamsRaw) : AnsiToken;

    public record DcsPut(char Code) : AnsiToken;

    public record DcsUnhook() : AnsiToken;

    public record ApcStart : AnsiToken;

    public record ApcEnd() : AnsiToken;

    public record ApcPut(char Code) : AnsiToken;
}