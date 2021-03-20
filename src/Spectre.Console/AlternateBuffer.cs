using System;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents an alternative screen.
    /// Once created, all output the console will be written
    /// to the alternate screen until disposed.
    /// </summary>
    public sealed class Screen : IDisposable
    {
        private readonly IAnsiConsole _console;

        /// <summary>
        /// Initializes a new instance of the <see cref="Screen"/> class.
        /// </summary>
        /// <param name="console">The console.</param>
        public Screen(IAnsiConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));

            if (!_console.Profile.Capabilities.Ansi)
            {
                throw new NotSupportedException("Alternate buffers are not supported since your terminal does not support ANSI.");
            }

            if (!_console.Profile.Capabilities.AlternateBuffer)
            {
                throw new NotSupportedException("Alternate buffers are not supported by your terminal.");
            }

            _console.Write(Segment.Control("\u001b[?1049h\u001b[H"));
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _console.Write(Segment.Control("\u001b[?1049l"));
        }
    }
}
