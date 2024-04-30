using Spectre.Console;
using Spectre.Console.Rendering;

namespace Charts;

public static class Program
{
    public static void Main()
    {
        // Render a bar chart
        AnsiConsole.WriteLine();
        Render("Fruits per month", new BarChart()
            .Width(60)
            .Label("[green bold underline]Number of fruits[/]")
            .CenterLabel()
            .AddItem("Apple", 12, Color.Yellow)
            .AddItem("Orange", 54, Color.Green)
            .AddItem("Banana", 33, Color.Red));

        // Render a breakdown chart
        AnsiConsole.WriteLine();
        Render("Languages used", new BreakdownChart()
            .FullSize()
            .Width(60)
            .ShowPercentage()
            .WithValueColor(Color.Orange1)
            .AddItem("SCSS", 37, Color.Red)
            .AddItem("HTML", 28.3, Color.Blue)
            .AddItem("C#", 22.6, Color.Green)
            .AddItem("JavaScript", 6, Color.Yellow)
            .AddItem("Ruby", 6, Color.LightGreen)
            .AddItem("Shell", 0.1, Color.Aqua));
    }

    private static void Render(string title, IRenderable chart)
    {
        AnsiConsole.Write(
            new Panel(chart)
                .Padding(1, 1)
                .Header(title));
    }
}
