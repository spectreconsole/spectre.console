using System.Collections.Generic;

namespace Spectre.Console.Enrichment
{
    internal sealed class AppVeyorEnricher : IProfileEnricher
    {
        public string Name => "AppVeyor";

        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("APPVEYOR");
        }

        public void Enrich(Profile profile)
        {
            profile.Capabilities.Interactive = false;
        }
    }
}
