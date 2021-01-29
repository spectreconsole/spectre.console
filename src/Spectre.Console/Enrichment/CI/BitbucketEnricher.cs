using System.Collections.Generic;

namespace Spectre.Console.Enrichment
{
    internal sealed class BitbucketEnricher : IProfileEnricher
    {
        public string Name => "Bitbucket";

        public bool Enabled(IDictionary<string, string> environmentVariables)
        {
            return environmentVariables.ContainsKey("BITBUCKET_REPO_OWNER") ||
                environmentVariables.ContainsKey("BITBUCKET_REPO_SLUG") ||
                environmentVariables.ContainsKey("BITBUCKET_COMMIT");
        }

        public void Enrich(Profile profile)
        {
            profile.Capabilities.Interactive = false;
        }
    }
}
