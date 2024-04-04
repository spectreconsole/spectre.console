namespace Spectre.Console.Cli;

internal static class ConfigurationHelper
{
    public static Type? GetSettingsType(Type commandType)
    {
        if (typeof(ICommand).GetTypeInfo().IsAssignableFrom(commandType) &&
            GetGenericTypeArguments(commandType, typeof(ICommand<>), out var result))
        {
            return result[0];
        }

        return null;
    }

#if NET6_0_OR_GREATER
    [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2070", Justification = TrimWarnings.SuppressMessage)]
#endif
    private static bool GetGenericTypeArguments(Type? type, Type genericType,
        [NotNullWhen(true)] out Type[]? genericTypeArguments)
    {
        while (type != null)
        {
            foreach (var @interface in type.GetTypeInfo().GetInterfaces())
            {
                if (!@interface.GetTypeInfo().IsGenericType || @interface.GetGenericTypeDefinition() != genericType)
                {
                    continue;
                }

                genericTypeArguments = @interface.GenericTypeArguments;
                return true;
            }

            type = type.GetTypeInfo().BaseType;
        }

        genericTypeArguments = null;
        return false;
    }
}