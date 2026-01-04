namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="Layout"/>.
/// </summary>
public static class LayoutExtensions
{
    /// <param name="layout">The layout.</param>
    extension(Layout layout)
    {
        /// <summary>
        /// Sets the ratio of the layout.
        /// </summary>
        /// <param name="ratio">The ratio.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Layout Ratio(int ratio)
        {
            if (layout is null)
            {
                throw new ArgumentNullException(nameof(layout));
            }

            layout.Ratio = ratio;
            return layout;
        }

        /// <summary>
        /// Sets the size of the layout.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Layout Size(int size)
        {
            if (layout is null)
            {
                throw new ArgumentNullException(nameof(layout));
            }

            layout.Size = size;
            return layout;
        }

        /// <summary>
        /// Sets the minimum width of the layout.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Layout MinimumSize(int size)
        {
            if (layout is null)
            {
                throw new ArgumentNullException(nameof(layout));
            }

            layout.MinimumSize = size;
            return layout;
        }
    }
}