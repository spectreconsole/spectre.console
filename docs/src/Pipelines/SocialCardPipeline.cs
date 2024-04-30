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
        private IPlaywright _playwright;
        private IBrowser _browser;
        private WebApplication _app;
        private IBrowserContext _context;

        protected override async Task BeforeExecutionAsync(IExecutionContext context)
        {
            var builder = WebApplication.CreateBuilder();
            builder.Logging.ClearProviders();

            builder.Services
                .AddRazorPages()
                .WithRazorPagesRoot("/src/SocialCards/");

            _app = builder.Build();
            _app.MapRazorPages();
            _app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(builder.Environment.ContentRootPath, "src/SocialCards")),
                RequestPath = "/static"
            });

            await _app.StartAsync().ConfigureAwait(false);

            _playwright = await Playwright.CreateAsync().ConfigureAwait(false);
            _browser = await _playwright.Chromium.LaunchAsync().ConfigureAwait(false);
            _context = await _browser.NewContextAsync(new BrowserNewContextOptions {
                ViewportSize = new ViewportSize { Width = 1200, Height = 618 },
            }).ConfigureAwait(false);
        }

        protected override async Task FinallyAsync(IExecutionContext context)
        {
            await _context.DisposeAsync().ConfigureAwait(false);
            await _browser.DisposeAsync().ConfigureAwait(false);
            _playwright.Dispose();
            await _app.DisposeAsync().ConfigureAwait(false);
            await base.FinallyAsync(context);
        }

        protected override async Task<IEnumerable<IDocument>> ExecuteInputAsync(IDocument input, IExecutionContext context)
        {
            var url = _app.Urls.FirstOrDefault(u => u.StartsWith("http://"));
            var page = await _context.NewPageAsync().ConfigureAwait(false);

            var title = input.GetString("Title");
            var description = input.GetString("Description");
            var highlights = input.GetList<string>("Highlights") ?? Array.Empty<string>();

            await page.GotoAsync($"{url}/?title={title}&desc={description}&highlights={string.Join("||", highlights)}");

            // This will not just wait for the  page to load over the network, but it'll also give
            // chrome a chance to complete rendering of the fonts while the wait timeout completes.
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle).ConfigureAwait(false);
            var bytes = await page.ScreenshotAsync().ConfigureAwait(false);
            await page.CloseAsync().ConfigureAwait(false);

            var destination = input.Destination.InsertSuffix("-social").ChangeExtension("png");
            var doc = context.CreateDocument(
                input.Source,
                destination,
                new MetadataItems { { "DocId", input.Id }},
                context.GetContentProvider(bytes));

            return new[] { doc };
        }
    }
}