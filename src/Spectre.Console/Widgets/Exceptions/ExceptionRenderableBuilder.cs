namespace Spectre.Console;

// ExceptionFormatter relies heavily on reflection of types unknown until runtime.
[UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2026:RequiresUnreferencedCode")]
[UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2070:RequiresUnreferencedCode")]
[UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2075:RequiresUnreferencedCode")]
[UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL3050:RequiresUnreferencedCode")]
internal static class ExceptionRenderableBuilder
{
    public const string AotWarning = "ExceptionFormatter is currently not supported for AOT.";

    public static IRenderable Format(Exception exception, ExceptionSettings settings)
    {
        ArgumentNullException.ThrowIfNull(exception);

        return GetException(exception, settings);
    }

    private static IRenderable GetException(Exception exception, ExceptionSettings settings)
    {
        ArgumentNullException.ThrowIfNull(exception);

        var renderable = new Rows(
                GetMessage(exception, settings),
                GetStackFrames(exception, settings))
            .Collapse();

        return renderable;
    }

    private static Markup GetMessage(Exception ex, ExceptionSettings settings)
    {
        var shortenTypes = (settings.Format & ExceptionFormats.ShortenTypes) != 0;
        var exceptionType = ex.GetType();
        var exceptionTypeName =
            TypeNameHelper.GetTypeDisplayName(exceptionType, fullName: !shortenTypes, includeSystemNamespace: true);
        var type = new StringBuilder();
        Emphasize(type, exceptionTypeName, ['.'], settings.Style.Exception, shortenTypes, settings, limit: '<');

        var message = $"[{settings.Style.Message.ToMarkup()}]{ex.Message.EscapeMarkup()}[/]";
        return new Markup($"{type}: {message}");
    }

    private static Grid GetStackFrames(Exception ex, ExceptionSettings settings)
    {
        var styles = settings.Style;
        var resolver = settings.Resolver ?? new ExceptionInfoResolver();

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
        if ((settings.Format & ExceptionFormats.NoStackTrace) != 0)
        {
            return grid;
        }

        var stackTrace = new StackTrace(ex, fNeedFileInfo: true);
        var allFrames = stackTrace.GetFrames();
        if (allFrames.Length > 0 && allFrames[0]?.GetMethod() == null)
        {
            // if we can't easily get the method for the frame, then we are in AOT
            // fallback to using ToString method of each frame.
            WriteAotFrames(grid, stackTrace.GetFrames(), styles);
            return grid;
        }

        var frames = allFrames
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

            var methodName = GetMethodName(resolver, ref method, out var isAsync);
            if (isAsync)
            {
                builder.Append("async ");
            }

            if (method is MethodInfo mi)
            {
                var returnParameter = mi.ReturnParameter;
                builder.AppendWithStyle(styles.ParameterType,
                    resolver.GetParameterName(returnParameter).EscapeMarkup());
                builder.Append(' ');
            }

            Emphasize(builder, methodName, ['.'], styles.Method, shortenMethods, settings);
            builder.AppendWithStyle(styles.Parenthesis, "(");
            AppendParameters(resolver, builder, method, settings);
            builder.AppendWithStyle(styles.Parenthesis, ")");

            var path = resolver.GetFileName(frame);
            if (path != null)
            {
                builder.Append(' ');
                builder.AppendWithStyle(styles.Dimmed, "in");
                builder.Append(' ');

                // Path
                AppendPath(builder, path, settings);

                // Line number
                var lineNumber = resolver.GetFileLineNumber(frame);
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

    private static void WriteAotFrames(Grid grid, StackFrame?[] frames, ExceptionStyle styles)
    {
        foreach (var stackFrame in frames)
        {
            if (stackFrame == null)
            {
                continue;
            }

            var s = stackFrame.ToString();
            s = s.Replace(" in file:line:column <filename unknown>:0:0", string.Empty).TrimEnd();
            grid.AddRow(
                $"[{styles.Dimmed.ToMarkup()}]at[/]",
                s.EscapeMarkup());
        }
    }

    private static void AppendParameters(ExceptionInfoResolver resolver, StringBuilder builder, MethodBase? method,
        ExceptionSettings settings)
    {
        var typeColor = settings.Style.ParameterType.ToMarkup();
        var nameColor = settings.Style.ParameterName.ToMarkup();
        var parameters = method?.GetParameters()
            .Select(x =>
                $"[{typeColor}]{resolver.GetParameterName(x).EscapeMarkup()}[/] [{nameColor}]{x.Name?.EscapeMarkup()}[/]");

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
            Emphasize(builder, path, ['/', '\\'], settings.Style.Path, shortenPaths, settings);
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

    private static void Emphasize(StringBuilder builder, string input, char[] separators, Style color, bool compact,
        ExceptionSettings settings, char? limit = null)
    {
        var limitIndex = limit.HasValue ? input.IndexOf(limit.Value) : -1;

        var index = limitIndex != -1
            ? input[..limitIndex].LastIndexOfAny(separators)
            : input.LastIndexOfAny(separators);
        if (index != -1)
        {
            if (!compact)
            {
                builder.AppendWithStyle(settings.Style.NonEmphasized, input[..(index + 1)]);
            }

            builder.AppendWithStyle(color, input[(index + 1)..]);
        }
        else
        {
            builder.AppendWithStyle(color, input);
        }
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
        var allFrames = frames?.ToArray() ?? [];
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

    private static string GetMethodName(ExceptionInfoResolver resolver, ref MethodBase method, out bool isAsync)
    {
        var declaringType = method.DeclaringType;

        if (declaringType?.IsDefined(typeof(CompilerGeneratedAttribute), false) == true)
        {
            isAsync = typeof(IAsyncStateMachine).IsAssignableFrom(declaringType);
            if (isAsync || typeof(IEnumerator).IsAssignableFrom(declaringType))
            {
                TryResolveStateMachineMethod(method, out declaringType);
            }
        }
        else
        {
            isAsync = false;
        }

        return resolver.GetMethodName(method);
    }

    [RequiresDynamicCode(ExceptionRenderableBuilder.AotWarning)]
    private static bool TryResolveStateMachineMethod(MethodBase method, out Type declaringType)
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