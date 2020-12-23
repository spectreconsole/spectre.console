using Spectre.Console;

namespace InfoExample
{
    public static class Program
    {
        public static void Main()
        {
            var chart = new BarChart()
                .Width(60)
                .Label("[green bold underline]Number of fruits[/]")
                .CenterLabel()
                .AddItem("Apple", 12, Color.Yellow)
                .AddItem("Orange", 54, Color.Green)
                .AddItem("Banana", 33, Color.Red);

            AnsiConsole.WriteLine();
            AnsiConsole.Render(chart);
        }
    }
}
