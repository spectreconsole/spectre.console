using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Spectre.Console
{
    internal static class ExceptionParser
    {
        private static readonly Regex _messageRegex = new Regex(@"^(?'type'.*):\s(?'message'.*)$");
        private static readonly Regex _stackFrameRegex = new Regex(@"^\s*\w*\s(?'method'.*)\((?'params'.*)\)");
        private static readonly Regex _fullStackFrameRegex = new Regex(@"^\s*(?'at'\w*)\s(?'method'.*)\((?'params'.*)\)\s(?'in'\w*)\s(?'path'.*)\:(?'line'\w*)\s(?'linenumber'\d*)$");

        public static ExceptionInfo? Parse(string exception)
        {
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            var lines = exception.SplitLines();
            return Parse(new Queue<string>(lines));
        }

        private static ExceptionInfo? Parse(Queue<string> lines)
        {
            if (lines.Count == 0)
            {
                // Error: No lines to parse
                return null;
            }

            var line = lines.Dequeue();
            line = line.ReplaceExact(" ---> ", string.Empty);

            var match = _messageRegex.Match(line);
            if (!match.Success)
            {
                return null;
            }

            var inner = (ExceptionInfo?)null;

            // Stack frames
            var frames = new List<StackFrameInfo>();
            while (lines.Count > 0)
            {
                if (lines.Peek().TrimStart().StartsWith("---> ", StringComparison.OrdinalIgnoreCase))
                {
                    inner = Parse(lines);
                    if (inner == null)
                    {
                        // Error: Could not parse inner exception
                        return null;
                    }

                    continue;
                }

                line = lines.Dequeue();

                if (string.IsNullOrWhiteSpace(line))
                {
                    // Empty line
                    continue;
                }

                if (line.TrimStart().StartsWith("--- ", StringComparison.OrdinalIgnoreCase))
                {
                    // End of inner exception
                    break;
                }

                var stackFrame = ParseStackFrame(line);
                if (stackFrame == null)
                {
                    // Error: Could not parse stack frame
                    return null;
                }

                frames.Add(stackFrame);
            }

            return new ExceptionInfo(
                match.Groups["type"].Value,
                match.Groups["message"].Value,
                frames, inner);
        }

        private static StackFrameInfo? ParseStackFrame(string frame)
        {
            var match = _fullStackFrameRegex.Match(frame);
            if (match?.Success != true)
            {
                match = _stackFrameRegex.Match(frame);
                if (match?.Success != true)
                {
                    return null;
                }
            }

            var parameters = ParseMethodParameters(match.Groups["params"].Value);
            if (parameters == null)
            {
                // Error: Could not parse parameters
                return null;
            }

            var method = match.Groups["method"].Value;
            var path = match.Groups["path"].Success ? match.Groups["path"].Value : null;

            var lineNumber = (int?)null;
            if (!string.IsNullOrWhiteSpace(match.Groups["linenumber"].Value))
            {
                lineNumber = int.Parse(match.Groups["linenumber"].Value, CultureInfo.InvariantCulture);
            }

            return new StackFrameInfo(method, parameters, path, lineNumber);
        }

        private static List<(string Type, string Name)>? ParseMethodParameters(string parameters)
        {
            var result = new List<(string Type, string Name)>();
            foreach (var parameterPart in parameters.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries))
            {
                var parameterNameIndex = parameterPart.LastIndexOf(' ');
                if (parameterNameIndex == -1)
                {
                    // Error: Could not parse parameter
                    return null;
                }

                var type = parameterPart.Substring(0, parameterNameIndex);
                var name = parameterPart.Substring(parameterNameIndex + 1, parameterPart.Length - parameterNameIndex - 1);

                result.Add((type, name));
            }

            return result;
        }
    }
}
