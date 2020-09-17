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
                .AddRow("[b]:artist_palette: Color system[/]", $"{AnsiConsole.Capabilities.ColorSystem}")
                .AddRow("[b]:nail_polish: Supports ansi?[/]", $"{GetEmoji(AnsiConsole.Capabilities.SupportsAnsi)}")
                .AddRow("[b]:top_hat: Legacy console?[/]", $"{GetEmoji(AnsiConsole.Capabilities.LegacyConsole)}")
                .AddRow("[b]:left-right_arrow: Buffer width[/]", $"{AnsiConsole.Console.Width}")
                .AddRow("[b]:up-down_arrow: Buffer height[/]", $"{AnsiConsole.Console.Height}");

            AnsiConsole.Render(
                new Panel(grid)
                    .SetHeader("Information"));
        }

        private static string GetEmoji(bool value) => value
            ? ":thumbs_up:"
            : ":thumbs_down:";
    }
}
