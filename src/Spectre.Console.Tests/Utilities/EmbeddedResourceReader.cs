namespace Spectre.Console.Tests;

public static class EmbeddedResourceReader
{
    public static Stream LoadResourceStream(string resourceName)
    {
        if (resourceName is null)
        {
            throw new ArgumentNullException(nameof(resourceName));
        }

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
        if (assembly is null)
        {
            throw new ArgumentNullException(nameof(assembly));
        }

        if (resourceName is null)
        {
            throw new ArgumentNullException(nameof(resourceName));
        }

        resourceName = resourceName.Replace("/", ".");
        var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new InvalidOperationException($"Could not load embedded resource '{resourceName}'");
        }

        return stream;
    }
}