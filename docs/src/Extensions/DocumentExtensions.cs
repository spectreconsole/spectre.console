using System.Collections.Generic;
using System.Linq;
using Statiq.CodeAnalysis;
using Statiq.Common;

namespace Docs.Extensions
{
    public static class DocumentExtensions
    {
        public static string GetDescription(this IDocument document)
        {
            return document?.GetString(Constants.Description, string.Empty) ?? string.Empty;
        }

        public static bool IsVisible(this IDocument document)
        {
            return !document.GetBool(Constants.Hidden, false);
        }

        public static bool ShowLink(this IDocument document)
        {
            return !document.GetBool(Constants.NoLink, false);
        }

        public static IEnumerable<IDocument> OnlyVisible(this IEnumerable<IDocument> source)
        {
            return source.Where(x => x.IsVisible());
        }

        public static string GetModifiers(this IDocument document) => document.GetModifiers(false);

        public static string GetModifiers(this IDocument document, bool skipStatic)
        {
            var modifiers = new List<string>();
            var accessibility = document.GetString(CodeAnalysisKeys.Accessibility).ToLower();
            if (accessibility != "public")
            {
                modifiers.Add(accessibility);
            }

            // for some things, like ExtensionMethods, static will always be set.
            if (!skipStatic && document.GetBool(CodeAnalysisKeys.IsStatic))
            {
                modifiers.Add("static");
            }

            if (document.GetBool(CodeAnalysisKeys.IsVirtual))
            {
                modifiers.Add("virtual");
            }

            if (document.GetBool(CodeAnalysisKeys.IsAbstract))
            {
                modifiers.Add("abstract");
            }

            if (document.GetBool(CodeAnalysisKeys.IsOverride))
            {
                modifiers.Add("override");
            }

            return string.Join(' ', modifiers);
        }
    }
}
