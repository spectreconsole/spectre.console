using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Docs.Extensions;
using Docs.Utilities;
using Microsoft.CodeAnalysis;
using Statiq.CodeAnalysis;
using Statiq.Common;

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

        if (!context.TryGetCommentIdDocument(symbolName, out var apiDocument, out _))
        {
            return string.Empty;
        }

        var options = HighlightService.HighlightOption.All;
        if (bodyOnly)
        {
            options = HighlightService.HighlightOption.Body;
        }

        var comp =  apiDocument.Get<Compilation>(CodeAnalysisKeys.Compilation);
        var symbol = apiDocument.Get<ISymbol>(CodeAnalysisKeys.Symbol);
        var highlightElement = await HighlightService.Highlight(comp, symbol, options);
        ShortcodeResult shortcodeResult = $"<pre><code>{highlightElement}</code></pre>";
        return shortcodeResult;
    }
}