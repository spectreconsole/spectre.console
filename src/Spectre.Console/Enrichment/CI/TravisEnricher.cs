using System.Collections.Generic;

namespace Spectre.Console.Enrichment
{
    internal sealed class TravisEnricher : IProfileEnricher
    {
        public string Name => "Travis";

        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("TRAVIS");
        }

        public void Enrich(Profile profile)
        {
            profile.Capabilities.Interactive = false;
        }
    }
}
