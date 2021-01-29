using System.Collections.Generic;

namespace Spectre.Console.Enrichment
{
    internal sealed class WindowsTerminalEnricher : IProfileEnricher
    {
        public string Name => "Windows Terminal";

        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("WT_SESSION");
        }

        public void Enrich(Profile profile)
        {
            profile.Capabilities.Links = true;
        }
    }
}
