using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A console recorder used to record output from a console.
    /// </summary>
    public class Recorder : IAnsiConsole, IDisposable
    {
        private readonly IAnsiConsole _console;
        private readonly List<Segment> _recorded;

        /// <inheritdoc/>
        public Profile Profile => _console.Profile;

        /// <inheritdoc/>
        public IAnsiConsoleCursor Cursor => _console.Cursor;

        /// <inheritdoc/>
        public IAnsiConsoleInput Input => _console.Input;

        /// <inheritdoc/>
        public RenderPipeline Pipeline => _console.Pipeline;

        /// <summary>
        /// Gets a list containing all recorded segments.
        /// </summary>
        protected List<Segment> Recorded => _recorded;

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
        [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize")]
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
        public void Write(IEnumerable<Segment> segments)
        {
            if (segments is null)
            {
                throw new ArgumentNullException(nameof(segments));
            }

            Record(segments);

            _console.Write(segments);
        }

        internal Recorder Clone(IAnsiConsole console)
        {
            var recorder = new Recorder(console);
            recorder.Recorded.AddRange(Recorded);
            return recorder;
        }

        /// <summary>
        /// Records the specified segments.
        /// </summary>
        /// <param name="segments">The segments to be recorded.</param>
        protected virtual void Record(IEnumerable<Segment> segments)
        {
            Recorded.AddRange(segments.Where(s => !s.IsControlCode));
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
