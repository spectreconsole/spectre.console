using System.Collections.Generic;

namespace Spectre.Console
{
    internal sealed class AppVeyorProfile : IProfileEnricher
    {
        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("APPVEYOR");
        }

        public void Enrich(Profile profile)
        {
            profile.Name = "AppVeyor";
            profile.Capabilities.Interactive = false;
        }
    }
}
