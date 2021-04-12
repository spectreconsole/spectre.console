using System;

namespace Spectre.Console
{
    /// <summary>
    /// A console capable of writing ANSI escape sequences.
    /// </summary>
    public static partial class AnsiConsole
    {
        private static Recorder? _recorder;
        private static Lazy<IAnsiConsole> _console = new Lazy<IAnsiConsole>(
            () =>
            {
                var console = Create(new AnsiConsoleSettings
                {
                    Ansi = AnsiSupport.Detect,
                    ColorSystem = ColorSystemSupport.Detect,
                    Out = new AnsiConsoleOutput(System.Console.Out),
                });

                Created = true;
                return console;
            });

        /// <summary>
        /// Gets or sets the underlying <see cref="IAnsiConsole"/>.
        /// </summary>
        public static IAnsiConsole Console
        {
            get
            {
                return _recorder ?? _console.Value;
            }
            set
            {
                _console = new Lazy<IAnsiConsole>(() => value);

                if (_recorder != null)
                {
                    // Recreate the recorder
                    _recorder = _recorder.Clone(value);
                }

                Created = true;
            }
        }

        /// <summary>
        /// Gets the <see cref="IAnsiConsoleCursor"/>.
        /// </summary>
        public static IAnsiConsoleCursor Cursor => _recorder?.Cursor ?? _console.Value.Cursor;

        /// <summary>
        /// Gets the console profile.
        /// </summary>
        public static Profile Profile => Console.Profile;

        /// <summary>
        /// Creates a new <see cref="IAnsiConsole"/> instance
        /// from the provided settings.
        /// </summary>
        /// <param name="settings">The settings to use.</param>
        /// <returns>An <see cref="IAnsiConsole"/> instance.</returns>
        public static IAnsiConsole Create(AnsiConsoleSettings settings)
        {
            var factory = new AnsiConsoleFactory();
            return factory.Create(settings);
        }

        /// <summary>
        /// Clears the console.
        /// </summary>
        public static void Clear()
        {
            Console.Clear();
        }
    }
}
