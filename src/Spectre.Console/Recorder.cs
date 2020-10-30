using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A console recorder used to record output from a console.
    /// </summary>
    public sealed class Recorder : IAnsiConsole, IDisposable
    {
        private readonly IAnsiConsole _console;
        private readonly List<Segment> _recorded;

        /// <inheritdoc/>
        public Capabilities Capabilities => _console.Capabilities;

        /// <inheritdoc/>
        public Encoding Encoding => _console.Encoding;

        /// <inheritdoc/>
        public IAnsiConsoleCursor Cursor => _console.Cursor;

        /// <inheritdoc/>
        public IAnsiConsoleInput Input => _console.Input;

        /// <inheritdoc/>
        public int Width => _console.Width;

        /// <inheritdoc/>
        public int Height => _console.Height;

        /// <summary>
        /// Initializes a new instance of the <see cref="Recorder"/> class.
        /// </summary>
        /// <param name="console">The console to record output for.</param>
        public Recorder(IAnsiConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _recorded = new List<Segment>();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Only used for scoping.
        }

        /// <inheritdoc/>
        public void Clear(bool home)
        {
            _console.Clear(home);
        }

        /// <inheritdoc/>
        public void Write(Segment segment)
        {
            if (segment is null)
            {
                throw new ArgumentNullException(nameof(segment));
            }

            // Don't record control codes.
            if (!segment.IsControlCode)
            {
                _recorded.Add(segment);
            }

            _console.Write(segment);
        }

        /// <summary>
        /// Exports the recorded data.
        /// </summary>
        /// <param name="encoder">The encoder.</param>
        /// <returns>The recorded data represented as a string.</returns>
        public string Export(IAnsiConsoleEncoder encoder)
        {
            if (encoder is null)
            {
                throw new ArgumentNullException(nameof(encoder));
            }

            return encoder.Encode(_recorded);
        }
    }
}
