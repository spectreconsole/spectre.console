namespace Spectre.Console;

internal static class MethodExtensions
{
    public static string GetName(this MethodBase? method)
    {
        if (method is null)
        {
            return "<unknown method>";
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
            builder.Append('[');
            builder.Append(string.Join(",", method.GetGenericArguments().Select(t => t.Name)));
            builder.Append(']');
        }

        return builder.ToString();
    }
}