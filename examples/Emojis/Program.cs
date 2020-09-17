using System;
using Spectre.Console;

namespace Emojis
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Markup
            AnsiConsole.Render(
                new Panel("[yellow]Hello :globe_showing_europe_africa:![/]")
                    .RoundedBorder()
                    .SetHeader("Markup"));
        }
    }
}
