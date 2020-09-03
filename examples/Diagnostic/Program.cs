using System;
using Spectre.Console;

namespace Diagnostic
{
    public static class Program
    {
        public static void Main()
        {
            AnsiConsole.MarkupLine("Color system: [bold]{0}[/]", AnsiConsole.Capabilities.ColorSystem);
            AnsiConsole.MarkupLine("Supports ansi? [bold]{0}[/]", AnsiConsole.Capabilities.SupportsAnsi);
            AnsiConsole.MarkupLine("Legacy console? [bold]{0}[/]", AnsiConsole.Capabilities.LegacyConsole);
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("Buffer width: [bold]{0}[/]", AnsiConsole.Console.Width);
            AnsiConsole.MarkupLine("Buffer height: [bold]{0}[/]", AnsiConsole.Console.Height);
        }
    }
}
