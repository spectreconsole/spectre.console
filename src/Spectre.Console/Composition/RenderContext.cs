using System.Text;

namespace Spectre.Console.Composition
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
        /// Initializes a new instance of the <see cref="RenderContext"/> class.
        /// </summary>
        /// <param name="encoding">The console's output encoding.</param>
        /// <param name="legacyConsole">A value indicating whether or not this a legacy console (i.e. cmd.exe).</param>
        /// <param name="justification">The justification to use when rendering.</param>
        public RenderContext(Encoding encoding, bool legacyConsole, Justify? justification = null)
        {
            Encoding = encoding ?? throw new System.ArgumentNullException(nameof(encoding));
            LegacyConsole = legacyConsole;
            Justification = justification;
            Unicode = Encoding == Encoding.UTF8 || Encoding == Encoding.Unicode;
        }

        /// <summary>
        /// Creates a new context with the specified justification.
        /// </summary>
        /// <param name="justification">The justification.</param>
        /// <returns>A new <see cref="RenderContext"/> instance with the specified justification.</returns>
        public RenderContext WithJustification(Justify? justification)
        {
            return new RenderContext(Encoding, LegacyConsole, justification);
        }
    }
}
