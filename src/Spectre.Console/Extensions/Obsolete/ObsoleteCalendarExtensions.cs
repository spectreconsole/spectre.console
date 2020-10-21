using System;
using System.ComponentModel;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="Calendar"/>.
    /// </summary>
    public static class ObsoleteCalendarExtensions
    {
        /// <summary>
        /// Sets the calendar's highlight <see cref="Style"/>.
        /// </summary>
        /// <param name="calendar">The calendar.</param>
        /// <param name="style">The highlight style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        [Obsolete("Use HighlightStyle(..) instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Calendar SetHighlightStyle(this Calendar calendar, Style? style)
        {
            if (calendar is null)
            {
                throw new ArgumentNullException(nameof(calendar));
            }

            calendar.HightlightStyle = style ?? Style.Plain;
            return calendar;
        }

        /// <summary>
        /// Sets the calendar's header <see cref="Style"/>.
        /// </summary>
        /// <param name="calendar">The calendar.</param>
        /// <param name="style">The header style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        [Obsolete("Use HeaderStyle(..) instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Calendar SetHeaderStyle(this Calendar calendar, Style? style)
        {
            if (calendar is null)
            {
                throw new ArgumentNullException(nameof(calendar));
            }

            calendar.HeaderStyle = style ?? Style.Plain;
            return calendar;
        }
    }
}
