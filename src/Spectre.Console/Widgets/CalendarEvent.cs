namespace Spectre.Console;

/// <summary>
/// Represents a calendar event.
/// </summary>
public sealed class CalendarEvent
{
    /// <summary>
    /// Gets the description of the calendar event.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the year of the calendar event.
    /// </summary>
    public int Year { get; }

    /// <summary>
    /// Gets the month of the calendar event.
    /// </summary>
    public int Month { get; }

    /// <summary>
    /// Gets the day of the calendar event.
    /// </summary>
    public int Day { get; }

    /// <summary>
    /// Gets the <see cref="Style"/> of the calendar event.
    /// </summary>
    public Style Style { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarEvent"/> class.
    /// </summary>
    /// <param name="year">The year of the calendar event.</param>
    /// <param name="month">The month of the calendar event.</param>
    /// <param name="day">The day of the calendar event.</param>
    public CalendarEvent(int year, int month, int day)
        : this(string.Empty, year, month, day, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarEvent"/> class.
    /// </summary>
    /// <param name="description">The calendar event description.</param>
    /// <param name="year">The year of the calendar event.</param>
    /// <param name="month">The month of the calendar event.</param>
    /// <param name="day">The day of the calendar event.</param>
    /// <param name="style">The style of the calendar event.</param>
    public CalendarEvent(string description, int year, int month, int day, Style? style = null)
    {
        Description = description ?? string.Empty;
        Year = year;
        Month = month;
        Day = day;
        Style = style ?? Style.Plain;
    }
}