using System.Threading.Tasks;
using Docs.Shortcodes;
using Statiq.App;
using Statiq.Common;
using Statiq.Web;

namespace Docs
{
    public static class Program
    {
        public static async Task<int> Main(string[] args) =>
            await Bootstrapper.Factory
                .CreateWeb(args)
                .AddSetting(Keys.Host, "spectreconsole.net")
                .AddSetting(Keys.LinksUseHttps, true)
                .AddSetting(Constants.EditLink, ConfigureEditLink())
                .ConfigureSite("spectreconsole", "spectre.console", "main")
                .ConfigureDeployment(deployBranch: "docs")
                .AddShortcode("Children", typeof(ChildrenShortcode))
                .AddShortcode("ColorTable", typeof(ColorTableShortcode))
                .AddShortcode("EmojiTable", typeof(EmojiTableShortcode))
                .AddShortcode("Alert", typeof(AlertShortcode))
                .AddShortcode("AsciiCast", typeof(AsciiCastShortcode))
                .AddPipelines()
                .AddProcess(ProcessTiming.Initialization, _ => new ProcessLauncher("npm", "install", "--audit", "false", "--fund", "false")
                {
                    LogErrors = false
                })
                .AddProcess(ProcessTiming.BeforeDeployment, _ => new ProcessLauncher("npm", "run build:tailwind")
                {
                    LogErrors = false
                })
                .RunAsync();

        private static Config<string> ConfigureEditLink()
        {
            return Config.FromDocument((doc, ctx) =>
            {
                return string.Format("https://github.com/{0}/{1}/edit/{2}/docs/input/{3}",
                    ctx.GetString(Constants.Site.Owner),
                    ctx.GetString(Constants.Site.Repository),
                    ctx.GetString(Constants.Site.Branch),
                    doc.Source.GetRelativeInputPath());
            });
        }
    }
}
