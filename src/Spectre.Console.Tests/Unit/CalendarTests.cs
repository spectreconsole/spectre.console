using System;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    public sealed class CalendarTests
    {
        [Fact]
        public Task Should_Render_Calendar_Correctly()
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
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Center_Calendar_Correctly()
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
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Left_Align_Calendar_Correctly()
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
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Right_Align_Calendar_Correctly()
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
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Render_Calendar_Correctly_For_Specific_Culture()
        {
            // Given
            var console = new PlainConsole(width: 80);
            var calendar = new Calendar(2020, 10, 15)
                .Culture("de-DE")
                .AddCalendarEvent(new DateTime(2020, 9, 1))
                .AddCalendarEvent(new DateTime(2020, 10, 3))
                .AddCalendarEvent(new DateTime(2020, 10, 12));

            // When
            console.Render(calendar);

            // Then
            return Verifier.Verify(console.Lines);
        }
    }
}
