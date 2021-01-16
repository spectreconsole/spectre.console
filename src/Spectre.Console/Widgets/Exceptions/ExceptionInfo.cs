using System.Collections.Generic;

namespace Spectre.Console
{
    internal sealed class ExceptionInfo
    {
        public string Type { get; }
        public string Message { get; }
        public List<StackFrameInfo> Frames { get; }
        public ExceptionInfo? Inner { get; }

        public ExceptionInfo(
            string type, string message,
            List<StackFrameInfo> frames,
            ExceptionInfo? inner)
        {
            Type = type ?? string.Empty;
            Message = message ?? string.Empty;
            Frames = frames ?? new List<StackFrameInfo>();
            Inner = inner;
        }
    }
}
