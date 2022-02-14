using Statiq.Common;
using Statiq.Web.GitHub;

namespace Docs.Pipelines
{
    public class DeploymentPipeline : Statiq.Core.Pipeline
    {
        public DeploymentPipeline()
        {
            Deployment = true;
            OutputModules = new ModuleList
            {
                new DeployGitHubPages(
                    Config.FromSetting<string>(Constants.Site.Owner),
                    Config.FromSetting<string>(Constants.Site.Repository),
                    Config.FromSetting<string>(Constants.Deployment.GitHubToken))
                        .ToBranch(Config.FromSetting<string>(Constants.Deployment.TargetBranch))
            };
        }
    }
}
