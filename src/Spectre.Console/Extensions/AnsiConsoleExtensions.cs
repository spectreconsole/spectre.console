using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IAnsiConsole"/>.
    /// </summary>
    public static partial class AnsiConsoleExtensions
    {
        /// <summary>
        /// Creates a recorder for the specified console.
        /// </summary>
        /// <param name="console">The console to record.</param>
        /// <returns>A recorder for the specified console.</returns>
        public static Recorder CreateRecorder(this IAnsiConsole console)
        {
            return new Recorder(console);
        }

        /// <summary>
        /// Writes the specified string value to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="text">The text to write.</param>
        public static void Write(this IAnsiConsole console, string text)
        {
            console.Write(new Text(text, Style.Plain));
        }

        /// <summary>
        /// Writes the specified string value to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="text">The text to write.</param>
        /// <param name="style">The text style.</param>
        public static void Write(this IAnsiConsole console, string text, Style style)
        {
            console.Write(new Text(text, style));
        }

        /// <summary>
        /// Writes an empty line to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        public static void WriteLine(this IAnsiConsole console)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.Write(new Text(Environment.NewLine, Style.Plain));
        }

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="text">The text to write.</param>
        public static void WriteLine(this IAnsiConsole console, string text)
        {
            WriteLine(console, text, Style.Plain);
        }

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="text">The text to write.</param>
        /// <param name="style">The text style.</param>
        public static void WriteLine(this IAnsiConsole console, string text, Style style)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            console.Write(text + Environment.NewLine, style);
        }

        /// <summary>
        /// Gets the segments for a renderable.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="renderable">The renderable.</param>
        /// <returns>An enumerable containing segments representing the specified <see cref="IRenderable"/>.</returns>
        public static IEnumerable<Segment> GetSegments(this IAnsiConsole console, IRenderable renderable)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            if (renderable is null)
            {
                throw new ArgumentNullException(nameof(renderable));
            }

            var context = new RenderContext(console.Profile.ColorSystem, console.Profile.Capabilities);
            var renderables = console.Pipeline.Process(context, new[] { renderable });

            var segments = renderables
                .Select(renderable => renderable.Render(context, console.Profile.Width))
                .SelectMany(_ => _);

            return Segment.Merge(segments);
        }
    }
}
