using System.Collections.Generic;
using Spectre.Console.Internal;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A renderable piece of markup text.
    /// </summary>
    public sealed class Markup : Renderable, IAlignable
    {
        private readonly Text _text;

        /// <inheritdoc/>
        public Justify? Alignment
        {
            get => _text.Alignment;
            set => _text.Alignment = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Markup"/> class.
        /// </summary>
        /// <param name="text">The markup text.</param>
        /// <param name="style">The style of the text.</param>
        public Markup(string text, Style? style = null)
        {
            _text = MarkupParser.Parse(text, style);
        }

        /// <inheritdoc/>
        protected override Measurement Measure(RenderContext context, int maxWidth)
        {
            return ((IRenderable)_text).Measure(context, maxWidth);
        }

        /// <inheritdoc/>
        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            return ((IRenderable)_text).Render(context, maxWidth);
        }
    }
}
