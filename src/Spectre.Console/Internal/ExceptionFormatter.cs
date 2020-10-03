using System;
using System.Linq;
using System.Text;
using Spectre.Console.Internal;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    internal static class ExceptionFormatter
    {
        private static readonly Color _typeColor = Color.White;
        private static readonly Color _methodColor = Color.Yellow;
        private static readonly Color _parameterColor = Color.Blue;
        private static readonly Color _pathColor = Color.Yellow;
        private static readonly Color _dimmedColor = Color.Grey;

        public static IRenderable Format(Exception exception, ExceptionFormats format)
        {
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            var info = ExceptionParser.Parse(exception.ToString());
            if (info == null)
            {
                return new Text(exception.ToString());
            }

            return GetException(info, format);
        }

        private static IRenderable GetException(ExceptionInfo info, ExceptionFormats format)
        {
            if (info is null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            return new Rows(new IRenderable[]
            {
                GetMessage(info, format),
                GetStackFrames(info, format),
            }).Expand();
        }

        private static Markup GetMessage(ExceptionInfo ex, ExceptionFormats format)
        {
            var shortenTypes = (format & ExceptionFormats.ShortenTypes) != 0;
            var type = Emphasize(ex.Type, new[] { '.' }, _typeColor.ToMarkupString(), shortenTypes);
            var message = $"[b red]{ex.Message.SafeMarkup()}[/]";
            return new Markup(string.Concat(type, ": ", message));
        }

        private static Grid GetStackFrames(ExceptionInfo ex, ExceptionFormats format)
        {
            var grid = new Grid();
            grid.AddColumn(new GridColumn().PadLeft(2).PadRight(0).NoWrap());
            grid.AddColumn(new GridColumn().PadLeft(1).PadRight(0));

            // Inner
            if (ex.Inner != null)
            {
                grid.AddRow(
                    Text.Empty,
                    GetException(ex.Inner, format));
            }

            // Stack frames
            foreach (var frame in ex.Frames)
            {
                var builder = new StringBuilder();

                // Method
                var shortenMethods = (format & ExceptionFormats.ShortenMethods) != 0;
                builder.Append(Emphasize(frame.Method, new[] { '.' }, _methodColor.ToMarkupString(), shortenMethods));
                builder.Append('(');
                builder.Append(string.Join(", ", frame.Parameters.Select(x => $"[{_parameterColor.ToMarkupString()}]{x.Type.SafeMarkup()}[/] {x.Name}")));
                builder.Append(')');

                if (frame.Path != null)
                {
                    builder.Append(" [").Append(_dimmedColor.ToMarkupString()).Append("]in[/] ");

                    // Path
                    AppendPath(builder, frame, format);

                    // Line number
                    if (frame.LineNumber != null)
                    {
                        builder.Append(':');
                        builder.Append('[').Append(_parameterColor.ToMarkupString()).Append(']').Append(frame.LineNumber).Append("[/]");
                    }
                }

                grid.AddRow($"[{_dimmedColor.ToMarkupString()}]at[/]", builder.ToString());
            }

            return grid;
        }

        private static void AppendPath(StringBuilder builder, StackFrameInfo frame, ExceptionFormats format)
        {
            if (frame?.Path is null)
            {
                return;
            }

            void RenderLink()
            {
                var shortenPaths = (format & ExceptionFormats.ShortenPaths) != 0;
                builder.Append(Emphasize(frame.Path, new[] { '/', '\\' }, $"b {_pathColor.ToMarkupString()}", shortenPaths));
            }

            if ((format & ExceptionFormats.ShowLinks) != 0)
            {
                var hasLink = frame.TryGetUri(out var uri);
                if (hasLink && uri != null)
                {
                    builder.Append("[link=").Append(uri.AbsoluteUri).Append(']');
                }

                RenderLink();

                if (hasLink && uri != null)
                {
                    builder.Append("[/]");
                }
            }
            else
            {
                RenderLink();
            }
        }

        private static string Emphasize(string input, char[] separators, string color, bool compact)
        {
            var builder = new StringBuilder();

            var type = input;
            var index = type.LastIndexOfAny(separators);
            if (index != -1)
            {
                if (!compact)
                {
                    builder.Append("[silver]").Append(type, 0, index + 1).Append("[/]");
                }

                builder.Append('[').Append(color).Append(']').Append(type, index + 1, type.Length - index - 1).Append("[/]");
            }
            else
            {
                builder.Append(type);
            }

            return builder.ToString();
        }
    }
}
