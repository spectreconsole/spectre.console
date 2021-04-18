using System;
using Spectre.Console.Cli;

namespace Spectre.Console.Testing
{
    /// <summary>
    /// Represents a <see cref="CommandApp"/> runtime failure.
    /// </summary>
    public sealed class CommandAppFailure
    {
        /// <summary>
        /// Gets the exception that was thrown.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Gets the console output.
        /// </summary>
        public string Output { get; }

        internal CommandAppFailure(Exception exception, string output)
        {
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
            Output = output.NormalizeLineEndings()
                .TrimLines()
                .Trim();
        }
    }
}
