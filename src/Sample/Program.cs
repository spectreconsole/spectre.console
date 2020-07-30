using System;
using Spectre.Console;

namespace Sample
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Use the static API to write some things to the console.
            AnsiConsole.Foreground = Color.Chartreuse2;
            AnsiConsole.Style = Styles.Underline | Styles.Bold;
            AnsiConsole.WriteLine("Hello World!");
            AnsiConsole.Reset();
            AnsiConsole.MarkupLine("Capabilities: [yellow underline]{0}[/]", AnsiConsole.Capabilities);
            AnsiConsole.MarkupLine("Width=[yellow]{0}[/], Height=[yellow]{1}[/]", AnsiConsole.Width, AnsiConsole.Height);
            AnsiConsole.MarkupLine("[white on red]Good[/] [red]bye[/]!");
            AnsiConsole.WriteLine();

            // We can also use System.ConsoleColor with AnsiConsole.
            foreach (ConsoleColor value in Enum.GetValues(typeof(ConsoleColor)))
            {
                AnsiConsole.Foreground = value;
                AnsiConsole.WriteLine("ConsoleColor.{0}", value);
            }

            // We can get the default console via the static API.
            var console = AnsiConsole.Console;

            // Or you can build it yourself the old fashion way.
            console = AnsiConsole.Create(
                new AnsiConsoleSettings()
                {
                    Ansi = AnsiSupport.Yes,
                    ColorSystem = ColorSystemSupport.Standard,
                    Out = Console.Out,
                });

            // In this case, we will find the closest colors
            // and downgrade them to the specified color system.
            console.WriteLine();
            console.Foreground = Color.Chartreuse2;
            console.Style = Styles.Underline | Styles.Bold;
            console.WriteLine("Hello World!");
            console.ResetColors();
            console.ResetStyle();
            console.MarkupLine("Capabilities: [yellow underline]{0}[/]", console.Capabilities);
            console.MarkupLine("Width=[yellow]{0}[/], Height=[yellow]{1}[/]", console.Width, console.Height);
            console.MarkupLine("[white on red]Good[/] [red]bye[/]!");
            console.WriteLine();

            // Nest some panels and text
            AnsiConsole.Foreground = Color.Maroon;
            AnsiConsole.Render(new Panel(new Panel(new Panel(new Panel(
                Text.New(
                    "I heard you like 📦\n\n\n\nSo I put a 📦 in a 📦",
                    foreground: Color.White,
                    justify: Justify.Center))))));

            // Reset colors
            AnsiConsole.ResetColors();

            // Left adjusted panel with text
            AnsiConsole.Render(new Panel(
                Text.New("Left adjusted\nLeft",
                    foreground: Color.White),
                fit: true));

            // Centered panel with text
            AnsiConsole.Render(new Panel(
                Text.New("Centered\nCenter",
                    foreground: Color.White,
                    justify: Justify.Center),
                fit: true));

            // Right adjusted panel with text
            AnsiConsole.Render(new Panel(
                Text.New("Right adjusted\nRight",
                    foreground: Color.White,
                    justify: Justify.Right),
                fit: true));
        }
    }
}