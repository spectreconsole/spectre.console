using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Statiq.Common;
using Statiq.Core;
using Statiq.Web;
using Statiq.Web.Modules;
using Statiq.Web.Pipelines;

namespace Docs.Pipelines
{
    public class SocialImages : Pipeline
    {
        public SocialImages()
        {
            Dependencies.AddRange(nameof(Inputs));

            ProcessModules = new ModuleList
            {
                new GetPipelineDocuments(ContentType.Content),

                // Filter to non-archive content
                new FilterDocuments(Config.FromDocument(doc => !Archives.IsArchive(doc))),

                // Process the content
                new CacheDocuments
                {
                    new AddTitle(),
                    new SetDestination(true),
                    new ExecuteIf(Config.FromSetting(WebKeys.OptimizeContentFileNames, true))
                    {
                        new OptimizeFileName()
                    },
                    new GenerateSocialImage(),
                }
            };

            OutputModules = new ModuleList { new WriteFiles() };
        }
    }

    class GenerateSocialImage : ParallelModule
    {
        private IBrowser _browser;
        private WebApplication _app;
        private string _url;

        protected override async Task BeforeExecutionAsync(IExecutionContext context)
        {
            var builder = WebApplication.CreateBuilder();
            builder.Logging.ClearProviders();

            builder.Services
                .AddRazorPages()
                .WithRazorPagesRoot("/src/SocialCards/");

            _app = builder.Build();
            _app.MapRazorPages();
            _app.UseDirectoryBrowser();
            _app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(builder.Environment.ContentRootPath, "src/SocialCards")),
                RequestPath = "/static"
            });

            await _app.StartAsync();

            var playwright = await Playwright.CreateAsync();
            _browser = await playwright.Chromium.LaunchAsync();
            _url = _app.Urls.FirstOrDefault(u => u.StartsWith("http://"));
        }

        protected override async Task FinallyAsync(IExecutionContext context)
        {
            await _app.DisposeAsync().ConfigureAwait(false);
            await _browser.DisposeAsync().ConfigureAwait(false);
            await base.FinallyAsync(context);
        }

        protected override async Task<IEnumerable<IDocument>> ExecuteInputAsync(IDocument input, IExecutionContext context)
        {
            var page = await _browser.NewPageAsync(new BrowserNewPageOptions
                {
                    ViewportSize = new ViewportSize { Width = 680, Height = 340 }
                }
            );

            var outputs = new List<IDocument>();
            var title = input.GetString("Title");
            var description = input.GetString("Description");
            var highlights = input.GetList<string>("Highlights") ?? Array.Empty<string>();

            await page.GotoAsync($"{_url}/?title={title}&desc={description}&highlights={string.Join("||", highlights)}");
            var bytes = await page.ScreenshotAsync();

            var destination = input.Destination.InsertSuffix("-social").ChangeExtension("png");
            var doc = context.CreateDocument(
                input.Source,
                destination,
                new MetadataItems { { "DocId", input.Id }},
                context.GetContentProvider(bytes));

            outputs.Add(doc);
            return outputs;
        }
    }
}