using Spectre.Console;

namespace Figlet;

public static class Program
{
    public static void Main(string[] args)
    {
        AnsiConsole.Write(new FigletText("Left aligned").LeftAligned().Color(Color.Red));
        AnsiConsole.Write(new FigletText("Centered").Centered().Color(Color.Green));
        AnsiConsole.Write(new FigletText("Right aligned").RightAligned().Color(Color.Blue));
    }
}
