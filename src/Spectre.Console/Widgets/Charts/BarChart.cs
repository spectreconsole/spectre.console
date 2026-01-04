namespace Spectre.Console;

/// <summary>
/// A renderable (horizontal) bar chart.
/// </summary>
public sealed class BarChart : Renderable, IHasCulture
{
    /// <summary>
    /// Gets the bar chart data.
    /// </summary>
    public List<IBarChartItem> Data { get; }

    /// <summary>
    /// Gets or sets the width of the bar chart.
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets the bar chart label.
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets the bar chart label alignment.
    /// </summary>
    public Justify? LabelAlignment { get; set; } = Justify.Center;

    /// <summary>
    /// Gets or sets a value indicating whether or not
    /// values should be shown next to each bar.
    /// </summary>
    public bool ShowValues { get; set; } = true;

    /// <summary>
    /// Gets or sets the culture that's used to format values.
    /// </summary>
    /// <remarks>Defaults to invariant culture.</remarks>
    public CultureInfo? Culture { get; set; }

    /// <summary>
    /// Gets or sets the fixed max value for a bar chart.
    /// </summary>
    /// <remarks>Defaults to null, which corresponds to largest value in chart.</remarks>
    public double? MaxValue { get; set; }

    /// <summary>
    /// Gets or sets the function used to format the values of the bar chart.
    /// </summary>
    public Func<double, CultureInfo, string>? ValueFormatter { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BarChart"/> class.
    /// </summary>
    public BarChart()
    {
        Data = new List<IBarChartItem>();
    }

    /// <inheritdoc/>
    protected override Measurement Measure(RenderOptions options, int maxWidth)
    {
        var width = Math.Min(Width ?? maxWidth, maxWidth);
        return new Measurement(width, width);
    }

    /// <inheritdoc/>
    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        var width = Math.Min(Width ?? maxWidth, maxWidth);
        var maxValue = Math.Max(MaxValue ?? 0d, Data.Max(item => item.Value));

        var grid = new Grid();
        grid.Collapse();
        grid.AddColumn(new GridColumn().PadRight(2).RightAligned());
        grid.AddColumn(new GridColumn().PadLeft(0));
        grid.Width = width;

        if (!string.IsNullOrWhiteSpace(Label))
        {
            grid.AddRow(Text.Empty, new Markup(Label).Justify(LabelAlignment));
        }

        foreach (var item in Data)
        {
            grid.AddRow(
                new Markup(item.Label),
                new ProgressBar()
                {
                    Value = item.Value,
                    MaxValue = maxValue,
                    ShowRemaining = false,
                    CompletedStyle = new Style().Foreground(item.Color ?? Color.Default),
                    FinishedStyle = new Style().Foreground(item.Color ?? Color.Default),
                    UnicodeBar = '█',
                    AsciiBar = '█',
                    ShowValue = ShowValues,
                    Culture = Culture,
                    ValueFormatter = ValueFormatter,
                });
        }

        return ((IRenderable)grid).Render(options, width);
    }
}

/// <summary>
/// Contains extension methods for <see cref="BarChart"/>.
/// </summary>
public static class BarChartExtensions
{
    /// <param name="chart">The bar chart.</param>
    extension(BarChart chart)
    {
        /// <summary>
        /// Adds an item to the bar chart.
        /// </summary>
        /// <param name="label">The item label.</param>
        /// <param name="value">The item value.</param>
        /// <param name="color">The item color.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BarChart AddItem(string label, double value, Color? color = null)
        {
            ArgumentNullException.ThrowIfNull(chart);

            chart.Data.Add(new BarChartItem(label, value, color));
            return chart;
        }

        /// <summary>
        /// Adds an item to the bar chart.
        /// </summary>
        /// <typeparam name="T">A type that implements <see cref="IBarChartItem"/>.</typeparam>
        /// <param name="item">The item.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BarChart AddItem<T>(T item)
            where T : IBarChartItem
        {
            ArgumentNullException.ThrowIfNull(chart);

            if (item is BarChartItem barChartItem)
            {
                chart.Data.Add(barChartItem);
            }
            else
            {
                chart.Data.Add(
                    new BarChartItem(
                        item.Label,
                        item.Value,
                        item.Color));
            }

            return chart;
        }

        /// <summary>
        /// Adds multiple items to the bar chart.
        /// </summary>
        /// <typeparam name="T">A type that implements <see cref="IBarChartItem"/>.</typeparam>
        /// <param name="items">The items.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BarChart AddItems<T>(IEnumerable<T> items)
            where T : IBarChartItem
        {
            ArgumentNullException.ThrowIfNull(chart);

            ArgumentNullException.ThrowIfNull(items);

            foreach (var item in items)
            {
                AddItem(chart, item);
            }

            return chart;
        }

        /// <summary>
        /// Adds multiple items to the bar chart.
        /// </summary>
        /// <typeparam name="T">A type that implements <see cref="IBarChartItem"/>.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="converter">The converter that converts instances of <c>T</c> to <see cref="BarChartItem"/>.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BarChart AddItems<T>(IEnumerable<T> items, Func<T, BarChartItem> converter)
        {
            ArgumentNullException.ThrowIfNull(chart);

            ArgumentNullException.ThrowIfNull(items);

            ArgumentNullException.ThrowIfNull(converter);

            foreach (var item in items)
            {
                chart.Data.Add(converter(item));
            }

            return chart;
        }

        /// <summary>
        /// Sets the value formatter for the bar chart using culture info.
        /// </summary>
        /// <param name="func">The value formatter function with culture info.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BarChart UseValueFormatter(Func<double, CultureInfo, string>? func)
        {
            ArgumentNullException.ThrowIfNull(chart);

            chart.ValueFormatter = func;
            return chart;
        }

        /// <summary>
        /// Sets the value formatter for the bar chart.
        /// </summary>
        /// <param name="func">The value formatter to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BarChart UseValueFormatter(Func<double, string>? func)
        {
            ArgumentNullException.ThrowIfNull(chart);

            chart.ValueFormatter = func != null
                ? (value, _) => func(value)
                : null;

            return chart;
        }

        /// <summary>
        /// Sets the width of the bar chart.
        /// </summary>
        /// <param name="width">The bar chart width.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BarChart Width(int? width)
        {
            ArgumentNullException.ThrowIfNull(chart);

            chart.Width = width;
            return chart;
        }

        /// <summary>
        /// Sets the label of the bar chart.
        /// </summary>
        /// <param name="label">The bar chart label.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BarChart Label(string? label)
        {
            ArgumentNullException.ThrowIfNull(chart);

            chart.Label = label;
            return chart;
        }

        /// <summary>
        /// Shows values next to each bar in the bar chart.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BarChart ShowValues()
        {
            return ShowValues(chart, true);
        }

        /// <summary>
        /// Hides values next to each bar in the bar chart.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BarChart HideValues()
        {
            return ShowValues(chart, false);
        }

        /// <summary>
        /// Sets whether or not values should be shown
        /// next to each bar.
        /// </summary>
        /// <param name="show">Whether or not values should be shown next to each bar.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BarChart ShowValues(bool show)
        {
            ArgumentNullException.ThrowIfNull(chart);

            chart.ShowValues = show;
            return chart;
        }

        /// <summary>
        /// Aligns the label to the left.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BarChart LeftAlignLabel()
        {
            ArgumentNullException.ThrowIfNull(chart);

            chart.LabelAlignment = Justify.Left;
            return chart;
        }

        /// <summary>
        /// Centers the label.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BarChart CenterLabel()
        {
            ArgumentNullException.ThrowIfNull(chart);

            chart.LabelAlignment = Justify.Center;
            return chart;
        }

        /// <summary>
        /// Aligns the label to the right.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BarChart RightAlignLabel()
        {
            ArgumentNullException.ThrowIfNull(chart);

            chart.LabelAlignment = Justify.Right;
            return chart;
        }

        /// <summary>
        /// Sets the max fixed value for the chart.
        /// </summary>
        /// <param name="maxValue">Max value for the chart.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BarChart WithMaxValue(double maxValue)
        {
            ArgumentNullException.ThrowIfNull(chart);

            chart.MaxValue = maxValue;
            return chart;
        }
    }
}