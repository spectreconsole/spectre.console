using Spectre.Console;

namespace Calendars
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Render(new Calendar(2020, 10)
                    .RoundedBorder()
                    .SetHighlightStyle(Style.Parse("red"))
                    .SetHeaderStyle(Style.Parse("yellow"))
                    .AddCalendarEvent("An event", 2020, 9, 22)
                    .AddCalendarEvent("Another event", 2020, 10, 2)
                    .AddCalendarEvent("A third event", 2020, 10, 13));
        }
    }
}
