using Spectre.Console;

namespace Generator.Commands.Samples
{
    internal class BreakdownChartSample : BaseSample
    {
        public override (int Cols, int Rows) ConsoleSize => (base.ConsoleSize.Cols, 5);

        public override void Run(IAnsiConsole console)
        {
            console.Write(new BreakdownChart()
                .Width(60)
                .AddItem("SCSS", 80, Color.Red)
                .AddItem("HTML", 28.3, Color.Blue)
                .AddItem("C#", 22.6, Color.Green)
                .AddItem("JavaScript", 6, Color.Yellow)
                .AddItem("Ruby", 6, Color.LightGreen)
                .AddItem("Shell", 0.1, Color.Aqua));
        }
    }
}