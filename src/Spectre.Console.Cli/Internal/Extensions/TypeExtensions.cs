namespace Spectre.Console.Cli;

internal static class TypeExtensions
{
    public static bool IsPairDeconstructable(this Type type)
    {
        if (type.IsGenericType)
        {
            if (type.GetGenericTypeDefinition() == typeof(ILookup<,>) ||
                type.GetGenericTypeDefinition() == typeof(IDictionary<,>) ||
                type.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>))
            {
                return true;
            }
        }

        return false;
    }

    // Taken from https://github.com/dotnet/sdk/blob/main/src/Cli/Microsoft.DotNet.Cli.Utils/Extensions/TypeExtensions.cs#L15
    // Licensed under MIT
    public static string ToCliTypeString(this Type type)
    {
        var typeName = type.FullName ?? string.Empty;
        if (!type.IsGenericType)
        {
            return typeName;
        }

        var genericTypeName = typeName.Substring(0, typeName.IndexOf('`'));
        var genericTypes = string.Join(", ", type.GenericTypeArguments.Select(generic => generic.ToCliTypeString()));
        return $"{genericTypeName}<{genericTypes}>";
    }
}