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
                .AddRow("[b]Enrichers[/]", string.Join(", ", AnsiConsole.Profile.Enrichers))
                .AddRow("[b]Color system[/]", $"{AnsiConsole.Profile.ColorSystem}")
                .AddRow("[b]Unicode?[/]", $"{YesNo(AnsiConsole.Profile.Capabilities.Unicode)}")
                .AddRow("[b]Supports ansi?[/]", $"{YesNo(AnsiConsole.Profile.Capabilities.Ansi)}")
                .AddRow("[b]Supports links?[/]", $"{YesNo(AnsiConsole.Profile.Capabilities.Links)}")
                .AddRow("[b]Legacy console?[/]", $"{YesNo(AnsiConsole.Profile.Capabilities.Legacy)}")
                .AddRow("[b]Interactive?[/]", $"{YesNo(AnsiConsole.Profile.Capabilities.Interactive)}")
                .AddRow("[b]TTY?[/]", $"{YesNo(AnsiConsole.Profile.Capabilities.Tty)}")
                .AddRow("[b]Buffer width[/]", $"{AnsiConsole.Console.Profile.Width}")
                .AddRow("[b]Buffer height[/]", $"{AnsiConsole.Console.Profile.Height}")
                .AddRow("[b]Encoding[/]", $"{AnsiConsole.Console.Profile.Encoding.EncodingName}");

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
