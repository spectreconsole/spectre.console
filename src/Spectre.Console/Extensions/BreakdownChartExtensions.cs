namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="BreakdownChart"/>.
/// </summary>
public static class BreakdownChartExtensions
{
    /// <param name="chart">The breakdown chart.</param>
    extension(BreakdownChart chart)
    {
        /// <summary>
        /// Adds an item to the breakdown chart.
        /// </summary>
        /// <param name="label">The item label.</param>
        /// <param name="value">The item value.</param>
        /// <param name="color">The item color.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BreakdownChart AddItem(string label, double value, Color color)
        {
            if (chart is null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

            chart.Data.Add(new BreakdownChartItem(label, value, color));
            return chart;
        }
    }

    /// <param name="chart">The breakdown chart.</param>
    extension(BreakdownChart chart)
    {
        /// <summary>
        /// Adds an item to the breakdown chart.
        /// </summary>
        /// <typeparam name="T">A type that implements <see cref="IBreakdownChartItem"/>.</typeparam>
        /// <param name="item">The item.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BreakdownChart AddItem<T>(T item)
            where T : IBreakdownChartItem
        {
            if (chart is null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

            if (item is BreakdownChartItem chartItem)
            {
                chart.Data.Add(chartItem);
            }
            else
            {
                chart.Data.Add(
                    new BreakdownChartItem(
                        item.Label,
                        item.Value,
                        item.Color));
            }

            return chart;
        }

        /// <summary>
        /// Adds multiple items to the breakdown chart.
        /// </summary>
        /// <typeparam name="T">A type that implements <see cref="IBreakdownChartItem"/>.</typeparam>
        /// <param name="items">The items.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BreakdownChart AddItems<T>(IEnumerable<T> items)
            where T : IBreakdownChartItem
        {
            if (chart is null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var item in items)
            {
                AddItem(chart, item);
            }

            return chart;
        }

        /// <summary>
        /// Adds multiple items to the breakdown chart.
        /// </summary>
        /// <typeparam name="T">A type that implements <see cref="IBarChartItem"/>.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="converter">The converter that converts instances of <c>T</c> to <see cref="IBreakdownChartItem"/>.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BreakdownChart AddItems<T>(IEnumerable<T> items, Func<T, IBreakdownChartItem> converter)
        {
            if (chart is null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (converter is null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            foreach (var item in items)
            {
                chart.Data.Add(converter(item));
            }

            return chart;
        }

        /// <summary>
        /// Sets the width of the breakdown chart.
        /// </summary>
        /// <param name="width">The breakdown chart width.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BreakdownChart Width(int? width)
        {
            if (chart is null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

            chart.Width = width;
            return chart;
        }

        /// <summary>
        /// Tags will be shown.
        /// </summary>
        /// <param name="func">The value formatter to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BreakdownChart UseValueFormatter(Func<double, CultureInfo, string>? func)
        {
            if (chart is null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

            chart.ValueFormatter = func;
            return chart;
        }

        /// <summary>
        /// Tags will be shown.
        /// </summary>
        /// <param name="func">The value formatter to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BreakdownChart UseValueFormatter(Func<double, string>? func)
        {
            if (chart is null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

            chart.ValueFormatter = func != null
                ? (value, _) => func(value)
                : null;

            return chart;
        }

        /// <summary>
        /// Tags will be shown.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BreakdownChart ShowPercentage()
        {
            if (chart is null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

            chart.ValueFormatter = (value, culture) => string.Format(culture, "{0}%", value);

            return chart;
        }

        /// <summary>
        /// Tags will be shown.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BreakdownChart ShowTags()
        {
            return ShowTags(chart, true);
        }

        /// <summary>
        /// Tags will be not be shown.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BreakdownChart HideTags()
        {
            return ShowTags(chart, false);
        }

        /// <summary>
        /// Sets whether or not tags will be shown.
        /// </summary>
        /// <param name="show">Whether or not tags will be shown.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BreakdownChart ShowTags(bool show)
        {
            if (chart is null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

            chart.ShowTags = show;
            return chart;
        }

        /// <summary>
        /// Tag values will be shown.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BreakdownChart ShowTagValues()
        {
            return ShowTagValues(chart, true);
        }

        /// <summary>
        /// Tag values will be not be shown.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BreakdownChart HideTagValues()
        {
            return ShowTagValues(chart, false);
        }

        /// <summary>
        /// Sets whether or not tag values will be shown.
        /// </summary>
        /// <param name="show">Whether or not tag values will be shown.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BreakdownChart ShowTagValues(bool show)
        {
            if (chart is null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

            chart.ShowTagValues = show;
            return chart;
        }

        /// <summary>
        /// Chart and tags is rendered in compact mode.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BreakdownChart Compact()
        {
            return Compact(chart, true);
        }

        /// <summary>
        /// Chart and tags is rendered in full size mode.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BreakdownChart FullSize()
        {
            return Compact(chart, false);
        }

        /// <summary>
        /// Sets whether or not the chart and tags should be rendered in compact mode.
        /// </summary>
        /// <param name="compact">Whether or not the chart and tags should be rendered in compact mode.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BreakdownChart Compact(bool compact)
        {
            if (chart is null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

            chart.Compact = compact;
            return chart;
        }

        /// <summary>
        /// Sets the <see cref="BreakdownChart.ValueColor"/>.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BreakdownChart WithValueColor(Color color)
        {
            if (chart is null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

            chart.ValueColor = color;
            return chart;
        }
    }
}