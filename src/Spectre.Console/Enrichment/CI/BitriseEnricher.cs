using System.Collections.Generic;

namespace Spectre.Console.Enrichment
{
    internal sealed class BitriseEnricher : IProfileEnricher
    {
        public string Name => "Bitrise";

        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("BITRISE_BUILD_URL");
        }

        public void Enrich(Profile profile)
        {
            profile.Capabilities.Interactive = false;
        }
    }
}
