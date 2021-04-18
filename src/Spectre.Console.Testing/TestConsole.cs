using System;
using System.Collections.Generic;
using System.IO;
using Spectre.Console.Rendering;

namespace Spectre.Console.Testing
{
    /// <summary>
    /// A testable console.
    /// </summary>
    public sealed class TestConsole : IAnsiConsole, IDisposable
    {
        private readonly IAnsiConsole _console;
        private readonly StringWriter _writer;
        private IAnsiConsoleCursor? _cursor;

        /// <inheritdoc/>
        public Profile Profile => _console.Profile;

        /// <inheritdoc/>
        public IExclusivityMode ExclusivityMode => _console.ExclusivityMode;

        /// <summary>
        /// Gets the console input.
        /// </summary>
        public TestConsoleInput Input { get; }

        /// <inheritdoc/>
        public RenderPipeline Pipeline => _console.Pipeline;

        /// <inheritdoc/>
        public IAnsiConsoleCursor Cursor => _cursor ?? _console.Cursor;

        /// <inheritdoc/>
        IAnsiConsoleInput IAnsiConsole.Input => Input;

        /// <summary>
        /// Gets the console output.
        /// </summary>
        public string Output => _writer.ToString();

        /// <summary>
        /// Gets the console output lines.
        /// </summary>
        public IReadOnlyList<string> Lines => Output.NormalizeLineEndings().TrimEnd('\n').Split(new char[] { '\n' });

        /// <summary>
        /// Gets or sets a value indicating whether or not VT/ANSI sequences
        /// should be emitted to the console.
        /// </summary>
        public bool EmitAnsiSequences { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestConsole"/> class.
        /// </summary>
        public TestConsole()
        {
            _writer = new StringWriter();
            _cursor = new NoopCursor();

            Input = new TestConsoleInput();
            EmitAnsiSequences = false;

            var factory = new AnsiConsoleFactory();
            _console = factory.Create(new AnsiConsoleSettings
            {
                Ansi = AnsiSupport.Yes,
                ColorSystem = (ColorSystemSupport)ColorSystem.TrueColor,
                Out = new AnsiConsoleOutput(_writer),
                Interactive = InteractionSupport.No,
                ExclusivityMode = new NoopExclusivityMode(),
                Enrichment = new ProfileEnrichment
                {
                    UseDefaultEnrichers = false,
                },
            });

            _console.Profile.Width = 80;
            _console.Profile.Height = 24;
            _console.Profile.Capabilities.Ansi = true;
            _console.Profile.Capabilities.Unicode = true;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _writer.Dispose();
        }

        /// <inheritdoc/>
        public void Clear(bool home)
        {
            _console.Clear(home);
        }

        /// <inheritdoc/>
        public void Write(IRenderable renderable)
        {
            if (EmitAnsiSequences)
            {
                _console.Write(renderable);
            }
            else
            {
                foreach (var segment in renderable.GetSegments(this))
                {
                    if (segment.IsControlCode)
                    {
                        continue;
                    }

                    Profile.Out.Writer.Write(segment.Text);
                }
            }
        }

        internal void SetCursor(IAnsiConsoleCursor? cursor)
        {
            _cursor = cursor;
        }
    }
}
