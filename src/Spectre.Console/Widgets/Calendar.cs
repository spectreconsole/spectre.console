namespace Spectre.Console;

/// <summary>
/// A renderable calendar.
/// </summary>
public sealed class Calendar : JustInTimeRenderable, IHasCulture, IHasTableBorder, IAlignable
{
    private const int NumberOfWeekDays = 7;
    private const int ExpectedRowCount = 6;

    private readonly ListWithCallback<CalendarEvent> _calendarEvents;

    private int _year;
    private int _month;
    private int _day;
    private TableBorder _border;
    private bool _useSafeBorder;
    private Style? _borderStyle;
    private CultureInfo? _culture;
    private Style _highlightStyle;
    private bool _showHeader;
    private Style? _headerStyle;
    private Justify? _alignment;

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
    public CultureInfo? Culture
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

    /// <inheritdoc/>
    [Obsolete("Use the Align widget instead. This property will be removed in a later release.")]
    public Justify? Alignment
    {
        get => _alignment;
        set => MarkAsDirty(() => _alignment = value);
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
        _border = TableBorder.Square;
        _useSafeBorder = true;
        _borderStyle = null;
        _culture = CultureInfo.InvariantCulture;
        _highlightStyle = new Style(foreground: Color.Blue);
        _showHeader = true;
        _calendarEvents = new ListWithCallback<CalendarEvent>(() => MarkAsDirty());
    }

    /// <inheritdoc/>
    protected override IRenderable Build()
    {
        var culture = Culture ?? CultureInfo.InvariantCulture;

#pragma warning disable CS0618 // Type or member is obsolete
        var table = new Table
        {
            Border = _border,
            UseSafeBorder = _useSafeBorder,
            BorderStyle = _borderStyle,
            Alignment = _alignment,
        };
#pragma warning restore CS0618 // Type or member is obsolete

        if (ShowHeader)
        {
            var heading = new DateTime(Year, Month, Day).ToString("Y", culture).EscapeMarkup();
            table.Title = new TableTitle(heading, HeaderStyle);
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
        if (table.Rows.Count < ExpectedRowCount)
        {
            var diff = Math.Max(0, ExpectedRowCount - table.Rows.Count);
            for (var i = 0; i < diff; i++)
            {
                table.AddEmptyRow();
            }
        }

        return table;
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