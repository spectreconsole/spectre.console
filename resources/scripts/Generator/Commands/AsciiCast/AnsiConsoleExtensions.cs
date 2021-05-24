using System;
using Spectre.Console;
using Generator.Commands;
using System.Threading;

namespace DocExampleGenerator
{
    internal static class AnsiConsoleExtensions
    {
        /// <summary>
        /// Displays something via AnsiConsole, waits a bit and then simulates typing based on the input. If the console
        /// doesn't have the focus this will just type into whatever window does so watch the alt-tab.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="action">The display action.</param>
        /// <param name="input">The characters to type. ↑ for an up arrow, ↓ for down arrow, ↲ for a return and ¦ for a pause.</param>
        /// <param name="initialDelayMs">How long to delay before typing. This should be at least 100ms because we won't check if the prompt has displayed before simulating typing.</param>
        /// <param name="keypressDelayMs">Delay between keypresses. There will be a bit of randomness between each keypress +/- 20% of this value.</param>
        public static void DisplayThenType(this IAnsiConsole console, Action<IAnsiConsole> action, string input, int initialDelayMs = 500, int keypressDelayMs = 200)
        {
            if (console is not AsciiCastConsole asciiConsole)
            {
                throw new InvalidOperationException("Not an ASCII cast console");
            }

            asciiConsole.Input.PushText(input, keypressDelayMs);

            Thread.Sleep(initialDelayMs);

            action(console);
        }
    }
}