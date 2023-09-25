namespace Spectre.Console.Cli.Internal.Extensions;

internal static class TypeResolverExtensions
{
    public static object? TryResolve(this ITypeResolver resolver, Type type)
    {
        if (resolver == null)
        {
            throw new ArgumentNullException(nameof(resolver));
        }

        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        if (resolver is TypeResolverAdapter adapter)
        {
            return adapter.TryResolve(type);
        }

        try
        {
            return resolver.Resolve(type);
        }
        catch (Exception)
        {
            return null;
        }
    }
}