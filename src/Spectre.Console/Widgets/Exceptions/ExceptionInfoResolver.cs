namespace Spectre.Console;

/// <summary>
/// Used to resolve information from an <see cref="Exception"/>.
/// </summary>
[UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2026:RequiresUnreferencedCode")]
[UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2070:RequiresUnreferencedCode")]
[UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2075:RequiresUnreferencedCode")]
[UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL3050:RequiresUnreferencedCode")]
public class ExceptionInfoResolver
{
    internal static ExceptionInfoResolver Shared { get; } = new();

    /// <summary>
    /// Gets the name of a method.
    /// </summary>
    /// <param name="method">The <see cref="MethodBase"/> instance to get the name for.</param>
    /// <returns>The method's name.</returns>
    public virtual string GetMethodName(MethodBase method)
    {
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

    /// <summary>
    /// Gets the name of a parameter.
    /// </summary>
    /// <param name="parameter">The <see cref="ParameterInfo"/> instance to get the name for.</param>
    /// <returns>The parameter's name.</returns>
    public virtual string GetParameterName(ParameterInfo parameter)
    {
        var prefix = GetParameterPrefix(parameter);
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

    /// <summary>
    /// Gets the file name of a <see cref="StackFrame"/>.
    /// </summary>
    /// <param name="frame">The <see cref="StackFrame"/> to get the file name for.</param>
    /// <returns>The file name.</returns>
    public virtual string? GetFileName(StackFrame frame)
    {
        return frame.GetFileName();
    }

    /// <summary>
    /// Gets the line number of a <see cref="StackFrame"/>.
    /// </summary>
    /// <param name="frame">The <see cref="StackFrame"/> to get the line number for.</param>
    /// <returns>The line number.</returns>
    public virtual int GetFileLineNumber(StackFrame frame)
    {
        return frame.GetFileLineNumber();
    }

    private static string GetParameterPrefix(ParameterInfo parameter)
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

    private static bool TryGetTupleName(ParameterInfo parameter, Type parameterType,
        [NotNullWhen(true)] out string? tupleName)
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
}