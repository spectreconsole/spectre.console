namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="Calendar"/>.
/// </summary>
public static class CalendarExtensions
{
    /// <param name="calendar">The calendar to add the calendar event to.</param>
    extension(Calendar calendar)
    {
        /// <summary>
        /// Adds a calendar event.
        /// </summary>
        /// <param name="date">The calendar event date.</param>
        /// <param name="customEventHighlightStyle">The calendar event custom highlight style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Calendar AddCalendarEvent(DateTime date, Style? customEventHighlightStyle = null)
        {
            return AddCalendarEvent(calendar, string.Empty, date.Year, date.Month, date.Day, customEventHighlightStyle);
        }

        /// <summary>
        /// Adds a calendar event.
        /// </summary>
        /// <param name="description">The calendar event description.</param>
        /// <param name="date">The calendar event date.</param>
        /// <param name="customEventHighlightStyle">The calendar event custom highlight style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Calendar AddCalendarEvent(string description, DateTime date, Style? customEventHighlightStyle = null)
        {
            return AddCalendarEvent(calendar, description, date.Year, date.Month, date.Day, customEventHighlightStyle);
        }

        /// <summary>
        /// Adds a calendar event.
        /// </summary>
        /// <param name="year">The year of the calendar event.</param>
        /// <param name="month">The month of the calendar event.</param>
        /// <param name="day">The day of the calendar event.</param>
        /// <param name="customEventHighlightStyle">The calendar event custom highlight style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Calendar AddCalendarEvent(int year, int month, int day, Style? customEventHighlightStyle = null)
        {
            return AddCalendarEvent(calendar, string.Empty, year, month, day, customEventHighlightStyle);
        }

        /// <summary>
        /// Adds a calendar event.
        /// </summary>
        /// <param name="description">The calendar event description.</param>
        /// <param name="year">The year of the calendar event.</param>
        /// <param name="month">The month of the calendar event.</param>
        /// <param name="day">The day of the calendar event.</param>
        /// <param name="customEventHighlightStyle">The calendar event custom highlight style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Calendar AddCalendarEvent(string description, int year, int month, int day, Style? customEventHighlightStyle = null)
        {
            if (calendar is null)
            {
                throw new ArgumentNullException(nameof(calendar));
            }

            calendar.CalendarEvents.Add(new CalendarEvent(description, year, month, day, customEventHighlightStyle));
            return calendar;
        }

        /// <summary>
        /// Sets the calendar's highlight <see cref="Style"/>.
        /// </summary>
        /// <param name="style">The default highlight style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Calendar HighlightStyle(Style? style)
        {
            if (calendar is null)
            {
                throw new ArgumentNullException(nameof(calendar));
            }

            calendar.HighlightStyle = style ?? Style.Plain;
            return calendar;
        }

        /// <summary>
        /// Sets the calendar's header <see cref="Style"/>.
        /// </summary>
        /// <param name="style">The header style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Calendar HeaderStyle(Style? style)
        {
            if (calendar is null)
            {
                throw new ArgumentNullException(nameof(calendar));
            }

            calendar.HeaderStyle = style ?? Style.Plain;
            return calendar;
        }

        /// <summary>
        /// Shows the calendar header.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Calendar ShowHeader()
        {
            if (calendar is null)
            {
                throw new ArgumentNullException(nameof(calendar));
            }

            calendar.ShowHeader = true;
            return calendar;
        }

        /// <summary>
        /// Hides the calendar header.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Calendar HideHeader()
        {
            if (calendar is null)
            {
                throw new ArgumentNullException(nameof(calendar));
            }

            calendar.ShowHeader = false;
            return calendar;
        }
    }
}