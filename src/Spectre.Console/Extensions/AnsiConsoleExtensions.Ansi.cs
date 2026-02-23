namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IAnsiConsole"/>.
/// </summary>
public static partial class AnsiConsoleExtensions
{
    /// <summary>
    /// Writes a VT/Ansi control code sequence to the console (if supported).
    /// </summary>
    /// <param name="console">The console to write to.</param>
    /// <param name="sequence">The VT/Ansi control code sequence to write.</param>
    public static void WriteAnsi(this IAnsiConsole console, string sequence)
    {
        ArgumentNullException.ThrowIfNull(console);

        if (console.Profile.Capabilities.Ansi)
        {
            console.Write(new ControlCode(sequence));
        }
    }

    /// <summary>
    /// Gets the VT/ANSI control code sequence for a <see cref="IRenderable"/>.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <param name="renderable">The renderable to the VT/ANSI control code sequence for.</param>
    /// <returns>The VT/ANSI control code sequence.</returns>
    public static string ToAnsi(this IAnsiConsole console, IRenderable renderable)
    {
        ArgumentNullException.ThrowIfNull(console);

        // TODO: Make this a bit more efficient
        var buffer = new StringWriter();
        var ansi = new AnsiWriter(buffer, console.Profile.Capabilities);
        ansi.Write(console, renderable);
        return buffer.ToString();
    }
}