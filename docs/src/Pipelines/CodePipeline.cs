using System.Linq;
using System.Net;
using Docs.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Statiq.CodeAnalysis;
using Statiq.Common;
using Statiq.Core;
using Statiq.Web;
using Statiq.Web.Pipelines;

namespace Docs.Pipelines;



/// <summary>
/// Loads source files.
/// </summary>
public class Code : Pipeline
{
    public Code()
    {
        InputModules = new ModuleList(
            new ReadFiles(
                Config.FromSettings(settings
                    => settings.GetList<string>(Constants.SourceFiles).AsEnumerable())));
    }
}

/// <summary>
/// Loads source files.
/// </summary>
public class ExampleCode : Pipeline
{
    public ExampleCode()
    {
        Dependencies.Add(nameof(Code));

        InputModules = new ModuleList(
            new ReadFiles(
                Config.FromSettings(settings
                    => settings.GetList<string>(Constants.ExampleSourceFiles).AsEnumerable())));
    }
}

/// <summary>
/// Uses Roslyn to analyze any source files loaded in the previous
/// pipeline along with any specified assemblies. This pipeline
/// results in documents that represent Roslyn symbols.
/// </summary>
public class ExampleSyntax : Pipeline
{
    public ExampleSyntax()
    {
        Dependencies.Add(nameof(ExampleCode));
        DependencyOf.Add(nameof(Content));

        ProcessModules = new ModuleList
        {
            new ConcatDocuments(nameof(Code)),
            new ConcatDocuments(nameof(ExampleCode)),
            new CacheDocuments(
                new AnalyzeCSharp()
                    .WhereNamespaces(true)
                    .WherePublic()
                    .WithCssClasses("code", "cs")
                    .WithDestinationPrefix("syntax")
                    .WithAssemblySymbols()
                    // we need to load Spectre.Console for compiling, but we don't need to process it in Statiq
                    .WhereNamespaces(i => !i.StartsWith("Spectre.Console"))
                    .WithImplicitInheritDoc(false),
                new ExecuteConfig(Config.FromDocument((doc, _) =>
                {
                    // Add metadata
                    var metadataItems = new MetadataItems
                    {
                        // Calculate an xref that includes a "api-" prefix to avoid collisions
                        { WebKeys.Xref, "syntax-" + doc.GetString(CodeAnalysisKeys.CommentId) },
                    };

                    var contentProvider = doc.ContentProvider;
                    return doc.Clone(metadataItems, contentProvider);
                }))).WithoutSourceMapping()
        };
    }
}

/// <summary>
/// Generates API documentation pipeline.
/// </summary>
public class Api : Pipeline
{
    public Api()
    {
        Dependencies.Add(nameof(Code));
        DependencyOf.Add(nameof(Content));

        ProcessModules = new ModuleList
        {
            new ConcatDocuments(nameof(Code)),
            new CacheDocuments(
                new AnalyzeCSharp()
                    .WhereNamespaces(ns => ns.StartsWith("Spectre.Console") && !ns.Contains("Analyzer") &&
                                           !ns.Contains("Testing") && !ns.Contains("Examples"))
                    .WherePublic(true)
                    .WithCssClasses("code", "cs")
                    .WithDestinationPrefix("api")
                    .WithAssemblySymbols()
                    .WithImplicitInheritDoc(false),
                new ExecuteConfig(Config.FromDocument((doc, ctx) =>
                {
                    // Calculate a type name to link lookup for auto linking
                    string name = null;

                    var kind = doc.GetString(CodeAnalysisKeys.Kind);
                    switch (kind)
                    {
                        case "NamedType":
                            name = doc.GetString(CodeAnalysisKeys.DisplayName);
                            break;
                        case "Method":
                            var containingType = doc.GetDocument(CodeAnalysisKeys.ContainingType);
                            if (containingType != null)
                            {
                                name =
                                    $"{containingType.GetString(CodeAnalysisKeys.DisplayName)}.{doc.GetString(CodeAnalysisKeys.DisplayName)}";
                            }
                            break;
                    }

                    if (name != null)
                    {
                        var typeNameLinks = ctx.GetRequiredService<TypeNameLinks>();
                        typeNameLinks.Links.AddOrUpdate(WebUtility.HtmlEncode(name), ctx.GetLink(doc),
                            (_, _) => string.Empty);
                    }

                    // Add metadata
                    var metadataItems = new MetadataItems
                    {
                        { WebKeys.Xref, doc.GetString(CodeAnalysisKeys.CommentId) },
                        { WebKeys.Layout, "api/_layout.cshtml" },
                        { Constants.Hidden, true }
                    };

                    var contentProvider = doc.ContentProvider.CloneWithMediaType(MediaTypes.Html);
                    metadataItems.Add(WebKeys.ContentType, ContentType.Content);
                    return doc.Clone(metadataItems, contentProvider);
                }))).WithoutSourceMapping()
        };
    }
}