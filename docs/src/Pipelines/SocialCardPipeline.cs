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

    class GenerateSocialImage : Module
    {
        protected override async Task<IEnumerable<IDocument>> ExecuteContextAsync(IExecutionContext context)
        {
            context.Logger.LogInformation("Starting social image generation");
            var builder = WebApplication.CreateBuilder();
            builder.Logging.ClearProviders();

            builder.Services
                .AddRazorPages()
                .WithRazorPagesRoot("/src/SocialCards/");

            await using var app = builder.Build();
            app.MapRazorPages();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(builder.Environment.ContentRootPath, "src/SocialCards")),
                RequestPath = "/static"
            });

            await app.StartAsync().ConfigureAwait(false);

            context.Logger.LogInformation("Web application started");

            using var playwright = await Playwright.CreateAsync().ConfigureAwait(false);
            context.Logger.LogInformation("Playwright started");
            var browser = await playwright.Chromium.LaunchAsync().ConfigureAwait(false);
            context.Logger.LogInformation("Chrome launched");

            var url = app.Urls.FirstOrDefault(u => u.StartsWith("http://"));
            var page = await browser.NewPageAsync(new BrowserNewPageOptions
                {
                    ViewportSize = new ViewportSize { Width = 1200, Height = 618 },
                }
            );

            var outputs = new List<IDocument>();
            foreach (var input in context.Inputs)
            {
                var title = input.GetString("Title");
                var description = input.GetString("Description");
                var highlights = input.GetList<string>("Highlights") ?? Array.Empty<string>();

                context.Logger.LogInformation("Build social card for {Url}", url);
                await page.GotoAsync($"{url}/?title={title}&desc={description}&highlights={string.Join("||", highlights)}");
                context.Logger.LogInformation("Finished building social card for {Url}", url);
                var bytes = await page.ScreenshotAsync();

                var destination = input.Destination.InsertSuffix("-social").ChangeExtension("png");
                var doc = context.CreateDocument(
                    input.Source,
                    destination,
                    new MetadataItems { { "DocId", input.Id }},
                    context.GetContentProvider(bytes));

                outputs.Add(doc);
            }

            return outputs;
        }
    }
}