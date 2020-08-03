using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Spectre.Console.Internal
{
    internal static class StyleTable
    {
        private static readonly Dictionary<string, Styles?> _styles;

        [SuppressMessage("Performance", "CA1810:Initialize reference type static fields inline")]
        static StyleTable()
        {
            _styles = new Dictionary<string, Styles?>(StringComparer.OrdinalIgnoreCase)
            {
                { "bold", Styles.Bold },
                { "dim", Styles.Dim },
                { "italic", Styles.Italic },
                { "underline", Styles.Underline },
                { "invert", Styles.Invert },
                { "conceal", Styles.Conceal },
                { "slowblink", Styles.SlowBlink },
                { "rapidblink", Styles.RapidBlink },
                { "strikethrough", Styles.Strikethrough },
            };
        }

        public static Styles? GetStyle(string name)
        {
            _styles.TryGetValue(name, out var style);
            return style;
        }
    }
}
