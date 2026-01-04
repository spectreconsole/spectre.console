namespace Spectre.Console;

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
            if (chart is null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

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
            if (chart is null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

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
        /// Adds multiple items to the bar chart.
        /// </summary>
        /// <typeparam name="T">A type that implements <see cref="IBarChartItem"/>.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="converter">The converter that converts instances of <c>T</c> to <see cref="BarChartItem"/>.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BarChart AddItems<T>(IEnumerable<T> items, Func<T, BarChartItem> converter)
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
        /// Sets the value formatter for the bar chart using culture info.
        /// </summary>
        /// <param name="func">The value formatter function with culture info.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BarChart UseValueFormatter(Func<double, CultureInfo, string>? func)
        {
            if (chart is null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

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
        /// Sets the width of the bar chart.
        /// </summary>
        /// <param name="width">The bar chart width.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BarChart Width(int? width)
        {
            if (chart is null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

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
            if (chart is null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

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
            if (chart is null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

            chart.ShowValues = show;
            return chart;
        }

        /// <summary>
        /// Aligns the label to the left.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BarChart LeftAlignLabel()
        {
            if (chart is null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

            chart.LabelAlignment = Justify.Left;
            return chart;
        }

        /// <summary>
        /// Centers the label.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BarChart CenterLabel()
        {
            if (chart is null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

            chart.LabelAlignment = Justify.Center;
            return chart;
        }

        /// <summary>
        /// Aligns the label to the right.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public BarChart RightAlignLabel()
        {
            if (chart is null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

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
            if (chart is null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

            chart.MaxValue = maxValue;
            return chart;
        }
    }
}