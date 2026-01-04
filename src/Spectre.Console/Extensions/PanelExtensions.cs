namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="Panel"/>.
/// </summary>
public static class PanelExtensions
{
    /// <param name="panel">The panel.</param>
    extension(Panel panel)
    {
        /// <summary>
        /// Sets the panel header.
        /// </summary>
        /// <param name="text">The header text.</param>
        /// <param name="alignment">The header alignment.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Panel Header(string text, Justify? alignment = null)
        {
            if (panel is null)
            {
                throw new ArgumentNullException(nameof(panel));
            }

            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            alignment ??= panel.Header?.Justification;
            return Header(panel, new PanelHeader(text, alignment));
        }

        /// <summary>
        /// Sets the panel header alignment.
        /// </summary>
        /// <param name="alignment">The header alignment.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Panel HeaderAlignment(Justify alignment)
        {
            if (panel is null)
            {
                throw new ArgumentNullException(nameof(panel));
            }

            if (panel.Header != null)
            {
                // Update existing style
                panel.Header.Justification = alignment;
            }
            else
            {
                // Create header
                Header(panel, string.Empty, alignment);
            }

            return panel;
        }

        /// <summary>
        /// Sets the panel header.
        /// </summary>
        /// <param name="header">The header to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Panel Header(PanelHeader header)
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