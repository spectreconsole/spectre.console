using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Docs.Pipeline;
using Docs.Pipelines;
using Docs.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Statiq.CodeAnalysis;
using Statiq.Common;
using Document = Microsoft.CodeAnalysis.Document;

namespace Docs.Shortcodes;

public class ExampleSnippet : Shortcode
{
    protected const string Solution = nameof(Solution);
    protected const string Project = nameof(Project);
    protected const string Symbol = nameof(Symbol);
    protected const string BodyOnly = nameof(BodyOnly);

    public override async Task<ShortcodeResult> ExecuteAsync(KeyValuePair<string, string>[] args, string content,
        IDocument document, IExecutionContext context)
    {
        var props = args.ToDictionary(Solution, Project, Symbol, BodyOnly);
        var symbolName = props.GetString(Symbol);
        var bodyOnly = props.Get<bool?>(BodyOnly) ?? symbolName.StartsWith("m:", StringComparison.InvariantCultureIgnoreCase);

        if (!context.TryGetCommentIdDocument(symbolName, out var apiDocument, out var error))
        {
            return string.Empty;
        }

        var doc = apiDocument.Get<IDocument>(CodeAnalysisKeys.Type);
        return string.Empty;

        // var highlightElement = await HighlightService.Highlight(symbol, workspace, bodyOnly);
        // ShortcodeResult shortcodeResult = $"<pre><code>{highlightElement}</code></pre>";
        // return shortcodeResult;
    }
}