using System;

namespace Spectre.Console
{
    /// <summary>
    /// An item that's shown in a bar chart.
    /// </summary>
    public sealed class BarChartItem : IBarChartItem
    {
        /// <summary>
        /// Gets the item label.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Gets the item value.
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// Gets the item color.
        /// </summary>
        public Color? Color { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BarChartItem"/> class.
        /// </summary>
        /// <param name="label">The item label.</param>
        /// <param name="value">The item value.</param>
        /// <param name="color">The item color.</param>
        public BarChartItem(string label, double value, Color? color = null)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
            Value = value;
            Color = color;
        }
    }
}
