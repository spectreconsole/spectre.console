using Spectre.Console;

namespace Generator.Commands.Samples
{
    internal abstract class BaseCalendarSample : BaseSample
    {
        public override (int Cols, int Rows) ConsoleSize => (base.ConsoleSize.Cols, 12);
    }

    internal class CalendarSample : BaseCalendarSample
    {

        public override void Run(IAnsiConsole console) => console.Write(new Calendar(2020,10));
    }

    internal class CalendarCultureSample : BaseCalendarSample
    {
        public override void Run(IAnsiConsole console) => console.Write(new Calendar(2020,10).Culture("sv-SE"));
    }

    internal class CalendarHeader : BaseCalendarSample
    {
        public override void Run(IAnsiConsole console)
        {
            var calendar = new Calendar(2020,10);
            calendar.HeaderStyle(Style.Parse("blue bold"));
            console.Write(calendar);
        }
    }

    internal class CalendarHighlightSample : BaseCalendarSample
    {
        public override void Run(IAnsiConsole console)
        {
            var calendar = new Calendar(2020, 10).HighlightStyle(Style.Parse("yellow bold"));
            calendar.AddCalendarEvent(2020, 10, 11);
            console.Write(calendar);
        }
    }
}