using System;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Docs.Pipelines;
using Microsoft.CodeAnalysis;
using Statiq.CodeAnalysis;
using Statiq.Common;
using IDocument = Statiq.Common.IDocument;

namespace Docs.Utilities;

public static class StatiqExtensions
{
    public static string RemoveAnchorTags(this string original)
    {
        var parser = new HtmlParser();
        var document = parser.ParseDocument(original);

        foreach (var element in document.All)
        {
            if (element.LocalName == "a")
            {
                element.Insert(AdjacentPosition.BeforeBegin, element.InnerHtml);
                element.Remove();
            }
        }

        return document.Body?.InnerHtml ?? original;
    }

    public static bool TryGetCommentIdDocument(this IExecutionContext context, string commentId, out IDocument document,
        out string error)
    {
        context.ThrowIfNull(nameof(context));

        if (string.IsNullOrWhiteSpace(commentId))
        {
            document = default;
            error = default;
            return false;
        }

        var documents = context.Outputs.FromPipeline(nameof(ExampleSyntax)).Flatten();
        var matches = documents
            .Where(x =>
                x.GetString(CodeAnalysisKeys.CommentId)?.Equals(commentId, StringComparison.OrdinalIgnoreCase) == true)
            .ToImmutableDocumentArray();

        if (matches.Length == 1)
        {
            document = matches[0];
            error = default;
            return true;
        }

        document = default;
        error = matches.Length > 1
            ? $"Multiple ambiguous matching documents found for commentId \"{commentId}\""
            : $"Couldn't find document with xref \"{commentId}\"";
        return false;
    }

    public static async Task<string> HighlightDeclaration(this IDocument document)
    {
        var comp =  document.Get<Compilation>(CodeAnalysisKeys.Compilation);
        var symbol = document.Get<ISymbol>(CodeAnalysisKeys.Symbol);
        var highlightElement = await HighlightService.Highlight(comp, symbol, HighlightService.HighlightOption.Declaration);
        return $"{highlightElement}";
    }

    public static async Task<string> HighlightBody(this IDocument document)
    {
        var comp =  document.Get<Compilation>(CodeAnalysisKeys.Compilation);
        var symbol = document.Get<ISymbol>(CodeAnalysisKeys.Symbol);
        var highlightElement = await HighlightService.Highlight(comp, symbol, HighlightService.HighlightOption.Body);
        return $"{highlightElement}";
    }
}