using System;
using System.Threading.Tasks;
using Spectre.Console.Testing;
using Spectre.Verify.Extensions;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    [ExpectationPath("Widgets/Calendar")]
    public sealed class CalendarTests
    {
        [Fact]
        [Expectation("Render")]
        public Task Should_Render_Calendar_Correctly()
        {
            // Given
            var console = new TestConsole();
            var calendar = new Calendar(2020, 10)
                .AddCalendarEvent(new DateTime(2020, 9, 1))
                .AddCalendarEvent(new DateTime(2020, 10, 3))
                .AddCalendarEvent(new DateTime(2020, 10, 12));

            // When
            console.Write(calendar);

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Centered")]
        public Task Should_Center_Calendar_Correctly()
        {
            // Given
            var console = new TestConsole();
            var calendar = new Calendar(2020, 10)
                .Centered()
                .AddCalendarEvent(new DateTime(2020, 9, 1))
                .AddCalendarEvent(new DateTime(2020, 10, 3))
                .AddCalendarEvent(new DateTime(2020, 10, 12));

            // When
            console.Write(calendar);

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("LeftAligned")]
        public Task Should_Left_Align_Calendar_Correctly()
        {
            // Given
            var console = new TestConsole();
            var calendar = new Calendar(2020, 10)
                .LeftAligned()
                .AddCalendarEvent(new DateTime(2020, 9, 1))
                .AddCalendarEvent(new DateTime(2020, 10, 3))
                .AddCalendarEvent(new DateTime(2020, 10, 12));

            // When
            console.Write(calendar);

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("RightAligned")]
        public Task Should_Right_Align_Calendar_Correctly()
        {
            // Given
            var console = new TestConsole();
            var calendar = new Calendar(2020, 10)
                .RightAligned()
                .AddCalendarEvent(new DateTime(2020, 9, 1))
                .AddCalendarEvent(new DateTime(2020, 10, 3))
                .AddCalendarEvent(new DateTime(2020, 10, 12));

            // When
            console.Write(calendar);

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Culture")]
        public Task Should_Render_Calendar_Correctly_For_Specific_Culture()
        {
            // Given
            var console = new TestConsole();
            var calendar = new Calendar(2020, 10, 15)
                .Culture("de-DE")
                .AddCalendarEvent(new DateTime(2020, 9, 1))
                .AddCalendarEvent(new DateTime(2020, 10, 3))
                .AddCalendarEvent(new DateTime(2020, 10, 12));

            // When
            console.Write(calendar);

            // Then
            return Verifier.Verify(console.Output);
        }
    }
}
