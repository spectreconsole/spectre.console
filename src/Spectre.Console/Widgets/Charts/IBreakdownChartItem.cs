namespace Spectre.Console
{
    /// <summary>
    /// Represents a breakdown chart item.
    /// </summary>
    public interface IBreakdownChartItem
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
        Color Color { get; }
    }
}
