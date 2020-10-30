using System;
using Spectre.Console;

namespace InfoExample
{
    public static class Program
    {
        public static void Main()
        {
            var grid = new Grid()
                .AddColumn(new GridColumn().NoWrap().PadRight(4))
                .AddColumn()
                .AddRow("[b]Color system[/]", $"{AnsiConsole.Capabilities.ColorSystem}")
                .AddRow("[b]Supports ansi?[/]", $"{YesNo(AnsiConsole.Capabilities.SupportsAnsi)}")
                .AddRow("[b]Legacy console?[/]", $"{YesNo(AnsiConsole.Capabilities.LegacyConsole)}")
                .AddRow("[b]Interactive?[/]", $"{YesNo(Environment.UserInteractive)}")
                .AddRow("[b]Buffer width[/]", $"{AnsiConsole.Console.Width}")
                .AddRow("[b]Buffer height[/]", $"{AnsiConsole.Console.Height}");

            AnsiConsole.Render(
                new Panel(grid)
                    .Header("Information"));
        }

        private static string YesNo(bool value)
        {
            return value ? "Yes" : "No";
        }
    }
}
