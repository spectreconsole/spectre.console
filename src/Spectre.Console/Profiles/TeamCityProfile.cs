using System.Collections.Generic;

namespace Spectre.Console
{
    internal sealed class TeamCityProfile : IProfileEnricher
    {
        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("TEAMCITY_VERSION");
        }

        public void Enrich(Profile profile)
        {
            profile.Name = "TeamCity";
            profile.Capabilities.Interactive = false;
        }
    }
}
