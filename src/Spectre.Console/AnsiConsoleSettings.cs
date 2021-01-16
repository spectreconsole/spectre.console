using System.Collections.Generic;
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
        /// Gets or sets the out buffer.
        /// </summary>
        public TextWriter? Out { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the
        /// terminal is interactive or not.
        /// </summary>
        public InteractionSupport Interactive { get; set; }

        /// <summary>
        /// Gets or sets the environment variables.
        /// If not value is provided the default environment variables will be used.
        /// </summary>
        public Dictionary<string, string>? EnvironmentVariables { get; set; }

        /// <summary>
        /// Gets or sets the profile enrichers to use.
        /// </summary>
        public List<IProfileEnricher>? Enrichers { get; set; }
    }
}
