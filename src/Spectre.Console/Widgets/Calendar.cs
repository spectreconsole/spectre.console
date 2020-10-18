using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Spectre.Console.Internal;
using Spectre.Console.Internal.Collections;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A renderable calendar.
    /// </summary>
    public sealed class Calendar : Renderable, IHasCulture, IHasTableBorder
    {
        private const int NumberOfWeekDays = 7;
        private const int ExpectedRowCount = 6;

        private readonly ListWithCallback<CalendarEvent> _calendarEvents;

        private int _year;
        private int _month;
        private int _day;
        private IRenderable? _table;
        private TableBorder _border;
        private bool _useSafeBorder;
        private Style? _borderStyle;
        private bool _dirty;
        private CultureInfo _culture;
        private Style _highlightStyle;
        private bool _showHeader;
        private Style? _headerStyle;

        /// <summary>
        /// Gets or sets the calendar year.
        /// </summary>
        public int Year
        {
            get => _year;
            set => MarkAsDirty(() => _year = value);
        }

        /// <summary>
        /// Gets or sets the calendar month.
        /// </summary>
        public int Month
        {
            get => _month;
            set => MarkAsDirty(() => _month = value);
        }

        /// <summary>
        /// Gets or sets the calendar day.
        /// </summary>
        public int Day
        {
            get => _day;
            set => MarkAsDirty(() => _day = value);
        }

        /// <inheritdoc/>
        public TableBorder Border
        {
            get => _border;
            set => MarkAsDirty(() => _border = value);
        }

        /// <inheritdoc/>
        public bool UseSafeBorder
        {
            get => _useSafeBorder;
            set => MarkAsDirty(() => _useSafeBorder = value);
        }

        /// <inheritdoc/>
        public Style? BorderStyle
        {
            get => _borderStyle;
            set => MarkAsDirty(() => _borderStyle = value);
        }

        /// <summary>
        /// Gets or sets the calendar's <see cref="CultureInfo"/>.
        /// </summary>
        public CultureInfo Culture
        {
            get => _culture;
            set => MarkAsDirty(() => _culture = value);
        }

        /// <summary>
        /// Gets or sets the calendar's highlight <see cref="Style"/>.
        /// </summary>
        public Style HightlightStyle
        {
            get => _highlightStyle;
            set => MarkAsDirty(() => _highlightStyle = value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the calendar header should be shown.
        /// </summary>
        public bool ShowHeader
        {
            get => _showHeader;
            set => MarkAsDirty(() => _showHeader = value);
        }

        /// <summary>
        /// Gets or sets the header style.
        /// </summary>
        public Style? HeaderStyle
        {
            get => _headerStyle;
            set => MarkAsDirty(() => _headerStyle = value);
        }

        /// <summary>
        /// Gets a list containing all calendar events.
        /// </summary>
        public IList<CalendarEvent> CalendarEvents => _calendarEvents;

        /// <summary>
        /// Initializes a new instance of the <see cref="Calendar"/> class.
        /// </summary>
        /// <param name="date">The calendar date.</param>
        public Calendar(DateTime date)
            : this(date.Year, date.Month, date.Day)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Calendar"/> class.
        /// </summary>
        /// <param name="year">The calendar year.</param>
        /// <param name="month">The calendar month.</param>
        public Calendar(int year, int month)
            : this(year, month, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Calendar"/> class.
        /// </summary>
        /// <param name="year">The calendar year.</param>
        /// <param name="month">The calendar month.</param>
        /// <param name="day">The calendar day.</param>
        public Calendar(int year, int month, int day)
        {
            _year = year;
            _month = month;
            _day = day;
            _table = null;
            _border = TableBorder.Square;
            _useSafeBorder = true;
            _borderStyle = null;
            _dirty = true;
            _culture = CultureInfo.InvariantCulture;
            _highlightStyle = new Style(foreground: Color.Blue);
            _showHeader = true;
            _calendarEvents = new ListWithCallback<CalendarEvent>(() => _dirty = true);
        }

        /// <inheritdoc/>
        protected override Measurement Measure(RenderContext context, int maxWidth)
        {
            var table = GetTable();
            return table.Measure(context, maxWidth);
        }

        /// <inheritdoc/>
        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            return GetTable().Render(context, maxWidth);
        }

        private IRenderable GetTable()
        {
            // Table needs to be built?
            if (_dirty || _table == null)
            {
                _table = BuildTable();
                _dirty = false;
            }

            return _table;
        }

        private IRenderable BuildTable()
        {
            var culture = Culture ?? CultureInfo.InvariantCulture;

            var table = new Table
            {
                Border = _border,
                UseSafeBorder = _useSafeBorder,
                BorderStyle = _borderStyle,
            };

            if (ShowHeader)
            {
                var heading = new DateTime(Year, Month, Day).ToString("Y", culture).SafeMarkup();
                table.Heading = new Title(heading, HeaderStyle);
            }

            // Add columns
            foreach (var order in GetWeekdays())
            {
                table.AddColumn(new TableColumn(order.GetAbbreviatedDayName(culture)));
            }

            var row = new List<IRenderable>();

            var currentDay = 1;
            var weekday = culture.DateTimeFormat.FirstDayOfWeek;
            var weekdays = BuildWeekDayTable();

            var daysInMonth = DateTime.DaysInMonth(Year, Month);
            while (currentDay <= daysInMonth)
            {
                if (weekdays[currentDay - 1] == weekday)
                {
                    if (_calendarEvents.Any(e => e.Month == Month && e.Day == currentDay))
                    {
                        row.Add(new Markup(currentDay.ToString(CultureInfo.InvariantCulture) + "*", _highlightStyle));
                    }
                    else
                    {
                        row.Add(new Text(currentDay.ToString(CultureInfo.InvariantCulture)));
                    }

                    currentDay++;
                }
                else
                {
                    // Add empty cell
                    row.Add(Text.Empty);
                }

                if (row.Count == NumberOfWeekDays)
                {
                    // Flush row
                    table.AddRow(row.ToArray());
                    row.Clear();
                }

                weekday = weekday.GetNextWeekDay();
            }

            if (row.Count > 0)
            {
                // Flush row
                table.AddRow(row.ToArray());
                row.Clear();
            }

            // We want all calendars to have the same height.
            if (table.RowCount < ExpectedRowCount)
            {
                var diff = Math.Max(0, ExpectedRowCount - table.RowCount);
                for (var i = 0; i < diff; i++)
                {
                    table.AddEmptyRow();
                }
            }

            return table;
        }

        private void MarkAsDirty(Action action)
        {
            action();
            _dirty = true;
        }

        private DayOfWeek[] GetWeekdays()
        {
            var culture = Culture ?? CultureInfo.InvariantCulture;

            var days = new DayOfWeek[7];
            days[0] = culture.DateTimeFormat.FirstDayOfWeek;
            for (var i = 1; i < 7; i++)
            {
                days[i] = days[i - 1].GetNextWeekDay();
            }

            return days;
        }

        private DayOfWeek[] BuildWeekDayTable()
        {
            var result = new List<DayOfWeek>();
            for (var day = 0; day < DateTime.DaysInMonth(Year, Month); day++)
            {
                result.Add(new DateTime(Year, Month, day + 1).DayOfWeek);
            }

            return result.ToArray();
        }
    }
}
