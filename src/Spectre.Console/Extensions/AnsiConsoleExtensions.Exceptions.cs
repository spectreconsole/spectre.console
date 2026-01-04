namespace Spectre.Console;

public static partial class AnsiConsoleExtensions
{
    extension(AnsiConsole)
    {
        /// <summary>
        /// Writes an exception to the console.
        /// </summary>
        /// <param name="exception">The exception to write to the console.</param>
        /// <param name="format">The exception format options.</param>
        [RequiresDynamicCode(ExceptionFormatter.AotWarning)]
        public static void WriteException(Exception exception, ExceptionFormats format = ExceptionFormats.Default)
        {
            AnsiConsole.Console.WriteException(exception, format);
        }

        /// <summary>
        /// Writes an exception to the console.
        /// </summary>
        /// <param name="exception">The exception to write to the console.</param>
        /// <param name="settings">The exception settings.</param>
        [RequiresDynamicCode(ExceptionFormatter.AotWarning)]
        public static void WriteException(Exception exception, ExceptionSettings settings)
        {
            AnsiConsole.Console.WriteException(exception, settings);
        }
    }

    /// <param name="console">The console.</param>
    extension(IAnsiConsole console)
    {
        /// <summary>
        /// Writes an exception to the console.
        /// </summary>
        /// <param name="exception">The exception to write to the console.</param>
        /// <param name="format">The exception format options.</param>
        [RequiresDynamicCode(ExceptionFormatter.AotWarning)]
        public void WriteException(Exception exception, ExceptionFormats format = ExceptionFormats.Default)
        {
            console.Write(exception.GetRenderable(format));
        }

        /// <summary>
        /// Writes an exception to the console.
        /// </summary>
        /// <param name="exception">The exception to write to the console.</param>
        /// <param name="settings">The exception settings.</param>
        [RequiresDynamicCode(ExceptionFormatter.AotWarning)]
        public void WriteException(Exception exception, ExceptionSettings settings)
        {
            console.Write(exception.GetRenderable(settings));
        }
    }
}