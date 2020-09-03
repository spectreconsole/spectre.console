using System;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a header.
    /// </summary>
    public sealed class Header : IAlignable
    {
        /// <summary>
        /// Gets the header text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets or sets the header style.
        /// </summary>
        public Style? Style { get; set; }

        /// <summary>
        /// Gets or sets the header alignment.
        /// </summary>
        public Justify? Alignment { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        /// <param name="text">The header text.</param>
        /// <param name="style">The header style.</param>
        /// <param name="alignment">The header alignment.</param>
        public Header(string text, Style? style = null, Justify? alignment = null)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Style = style;
            Alignment = alignment;
        }

        /// <summary>
        /// Sets the header style.
        /// </summary>
        /// <param name="style">The header style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Header SetStyle(Style? style)
        {
            Style = style ?? Style.Plain;
            return this;
        }

        /// <summary>
        /// Sets the header alignment.
        /// </summary>
        /// <param name="alignment">The header alignment.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Header SetAlignment(Justify alignment)
        {
            Alignment = alignment;
            return this;
        }
    }
}
