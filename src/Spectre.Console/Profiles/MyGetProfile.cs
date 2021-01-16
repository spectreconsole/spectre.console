using System;
using System.Collections.Generic;

namespace Spectre.Console
{
    internal sealed class MyGetProfile : IProfileEnricher
    {
        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            if (environmentVariables.TryGetValue("BuildRunner", out var value))
            {
                return value?.Equals("MyGet", StringComparison.OrdinalIgnoreCase) ?? false;
            }

            return false;
        }

        public void Enrich(Profile profile)
        {
            profile.Name = "MyGet";
            profile.Capabilities.Interactive = false;
        }
    }
}
