using System;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a title.
    /// </summary>
    public sealed class Title : IAlignable
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
        /// Gets or sets the title alignment.
        /// </summary>
        public Justify? Alignment { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Title"/> class.
        /// </summary>
        /// <param name="text">The title text.</param>
        /// <param name="style">The title style.</param>
        /// <param name="alignment">The title alignment.</param>
        public Title(string text, Style? style = null, Justify? alignment = null)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Style = style;
            Alignment = alignment;
        }

        /// <summary>
        /// Sets the title style.
        /// </summary>
        /// <param name="style">The title style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Title SetStyle(Style? style)
        {
            Style = style ?? Style.Plain;
            return this;
        }

        /// <summary>
        /// Sets the title alignment.
        /// </summary>
        /// <param name="alignment">The title alignment.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Title SetAlignment(Justify alignment)
        {
            Alignment = alignment;
            return this;
        }
    }
}
