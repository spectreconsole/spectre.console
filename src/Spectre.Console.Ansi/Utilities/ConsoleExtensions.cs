#if NET10_0_OR_GREATER
using System.Threading;

namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="System.Console"/>.
/// </summary>
public static class SystemConsoleExtensions
{
    private static readonly Lock _lock = LockFactory.Create();
    private static AnsiWriter? _writer;
    private static AnsiMarkup? _markup;

    private static (AnsiWriter, AnsiMarkup) GetAnsiWriter()
    {
        _writer ??= new AnsiWriter(System.Console.Out);
        _markup ??= new AnsiMarkup(_writer);
        return (_writer, _markup);
    }

    extension(System.Console)
    {
        /// <summary>
        /// Write ANSI, using the default <see cref="AnsiWriter"/> instance.
        /// </summary>
        /// <param name="action">The <see cref="AnsiWriter"/> action</param>
        public static void Ansi(Action<AnsiWriter> action)
        {
            lock (_lock)
            {
                var (writer, _) = GetAnsiWriter();
                action(writer);
            }
        }

        /// <summary>
        /// Writes the specified markup to the console, using the default <see cref="AnsiWriter"/> instance.
        /// </summary>
        /// <param name="markup">The markup.</param>
        public static void Markup(string markup)
        {
            lock (_lock)
            {
                var (_, writer) = GetAnsiWriter();
                writer.Write(markup);
            }
        }

        /// <summary>
        /// Writes the specified markup to the console, followed by the current line terminator,
        /// using the default <see cref="AnsiWriter"/> instance.
        /// </summary>
        /// <param name="markup">The markup.</param>
        public static void MarkupLine(string markup)
        {
            lock (_lock)
            {
                var (_, writer) = GetAnsiWriter();
                writer.WriteLine(markup);
            }
        }
    }
}
#endif