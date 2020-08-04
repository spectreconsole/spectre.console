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

        /// <summary>
        /// Gets the current renderer.
        /// </summary>
        public static IAnsiConsole Console => _console.Value;

        /// <summary>
        /// Gets the console's capabilities.
        /// </summary>
        public static Capabilities Capabilities => Console.Capabilities;

        internal static bool Created { get; private set; }

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
        /// Gets or sets the foreground color.
        /// </summary>
        public static Color Foreground
        {
            get => Console.Foreground;
            set => Console.SetColor(value, true);
        }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public static Color Background
        {
            get => Console.Background;
            set => Console.SetColor(value, false);
        }

        /// <summary>
        /// Gets or sets the text decoration.
        /// </summary>
        public static Decoration Decoration
        {
            get => Console.Decoration;
            set => Console.Decoration = value;
        }

        /// <summary>
        /// Creates a new <see cref="IAnsiConsole"/> instance
        /// from the provided settings.
        /// </summary>
        /// <param name="settings">The settings to use.</param>
        /// <returns>An <see cref="IAnsiConsole"/> instance.</returns>
        public static IAnsiConsole Create(AnsiConsoleSettings settings)
        {
            return ConsoleBuilder.Build(settings);
        }

        /// <summary>
        /// Resets colors and text decorations.
        /// </summary>
        public static void Reset()
        {
            Console.Reset();
        }

        /// <summary>
        /// Resets the current applied text decorations.
        /// </summary>
        public static void ResetDecoration()
        {
            Console.ResetDecoration();
        }

        /// <summary>
        /// Resets the current applied foreground and background colors.
        /// </summary>
        public static void ResetColors()
        {
            Console.ResetColors();
        }
    }
}
