using System;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a panel header.
    /// </summary>
    public sealed class PanelHeader : IAlignable
    {
        /// <summary>
        /// Gets the panel header text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets or sets the panel header style.
        /// </summary>
        public Style? Style { get; set; }

        /// <summary>
        /// Gets or sets the panel header alignment.
        /// </summary>
        public Justify? Alignment { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelHeader"/> class.
        /// </summary>
        /// <param name="text">The panel header text.</param>
        /// <param name="style">The panel header style.</param>
        /// <param name="alignment">The panel header alignment.</param>
        public PanelHeader(string text, Style? style = null, Justify? alignment = null)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Style = style;
            Alignment = alignment;
        }

        /// <summary>
        /// Sets the panel header style.
        /// </summary>
        /// <param name="style">The panel header style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public PanelHeader SetStyle(Style? style)
        {
            Style = style ?? Style.Plain;
            return this;
        }

        /// <summary>
        /// Sets the panel header style.
        /// </summary>
        /// <param name="style">The panel header style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public PanelHeader SetStyle(string style)
        {
            if (style is null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            Style = Style.Parse(style);
            return this;
        }

        /// <summary>
        /// Sets the panel header alignment.
        /// </summary>
        /// <param name="alignment">The panel header alignment.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public PanelHeader SetAlignment(Justify alignment)
        {
            Alignment = alignment;
            return this;
        }
    }
}
