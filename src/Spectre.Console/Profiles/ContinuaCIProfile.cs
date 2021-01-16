using System.Collections.Generic;

namespace Spectre.Console
{
    internal sealed class ContinuaCIProfile : IProfileEnricher
    {
        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("ContinuaCI.Version");
        }

        public void Enrich(Profile profile)
        {
            profile.Name = "Continua CI";
            profile.Capabilities.Interactive = false;
        }
    }
}
