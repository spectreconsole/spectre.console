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
            console.Lines.Count.ShouldBe(11);
            console.Lines[00].ShouldBe("               2020 October                ");
            console.Lines[01].ShouldBe("┌─────┬─────┬─────┬─────┬─────┬─────┬─────┐");
            console.Lines[02].ShouldBe("│ Sun │ Mon │ Tue │ Wed │ Thu │ Fri │ Sat │");
            console.Lines[03].ShouldBe("├─────┼─────┼─────┼─────┼─────┼─────┼─────┤");
            console.Lines[04].ShouldBe("│     │     │     │     │ 1   │ 2   │ 3*  │");
            console.Lines[05].ShouldBe("│ 4   │ 5   │ 6   │ 7   │ 8   │ 9   │ 10  │");
            console.Lines[06].ShouldBe("│ 11  │ 12* │ 13  │ 14  │ 15  │ 16  │ 17  │");
            console.Lines[07].ShouldBe("│ 18  │ 19  │ 20  │ 21  │ 22  │ 23  │ 24  │");
            console.Lines[08].ShouldBe("│ 25  │ 26  │ 27  │ 28  │ 29  │ 30  │ 31  │");
            console.Lines[09].ShouldBe("│     │     │     │     │     │     │     │");
            console.Lines[10].ShouldBe("└─────┴─────┴─────┴─────┴─────┴─────┴─────┘");
        }

        [Fact]
        public void Should_Center_Calendar_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 80);
            var calendar = new Calendar(2020, 10)
                .Centered()
                .AddCalendarEvent(new DateTime(2020, 9, 1))
                .AddCalendarEvent(new DateTime(2020, 10, 3))
                .AddCalendarEvent(new DateTime(2020, 10, 12));

            // When
            console.Render(calendar);

            // Then
            console.Lines.Count.ShouldBe(11);
            console.Lines[00].ShouldBe("                                 2020 October                                   ");
            console.Lines[01].ShouldBe("                  ┌─────┬─────┬─────┬─────┬─────┬─────┬─────┐                   ");
            console.Lines[02].ShouldBe("                  │ Sun │ Mon │ Tue │ Wed │ Thu │ Fri │ Sat │                   ");
            console.Lines[03].ShouldBe("                  ├─────┼─────┼─────┼─────┼─────┼─────┼─────┤                   ");
            console.Lines[04].ShouldBe("                  │     │     │     │     │ 1   │ 2   │ 3*  │                   ");
            console.Lines[05].ShouldBe("                  │ 4   │ 5   │ 6   │ 7   │ 8   │ 9   │ 10  │                   ");
            console.Lines[06].ShouldBe("                  │ 11  │ 12* │ 13  │ 14  │ 15  │ 16  │ 17  │                   ");
            console.Lines[07].ShouldBe("                  │ 18  │ 19  │ 20  │ 21  │ 22  │ 23  │ 24  │                   ");
            console.Lines[08].ShouldBe("                  │ 25  │ 26  │ 27  │ 28  │ 29  │ 30  │ 31  │                   ");
            console.Lines[09].ShouldBe("                  │     │     │     │     │     │     │     │                   ");
            console.Lines[10].ShouldBe("                  └─────┴─────┴─────┴─────┴─────┴─────┴─────┘                   ");
        }

        [Fact]
        public void Should_Left_Align_Calendar_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 80);
            var calendar = new Calendar(2020, 10)
                .LeftAligned()
                .AddCalendarEvent(new DateTime(2020, 9, 1))
                .AddCalendarEvent(new DateTime(2020, 10, 3))
                .AddCalendarEvent(new DateTime(2020, 10, 12));

            // When
            console.Render(calendar);

            // Then
            console.Lines.Count.ShouldBe(11);
            console.Lines[00].ShouldBe("               2020 October                ");
            console.Lines[01].ShouldBe("┌─────┬─────┬─────┬─────┬─────┬─────┬─────┐");
            console.Lines[02].ShouldBe("│ Sun │ Mon │ Tue │ Wed │ Thu │ Fri │ Sat │");
            console.Lines[03].ShouldBe("├─────┼─────┼─────┼─────┼─────┼─────┼─────┤");
            console.Lines[04].ShouldBe("│     │     │     │     │ 1   │ 2   │ 3*  │");
            console.Lines[05].ShouldBe("│ 4   │ 5   │ 6   │ 7   │ 8   │ 9   │ 10  │");
            console.Lines[06].ShouldBe("│ 11  │ 12* │ 13  │ 14  │ 15  │ 16  │ 17  │");
            console.Lines[07].ShouldBe("│ 18  │ 19  │ 20  │ 21  │ 22  │ 23  │ 24  │");
            console.Lines[08].ShouldBe("│ 25  │ 26  │ 27  │ 28  │ 29  │ 30  │ 31  │");
            console.Lines[09].ShouldBe("│     │     │     │     │     │     │     │");
            console.Lines[10].ShouldBe("└─────┴─────┴─────┴─────┴─────┴─────┴─────┘");
        }

        [Fact]
        public void Should_Right_Align_Calendar_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 80);
            var calendar = new Calendar(2020, 10)
                .RightAligned()
                .AddCalendarEvent(new DateTime(2020, 9, 1))
                .AddCalendarEvent(new DateTime(2020, 10, 3))
                .AddCalendarEvent(new DateTime(2020, 10, 12));

            // When
            console.Render(calendar);

            // Then
            console.Lines.Count.ShouldBe(11);
            console.Lines[00].ShouldBe("                                                    2020 October                ");
            console.Lines[01].ShouldBe("                                     ┌─────┬─────┬─────┬─────┬─────┬─────┬─────┐");
            console.Lines[02].ShouldBe("                                     │ Sun │ Mon │ Tue │ Wed │ Thu │ Fri │ Sat │");
            console.Lines[03].ShouldBe("                                     ├─────┼─────┼─────┼─────┼─────┼─────┼─────┤");
            console.Lines[04].ShouldBe("                                     │     │     │     │     │ 1   │ 2   │ 3*  │");
            console.Lines[05].ShouldBe("                                     │ 4   │ 5   │ 6   │ 7   │ 8   │ 9   │ 10  │");
            console.Lines[06].ShouldBe("                                     │ 11  │ 12* │ 13  │ 14  │ 15  │ 16  │ 17  │");
            console.Lines[07].ShouldBe("                                     │ 18  │ 19  │ 20  │ 21  │ 22  │ 23  │ 24  │");
            console.Lines[08].ShouldBe("                                     │ 25  │ 26  │ 27  │ 28  │ 29  │ 30  │ 31  │");
            console.Lines[09].ShouldBe("                                     │     │     │     │     │     │     │     │");
            console.Lines[10].ShouldBe("                                     └─────┴─────┴─────┴─────┴─────┴─────┴─────┘");
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
            console.Lines.Count.ShouldBe(11);
            console.Lines[00].ShouldBe("            Oktober 2020             ");
            console.Lines[01].ShouldBe("┌─────┬────┬────┬────┬────┬────┬────┐");
            console.Lines[02].ShouldBe("│ Mo  │ Di │ Mi │ Do │ Fr │ Sa │ So │");
            console.Lines[03].ShouldBe("├─────┼────┼────┼────┼────┼────┼────┤");
            console.Lines[04].ShouldBe("│     │    │    │ 1  │ 2  │ 3* │ 4  │");
            console.Lines[05].ShouldBe("│ 5   │ 6  │ 7  │ 8  │ 9  │ 10 │ 11 │");
            console.Lines[06].ShouldBe("│ 12* │ 13 │ 14 │ 15 │ 16 │ 17 │ 18 │");
            console.Lines[07].ShouldBe("│ 19  │ 20 │ 21 │ 22 │ 23 │ 24 │ 25 │");
            console.Lines[08].ShouldBe("│ 26  │ 27 │ 28 │ 29 │ 30 │ 31 │    │");
            console.Lines[09].ShouldBe("│     │    │    │    │    │    │    │");
            console.Lines[10].ShouldBe("└─────┴────┴────┴────┴────┴────┴────┘");
        }
    }
}
