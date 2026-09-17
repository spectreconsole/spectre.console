namespace Spectre.Console;

// The reflection path below relies on metadata that trimming can remove. Every use of it
// degrades to the metadata path when the information is missing, so the warnings are
// suppressed here rather than pushed out to callers.
[UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2026:RequiresUnreferencedCode")]
[UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2070:RequiresUnreferencedCode")]
[UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2075:RequiresUnreferencedCode")]
internal static class ExceptionRenderableBuilder
{
    // The same switch StackTrace.ToString() reads before it falls back to
    // "in {module}:token 0x{token:x}+0x{iloffset:x}" for frames with no source information.
    private const string ShowILOffsetsSwitch = "Switch.System.Diagnostics.StackTrace.ShowILOffsets";

    // The runtime marks its own async plumbing [StackTraceHidden]. Without a MethodBase the
    // attribute cannot be read, so these declaring types are recognised by name instead.
    private static readonly string[] _hiddenTypePrefixes =
    [
        "System.Runtime.ExceptionServices.ExceptionDispatchInfo",
        "System.Runtime.CompilerServices.TaskAwaiter",
        "System.Runtime.CompilerServices.ValueTaskAwaiter",
        "System.Runtime.CompilerServices.ConfiguredTaskAwaitable",
        "System.Runtime.CompilerServices.ConfiguredValueTaskAwaitable",
    ];

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

        var frames = new StackTrace(ex, fNeedFileInfo: true).GetFrames() ?? [];
        for (var i = 0; i < frames.Length; i++)
        {
            var frame = frames[i];
            if (frame == null)
            {
                continue;
            }

            // The runtime never filters the last frame, so neither do we.
            var isLast = i == frames.Length - 1;
            var builder = new StringBuilder();

            // GetMethod() returns null under NativeAOT, and for any frame whose reflection
            // metadata has been trimmed away. Those frames still carry names in the stack
            // trace metadata, which the metadata path reads instead.
            var method = frame.GetMethod();
            if (method != null)
            {
                if (!isLast && !ShowInStackTrace(method))
                {
                    continue;
                }

                AppendReflectedMethod(builder, ref method, resolver, settings);
            }
            else if (!TryAppendMetadataMethod(builder, frame, isLast, settings))
            {
                continue;
            }

            AppendLocation(builder, frame, method, resolver, settings);

            grid.AddRow(
                $"[{styles.Dimmed.ToMarkup()}]at[/]",
                builder.ToString());
        }

        return grid;
    }

    private static void AppendReflectedMethod(StringBuilder builder, ref MethodBase method,
        ExceptionInfoResolver resolver, ExceptionSettings settings)
    {
        var styles = settings.Style;
        var shortenMethods = (settings.Format & ExceptionFormats.ShortenMethods) != 0;

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
    }

    private static bool TryAppendMetadataMethod(StringBuilder builder, StackFrame frame, bool isLast,
        ExceptionSettings settings)
    {
        var text = StackFrameText.Parse(frame);
        string? declaringType = null;
        string? name = null;

#if NET9_0_OR_GREATER
        // The trim- and AOT-safe replacement for GetMethod(). It reads the names out of
        // the stack trace metadata the compiler emits, and needs no MethodBase.
        if (DiagnosticMethodInfo.Create(frame) is { } info)
        {
            declaringType = info.DeclaringTypeName;
            name = info.Name;
        }
#endif

        if (name == null)
        {
            // No metadata at all. Use whatever the frame is willing to say about itself,
            // which is a qualified name on NativeAOT up to .NET 10, a synthetic
            // "MyApp!<BaseAddress>+0x1a2b" when stack trace data was stripped,
            // or nothing.
            if (!text.HasMethod)
            {
                return false;
            }

            name = text.Name!;
            if (!text.IsSynthetic)
            {
                var index = name.LastIndexOf('.');
                if (index > 0)
                {
                    declaringType = name.Substring(0, index);
                    name = name.Substring(index + 1);
                }
            }
        }

        if (!isLast && IsHiddenByName(declaringType))
        {
            return false;
        }

        AppendMetadataMethod(builder, declaringType, name, text, settings);
        return true;
    }

    /// <summary>
    /// Renders a method from its names alone. The names come from the stack trace metadata
    /// (or the frame text), and <paramref name="text"/> supplies whatever extras the runtime
    /// put in <see cref="StackFrame.ToString()"/>: generic arguments and, on NativeAOT up to
    /// .NET 10, parameter types. Parameter names and return types are not available.
    /// </summary>
    internal static void AppendMetadataMethod(StringBuilder builder, string? declaringType, string name,
        StackFrameText text, ExceptionSettings settings)
    {
        var styles = settings.Style;

        // A synthetic frame is an address, not a method, so generic arguments
        // and a parameter list would be nonsense.
        if (text.IsSynthetic)
        {
            builder.AppendWithStyle(styles.Method, name);
            return;
        }

        if (TryResolveStateMachine(ref declaringType, ref name))
        {
            builder.Append("async ");
        }

        // Same shape as ExceptionInfoResolver.GetMethodName: nested types joined with '.'.
        var methodName = declaringType == null
            ? name
            : declaringType.Replace('+', '.') + "." + name;

        var shortenMethods = (settings.Format & ExceptionFormats.ShortenMethods) != 0;
        Emphasize(builder, methodName, ['.'], styles.Method, shortenMethods, settings);

        if (text.GenericArguments is { Length: > 0 } genericArguments)
        {
            builder.AppendWithStyle(styles.Method, "<" + genericArguments + ">");
        }

        builder.AppendWithStyle(styles.Parenthesis, "(");
        if (text.Signature == null)
        {
            // The runtime did not say, which is different from "no parameters".
            builder.AppendWithStyle(styles.Dimmed, "…");
        }
        else if (text.Signature.Length > 0)
        {
            builder.AppendWithStyle(styles.ParameterType, text.Signature);
        }

        builder.AppendWithStyle(styles.Parenthesis, ")");
    }

    /// <summary>
    /// Undoes the compiler's name mangling for async and iterator state machines using the
    /// names alone: <c>Ns.Outer+&lt;ThrowAsync&gt;d__0</c> / <c>MoveNext</c> becomes
    /// <c>Ns.Outer</c> / <c>ThrowAsync</c>. This is what <see cref="TryResolveStateMachineMethod"/>
    /// does with reflection when a <see cref="MethodBase"/> is available.
    /// </summary>
    /// <remarks>
    /// Iterators mangle identically to async methods and cannot be told apart without
    /// reflection, so an iterator frame is labelled async too.
    /// </remarks>
    internal static bool TryResolveStateMachine(ref string? declaringType, ref string name)
    {
        if (name != "MoveNext" || declaringType == null)
        {
            return false;
        }

        // The state machine type is "<{Method}>d__{N}" for a method declared in source, and a
        // bare "<{Method}>d" for async local functions and lambdas, whose mangled method name
        // already carries the ordinal: "<<Outer>g__Local|0_1>d", "<<Outer>b__0_0>d".
        var close = declaringType.LastIndexOf('>');
        if (close == -1)
        {
            return false;
        }

        var suffix = declaringType.Substring(close + 1);
        if (suffix != "d" && !suffix.StartsWith("d__", StringComparison.Ordinal))
        {
            return false;
        }

        // Walk back to the balancing '<'. The method name inside may itself be mangled
        // and contain brackets of its own.
        var open = -1;
        var depth = 0;
        for (var i = close; i >= 0; i--)
        {
            var c = declaringType[i];
            if (c == '>')
            {
                depth++;
            }
            else if (c == '<' && --depth == 0)
            {
                open = i;
                break;
            }
        }

        // "+<" from DiagnosticMethodInfo, ".<" from the frame text.
        if (open <= 0 || (declaringType[open - 1] != '+' && declaringType[open - 1] != '.'))
        {
            return false;
        }

        name = declaringType.Substring(open + 1, close - open - 1);
        declaringType = declaringType.Substring(0, open - 1);
        return true;
    }

    internal static bool IsHiddenByName(string? declaringType)
    {
        if (declaringType == null)
        {
            return false;
        }

        foreach (var prefix in _hiddenTypePrefixes)
        {
            if (declaringType.StartsWith(prefix, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    private static void AppendLocation(StringBuilder builder, StackFrame frame, MethodBase? method,
        ExceptionInfoResolver resolver, ExceptionSettings settings)
    {
        var styles = settings.Style;

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

            return;
        }

        // No symbols. StackTrace.ToString() falls back to the metadata token and IL offset
        // when asked to, and so do we. Only CoreCLR frames have an IL offset.
        if (method == null || !ShowILOffsets())
        {
            return;
        }

        var ilOffset = frame.GetILOffset();
        if (ilOffset == StackFrame.OFFSET_UNKNOWN || method.ReflectedType is not { } reflectedType)
        {
            return;
        }

        int token;
        try
        {
            token = method.MetadataToken;
        }
        catch (InvalidOperationException)
        {
            // Metadata token not available.
            return;
        }

        builder.Append(' ');
        builder.AppendWithStyle(styles.Dimmed, "in");
        builder.Append(' ');
        builder.AppendWithStyle(styles.Path, reflectedType.Module.ScopeName);
        builder.AppendWithStyle(styles.Dimmed, ":token ");
        builder.AppendWithStyle(styles.LineNumber, "0x" + token.ToString("x", CultureInfo.InvariantCulture));
        builder.AppendWithStyle(styles.Dimmed, "+");
        builder.AppendWithStyle(styles.LineNumber, "0x" + ilOffset.ToString("x", CultureInfo.InvariantCulture));
    }

    private static bool ShowILOffsets()
    {
        return AppContext.TryGetSwitch(ShowILOffsetsSwitch, out var enabled) && enabled;
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

    private static bool ShowInStackTrace(MethodBase mb)
    {
        // NET 6 has an attribute of StackTraceHiddenAttribute that we can use to clean up the stack trace
        // cleanly. If the user is on an older version we'll fall back to all the stack frames being included.
#if NET6_0_OR_GREATER
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

    private static string GetMethodName(ExceptionInfoResolver resolver, ref MethodBase method, out bool isAsync)
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

        return resolver.GetMethodName(method);
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
