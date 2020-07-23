using System;
using Spectre.Console.Internal;

namespace Spectre.Console
{
    /// <summary>
    /// A console capable of writing ANSI escape sequences.
    /// </summary>
    public static class AnsiConsole
    {
        private static readonly Lazy<IAnsiConsole> _console = new Lazy<IAnsiConsole>(() =>
        {
            return Create(new AnsiConsoleSettings
            {
                Ansi = AnsiSupport.Detect,
                ColorSystem = ColorSystemSupport.Detect,
                Out = System.Console.Out,
            });
        });

        /// <summary>
        /// Gets the current renderer.
        /// </summary>
        public static IAnsiConsole Console => _console.Value;

        /// <summary>
        /// Gets the console's capabilities.
        /// </summary>
        public static AnsiConsoleCapabilities Capabilities => Console.Capabilities;

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
        /// Gets or sets the style.
        /// </summary>
        public static Styles Style
        {
            get => Console.Style;
            set => Console.Style = value;
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
        /// Resets colors and styles to the default ones.
        /// </summary>
        public static void Reset()
        {
            Console.Reset();
        }

        /// <summary>
        /// Resets the current style back to the default one.
        /// </summary>
        public static void ResetStyle()
        {
            Console.ResetStyle();
        }

        /// <summary>
        /// Resets the foreground and background colors to the default ones.
        /// </summary>
        public static void ResetColors()
        {
            Console.ResetColors();
        }

        /// <summary>
        /// Writes the content to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void Write(string value)
        {
            Console.Write(value);
        }

        /// <summary>
        /// Writes an empty line to the console.
        /// </summary>
        public static void WriteLine()
        {
            Console.WriteLine();
        }

        /// <summary>
        /// Writes a line to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(string value)
        {
            Console.WriteLine(value);
        }
    }
}
