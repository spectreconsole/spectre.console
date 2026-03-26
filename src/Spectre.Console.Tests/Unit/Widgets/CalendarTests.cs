namespace Spectre.Console.Tests.Unit;

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
    public Task Should_Center_Calendar_Correctly_Using_Aligner()
    {
        // Given
        var console = new TestConsole();
        var calendar = new Align(
            new Calendar(2020, 10)
                .AddCalendarEvent(new DateTime(2020, 9, 1))
                .AddCalendarEvent(new DateTime(2020, 10, 3))
                .AddCalendarEvent(new DateTime(2020, 10, 12)),
            HorizontalAlignment.Center);

        // When
        console.Write(calendar);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("LeftAligned")]
    public Task Should_Left_Align_Calendar_By_Default()
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
    [Expectation("RightAligned")]
    public Task Should_Right_Align_Calendar_Correctly_Using_Aligner()
    {
        // Given
        var console = new TestConsole();
        var calendar = new Align(
            new Calendar(2020, 10)
                .AddCalendarEvent(new DateTime(2020, 9, 1))
                .AddCalendarEvent(new DateTime(2020, 10, 3))
                .AddCalendarEvent(new DateTime(2020, 10, 12)),
            HorizontalAlignment.Right);

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
    [Fact]
public void Should_Remove_Calendar_Event_By_Date()
{
    // Given
    var calendar = new Calendar(2020, 10)
        .AddCalendarEvent(new DateTime(2020, 10, 3))
        .AddCalendarEvent(new DateTime(2020, 10, 12));

    // When
    calendar.RemoveCalendarEvent(new DateTime(2020, 10, 3));

    // Then
    Assert.Single(calendar.CalendarEvents);
    Assert.Equal(12, calendar.CalendarEvents[0].Day);
}

[Fact]
public void Should_Remove_Calendar_Event_By_Year_Month_Day()
{
    // Given
    var calendar = new Calendar(2020, 10)
        .AddCalendarEvent(new DateTime(2020, 10, 3))
        .AddCalendarEvent(new DateTime(2020, 10, 12));

    // When
    calendar.RemoveCalendarEvent(2020, 10, 3);

    // Then
    Assert.Single(calendar.CalendarEvents);
    Assert.Equal(12, calendar.CalendarEvents[0].Day);
}

[Fact]
public void Should_Remove_Calendar_Event_By_Description_And_Date()
{
    // Given
    var calendar = new Calendar(2020, 10)
        .AddCalendarEvent("Meeting", new DateTime(2020, 10, 3))
        .AddCalendarEvent("Holiday", new DateTime(2020, 10, 12));

    // When
    calendar.RemoveCalendarEvent("Meeting", new DateTime(2020, 10, 3));

    // Then
    Assert.Single(calendar.CalendarEvents);
    Assert.Equal(12, calendar.CalendarEvents[0].Day);
}

[Fact]
public void Should_Do_Nothing_When_Removing_Nonexistent_Calendar_Event()
{
    // Given
    var calendar = new Calendar(2020, 10)
        .AddCalendarEvent(new DateTime(2020, 10, 3));

    // When
calendar.RemoveCalendarEvent(new DateTime(2020, 11, 5));

    // Then
    Assert.Single(calendar.CalendarEvents);
}
}