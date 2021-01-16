using System.Collections.Generic;

namespace Spectre.Console
{
    internal sealed class TfsProfile : IProfileEnricher
    {
        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("TF_BUILD");
        }

        public void Enrich(Profile profile)
        {
            profile.Name = "TFS";
            profile.Capabilities.Interactive = false;
        }
    }
}
