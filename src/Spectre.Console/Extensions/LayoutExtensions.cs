namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="Layout"/>.
/// </summary>
public static class LayoutExtensions
{
    /// <summary>
    /// Sets the ratio of the layout.
    /// </summary>
    /// <param name="layout">The layout.</param>
    /// <param name="ratio">The ratio.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Layout Ratio(this Layout layout, int ratio)
    {
        if (layout is null)
        {
            throw new ArgumentNullException(nameof(layout));
        }

        layout.Ratio = ratio;
        return layout;
    }

    /// <summary>
    /// Sets the width of the layout.
    /// </summary>
    /// <param name="layout">The layout.</param>
    /// <param name="width">The width.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Layout Width(this Layout layout, int width)
    {
        if (layout is null)
        {
            throw new ArgumentNullException(nameof(layout));
        }

        layout.Width = width;
        return layout;
    }

    /// <summary>
    /// Sets the minimum width of the layout.
    /// </summary>
    /// <param name="layout">The layout.</param>
    /// <param name="width">The width.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Layout MinimumWidth(this Layout layout, int width)
    {
        if (layout is null)
        {
            throw new ArgumentNullException(nameof(layout));
        }

        layout.MinimumWidth = width;
        return layout;
    }
}
