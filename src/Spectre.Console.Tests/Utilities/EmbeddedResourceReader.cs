namespace Spectre.Console.Tests;

public static class EmbeddedResourceReader
{
    public static Stream LoadResourceStream(string resourceName)
    {
        ArgumentNullException.ThrowIfNull(resourceName);

        var assembly = Assembly.GetCallingAssembly();
        resourceName = resourceName.Replace("/", ".");

        var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new InvalidOperationException($"Could not load embedded resource '{resourceName}'");
        }

        return stream;
    }

    public static Stream LoadResourceStream(Assembly assembly, string resourceName)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        ArgumentNullException.ThrowIfNull(resourceName);

        resourceName = resourceName.Replace("/", ".");
        var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new InvalidOperationException($"Could not load embedded resource '{resourceName}'");
        }

        return stream;
    }
}