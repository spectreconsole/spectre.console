using Spectre.Console;

namespace Info
{
    public static class Program
    {
        public static void Main()
        {
            var grid = new Grid()
                .AddColumn(new GridColumn().NoWrap().PadRight(4))
                .AddColumn()
                .AddRow("[b]Color system[/]", $"{AnsiConsole.Capabilities.ColorSystem}")
                .AddRow("[b]Supports ansi?[/]", $"{AnsiConsole.Capabilities.SupportsAnsi}")
                .AddRow("[b]Legacy console?[/]", $"{AnsiConsole.Capabilities.LegacyConsole}")
                .AddRow("[b]Buffer width[/]", $"{AnsiConsole.Console.Width}")
                .AddRow("[b]Buffer height[/]", $"{AnsiConsole.Console.Height}");

            AnsiConsole.Render(
                new Panel(grid)
                    .SetHeader("Information"));
        }
    }
}
