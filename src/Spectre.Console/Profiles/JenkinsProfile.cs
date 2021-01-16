using System.Collections.Generic;

namespace Spectre.Console
{
    internal sealed class JenkinsProfile : IProfileEnricher
    {
        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("JENKINS_URL");
        }

        public void Enrich(Profile profile)
        {
            profile.Name = "Jenkins";
            profile.Capabilities.Interactive = false;
        }
    }
}
