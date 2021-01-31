namespace Spectre.Console
{
    /// <summary>
    /// Represents a bar chart item.
    /// </summary>
    public interface IBarChartItem
    {
        /// <summary>
        /// Gets the item label.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Gets the item value.
        /// </summary>
        double Value { get; }

        /// <summary>
        /// Gets the item color.
        /// </summary>
        Color? Color { get; }
    }
}
