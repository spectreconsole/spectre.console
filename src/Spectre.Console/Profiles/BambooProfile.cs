using System.Collections.Generic;

namespace Spectre.Console
{
    internal sealed class BambooProfile : IProfileEnricher
    {
        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("bamboo_buildNumber");
        }

        public void Enrich(Profile profile)
        {
            profile.Name = "Bamboo";
            profile.Capabilities.Interactive = false;
        }
    }
}
