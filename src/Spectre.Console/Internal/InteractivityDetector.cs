using System;
using System.Collections.Generic;

namespace Spectre.Console.Internal
{
    internal static class InteractivityDetector
    {
        private static readonly Dictionary<string, Func<string, bool>> _environmentVariables;

        static InteractivityDetector()
        {
            _environmentVariables = new Dictionary<string, Func<string, bool>>
            {
                { "APPVEYOR", v => !string.IsNullOrWhiteSpace(v) },
                { "bamboo_buildNumber", v => !string.IsNullOrWhiteSpace(v) },
                { "BITBUCKET_REPO_OWNER", v => !string.IsNullOrWhiteSpace(v) },
                { "BITBUCKET_REPO_SLUG", v => !string.IsNullOrWhiteSpace(v) },
                { "BITBUCKET_COMMIT", v => !string.IsNullOrWhiteSpace(v) },
                { "BITRISE_BUILD_URL", v => !string.IsNullOrWhiteSpace(v) },
                { "ContinuaCI.Version", v => !string.IsNullOrWhiteSpace(v) },
                { "CI_SERVER", v => v.Equals("yes", StringComparison.OrdinalIgnoreCase) }, // GitLab
                { "GITHUB_ACTIONS", v => v.Equals("true", StringComparison.OrdinalIgnoreCase) },
                { "GO_SERVER_URL", v => !string.IsNullOrWhiteSpace(v) },
                { "JENKINS_URL", v => !string.IsNullOrWhiteSpace(v) },
                { "BuildRunner", v => v.Equals("MyGet", StringComparison.OrdinalIgnoreCase) },
                { "TEAMCITY_VERSION", v => !string.IsNullOrWhiteSpace(v) },
                { "TF_BUILD", v => !string.IsNullOrWhiteSpace(v) }, // TFS and Azure
                { "TRAVIS", v => !string.IsNullOrWhiteSpace(v) },
            };
        }

        public static bool IsInteractive()
        {
            if (!Environment.UserInteractive)
            {
                return false;
            }

            foreach (var variable in _environmentVariables)
            {
                var func = variable.Value;
                var value = Environment.GetEnvironmentVariable(variable.Key);
                if (!string.IsNullOrWhiteSpace(value) && variable.Value(value))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
