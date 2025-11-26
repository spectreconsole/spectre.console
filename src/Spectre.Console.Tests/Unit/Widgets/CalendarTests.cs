namespace Spectre.Console.Tests.Unit;

[ExpectationPath("Widgets/Calendar")]
public sealed class CalendarTests
{
    [Fact]
    [Expectation("RenderNov2025woEvents")]
    public Task Should_Render_Calendar()
    {
        //Given
        var console = new TestConsole();
        var calendar = new Calendar(2025, 11);

        //When
        console.Write(calendar);

        //Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("RenderNov2025RemoveEvent")]
    public Task Should_Render_Calendar_Remove_Event()
    {
        //Given
        var console = new TestConsole();
        var calendar = new Calendar(2025, 11)
            .AddCalendarEvent(2025, 11, 26);

        calendar.RemoveCalendarEvent(2025, 11, 26);
        //When
        console.Write(calendar);

        //Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("RenderNov2025RemoveEvents")]
    public Task Should_Render_Calendar_Remove_Events()
    {
        //Given
        var console = new TestConsole();
        var calendar = new Calendar(2025, 11)
            .AddCalendarEvent(2025, 11, 1)
            .AddCalendarEvent(2025, 11, 15)
            .AddCalendarEvent(2025, 11, 13)
            .AddCalendarEvent(2025, 11, 23)
            .AddCalendarEvent(2025, 11, 27);

        calendar.RemoveCalendarEvent(2025, 11, 15);
        calendar.RemoveCalendarEvent(2025, 11, 23);
        //When
        console.Write(calendar);

        //Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("RenderNov2025RemoveFaultyEvent")]
    public Task Should_Render_Calendar_Remove_Faulty_Event()
    {
        //Given
        var console = new TestConsole();
        var calendar = new Calendar(2025, 11)
            .AddCalendarEvent(2025, 11, 1)
            .AddCalendarEvent(2025, 11, 15)
            .AddCalendarEvent(2025, 11, 13);

        calendar.RemoveCalendarEvent(2025, 11, 15);

        // Remove event from May -> not in this calendar
        calendar.RemoveCalendarEvent(2025, 5, 23);

        //When
        console.Write(calendar);

        //Then
        return Verifier.Verify(console.Output);
    }

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