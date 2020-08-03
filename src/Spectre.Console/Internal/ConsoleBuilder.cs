using System;

namespace Spectre.Console.Internal
{
    internal static class ConsoleBuilder
    {
        public static IAnsiConsole Build(AnsiConsoleSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var buffer = settings.Out ?? System.Console.Out;

            var supportsAnsi = settings.Ansi == AnsiSupport.Detect
                ? AnsiDetector.Detect(true)
                : settings.Ansi == AnsiSupport.Yes;

            var colorSystem = settings.ColorSystem == ColorSystemSupport.Detect
                ? ColorSystemDetector.Detect(supportsAnsi)
                : (ColorSystem)settings.ColorSystem;

            if (supportsAnsi)
            {
                return new AnsiConsoleRenderer(buffer, colorSystem)
                {
                    Decoration = Decoration.None,
                };
            }

            return new FallbackConsoleRenderer(buffer, colorSystem);
        }
    }
}
