using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Spectre.Console
{
    internal static class ExceptionParser
    {
        private static readonly Regex _stackFrameRegex = new Regex(@"^\s*\w*\s(?'method'.*)\((?'params'.*)\)");
        private static readonly Regex _fullStackFrameRegex = new Regex(@"^\s*(?'at'\w*)\s(?'method'.*)\((?'params'.*)\)\s(?'in'\w*)\s(?'path'.*)\:(?'line'\w*)\s(?'linenumber'\d*)$");

        public static ExceptionInfo Parse(Exception exception)
        {
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            var exceptionType = exception.GetType();
            var frames = exception.StackTrace?.SplitLines().Select(ParseStackFrame).Where(e => e != null).Cast<StackFrameInfo>().ToList() ?? new List<StackFrameInfo>();
            var inner = exception.InnerException is null ? null : Parse(exception.InnerException);
            return new ExceptionInfo(exceptionType.FullName ?? exceptionType.Name, exception.Message, frames, inner);
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
