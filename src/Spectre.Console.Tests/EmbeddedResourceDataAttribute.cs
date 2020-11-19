using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xunit.Sdk;

namespace Spectre.Console.Tests
{
    public sealed class EmbeddedResourceDataAttribute : DataAttribute
    {
        private readonly string _args;

        public EmbeddedResourceDataAttribute(string args)
        {
            _args = args ?? throw new ArgumentNullException(nameof(args));
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var result = new object[1];
            result[0] = ReadManifestData(_args);
            return new[] { result };
        }

        public static string ReadManifestData(string resourceName)
        {
            if (resourceName is null)
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            using (var stream = ResourceReader.LoadResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException("Could not load manifest resource stream.");
                }

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd().NormalizeLineEndings();
                }
            }
        }
    }
}
