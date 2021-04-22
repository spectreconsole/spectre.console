using System;

namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Represents a render context.
    /// </summary>
    public sealed class RenderContext
    {
        private readonly IReadOnlyCapabilities _capabilities;

        /// <summary>
        /// Gets the current color system.
        /// </summary>
        public ColorSystem ColorSystem => _capabilities.ColorSystem;

        /// <summary>
        /// Gets a value indicating whether or not VT/Ansi codes are supported.
        /// </summary>
        public bool Ansi => _capabilities.Ansi;

        /// <summary>
        /// Gets a value indicating whether or not unicode is supported.
        /// </summary>
        public bool Unicode => _capabilities.Unicode;

        /// <summary>
        /// Gets the current justification.
        /// </summary>
        public Justify? Justification { get; }

        /// <summary>
        /// Gets a value indicating whether the context want items to render without
        /// line breaks and return a single line where applicable.
        /// </summary>
        internal bool SingleLine { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderContext"/> class.
        /// </summary>
        /// <param name="capabilities">The capabilities.</param>
        /// <param name="justification">The justification.</param>
        public RenderContext(IReadOnlyCapabilities capabilities, Justify? justification = null)
            : this(capabilities, justification, false)
        {
        }

        private RenderContext(IReadOnlyCapabilities capabilities, Justify? justification = null, bool singleLine = false)
        {
            _capabilities = capabilities ?? throw new ArgumentNullException(nameof(capabilities));

            Justification = justification;
            SingleLine = singleLine;
        }

        /// <summary>
        /// Creates a new context with the specified justification.
        /// </summary>
        /// <param name="justification">The justification.</param>
        /// <returns>A new <see cref="RenderContext"/> instance.</returns>
        public RenderContext WithJustification(Justify? justification)
        {
            return new RenderContext(_capabilities, justification, SingleLine);
        }

        /// <summary>
        /// Creates a new context that tell <see cref="IRenderable"/> instances
        /// to not care about splitting things in new lines. Whether or not to
        /// comply to the request is up to the item being rendered.
        /// </summary>
        /// <remarks>
        /// Use with care since this has the potential to mess things up.
        /// Only use this kind of context with items that you know about.
        /// </remarks>
        /// <returns>A new <see cref="RenderContext"/> instance.</returns>
        internal RenderContext WithSingleLine()
        {
            return new RenderContext(_capabilities, Justification, true);
        }
    }
}
