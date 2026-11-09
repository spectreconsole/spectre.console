namespace Spectre.Console;

/// <summary>
/// Decomposes the text produced by <see cref="StackFrame.ToString()"/>. On NativeAOT this is the
/// only place a frame reports its generic arguments and parameter types, so it is used to add
/// those extras to a frame whose names came from elsewhere. It is not a stable contract: the
/// NativeAOT shape changed between .NET 10 and .NET 11, so nothing load-bearing may depend on it.
/// </summary>
/// <remarks>
/// The envelope is fixed: <c>{method} at offset {offset} in file:line:column {file}:{line}:{column}</c>
/// followed by a newline, or the literal <c>&lt;null&gt;</c> when there is no method at all.
/// The method part is produced per platform:
/// <list type="bullet">
/// <item>CoreCLR: <c>MoveNext</c> or <c>Level2&lt;T&gt;</c>. The name only, generic arguments in angle brackets.</item>
/// <item>NativeAOT up to .NET 10: <c>Ns.Type.Level2[T](Int32, T&amp;, String[]) + 0x28</c>. Qualified,
/// generic arguments in square brackets, parameter types, and a trailing native offset.</item>
/// <item>NativeAOT from .NET 11: <c>Level2&lt;T&gt;</c>, the CoreCLR shape.</item>
/// <item>NativeAOT without stack trace metadata: <c>MyApp!&lt;BaseAddress&gt;+0x1a2b</c>.</item>
/// <item>NativeAOT when the module cannot be resolved: <c>&lt;unknown&gt;</c>.</item>
/// </list>
/// </remarks>
internal sealed class StackFrameText
{
    private const string OffsetMarker = " at offset ";
    private const string FileMarker = " in file:line:column ";
    private const string NativeOffsetMarker = " + 0x";
    private const string NullFrame = "<null>";
    private const string UnknownMethod = "<unknown>";
    private const string BaseAddressMarker = "!<BaseAddress>+0x";

    private static readonly StackFrameText _empty = new(null, null, null, isSynthetic: false);

    private StackFrameText(string? name, string? genericArguments, string? signature, bool isSynthetic)
    {
        Name = name;
        GenericArguments = genericArguments;
        Signature = signature;
        IsSynthetic = isSynthetic;
    }

    /// <summary>
    /// Gets a value indicating whether the frame described a method at all.
    /// </summary>
    public bool HasMethod => Name != null;

    /// <summary>
    /// Gets the method name without generic arguments or signature. On CoreCLR this is the bare
    /// name; on NativeAOT up to .NET 10 it is qualified with the declaring type.
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// Gets the generic arguments without their brackets, e.g. <c>T</c> or <c>TKey,TValue</c>.
    /// </summary>
    public string? GenericArguments { get; }

    /// <summary>
    /// Gets the parameter types without their parentheses. <see langword="null"/> means the
    /// runtime did not say; an empty string means the method takes no parameters.
    /// </summary>
    public string? Signature { get; }

    /// <summary>
    /// Gets a value indicating whether the method text is a placeholder rather than a name,
    /// such as <c>&lt;unknown&gt;</c> or <c>MyApp!&lt;BaseAddress&gt;+0x1a2b</c>.
    /// </summary>
    public bool IsSynthetic { get; }

    public static StackFrameText Parse(StackFrame frame)
    {
        return Parse(frame.ToString());
    }

    public static StackFrameText Parse(string text)
    {
        // ToString() always ends with AppendLine().
        var method = text.TrimEnd('\r', '\n');
        if (method == NullFrame)
        {
            return _empty;
        }

        // The tail is fixed-format. Anchor on it from the right so that a method name
        // containing one of the markers cannot confuse the split.
        var index = method.LastIndexOf(FileMarker, StringComparison.Ordinal);
        if (index != -1)
        {
            method = method.Substring(0, index);
        }

        index = method.LastIndexOf(OffsetMarker, StringComparison.Ordinal);
        if (index != -1)
        {
            method = method.Substring(0, index);
        }

        // Placeholders first: both contain angle brackets and would otherwise
        // look like generic arguments.
        if (method == UnknownMethod || method.Contains(BaseAddressMarker))
        {
            return new StackFrameText(method, null, null, isSynthetic: true);
        }

        // NativeAOT up to .NET 10 appends the native offset after the signature.
        index = method.LastIndexOf(NativeOffsetMarker, StringComparison.Ordinal);
        if (index != -1 && IsHex(method, index + NativeOffsetMarker.Length))
        {
            method = method.Substring(0, index);
        }

        string? signature = null;
        if (TrySplitTrailingGroup(method, '(', ')', out var open))
        {
            signature = method.Substring(open + 1, method.Length - open - 2);
            method = method.Substring(0, open);
        }

        // Square brackets on NativeAOT up to .NET 10, angle brackets on CoreCLR and
        // on NativeAOT from .NET 11.
        string? genericArguments = null;
        if (TrySplitTrailingGroup(method, '[', ']', out open) || TrySplitTrailingGroup(method, '<', '>', out open))
        {
            genericArguments = method.Substring(open + 1, method.Length - open - 2);
            method = method.Substring(0, open);
        }

        return new StackFrameText(method, genericArguments, signature, isSynthetic: false);
    }

    /// <summary>
    /// Finds the opening bracket of a bracketed group that ends the text, matching nested
    /// brackets on the way. The group must not be the whole text.
    /// </summary>
    private static bool TrySplitTrailingGroup(string text, char open, char close, out int index)
    {
        index = -1;
        if (text.Length == 0 || text[text.Length - 1] != close)
        {
            return false;
        }

        var depth = 0;
        for (var i = text.Length - 1; i >= 0; i--)
        {
            var c = text[i];
            if (c == close)
            {
                depth++;
            }
            else if (c == open && --depth == 0)
            {
                index = i;
                return i > 0;
            }
        }

        return false;
    }

    private static bool IsHex(string text, int start)
    {
        if (start >= text.Length)
        {
            return false;
        }

        for (var i = start; i < text.Length; i++)
        {
            if (!Uri.IsHexDigit(text[i]))
            {
                return false;
            }
        }

        return true;
    }
}
