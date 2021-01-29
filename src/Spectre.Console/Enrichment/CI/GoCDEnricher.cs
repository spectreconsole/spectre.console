using System.Collections.Generic;

namespace Spectre.Console.Enrichment
{
    internal sealed class GoCDEnricher : IProfileEnricher
    {
        public string Name => "GoCD";

        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("GO_SERVER_URL");
        }

        public void Enrich(Profile profile)
        {
            profile.Capabilities.Interactive = false;
        }
    }
}
