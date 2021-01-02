using System;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a table title such as a heading or footnote.
    /// </summary>
    public sealed class TableTitle
    {
        /// <summary>
        /// Gets the title text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets or sets the title style.
        /// </summary>
        public Style? Style { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableTitle"/> class.
        /// </summary>
        /// <param name="text">The title text.</param>
        /// <param name="style">The title style.</param>
        public TableTitle(string text, Style? style = null)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Style = style;
        }

        /// <summary>
        /// Sets the title style.
        /// </summary>
        /// <param name="style">The title style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TableTitle SetStyle(Style? style)
        {
            Style = style ?? Style.Plain;
            return this;
        }

        /// <summary>
        /// Sets the title style.
        /// </summary>
        /// <param name="style">The title style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TableTitle SetStyle(string style)
        {
            if (style is null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            Style = Style.Parse(style);
            return this;
        }
    }
}
