using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="Panel"/>.
    /// </summary>
    public static class PanelExtensions
    {
        /// <summary>
        /// Sets the panel header.
        /// </summary>
        /// <param name="panel">The panel.</param>
        /// <param name="text">The header text.</param>
        /// <param name="style">The header style.</param>
        /// <param name="alignment">The header alignment.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Panel Header(this Panel panel, string text, Style? style = null, Justify? alignment = null)
        {
            if (panel is null)
            {
                throw new ArgumentNullException(nameof(panel));
            }

            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            style ??= panel.Header?.Style;
            alignment ??= panel.Header?.Alignment;

            return Header(panel, new PanelHeader(text, style, alignment));
        }

        /// <summary>
        /// Sets the panel header style.
        /// </summary>
        /// <param name="panel">The panel.</param>
        /// <param name="style">The header style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Panel HeaderStyle(this Panel panel, Style style)
        {
            if (panel is null)
            {
                throw new ArgumentNullException(nameof(panel));
            }

            if (style is null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            if (panel.Header != null)
            {
                // Update existing style
                panel.Header.Style = style;
            }
            else
            {
                // Create header
                Header(panel, string.Empty, style, null);
            }

            return panel;
        }

        /// <summary>
        /// Sets the panel header alignment.
        /// </summary>
        /// <param name="panel">The panel.</param>
        /// <param name="alignment">The header alignment.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Panel HeaderAlignment(this Panel panel, Justify alignment)
        {
            if (panel is null)
            {
                throw new ArgumentNullException(nameof(panel));
            }

            if (panel.Header != null)
            {
                // Update existing style
                panel.Header.Alignment = alignment;
            }
            else
            {
                // Create header
                Header(panel, string.Empty, null, alignment);
            }

            return panel;
        }

        /// <summary>
        /// Sets the panel header.
        /// </summary>
        /// <param name="panel">The panel.</param>
        /// <param name="header">The header to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Panel Header(this Panel panel, PanelHeader header)
        {
            if (panel is null)
            {
                throw new ArgumentNullException(nameof(panel));
            }

            panel.Header = header;
            return panel;
        }
    }
}
