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
        /// Initializes a new instance of the <see cref="RenderContext"/> class.
        /// </summary>
        /// <param name="encoding">The console's output encoding.</param>
        /// <param name="legacyConsole">A value indicating whether or not this a legacy console (i.e. cmd.exe).</param>
        public RenderContext(Encoding encoding, bool legacyConsole)
        {
            Encoding = encoding ?? throw new System.ArgumentNullException(nameof(encoding));
            LegacyConsole = legacyConsole;
            Unicode = Encoding == Encoding.UTF8 || Encoding == Encoding.Unicode;
        }
    }
}
