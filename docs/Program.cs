using System.Collections.Generic;
using System.Threading.Tasks;
using Docs.Pipelines;
using Docs.Shortcodes;
using Docs.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Statiq.App;
using Statiq.Common;
using Statiq.Core;
using Statiq.Web;
using Statiq.Web.Pipelines;

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
                .AddSetting(Constants.SourceFiles, new List<string>
                {
                    "../../examples/**/{!bin,!obj,!packages,!*.Tests,}/**/*.cs",
                    "../../src/**/{!bin,!obj,!packages,!*.Tests,}/**/*.cs" }
                )
                .AddSetting(Constants.SolutionFiles, new List<string> { @"../examples/Examples.sln" } )
                .ConfigureSite("spectreconsole", "spectre.console", "main")
                .ConfigureDeployment(deployBranch: "docs")
                .ConfigureServices(i => i.AddTransient<SolutionWorkspaceProvider>())
                .AddShortcode("Children", typeof(ChildrenShortcode))
                .AddShortcode("ColorTable", typeof(ColorTableShortcode))
                .AddShortcode("EmojiTable", typeof(EmojiTableShortcode))
                .AddShortcode("Alert", typeof(AlertShortcode))
                .AddShortcode("AsciiCast", typeof(AsciiCastShortcode))
                .AddShortcode("Example", typeof(ExampleSnippet))
                .AddPipelines()
                .BuildPipeline(
			        "Bootstrap",
			            builder => builder
				            .WithInputReadFiles("../node_modules/asciinema-player/dist/bundle/asciinema-player.js")
				            .WithProcessModules(new SetDestination(Config.FromDocument(doc => new NormalizedPath($"./assets/{doc.Source.FileName}")), true))
				    .WithOutputWriteFiles()
                )
                .AddProcess(ProcessTiming.Initialization, _ => new ProcessLauncher("npm", "install --audit false --fund false --progress false")
                {
                    LogErrors = false
                })
                .AddProcess(ProcessTiming.Initialization, _ => new ProcessLauncher("dotnet", "playwright install chromium"))
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
