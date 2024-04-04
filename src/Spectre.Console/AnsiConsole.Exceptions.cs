namespace Spectre.Console;

/// <summary>
/// A console capable of writing ANSI escape sequences.
/// </summary>
public static partial class AnsiConsole
{
    /// <summary>
    /// Writes an exception to the console.
    /// </summary>
    /// <param name="exception">The exception to write to the console.</param>
    /// <param name="format">The exception format options.</param>
#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(ExceptionExtensions.UnreferencedWarning)]
#endif
    public static void WriteException(Exception exception, ExceptionFormats format = ExceptionFormats.Default)
    {
        Console.WriteException(exception, format);
    }

    /// <summary>
    /// Writes an exception to the console.
    /// </summary>
    /// <param name="exception">The exception to write to the console.</param>
    /// <param name="settings">The exception settings.</param>
#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(ExceptionExtensions.UnreferencedWarning)]
#endif
    public static void WriteException(Exception exception, ExceptionSettings settings)
    {
        Console.WriteException(exception, settings);
    }
}