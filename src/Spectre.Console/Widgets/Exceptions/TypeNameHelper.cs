namespace Spectre.Console;

internal static class TypeNameHelper
{
    // from  https://github.com/benaadams/Ben.Demystifier/blob/main/src/Ben.Demystifier/TypeNameHelper.cs
    // which was adapted from https://github.com/aspnet/Common/blob/dev/shared/Microsoft.Extensions.TypeNameHelper.Sources/TypeNameHelper.cs
    public static readonly Dictionary<Type, string> BuiltInTypeNames = new Dictionary<Type, string>
    {
        { typeof(void), "void" },
        { typeof(bool), "bool" },
        { typeof(byte), "byte" },
        { typeof(char), "char" },
        { typeof(decimal), "decimal" },
        { typeof(double), "double" },
        { typeof(float), "float" },
        { typeof(int), "int" },
        { typeof(long), "long" },
        { typeof(object), "object" },
        { typeof(sbyte), "sbyte" },
        { typeof(short), "short" },
        { typeof(string), "string" },
        { typeof(uint), "uint" },
        { typeof(ulong), "ulong" },
        { typeof(ushort), "ushort" },
    };

    public static readonly Dictionary<string, string> FSharpTypeNames = new Dictionary<string, string>
    {
        { "Unit", "void" },
        { "FSharpOption", "Option" },
        { "FSharpAsync", "Async" },
        { "FSharpOption`1", "Option" },
        { "FSharpAsync`1", "Async" },
    };

    /// <summary>
    /// Pretty print a type name.
    /// </summary>
    /// <param name="type">The <see cref="Type"/>.</param>
    /// <param name="fullName"><c>true</c> to print a fully qualified name.</param>
    /// <param name="includeGenericParameterNames"><c>true</c> to include generic parameter names.</param>
    /// <returns>The pretty printed type name.</returns>
    public static string GetTypeDisplayName(Type type, bool fullName = false, bool includeGenericParameterNames = true)
    {
        var builder = new StringBuilder();
        ProcessType(builder, type, new DisplayNameOptions(fullName, includeGenericParameterNames));
        return builder.ToString();
    }

    private static void ProcessType(StringBuilder builder, Type type, DisplayNameOptions options)
    {
        if (type.IsGenericType)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
            {
                ProcessType(builder, underlyingType, options);
                builder.Append('?');
            }
            else
            {
                var genericArguments = type.GetGenericArguments();
                ProcessGenericType(builder, type, genericArguments, genericArguments.Length, options);
            }
        }
        else if (type.IsArray)
        {
            ProcessArrayType(builder, type, options);
        }
        else if (BuiltInTypeNames.TryGetValue(type, out var builtInName))
        {
            builder.Append(builtInName);
        }
        else if (type.Namespace == nameof(System))
        {
            builder.Append(type.Name);
        }
        else if (type.Assembly.ManifestModule.Name == "FSharp.Core.dll"
                 && FSharpTypeNames.TryGetValue(type.Name, out builtInName))
        {
            builder.Append(builtInName);
        }
        else if (type.IsGenericParameter)
        {
            if (options.IncludeGenericParameterNames)
            {
                builder.Append(type.Name);
            }
        }
        else
        {
            builder.Append(options.FullName ? type.FullName ?? type.Name : type.Name);
        }
    }

    private static void ProcessArrayType(StringBuilder builder, Type type, DisplayNameOptions options)
    {
        var innerType = type;
        while (innerType.IsArray)
        {
            if (innerType.GetElementType() is { } inner)
            {
                innerType = inner;
            }
        }

        ProcessType(builder, innerType, options);

        while (type.IsArray)
        {
            builder.Append('[');
            builder.Append(',', type.GetArrayRank() - 1);
            builder.Append(']');
            if (type.GetElementType() is not { } elementType)
            {
                break;
            }

            type = elementType;
        }
    }

    private static void ProcessGenericType(StringBuilder builder, Type type, Type[] genericArguments, int length,
        DisplayNameOptions options)
    {
        var offset = 0;
        if (type.IsNested && type.DeclaringType is not null)
        {
            offset = type.DeclaringType.GetGenericArguments().Length;
        }

        if (options.FullName)
        {
            if (type.IsNested && type.DeclaringType is not null)
            {
                ProcessGenericType(builder, type.DeclaringType, genericArguments, offset, options);
                builder.Append('+');
            }
            else if (!string.IsNullOrEmpty(type.Namespace))
            {
                builder.Append(type.Namespace);
                builder.Append('.');
            }
        }

        var genericPartIndex = type.Name.IndexOf('`');
        if (genericPartIndex <= 0)
        {
            builder.Append(type.Name);
            return;
        }

        if (type.Assembly.ManifestModule.Name == "FSharp.Core.dll"
            && FSharpTypeNames.TryGetValue(type.Name, out var builtInName))
        {
            builder.Append(builtInName);
        }
        else
        {
            builder.Append(type.Name, 0, genericPartIndex);
        }

        builder.Append('<');
        for (var i = offset; i < length; i++)
        {
            ProcessType(builder, genericArguments[i], options);
            if (i + 1 == length)
            {
                continue;
            }

            builder.Append(',');
            if (options.IncludeGenericParameterNames || !genericArguments[i + 1].IsGenericParameter)
            {
                builder.Append(' ');
            }
        }

        builder.Append('>');
    }

    private struct DisplayNameOptions
    {
        public DisplayNameOptions(bool fullName, bool includeGenericParameterNames)
        {
            FullName = fullName;
            IncludeGenericParameterNames = includeGenericParameterNames;
        }

        public bool FullName { get; }

        public bool IncludeGenericParameterNames { get; }
    }
}