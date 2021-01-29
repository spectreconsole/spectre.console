using System.Collections.Generic;

namespace Spectre.Console.Enrichment
{
    internal sealed class TfsEnricher : IProfileEnricher
    {
        public string Name => "TFS";

        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("TF_BUILD");
        }

        public void Enrich(Profile profile)
        {
            profile.Capabilities.Interactive = false;
        }
    }
}
