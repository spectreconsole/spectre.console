using System;
using Spectre.Console.Rendering;

namespace Spectre.Console.Advanced
{
    /// <summary>
    /// Contains extension methods for <see cref="IAnsiConsole"/>.
    /// </summary>
    public static class AnsiConsoleExtensions
    {
        /// <summary>
        /// Writes a VT/Ansi control code sequence to the console (if supported).
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="sequence">The VT/Ansi control code sequence to write.</param>
        public static void WriteAnsi(this IAnsiConsole console, string sequence)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

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
            return AnsiBuilder.Build(console, renderable);
        }
    }
}
