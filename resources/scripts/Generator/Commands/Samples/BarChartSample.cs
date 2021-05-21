using Spectre.Console;

namespace Generator.Commands.Samples
{
    internal class BarChartSample : BaseSample
    {
        public override (int Cols, int Rows) ConsoleSize => (base.ConsoleSize.Cols, 5);

        public override void Run(IAnsiConsole console)
        {
            console.Write(new BarChart()
                .Width(60)
                .Label("[green bold underline]Number of fruits[/]")
                .CenterLabel()
                .AddItem("Apple", 12, Color.Yellow)
                .AddItem("Orange", 54, Color.Green)
                .AddItem("Banana", 33, Color.Red));
        }
    }
}