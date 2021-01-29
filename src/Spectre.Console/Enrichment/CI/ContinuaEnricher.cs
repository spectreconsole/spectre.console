using System.Collections.Generic;

namespace Spectre.Console.Enrichment
{
    internal sealed class ContinuaEnricher : IProfileEnricher
    {
        public string Name => "ContinuaCI";

        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("ContinuaCI.Version");
        }

        public void Enrich(Profile profile)
        {
            profile.Capabilities.Interactive = false;
        }
    }
}
