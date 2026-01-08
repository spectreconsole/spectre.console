namespace Spectre.Console;

/// <summary>
/// Renders things in columns.
/// </summary>
public sealed class Columns : Renderable, IPaddable, IExpandable
{
    private readonly List<IRenderable> _items;

    /// <inheritdoc/>
    public Padding? Padding { get; set; } = new Padding(0, 0, 1, 0);

    /// <summary>
    /// Gets or sets a value indicating whether or not the columns should
    /// expand to the available space. If <c>false</c>, the column
    /// width will be auto calculated. Defaults to <c>true</c>.
    /// </summary>
    public bool Expand { get; set; } = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="Columns"/> class.
    /// </summary>
    /// <param name="items">The items to render as columns.</param>
    public Columns(params IRenderable[] items)
        : this((IEnumerable<IRenderable>)items)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Columns"/> class.
    /// </summary>
    /// <param name="items">The items to render as columns.</param>
    public Columns(IEnumerable<IRenderable> items)
    {
        if (items is null)
        {
            throw new ArgumentNullException(nameof(items));
        }

        _items = new List<IRenderable>(items);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Columns"/> class.
    /// </summary>
    /// <param name="items">The items to render.</param>
    public Columns(IEnumerable<string> items)
    {
        if (items is null)
        {
            throw new ArgumentNullException(nameof(items));
        }

        _items = new List<IRenderable>(items.Select(item => new Markup(item)));
    }

    /// <inheritdoc/>
    protected override Measurement Measure(RenderOptions options, int maxWidth)
    {
        var maxPadding = Math.Max(Padding.GetLeftSafe(), Padding.GetRightSafe());

        var itemWidths = _items.Select(item => item.Measure(options, maxWidth).Max).ToArray();
        var columnCount = CalculateColumnCount(maxWidth, itemWidths, _items.Count, maxPadding);
        if (columnCount == 0)
        {
            // Temporary work around for extremely small consoles
            return new Measurement(maxWidth, maxWidth);
        }

        var rows = _items.Count / Math.Max(columnCount, 1);
        var greatestWidth = 0;
        for (var row = 0; row < rows; row += Math.Max(1, columnCount))
        {
            var widths = itemWidths.Skip(row * columnCount).Take(columnCount).ToList();
            var totalWidth = widths.Sum() + (maxPadding * (widths.Count - 1));
            if (totalWidth > greatestWidth)
            {
                greatestWidth = totalWidth;
            }
        }

        return new Measurement(greatestWidth, greatestWidth);
    }

    /// <inheritdoc/>
    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        var maxPadding = Math.Max(Padding.GetLeftSafe(), Padding.GetRightSafe());

        var itemWidths = _items.Select(item => item.Measure(options, maxWidth).Max).ToArray();
        var columnCount = CalculateColumnCount(maxWidth, itemWidths, _items.Count, maxPadding);
        if (columnCount == 0)
        {
            // Temporary work around for extremely small consoles
            columnCount = 1;
        }

        var table = new Table();
        table.NoBorder();
        table.HideHeaders();
        table.PadRightCell = false;

        if (Expand)
        {
            table.Expand();
        }

        // Add columns
        for (var index = 0; index < columnCount; index++)
        {
            table.AddColumn(new TableColumn(string.Empty)
            {
                Padding = Padding,
                NoWrap = true,
            });
        }

        // Add rows
        for (var start = 0; start < _items.Count; start += columnCount)
        {
            table.AddRow(_items.Skip(start).Take(columnCount).ToArray());
        }

        return ((IRenderable)table).Render(options, maxWidth);
    }

    // Algorithm borrowed from https://github.com/willmcgugan/rich/blob/master/rich/columns.py
    private int CalculateColumnCount(int maxWidth, int[] itemWidths, int columnCount, int padding)
    {
        var widths = new Dictionary<int, int>();
        while (columnCount > 1)
        {
            var columnIndex = 0;
            widths.Clear();

            var exceededTotalWidth = false;
            foreach (var renderableWidth in IterateWidths(itemWidths, columnCount))
            {
                widths[columnIndex] = Math.Max(widths.ContainsKey(columnIndex) ? widths[columnIndex] : 0, renderableWidth);
                var totalWidth = widths.Values.Sum() + (padding * (widths.Count - 1));
                if (totalWidth > maxWidth)
                {
                    columnCount = widths.Count - 1;
                    exceededTotalWidth = true;
                    break;
                }
                else
                {
                    columnIndex = (columnIndex + 1) % columnCount;
                }
            }

            if (!exceededTotalWidth)
            {
                break;
            }
        }

        return columnCount;
    }

    private IEnumerable<int> IterateWidths(int[] itemWidths, int columnCount)
    {
        foreach (var width in itemWidths)
        {
            yield return width;
        }

        if (_items.Count % columnCount != 0)
        {
            for (var i = 0; i < columnCount - (_items.Count % columnCount) - 1; i++)
            {
                yield return 0;
            }
        }
    }
}