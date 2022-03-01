using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using Docs.Pipelines;
using Microsoft.AspNetCore.Html;
using Statiq.CodeAnalysis;
using Statiq.Common;

namespace Docs.Extensions;

public static class IExecutionContextExtensions
{
    private static readonly object _executionCacheLock = new();
    private static readonly ConcurrentDictionary<string, object> _executionCache = new();
    private static Guid _lastExecutionId = Guid.Empty;

    public record SidebarItem(IDocument Node, string Title, bool ShowLink, ImmutableList<SidebarItem> Leafs);

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

        var documents = context.GetExecutionCache(nameof(TryGetCommentIdDocument), ctx => ctx.Outputs.FromPipeline(nameof(ExampleSyntax)).Flatten());

        var matches = documents
            .Where(x => x.GetString(CodeAnalysisKeys.CommentId)?.Equals(commentId, StringComparison.OrdinalIgnoreCase) == true)
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

    public static T GetExecutionCache<T>(this IExecutionContext context, string key, Func<IExecutionContext, T> getter)
    {
        lock (_executionCacheLock)
        {
            if (_lastExecutionId != context.ExecutionId)
            {
                _executionCache.Clear();
                _lastExecutionId = context.ExecutionId;
            }

            return (T)_executionCache.GetOrAdd(key, valueFactory: _ => getter.Invoke(context));
        }
    }

    public static NormalizedPath FindCard(this IExecutionContext context, Guid docId)
    {
        var cardLookups = context.GetExecutionCache(nameof(FindCard), ctx =>
        {
            return ctx.Outputs
                .Select(i => new { DocId = i.GetString("DocId"), Destination = i.Destination })
                .Where(i => i.DocId != null)
                .ToDictionary(i => i.DocId, i => i.Destination);
        });

        return !cardLookups.ContainsKey(docId.ToString()) ? null : cardLookups[docId.ToString()];
    }

    public static SidebarItem GetSidebar(this IExecutionContext context)
    {
        return context.GetExecutionCache(nameof(GetSidebar), ctx =>
        {
            var outputPages = ctx.OutputPages;
            var root = outputPages["index.html"][0];
            var children = outputPages
                .GetChildrenOf(root)
                .OrderBy(i => i.GetInt("Order"))
                .OnlyVisible().Select(child =>
                {
                    var showLink = child.ShowLink();
                    var children = outputPages
                        .GetChildrenOf(child)
                        .OnlyVisible()
                        .Select(subChild =>
                            new SidebarItem(subChild, subChild.GetTitle(), true, ImmutableList<SidebarItem>.Empty))
                        .ToImmutableList();

                    return new SidebarItem(child, child.GetTitle(), showLink, children);
                }).ToImmutableList();

            return new SidebarItem(root, root.GetTitle(), false, children);
        });
    }

    public static HtmlString GetTypeLink(this IExecutionContext context, IDocument document) =>
        context.GetTypeLink(document, null, true);

    public static HtmlString GetTypeLink(this IExecutionContext context, IDocument document, bool linkTypeArguments) =>
        context.GetTypeLink(document, null, linkTypeArguments);

    public static HtmlString GetTypeLink(this IExecutionContext context, IDocument document, string name) =>
        context.GetTypeLink(document, name, true);

    public static HtmlString GetTypeLink(this IExecutionContext context, IDocument document, string name,
        bool linkTypeArguments)
    {
        name ??= document.GetString(CodeAnalysisKeys.DisplayName);

        // Link nullable types to their type argument
        if (document.GetString(CodeAnalysisKeys.Name) == "Nullable")
        {
            var nullableType = document.GetDocumentList(CodeAnalysisKeys.TypeArguments)?.FirstOrDefault();
            if (nullableType != null)
            {
                return context.GetTypeLink(nullableType, name);
            }
        }

        // If it wasn't nullable, format the name
        name = context.GetFormattedHtmlName(name);

        // Link the type and type parameters separately for generic types
        IReadOnlyList<IDocument> typeArguments = document.GetDocumentList(CodeAnalysisKeys.TypeArguments);
        if (typeArguments?.Count > 0)
        {
            // Link to the original definition of the generic type
            document = document.GetDocument(CodeAnalysisKeys.OriginalDefinition) ?? document;

            if (linkTypeArguments)
            {
                // Get the type argument positions
                var begin = name.IndexOf("<wbr>&lt;", StringComparison.Ordinal) + 9;
                var openParen = name.IndexOf("&gt;<wbr>(", StringComparison.Ordinal);
                var end = name.LastIndexOf("&gt;<wbr>", openParen == -1 ? name.Length : openParen,
                    StringComparison.Ordinal); // Don't look past the opening paren if there is one

                if (begin == -1 || end == -1)
                {
                    return new HtmlString(name);
                }

                // Remove existing type arguments and insert linked type arguments (do this first to preserve original indexes)
                name = name
                    .Remove(begin, end - begin)
                    .Insert(begin,
                        string.Join(", <wbr>", typeArguments.Select(x => context.GetTypeLink(x, true).Value)));

                // Insert the link for the type
                if (!document.Destination.IsNullOrEmpty)
                {
                    name = name.Insert(begin - 9, "</a>").Insert(0, $"<a href=\"{context.GetLink(document)}\">");
                }

                return new HtmlString(name);
            }
        }

        // If it's a type parameter, create an anchor link to the declaring type's original definition
        if (document.GetString(CodeAnalysisKeys.Kind) == "TypeParameter")
        {
            var declaringType = document.GetDocument(CodeAnalysisKeys.DeclaringType)
                ?.GetDocument(CodeAnalysisKeys.OriginalDefinition);
            if (declaringType != null)
            {
                return new HtmlString(declaringType.Destination.IsNullOrEmpty
                    ? name
                    : $"<a href=\"{context.GetLink(declaringType)}#typeparam-{document["Name"]}\">{name}</a>");
            }
        }

        return new HtmlString(document.Destination.IsNullOrEmpty
            ? name
            : $"<a href=\"{context.GetLink(document)}\">{name}</a>");
    }

    /// <summary>
    /// Formats a symbol or other name by encoding HTML characters and
    /// adding HTML break elements as appropriate.
    /// </summary>
    /// <param name="context">The execution context.</param>
    /// <param name="name">The name to format.</param>
    /// <returns>The name formatted for use in HTML.</returns>
    public static string GetFormattedHtmlName(this IExecutionContext context, string name)
    {
        if (name == null)
        {
            return string.Empty;
        }

        // Encode and replace .()<> with word break opportunities
        name = WebUtility.HtmlEncode(name)
            .Replace(".", "<wbr>.")
            .Replace("(", "<wbr>(")
            .Replace(")", ")<wbr>")
            .Replace(", ", ", <wbr>")
            .Replace("&lt;", "<wbr>&lt;")
            .Replace("&gt;", "&gt;<wbr>");

        // Add additional break opportunities in long un-broken segments
        var segments = name.Split(new[] { "<wbr>" }, StringSplitOptions.None).ToList();
        var replaced = false;
        for (var c = 0; c < segments.Count; c++)
        {
            if (segments[c].Length > 20)
            {
                segments[c] = new string(segments[c]
                    .SelectMany(
                        (x, i) => char.IsUpper(x) && i != 0 ? new[] { '<', 'w', 'b', 'r', '>', x } : new[] { x })
                    .ToArray());
                replaced = true;
            }
        }

        return replaced ? string.Join("<wbr>", segments) : name;
    }
}