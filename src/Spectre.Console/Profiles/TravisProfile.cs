using System.Collections.Generic;

namespace Spectre.Console
{
    internal sealed class TravisProfile : IProfileEnricher
    {
        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("TRAVIS");
        }

        public void Enrich(Profile profile)
        {
            profile.Name = "Travis";
            profile.Capabilities.Interactive = false;
        }
    }
}
