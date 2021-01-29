using System.Collections.Generic;

namespace Spectre.Console.Enrichment
{
    internal sealed class TeamCityEnricher : IProfileEnricher
    {
        public string Name => "TeamCity";

        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("TEAMCITY_VERSION");
        }

        public void Enrich(Profile profile)
        {
            profile.Capabilities.Interactive = false;
        }
    }
}
