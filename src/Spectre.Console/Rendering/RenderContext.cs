using System.Text;

namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Represents a render context.
    /// </summary>
    public sealed class RenderContext
    {
        /// <summary>
        /// Gets the console's output encoding.
        /// </summary>
        public Encoding Encoding { get; }

        /// <summary>
        /// Gets a value indicating whether or not this a legacy console (i.e. cmd.exe).
        /// </summary>
        public bool LegacyConsole { get; }

        /// <summary>
        /// Gets a value indicating whether or not unicode is supported.
        /// </summary>
        public bool Unicode { get; }

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
        /// <param name="encoding">The console's output encoding.</param>
        /// <param name="legacyConsole">A value indicating whether or not this a legacy console (i.e. cmd.exe).</param>
        /// <param name="justification">The justification to use when rendering.</param>
        public RenderContext(Encoding encoding, bool legacyConsole, Justify? justification = null)
            : this(encoding, legacyConsole, justification, false)
        {
        }

        private RenderContext(Encoding encoding, bool legacyConsole, Justify? justification = null, bool singleLine = false)
        {
            Encoding = encoding ?? throw new System.ArgumentNullException(nameof(encoding));
            LegacyConsole = legacyConsole;
            Justification = justification;
            Unicode = Encoding == Encoding.UTF8 || Encoding == Encoding.Unicode;
            SingleLine = singleLine;
        }

        /// <summary>
        /// Creates a new context with the specified justification.
        /// </summary>
        /// <param name="justification">The justification.</param>
        /// <returns>A new <see cref="RenderContext"/> instance.</returns>
        public RenderContext WithJustification(Justify? justification)
        {
            return new RenderContext(Encoding, LegacyConsole, justification);
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
            return new RenderContext(Encoding, LegacyConsole, Justification, true);
        }
    }
}
