using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using Docs.Extensions;
using Microsoft.AspNetCore.Html;
using Statiq.CodeAnalysis;
using Statiq.Common;

namespace Docs.Utilities;

public static class IExecutionContextExtensions
{
    private static readonly object _sidebarLock = new();
    private static readonly object _cardLookupLock = new();

    private static SidebarItem _sidebarItem;
    private static Dictionary<string, NormalizedPath> _cardLookup;

    public record SidebarItem(IDocument Node, string Title, bool ShowLink, ImmutableList<SidebarItem> Leafs);

    public static NormalizedPath FindCard(this IExecutionContext context, Guid docId)
    {
        lock (_cardLookupLock)
        {
            if (_cardLookup == null)
            {
                 _cardLookup = context.Outputs
                     .Select(i => new {DocId = i.GetString("DocId"), Destination = i.Destination})
                     .Where(i => i.DocId != null)
                     .ToDictionary(i => i.DocId, i => i.Destination);
            }

            return !_cardLookup.ContainsKey(docId.ToString()) ? null : _cardLookup[docId.ToString()];
        }
    }

    public static SidebarItem GetSidebar(this IExecutionContext context)
    {
        if (_sidebarItem != null)
        {
            return _sidebarItem;
        }

        lock (_sidebarLock)
        {
            if (_sidebarItem != null)
            {
                return _sidebarItem;
            }

            var outputPages = context.OutputPages.Cached();
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
                        .Select(subChild => new SidebarItem(subChild, subChild.GetTitle(), true, ImmutableList<SidebarItem>.Empty))
                        .ToImmutableList();

                    return new SidebarItem(child, child.GetTitle(), showLink, children);
                }).ToImmutableList();

            _sidebarItem = new SidebarItem(root, root.GetTitle(), false, children);

        }

        return _sidebarItem;
    }

    public static HtmlString GetTypeLink(this IExecutionContext context, IDocument document) => context.GetTypeLink(document, null, true);

    public static HtmlString GetTypeLink(this IExecutionContext context, IDocument document, bool linkTypeArguments) => context.GetTypeLink(document, null, linkTypeArguments);

    public static HtmlString GetTypeLink(this IExecutionContext context, IDocument document, string name) => context.GetTypeLink(document, name, true);

    public static HtmlString GetTypeLink(this IExecutionContext context, IDocument document, string name, bool linkTypeArguments)
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
                var end = name.LastIndexOf("&gt;<wbr>", openParen == -1 ? name.Length : openParen, StringComparison.Ordinal);  // Don't look past the opening paren if there is one

                if (begin == -1 || end == -1)
                {
                    return new HtmlString(name);
                }

                // Remove existing type arguments and insert linked type arguments (do this first to preserve original indexes)
                name = name
                    .Remove(begin, end - begin)
                    .Insert(begin, string.Join(", <wbr>", typeArguments.Select(x => context.GetTypeLink(x, true).Value)));

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
            var declaringType = document.GetDocument(CodeAnalysisKeys.DeclaringType)?.GetDocument(CodeAnalysisKeys.OriginalDefinition);
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
                    .SelectMany((x, i) => char.IsUpper(x) && i != 0 ? new[] { '<', 'w', 'b', 'r', '>', x } : new[] { x })
                    .ToArray());
                replaced = true;
            }
        }

        return replaced ? string.Join("<wbr>", segments) : name;
    }
}