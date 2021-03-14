using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A renderable piece of markup text.
    /// </summary>
    [SuppressMessage("Naming", "CA1724:Type names should not match namespaces")]
    public sealed class Markup : Renderable, IAlignable, IOverflowable
    {
        private readonly Paragraph _paragraph;

        /// <inheritdoc/>
        public Justify? Alignment
        {
            get => _paragraph.Alignment;
            set => _paragraph.Alignment = value;
        }

        /// <inheritdoc/>
        public Overflow? Overflow
        {
            get => _paragraph.Overflow;
            set => _paragraph.Overflow = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Markup"/> class.
        /// </summary>
        /// <param name="text">The markup text.</param>
        /// <param name="style">The style of the text.</param>
        public Markup(string text, Style? style = null)
        {
            _paragraph = MarkupParser.Parse(text, style);
        }

        /// <inheritdoc/>
        protected override Measurement Measure(RenderContext context, int maxWidth)
        {
            return ((IRenderable)_paragraph).Measure(context, maxWidth);
        }

        /// <inheritdoc/>
        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            return ((IRenderable)_paragraph).Render(context, maxWidth);
        }

        /// <summary>
        /// Escapes text so that it won’t be interpreted as markup.
        /// </summary>
        /// <param name="text">The text to escape.</param>
        /// <returns>A string that is safe to use in markup.</returns>
        public static string Escape(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return text.EscapeMarkup();
        }

        /// <summary>
        /// Removes markup from the specified string.
        /// </summary>
        /// <param name="text">The text to remove markup from.</param>
        /// <returns>A string that does not have any markup.</returns>
        public static string Remove(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return text.RemoveMarkup();
        }
    }
}
