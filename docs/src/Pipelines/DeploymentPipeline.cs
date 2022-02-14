using Statiq.Common;
using Statiq.Web.GitHub;
using Statiq.Web.Netlify;

namespace Docs.Pipelines
{
    public class DeploymentPipeline : Statiq.Core.Pipeline
    {
        public DeploymentPipeline()
        {
            Deployment = true;
            OutputModules = new ModuleList
            {
                new DeployNetlifySite(
                    siteId: Config.FromSetting<string>(Constants.Deployment.NetlifySiteId),
                    accessToken: Config.FromSetting<string>(Constants.Deployment.NetlifyAccessToken)
                )
            };
        }
    }
}