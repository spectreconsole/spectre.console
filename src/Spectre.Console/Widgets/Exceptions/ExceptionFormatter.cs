namespace Spectre.Console;

internal static class ExceptionFormatter
{
    public static IRenderable Format(Exception exception, ExceptionSettings settings)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        return GetException(exception, settings);
    }

    private static IRenderable GetException(Exception exception, ExceptionSettings settings)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        return new Rows(new IRenderable[]
        {
                GetMessage(exception, settings),
                GetStackFrames(exception, settings),
        }).Expand();
    }

    private static Markup GetMessage(Exception ex, ExceptionSettings settings)
    {
        var shortenTypes = (settings.Format & ExceptionFormats.ShortenTypes) != 0;
        var exceptionType = ex.GetType();
        var exceptionTypeFullName = exceptionType.FullName ?? exceptionType.Name;
        var type = Emphasize(exceptionTypeFullName, new[] { '.' }, settings.Style.Exception, shortenTypes, settings);

        var message = $"[{settings.Style.Message.ToMarkup()}]{ex.Message.EscapeMarkup()}[/]";
        return new Markup(string.Concat(type, ": ", message));
    }

    private static Grid GetStackFrames(Exception ex, ExceptionSettings settings)
    {
        var styles = settings.Style;

        var grid = new Grid();
        grid.AddColumn(new GridColumn().PadLeft(2).PadRight(0).NoWrap());
        grid.AddColumn(new GridColumn().PadLeft(1).PadRight(0));

        // Inner
        if (ex.InnerException != null)
        {
            grid.AddRow(
                Text.Empty,
                GetException(ex.InnerException, settings));
        }

        // Stack frames
        var stackTrace = new StackTrace(ex, fNeedFileInfo: true);
        foreach (var frame in stackTrace.GetFrames().Where(f => f != null).Cast<StackFrame>())
        {
            var builder = new StringBuilder();

            // Method
            var shortenMethods = (settings.Format & ExceptionFormats.ShortenMethods) != 0;
            var method = frame.GetMethod();
            var methodName = method.GetName();
            builder.Append(Emphasize(methodName, new[] { '.' }, styles.Method, shortenMethods, settings));
            builder.AppendWithStyle(styles.Parenthesis, "(");
            AppendParameters(builder, method, settings);
            builder.AppendWithStyle(styles.Parenthesis, ")");

            var path = frame.GetFileName();
            if (path != null)
            {
                builder.Append(' ');
                builder.AppendWithStyle(styles.Dimmed, "in");
                builder.Append(' ');

                // Path
                AppendPath(builder, path, settings);

                // Line number
                var lineNumber = frame.GetFileLineNumber();
                if (lineNumber != 0)
                {
                    builder.AppendWithStyle(styles.Dimmed, ":");
                    builder.AppendWithStyle(styles.LineNumber, lineNumber);
                }
            }

            grid.AddRow(
                $"[{styles.Dimmed.ToMarkup()}]at[/]",
                builder.ToString());
        }

        return grid;
    }

    private static void AppendParameters(StringBuilder builder, MethodBase? method, ExceptionSettings settings)
    {
        var typeColor = settings.Style.ParameterType.ToMarkup();
        var nameColor = settings.Style.ParameterName.ToMarkup();
        var parameters = method?.GetParameters().Select(x => $"[{typeColor}]{x.ParameterType.Name.EscapeMarkup()}[/] [{nameColor}]{x.Name?.EscapeMarkup()}[/]");
        if (parameters != null)
        {
            builder.Append(string.Join(", ", parameters));
        }
    }

    private static void AppendPath(StringBuilder builder, string path, ExceptionSettings settings)
    {
        void AppendPath()
        {
            var shortenPaths = (settings.Format & ExceptionFormats.ShortenPaths) != 0;
            builder.Append(Emphasize(path, new[] { '/', '\\' }, settings.Style.Path, shortenPaths, settings));
        }

        if ((settings.Format & ExceptionFormats.ShowLinks) != 0)
        {
            var hasLink = path.TryGetUri(out var uri);
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
                builder.AppendWithStyle(
                    settings.Style.NonEmphasized,
                    type.Substring(0, index + 1));
            }

            builder.AppendWithStyle(
                color,
                type.Substring(index + 1, type.Length - index - 1));
        }
        else
        {
            builder.Append(type.EscapeMarkup());
        }

        return builder.ToString();
    }
}