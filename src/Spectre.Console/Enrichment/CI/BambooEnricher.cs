using System.Collections.Generic;

namespace Spectre.Console.Enrichment
{
    internal sealed class BambooEnricher : IProfileEnricher
    {
        public string Name => "Bamboo";

        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("bamboo_buildNumber");
        }

        public void Enrich(Profile profile)
        {
            profile.Capabilities.Interactive = false;
        }
    }
}
