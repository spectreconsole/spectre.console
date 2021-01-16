using System;
using System.Collections.Generic;

namespace Spectre.Console
{
    internal sealed class GitLabProfile : IProfileEnricher
    {
        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            if (environmentVariables.TryGetValue("CI_SERVER", out var value))
            {
                return value?.Equals("yes", StringComparison.OrdinalIgnoreCase) ?? false;
            }

            return false;
        }

        public void Enrich(Profile profile)
        {
            profile.Name = "GitLab";
            profile.Capabilities.Interactive = false;
        }
    }
}
