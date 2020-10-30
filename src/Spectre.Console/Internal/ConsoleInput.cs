using System;

namespace Spectre.Console.Internal
{
    internal sealed class ConsoleInput : IAnsiConsoleInput
    {
        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            if (!Environment.UserInteractive)
            {
                throw new InvalidOperationException("Failed to read input in non-interactive mode.");
            }

            return System.Console.ReadKey(intercept);
        }
    }
}
