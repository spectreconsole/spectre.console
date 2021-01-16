using System;
using System.IO;

namespace Spectre.Console
{
    internal static class ResourceReader
    {
        public static string ReadManifestData(string resourceName)
        {
            if (resourceName is null)
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            var assembly = typeof(ResourceReader).Assembly;
            resourceName = resourceName.ReplaceExact("/", ".");

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException("Could not load manifest resource stream.");
                }

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd().NormalizeNewLines();
                }
            }
        }
    }
}
