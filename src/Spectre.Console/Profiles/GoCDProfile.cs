using System.Collections.Generic;

namespace Spectre.Console
{
    internal sealed class GoCDProfile : IProfileEnricher
    {
        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("GO_SERVER_URL");
        }

        public void Enrich(Profile profile)
        {
            profile.Name = "GoCD";
            profile.Capabilities.Interactive = false;
        }
    }
}
