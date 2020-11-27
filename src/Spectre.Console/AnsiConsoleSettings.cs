using System.IO;

namespace Spectre.Console
{
    /// <summary>
    /// Settings used when building a <see cref="IAnsiConsole"/>.
    /// </summary>
    public sealed class AnsiConsoleSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether or
        /// not ANSI escape sequences are supported.
        /// </summary>
        public AnsiSupport Ansi { get; set; }

        /// <summary>
        /// Gets or sets the color system to use.
        /// </summary>
        public ColorSystemSupport ColorSystem { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or
        /// not the console is interactive.
        /// </summary>
        public InteractionSupport Interactive { get; set; }

        /// <summary>
        /// Gets or sets the link identity generator.
        /// </summary>
        public ILinkIdentityGenerator? LinkIdentityGenerator { get; set; }

        /// <summary>
        /// Gets or sets the out buffer.
        /// </summary>
        public TextWriter? Out { get; set; }
    }
}
