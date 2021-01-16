using System.Collections.Generic;

namespace Spectre.Console
{
    internal sealed class BitbucketProfile : IProfileEnricher
    {
        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("BITBUCKET_REPO_OWNER") ||
                environmentVariables.ContainsKey("BITBUCKET_REPO_SLUG") ||
                environmentVariables.ContainsKey("BITBUCKET_COMMIT");
        }

        public void Enrich(Profile profile)
        {
            profile.Name = "BitBucket";
            profile.Capabilities.Interactive = false;
        }
    }
}
