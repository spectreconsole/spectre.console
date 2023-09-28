namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="BarChart"/>.
/// </summary>
public static class VerticalBarChartExtensions
{
    /// <summary>
    /// Sets the vertical bar chart Color.
    /// </summary>
    /// <param name="chart">The vertical bar chart.</param>
    /// <param name="color">The color.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static VerticalBarChart SetColor(this VerticalBarChart chart, Color color)
    {
        if (chart is null)
        {
            throw new ArgumentNullException(nameof(chart));
        }

        chart.Color = color;
        return chart;
    }

    /// <summary>
    /// Sets the vertical bar chart height.
    /// </summary>
    /// <param name="chart">The vertical bar chart.</param>
    /// <param name="height">The height.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static VerticalBarChart SetHeight(this VerticalBarChart chart, int height)
    {
        if (chart is null)
        {
            throw new ArgumentNullException(nameof(chart));
        }

        chart.Height = height;
        return chart;
    }

    /// <summary>
    /// Adds data to the vertical bar chart.
    /// </summary>
    /// <param name="chart">The vertical bar chart.</param>
    /// <param name="data">The values.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static VerticalBarChart SetData(this VerticalBarChart chart, IEnumerable<double> data)
    {
        if (chart is null)
        {
            throw new ArgumentNullException(nameof(chart));
        }

        chart.Data.AddRange(data);
        return chart;
    }
}