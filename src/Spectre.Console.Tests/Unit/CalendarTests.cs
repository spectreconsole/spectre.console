using System;
using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class CalendarTests
    {
        [Fact]
        public void Should_Render_Calendar_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 80);
            var calendar = new Calendar(2020, 10)
                .AddCalendarEvent(new DateTime(2020, 9, 1))
                .AddCalendarEvent(new DateTime(2020, 10, 3))
                .AddCalendarEvent(new DateTime(2020, 10, 12));

            // When
            console.Render(calendar);

            // Then
            console.Lines.Count.ShouldBe(10);
            console.Lines[0].ShouldBe("┌─────┬─────┬─────┬─────┬─────┬─────┬─────┐");
            console.Lines[1].ShouldBe("│ Sun │ Mon │ Tue │ Wed │ Thu │ Fri │ Sat │");
            console.Lines[2].ShouldBe("├─────┼─────┼─────┼─────┼─────┼─────┼─────┤");
            console.Lines[3].ShouldBe("│     │     │     │     │ 1   │ 2   │ 3*  │");
            console.Lines[4].ShouldBe("│ 4   │ 5   │ 6   │ 7   │ 8   │ 9   │ 10  │");
            console.Lines[5].ShouldBe("│ 11  │ 12* │ 13  │ 14  │ 15  │ 16  │ 17  │");
            console.Lines[6].ShouldBe("│ 18  │ 19  │ 20  │ 21  │ 22  │ 23  │ 24  │");
            console.Lines[7].ShouldBe("│ 25  │ 26  │ 27  │ 28  │ 29  │ 30  │ 31  │");
            console.Lines[8].ShouldBe("│     │     │     │     │     │     │     │");
            console.Lines[9].ShouldBe("└─────┴─────┴─────┴─────┴─────┴─────┴─────┘");
        }

        [Fact]
        public void Should_Render_Calendar_Correctly_For_Specific_Culture()
        {
            // Given
            var console = new PlainConsole(width: 80);
            var calendar = new Calendar(2020, 10, 15)
                .SetCulture("de-DE")
                .AddCalendarEvent(new DateTime(2020, 9, 1))
                .AddCalendarEvent(new DateTime(2020, 10, 3))
                .AddCalendarEvent(new DateTime(2020, 10, 12));

            // When
            console.Render(calendar);

            // Then
            console.Lines.Count.ShouldBe(10);
            console.Lines[0].ShouldBe("┌─────┬────┬────┬────┬────┬────┬────┐");
            console.Lines[1].ShouldBe("│ Mo  │ Di │ Mi │ Do │ Fr │ Sa │ So │");
            console.Lines[2].ShouldBe("├─────┼────┼────┼────┼────┼────┼────┤");
            console.Lines[3].ShouldBe("│     │    │    │ 1  │ 2  │ 3* │ 4  │");
            console.Lines[4].ShouldBe("│ 5   │ 6  │ 7  │ 8  │ 9  │ 10 │ 11 │");
            console.Lines[5].ShouldBe("│ 12* │ 13 │ 14 │ 15 │ 16 │ 17 │ 18 │");
            console.Lines[6].ShouldBe("│ 19  │ 20 │ 21 │ 22 │ 23 │ 24 │ 25 │");
            console.Lines[7].ShouldBe("│ 26  │ 27 │ 28 │ 29 │ 30 │ 31 │    │");
            console.Lines[8].ShouldBe("│     │    │    │    │    │    │    │");
            console.Lines[9].ShouldBe("└─────┴────┴────┴────┴────┴────┴────┘");
        }

        [Fact]
        public void Should_Render_List_Of_Events_If_Enabled()
        {
            // Given
            var console = new PlainConsole(width: 80);
            var calendar = new Calendar(2020, 10, 15)
                .SetCulture("de-DE")
                .AddCalendarEvent(new DateTime(2020, 9, 1))
                .AddCalendarEvent(new DateTime(2020, 10, 3))
                .AddCalendarEvent(new DateTime(2020, 10, 12));

            // When
            console.Render(calendar);

            // Then
            console.Lines.Count.ShouldBe(10);
            console.Lines[0].ShouldBe("┌─────┬────┬────┬────┬────┬────┬────┐");
            console.Lines[1].ShouldBe("│ Mo  │ Di │ Mi │ Do │ Fr │ Sa │ So │");
            console.Lines[2].ShouldBe("├─────┼────┼────┼────┼────┼────┼────┤");
            console.Lines[3].ShouldBe("│     │    │    │ 1  │ 2  │ 3* │ 4  │");
            console.Lines[4].ShouldBe("│ 5   │ 6  │ 7  │ 8  │ 9  │ 10 │ 11 │");
            console.Lines[5].ShouldBe("│ 12* │ 13 │ 14 │ 15 │ 16 │ 17 │ 18 │");
            console.Lines[6].ShouldBe("│ 19  │ 20 │ 21 │ 22 │ 23 │ 24 │ 25 │");
            console.Lines[7].ShouldBe("│ 26  │ 27 │ 28 │ 29 │ 30 │ 31 │    │");
            console.Lines[8].ShouldBe("│     │    │    │    │    │    │    │");
            console.Lines[9].ShouldBe("└─────┴────┴────┴────┴────┴────┴────┘");
        }
    }
}
