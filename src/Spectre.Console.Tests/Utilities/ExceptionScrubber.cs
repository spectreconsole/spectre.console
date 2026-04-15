using System.Diagnostics;

namespace Spectre.Console.Tests;

public sealed class ExceptionScrubber : ExceptionInfoResolver
{
    private int _lineNumber;

    private static readonly Regex _filenameRegex = new Regex(".*cs:?.*", RegexOptions.Multiline);
    private static readonly Regex _pathSeparatorRegex = new Regex(@"[/\\]+");

    public override int GetFileLineNumber(StackFrame frame)
    {
        return _lineNumber += 100;
    }

    public override string? GetFileName(StackFrame frame)
    {
        var text = base.GetFileName(frame) ?? string.Empty;
        text = _filenameRegex.Replace(text, match =>
        {
            var value = match.Value;
            var index = value.LastIndexOfAny(['\\', '/']);
            var filename = value.Substring(index + 1, value.Length - index - 1);

            return $"/xyz/{filename}";
        });

        return _pathSeparatorRegex.Replace(text, "/");
    }
}