namespace Spectre.Console;

public static partial class AnsiConsoleExtensions
{
    extension(AnsiConsole)
    {
        /// <summary>
        /// Writes a VT/Ansi control code sequence to the console (if supported).
        /// </summary>
        /// <param name="sequence">The VT/Ansi control code sequence to write.</param>
        public static void WriteAnsi(string sequence)
        {
            AnsiConsole.Console.WriteAnsi(sequence);
        }

        /// <summary>
        /// Gets the VT/ANSI control code sequence for a <see cref="IRenderable"/>.
        /// </summary>
        /// <param name="renderable">The renderable to the VT/ANSI control code sequence for.</param>
        /// <returns>The VT/ANSI control code sequence.</returns>
        public static string ToAnsi(IRenderable renderable)
        {
            return AnsiConsole.Console.ToAnsi(renderable);
        }
    }

    extension(IAnsiConsole console)
    {
        /// <summary>
        /// Writes a VT/Ansi control code sequence to the console (if supported).
        /// </summary>
        /// <param name="sequence">The VT/Ansi control code sequence to write.</param>
        public void WriteAnsi(string sequence)
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
        /// <param name="renderable">The renderable to the VT/ANSI control code sequence for.</param>
        /// <returns>The VT/ANSI control code sequence.</returns>
        public string ToAnsi(IRenderable renderable)
        {
            return AnsiBuilder.Build(console, renderable);
        }
    }
}