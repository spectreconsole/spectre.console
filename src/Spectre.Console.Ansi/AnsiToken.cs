namespace Spectre.Console.Ansi;

/// <summary>
/// Represents a parsed ANSI/VT token.
/// </summary>
public abstract record AnsiToken
{
    /// <summary>
    /// Prints a (Unicode codepoint) character to the screen.
    /// </summary>
    /// <param name="Character">The character to print.</param>
    public record Print(char Character) : AnsiToken;

    /// <summary>
    /// Executes the specified function.
    /// </summary>
    /// <param name="Function">The C0/C1 function to execute.</param>
    public record Execute(char Function) : AnsiToken;

    /// <summary>
    /// Execute an ESC command.
    /// </summary>
    /// <param name="Intermediates">Intermediate bytes.</param>
    /// <param name="Final">The final byte identifying the ESC command.</param>
    public record Esc(List<char> Intermediates, char Final) : AnsiToken;

    /// <summary>
    /// Executes a CSI command.
    /// </summary>
    /// <param name="Intermediates">Intermediate bytes.</param>
    /// <param name="Params">The parameters.</param>
    /// <param name="Final">The final byte identifying the CSI command.</param>
    /// <param name="ParamsRaw">The raw parameters.</param>
    public record Csi(List<char> Intermediates, List<int> Params, char Final, string ParamsRaw) : AnsiToken;

    /// <summary>
    /// Executes an OSC command.
    /// </summary>
    /// <param name="Command">The command to execute.</param>
    public record Osc(OscCommand Command) : AnsiToken;

    /// <summary>
    /// Sets up a DCS handler.
    /// </summary>
    /// <param name="Intermediates">Intermediate bytes.</param>
    /// <param name="Params">The parameters.</param>
    /// <param name="Final">The final byte identifying the DCS command.</param>
    /// <param name="ParamsRaw">The raw parameters.</param>
    public record DcsHook(List<char> Intermediates, List<int> Params, char Final, string ParamsRaw) : AnsiToken;

    /// <summary>
    /// Puts a byte into the selected handler.
    /// </summary>
    /// <param name="Code">The byte to put.</param>
    public record DcsPut(char Code) : AnsiToken;

    /// <summary>
    /// Unselects the current specified DCS handler.
    /// </summary>
    public record DcsUnhook : AnsiToken;
}