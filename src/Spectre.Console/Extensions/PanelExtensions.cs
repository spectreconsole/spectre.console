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
        public static Panel SetHeader(this Panel panel, string text, Style? style = null, Justify? alignment = null)
        {
            if (panel is null)
            {
                throw new ArgumentNullException(nameof(panel));
            }

            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return SetHeader(panel, new PanelHeader(text, style, alignment));
        }

        /// <summary>
        /// Sets the panel header.
        /// </summary>
        /// <param name="panel">The panel.</param>
        /// <param name="header">The header to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Panel SetHeader(this Panel panel, PanelHeader header)
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
