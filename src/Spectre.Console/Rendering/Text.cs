using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A renderable piece of text.
    /// </summary>
    [DebuggerDisplay("{_text,nq}")]
    [SuppressMessage("Naming", "CA1724:Type names should not match namespaces")]
    public sealed class Text : Renderable, IAlignable, IOverflowable
    {
        private readonly Paragraph _paragraph;

        /// <summary>
        /// Gets an empty <see cref="Text"/> instance.
        /// </summary>
        public static Text Empty { get; } = new Text(string.Empty);

        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="style">The style of the text.</param>
        public Text(string text, Style? style = null)
        {
            _paragraph = new Paragraph(text, style);
        }

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        public Justify? Alignment
        {
            get => _paragraph.Alignment;
            set => _paragraph.Alignment = value;
        }

        /// <summary>
        /// Gets or sets the text overflow strategy.
        /// </summary>
        public Overflow? Overflow
        {
            get => _paragraph.Overflow;
            set => _paragraph.Overflow = value;
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
    }
}
