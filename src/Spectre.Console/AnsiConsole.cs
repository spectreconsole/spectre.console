using System;
using Spectre.Console.Internal;

namespace Spectre.Console
{
    /// <summary>
    /// A console capable of writing ANSI escape sequences.
    /// </summary>
    public static partial class AnsiConsole
    {
        private static readonly Lazy<IAnsiConsole> _console = new Lazy<IAnsiConsole>(() =>
        {
            var console = Create(new AnsiConsoleSettings
            {
                Ansi = AnsiSupport.Detect,
                ColorSystem = ColorSystemSupport.Detect,
                Out = System.Console.Out,
            });
            Created = true;
            return console;
        });

        private static Recorder? _recorder;

        /// <summary>
        /// Gets the underlying <see cref="IAnsiConsole"/>.
        /// </summary>
        public static IAnsiConsole Console => _recorder ?? _console.Value;

        /// <summary>
        /// Gets the <see cref="IAnsiConsoleCursor"/>.
        /// </summary>
        public static IAnsiConsoleCursor Cursor => _recorder?.Cursor ?? _console.Value.Cursor;

        /// <summary>
        /// Gets the console's capabilities.
        /// </summary>
        public static Capabilities Capabilities => Console.Capabilities;

        /// <summary>
        /// Gets the buffer width of the console.
        /// </summary>
        public static int Width
        {
            get => Console.Width;
        }

        /// <summary>
        /// Gets the buffer height of the console.
        /// </summary>
        public static int Height
        {
            get => Console.Height;
        }

        /// <summary>
        /// Creates a new <see cref="IAnsiConsole"/> instance
        /// from the provided settings.
        /// </summary>
        /// <param name="settings">The settings to use.</param>
        /// <returns>An <see cref="IAnsiConsole"/> instance.</returns>
        public static IAnsiConsole Create(AnsiConsoleSettings settings)
        {
            return BackendBuilder.Build(settings);
        }
    }
}
