using System.Collections.Generic;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Calendars
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Render(
                new Columns(GetCalendars())
                    .Collapse());
        }

        private static IEnumerable<IRenderable> GetCalendars()
        {
            yield return EmbedInPanel(
                "Invariant calendar",
                new Calendar(2020, 10)
                    .SimpleHeavyBorder()
                    .SetHighlightStyle(Style.Parse("red"))
                    .AddCalendarEvent("An event", 2020, 9, 22)
                    .AddCalendarEvent("Another event", 2020, 10, 2)
                    .AddCalendarEvent("A third event", 2020, 10, 13));

            yield return EmbedInPanel(
                "Swedish calendar (sv-SE)",
                new Calendar(2020, 10)
                    .RoundedBorder()
                    .SetHighlightStyle(Style.Parse("blue"))
                    .SetCulture("sv-SE")
                    .AddCalendarEvent("An event", 2020, 9, 22)
                    .AddCalendarEvent("Another event", 2020, 10, 2)
                    .AddCalendarEvent("A third event", 2020, 10, 13));

            yield return EmbedInPanel(
                "German calendar (de-DE)",
                new Calendar(2020, 10)
                    .MarkdownBorder()
                    .SetHighlightStyle(Style.Parse("yellow"))
                    .SetCulture("de-DE")
                    .AddCalendarEvent("An event", 2020, 9, 22)
                    .AddCalendarEvent("Another event", 2020, 10, 2)
                    .AddCalendarEvent("A third event", 2020, 10, 13));

            yield return EmbedInPanel(
                "Italian calendar (it-IT)",
                new Calendar(2020, 10)
                    .DoubleBorder()
                    .SetHighlightStyle(Style.Parse("green"))
                    .SetCulture("it-IT")
                    .AddCalendarEvent("An event", 2020, 9, 22)
                    .AddCalendarEvent("Another event", 2020, 10, 2)
                    .AddCalendarEvent("A third event", 2020, 10, 13));
        }

        private static IRenderable EmbedInPanel(string title, Calendar calendar)
        {
            return new Panel(calendar)
                .Expand()
                .RoundedBorder()
                .SetBorderStyle(Style.Parse("grey"))
                .SetHeader($" {title} ", Style.Parse("yellow"));
        }
    }
}
