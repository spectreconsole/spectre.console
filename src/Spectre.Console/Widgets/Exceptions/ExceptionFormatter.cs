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

        return new Rows(GetMessage(exception, settings), GetStackFrames(exception, settings)).Expand();
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
        var frames = stackTrace
            .GetFrames()
            .FilterStackFrames()
            .ToList();

        foreach (var frame in frames)
        {
            var builder = new StringBuilder();

            // Method
            var shortenMethods = (settings.Format & ExceptionFormats.ShortenMethods) != 0;
            var method = frame.GetMethod();
            if (method == null)
            {
                continue;
            }

            var methodName = GetMethodName(ref method, out var isAsync);
            if (isAsync)
            {
                builder.Append("async ");
            }

            if (method is MethodInfo mi)
            {
                var returnParameter = mi.ReturnParameter;
                builder.AppendWithStyle(styles.ParameterType, GetParameterName(returnParameter).EscapeMarkup());
                builder.Append(' ');
            }

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
        var parameters = method?.GetParameters()
            .Select(x => $"[{typeColor}]{GetParameterName(x).EscapeMarkup()}[/] [{nameColor}]{x.Name?.EscapeMarkup()}[/]");

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

    private static string Emphasize(string input, char[] separators, Style color, bool compact,
        ExceptionSettings settings)
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

    private static bool ShowInStackTrace(StackFrame frame)
    {
        // NET 6 has an attribute of StackTraceHiddenAttribute that we can use to clean up the stack trace
        // cleanly. If the user is on an older version we'll fall back to all the stack frames being included.
#if NET6_0_OR_GREATER
        var mb = frame.GetMethod();
        if (mb == null)
        {
            return false;
        }

        if ((mb.MethodImplementationFlags & MethodImplAttributes.AggressiveInlining) != 0)
        {
            return false;
        }

        try
        {
            if (mb.IsDefined(typeof(StackTraceHiddenAttribute), false))
            {
                return false;
            }

            var declaringType = mb.DeclaringType;
            if (declaringType?.IsDefined(typeof(StackTraceHiddenAttribute), false) == true)
            {
                return false;
            }
        }
        catch
        {
            // if we can't get the attributes then fall back to including it.
        }
#endif

        return true;
    }

    private static IEnumerable<StackFrame> FilterStackFrames(this IEnumerable<StackFrame?>? frames)
    {
        var allFrames = frames?.ToArray() ?? Array.Empty<StackFrame>();
        var numberOfFrames = allFrames.Length;

        for (var i = 0; i < numberOfFrames; i++)
        {
            var thisFrame = allFrames[i];
            if (thisFrame == null)
            {
                continue;
            }

            // always include the last frame
            if (i == numberOfFrames - 1)
            {
                yield return thisFrame;
            }
            else if (ShowInStackTrace(thisFrame))
            {
                yield return thisFrame;
            }
        }
    }

    private static string GetPrefix(ParameterInfo parameter)
    {
        if (Attribute.IsDefined(parameter, typeof(ParamArrayAttribute), false))
        {
            return "params";
        }

        if (parameter.IsOut)
        {
            return "out";
        }

        if (parameter.IsIn)
        {
            return "in";
        }

        if (parameter.ParameterType.IsByRef)
        {
            return "ref";
        }

        return string.Empty;
    }

    private static string GetParameterName(ParameterInfo parameter)
    {
        var prefix = GetPrefix(parameter);
        var parameterType = parameter.ParameterType;

        string typeName;
        if (parameterType.IsGenericType && TryGetTupleName(parameter, parameterType, out var s))
        {
            typeName = s;
        }
        else
        {
            if (parameterType.IsByRef && parameterType.GetElementType() is { } elementType)
            {
                parameterType = elementType;
            }

            typeName = TypeNameHelper.GetTypeDisplayName(parameterType);
        }

        return string.IsNullOrWhiteSpace(prefix) ? typeName : $"{prefix} {typeName}";
    }

    private static bool TryGetTupleName(ParameterInfo parameter, Type parameterType, [NotNullWhen(true)] out string? tupleName)
    {
        var customAttribs = parameter.GetCustomAttributes(inherit: false);

        var tupleNameAttribute = customAttribs
            .OfType<Attribute>()
            .FirstOrDefault(a =>
            {
                var attributeType = a.GetType();
                return attributeType.Namespace == "System.Runtime.CompilerServices" &&
                       attributeType.Name == "TupleElementNamesAttribute";
            });

        if (tupleNameAttribute != null)
        {
            var propertyInfo = tupleNameAttribute.GetType()
                .GetProperty("TransformNames", BindingFlags.Instance | BindingFlags.Public)!;
            var tupleNames = propertyInfo.GetValue(tupleNameAttribute) as IList<string>;
            if (tupleNames?.Count > 0)
            {
                var args = parameterType.GetGenericArguments();
                var sb = new StringBuilder();

                sb.Append('(');
                for (var i = 0; i < args.Length; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }

                    sb.Append(TypeNameHelper.GetTypeDisplayName(args[i]));

                    if (i >= tupleNames.Count)
                    {
                        continue;
                    }

                    var argName = tupleNames[i];

                    sb.Append(' ');
                    sb.Append(argName);
                }

                sb.Append(')');

                tupleName = sb.ToString();
                return true;
            }
        }
        else if (parameterType.Namespace == "System" && parameterType.Name.Contains("ValueTuple`"))
        {
            var args = parameterType.GetGenericArguments().Select(i => TypeNameHelper.GetTypeDisplayName(i));
            tupleName = $"({string.Join(", ", args)})";
            return true;
        }

        tupleName = null;
        return false;
    }

    private static string GetMethodName(ref MethodBase method, out bool isAsync)
    {
        var declaringType = method.DeclaringType;

        if (declaringType?.IsDefined(typeof(CompilerGeneratedAttribute), false) == true)
        {
            isAsync = typeof(IAsyncStateMachine).IsAssignableFrom(declaringType);
            if (isAsync || typeof(IEnumerator).IsAssignableFrom(declaringType))
            {
                TryResolveStateMachineMethod(ref method, out declaringType);
            }
        }
        else
        {
            isAsync = false;
        }

        var builder = new StringBuilder(256);

        var fullName = method.DeclaringType?.FullName;
        if (fullName != null)
        {
            // See https://github.com/dotnet/runtime/blob/v6.0.0/src/libraries/System.Private.CoreLib/src/System/Diagnostics/StackTrace.cs#L247-L253
            builder.Append(fullName.Replace('+', '.'));
            builder.Append('.');
        }

        builder.Append(method.Name);
        if (method.IsGenericMethod)
        {
            builder.Append('<');
            builder.Append(string.Join(",", method.GetGenericArguments().Select(t => t.Name)));
            builder.Append('>');
        }

        return builder.ToString();
    }

    private static bool TryResolveStateMachineMethod(ref MethodBase method, out Type declaringType)
    {
        // https://github.com/dotnet/runtime/blob/v6.0.0/src/libraries/System.Private.CoreLib/src/System/Diagnostics/StackTrace.cs#L400-L455
        declaringType = method.DeclaringType ??
                        throw new ArgumentException("Method must have a declaring type.", nameof(method));

        var parentType = declaringType.DeclaringType;
        if (parentType == null)
        {
            return false;
        }

        static IEnumerable<MethodInfo> GetDeclaredMethods(IReflect type) => type.GetMethods(
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Static |
            BindingFlags.Instance |
            BindingFlags.DeclaredOnly);

        var methods = GetDeclaredMethods(parentType);

        foreach (var candidateMethod in methods)
        {
            var attributes = candidateMethod.GetCustomAttributes<StateMachineAttribute>(false);

            bool foundAttribute = false, foundIteratorAttribute = false;
            foreach (var asma in attributes)
            {
                if (asma.StateMachineType != declaringType)
                {
                    continue;
                }

                foundAttribute = true;
#if NET6_0_OR_GREATER
                foundIteratorAttribute |= asma is IteratorStateMachineAttribute or AsyncIteratorStateMachineAttribute;
#else
                foundIteratorAttribute |= asma is IteratorStateMachineAttribute;
#endif
            }

            if (!foundAttribute)
            {
                continue;
            }

            method = candidateMethod;
            declaringType = candidateMethod.DeclaringType!;
            return foundIteratorAttribute;
        }

        return false;
    }
}