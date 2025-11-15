namespace Spectre.Console.Tests;

public static class TestConsoleExtensions
{
    private static readonly Regex _lineNumberRegex = new Regex(":\\d+", RegexOptions.Singleline);
    private static readonly Regex _filenameRegex = new Regex("\\sin\\s.*cs:nn", RegexOptions.Multiline);
    private static readonly Regex _pathSeparatorRegex = new Regex(@"[/\\]+");

    public static string WriteNormalizedException(this TestConsole console, Exception ex, ExceptionFormats formats = ExceptionFormats.Default)
    {
        if (!string.IsNullOrWhiteSpace(console.Output))
        {
            throw new InvalidOperationException("Output buffer is not empty.");
        }

        console.WriteException(ex, formats);

        return string.Join("\n", NormalizeStackTrace(console.Output)
            .NormalizeLineEndings()
            .Split(['\n'])
            .Select(line => line.TrimEnd()));
    }

    public static string NormalizeStackTrace(string text)
    {
        // First normalize line numbers
        text = _lineNumberRegex.Replace(text, ":nn");

        // Then normalize paths and filenames
        text = _filenameRegex.Replace(text, match =>
        {
            var value = match.Value;
            var index = value.LastIndexOfAny(new[] { '\\', '/' });
            var filename = value.Substring(index + 1, value.Length - index - 1);

            return $" in /xyz/{filename}";
        });

        // Finally normalize any remaining path separators
        return _pathSeparatorRegex.Replace(text, "/");
    }
}