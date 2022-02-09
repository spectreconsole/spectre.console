using System.Collections.Generic;
using System.Linq;
using System.Net;
using Statiq.CodeAnalysis;
using Statiq.Common;
using Statiq.Core;
using Statiq.Web;
using Statiq.Web.Pipelines;

namespace Docs.Pipelines;

/// <summary>
/// Loads source files.
/// </summary>
public class Code : Statiq.Core.Pipeline
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
/// Uses Roslyn to analyze any source files loaded in the previous
/// pipeline along with any specified assemblies. This pipeline
/// results in documents that represent Roslyn symbols.
/// </summary>
public class Api : Statiq.Core.Pipeline
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
                    .WhereNamespaces(true)
                    .WherePublic()
                    .WithCssClasses("code", "cs")
                    .WithSolutions(Config.FromContext<IEnumerable<string>>(ctx =>
                        ctx.GetList<string>(Constants.SolutionFiles)))
                    .WithAssemblySymbols()
                    .WithImplicitInheritDoc(false)),
            new ExecuteConfig(Config.FromDocument((doc, ctx) =>
            {
                // Add metadata
                var metadataItems = new MetadataItems
                {
                    // Calculate an xref that includes a "api-" prefix to avoid collisions
                    { WebKeys.Xref, "api-" + doc.GetString(CodeAnalysisKeys.QualifiedName) },
                    { Constants.Hidden, true }
                };


                // Change the content provider if needed
                var contentProvider = doc.ContentProvider;
                return doc.Clone(metadataItems, contentProvider);
            }))
        };
    }
}