using Spectre.Console;

namespace Generator.Commands.Samples
{
    public class FigletSample : BaseSample
    {
        public override (int Cols, int Rows) ConsoleSize => (100, 24);

        public override void Run(IAnsiConsole console)
        {
            console.Write(new FigletText("Left aligned").LeftJustified().Color(Color.Red));
            console.Write(new FigletText("Centered").Centered().Color(Color.Green));
            console.Write(new FigletText("Right aligned").RightJustified().Color(Color.Blue));
        }
    }
}