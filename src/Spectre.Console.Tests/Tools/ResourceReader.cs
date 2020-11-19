using System;
using System.IO;

namespace Spectre.Console.Tests
{
    public static class ResourceReader
    {
        public static Stream LoadResourceStream(string resourceName)
        {
            if (resourceName is null)
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            var assembly = typeof(EmbeddedResourceDataAttribute).Assembly;
            resourceName = resourceName.Replace("/", ".", StringComparison.Ordinal);

            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}
