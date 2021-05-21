using System;
using System.Globalization;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;
using Spectre.Console;

namespace DocExampleGenerator
{
    internal static class ConsoleExtensions
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
            if (initialDelayMs < 100)
            {
                throw new ArgumentOutOfRangeException(nameof(initialDelayMs), "Initial delay must be greater than 100");
            }

            var random = new Random(Environment.TickCount);
            var inputTask = Task.Run(() => action(console));
            var typingTask = Task.Run(async () =>
            {
                await Task.Delay(initialDelayMs);
                var inputSimulator = new InputSimulator();
                foreach (var character in input)
                {
                    switch (character)
                    {
                        case '↑':
                            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.UP);
                            break;
                        case '↓':
                            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.DOWN);
                            break;
                        case '↲':
                            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                            break;
                        case '¦':
                            await Task.Delay(keypressDelayMs + random.Next((int) (keypressDelayMs * -.2), (int) (keypressDelayMs * .2)));
                            break;
                        default:
                            inputSimulator.Keyboard.TextEntry(character);
                            break;
                    }

                    await Task.Delay(keypressDelayMs + random.Next((int) (keypressDelayMs * -.2), (int) (keypressDelayMs * .2)));
                }
            });

            Task.WaitAll(inputTask, typingTask);
        }
    }
}