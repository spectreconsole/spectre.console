using Spectre.Console;

namespace EmojiExample
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            AnsiConsole.Render(new FigletText("Left aligned").LeftAligned().Color(Color.Red));
            AnsiConsole.Render(new FigletText("Centered").Centered().Color(Color.Green));
            AnsiConsole.Render(new FigletText("Right aligned").RightAligned().Color(Color.Blue));
        }
    }
}
