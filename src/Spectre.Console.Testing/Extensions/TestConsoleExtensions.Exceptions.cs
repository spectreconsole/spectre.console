namespace Spectre.Console.Testing;

/// <summary>
/// Provides extension methods for working with <see cref="TestConsole"/> in a testing context,
/// including stack trace normalization for consistent and deterministic test output.
/// </summary>
public static partial class TestConsoleExtensions
{
    private static readonly Regex _lineNumberRegex = new Regex(":\\d+", RegexOptions.Singleline);
    private static readonly Regex _filenameRegex = new Regex("\\sin\\s.*cs:nn", RegexOptions.Multiline);

    /// <summary>
    /// Writes the given exception to the <see cref="TestConsole"/> and returns a normalized string
    /// representation of the exception, with file paths and line numbers sanitized.
    /// </summary>
    /// <param name="console">The <see cref="TestConsole"/> to write to.</param>
    /// <param name="ex">The exception to write and normalize.</param>
    /// <param name="formats">Optional formatting options for exception output.</param>
    /// <returns>A normalized string of the exception's output, safe for snapshot testing.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the console's output buffer is not empty before writing the exception.
    /// </exception>
    public static string WriteNormalizedException(this TestConsole console, Exception ex, ExceptionFormats formats = ExceptionFormats.Default)
    {
        if (!string.IsNullOrWhiteSpace(console.Output))
        {
            throw new InvalidOperationException("Output buffer is not empty.");
        }

        console.WriteException(ex, formats);
        return string.Join("\n", NormalizeStackTrace(console.Output)
            .NormalizeLineEndings()
            .Split(new char[] { '\n' })
            .Select(line => line.TrimEnd()));
    }

    /// <summary>
    /// Normalizes a stack trace string by replacing line numbers with ":nn"
    /// and converting full file paths to a fixed placeholder path ("/xyz/filename.cs").
    /// </summary>
    /// <param name="text">The stack trace text to normalize.</param>
    /// <returns>A sanitized stack trace suitable for stable testing output.</returns>
    public static string NormalizeStackTrace(string text)
    {
        text = _lineNumberRegex.Replace(text, match =>
        {
            return ":nn";
        });

        return _filenameRegex.Replace(text, match =>
        {
            var value = match.Value;
            var index = value.LastIndexOfAny(new[] { '\\', '/' });
            var filename = value.Substring(index + 1, value.Length - index - 1);

            return $" in /xyz/{filename}";
        });
    }
}
