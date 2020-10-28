using System;
using System.Linq;
using System.Text;
using Spectre.Console.Rendering;

namespace Spectre.Console.Internal
{
    internal static class ExceptionFormatter
    {
        public static IRenderable Format(Exception exception, ExceptionSettings settings)
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

            return GetException(info, settings);
        }

        private static IRenderable GetException(ExceptionInfo info, ExceptionSettings settings)
        {
            if (info is null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            return new Rows(new IRenderable[]
            {
                GetMessage(info, settings),
                GetStackFrames(info, settings),
            }).Expand();
        }

        private static Markup GetMessage(ExceptionInfo ex, ExceptionSettings settings)
        {
            var shortenTypes = (settings.Format & ExceptionFormats.ShortenTypes) != 0;
            var type = Emphasize(ex.Type, new[] { '.' }, settings.Style.Exception, shortenTypes, settings);
            var message = $"[{settings.Style.Message.ToMarkup()}]{ex.Message.EscapeMarkup()}[/]";
            return new Markup(string.Concat(type, ": ", message));
        }

        private static Grid GetStackFrames(ExceptionInfo ex, ExceptionSettings settings)
        {
            var grid = new Grid();
            grid.AddColumn(new GridColumn().PadLeft(2).PadRight(0).NoWrap());
            grid.AddColumn(new GridColumn().PadLeft(1).PadRight(0));

            // Inner
            if (ex.Inner != null)
            {
                grid.AddRow(
                    Text.Empty,
                    GetException(ex.Inner, settings));
            }

            // Stack frames
            foreach (var frame in ex.Frames)
            {
                var builder = new StringBuilder();

                // Method
                var shortenMethods = (settings.Format & ExceptionFormats.ShortenMethods) != 0;
                builder.Append(Emphasize(frame.Method, new[] { '.' }, settings.Style.Method, shortenMethods, settings));
                builder.Append('[').Append(settings.Style.Parenthesis.ToMarkup()).Append(']').Append('(').Append("[/]");
                AppendParameters(builder, frame, settings);
                builder.Append('[').Append(settings.Style.Parenthesis.ToMarkup()).Append(']').Append(')').Append("[/]");

                if (frame.Path != null)
                {
                    builder.Append(" [").Append(settings.Style.Dimmed.ToMarkup()).Append("]in[/] ");

                    // Path
                    AppendPath(builder, frame, settings);

                    // Line number
                    if (frame.LineNumber != null)
                    {
                        builder.Append('[').Append(settings.Style.Dimmed.ToMarkup()).Append("]:[/]");
                        builder.Append('[').Append(settings.Style.LineNumber.ToMarkup()).Append(']').Append(frame.LineNumber).Append("[/]");
                    }
                }

                grid.AddRow(
                    $"[{settings.Style.Dimmed.ToMarkup()}]at[/]",
                    builder.ToString());
            }

            return grid;
        }

        private static void AppendParameters(StringBuilder builder, StackFrameInfo frame, ExceptionSettings settings)
        {
            var typeColor = settings.Style.ParameterType.ToMarkup();
            var nameColor = settings.Style.ParameterName.ToMarkup();
            var parameters = frame.Parameters.Select(x => $"[{typeColor}]{x.Type.EscapeMarkup()}[/] [{nameColor}]{x.Name}[/]");
            builder.Append(string.Join(", ", parameters));
        }

        private static void AppendPath(StringBuilder builder, StackFrameInfo frame, ExceptionSettings settings)
        {
            if (frame?.Path is null)
            {
                return;
            }

            void AppendPath()
            {
                var shortenPaths = (settings.Format & ExceptionFormats.ShortenPaths) != 0;
                builder.Append(Emphasize(frame.Path, new[] { '/', '\\' }, settings.Style.Path, shortenPaths, settings));
            }

            if ((settings.Format & ExceptionFormats.ShowLinks) != 0)
            {
                var hasLink = frame.TryGetUri(out var uri);
                if (hasLink && uri != null)
                {
                    builder.Append("[link=").Append(uri.AbsoluteUri).Append(']');
                }

                AppendPath();

                if (hasLink && uri != null)
                {
                    builder.Append("[/]");
                }
            }
            else
            {
                AppendPath();
            }
        }

        private static string Emphasize(string input, char[] separators, Style color, bool compact, ExceptionSettings settings)
        {
            var builder = new StringBuilder();

            var type = input;
            var index = type.LastIndexOfAny(separators);
            if (index != -1)
            {
                if (!compact)
                {
                    builder.Append('[').Append(settings.Style.NonEmphasized.ToMarkup()).Append(']')
                        .Append(type, 0, index + 1).Append("[/]");
                }

                builder.Append('[').Append(color.ToMarkup()).Append(']')
                    .Append(type, index + 1, type.Length - index - 1).Append("[/]");
            }
            else
            {
                builder.Append(type);
            }

            return builder.ToString();
        }
    }
}
