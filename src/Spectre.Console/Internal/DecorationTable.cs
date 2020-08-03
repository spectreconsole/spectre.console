using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Spectre.Console.Internal
{
    internal static class DecorationTable
    {
        private static readonly Dictionary<string, Decoration?> _lookup;

        [SuppressMessage("Performance", "CA1810:Initialize reference type static fields inline")]
        static DecorationTable()
        {
            _lookup = new Dictionary<string, Decoration?>(StringComparer.OrdinalIgnoreCase)
            {
                { "none", Decoration.None },
                { "bold", Decoration.Bold },
                { "dim", Decoration.Dim },
                { "italic", Decoration.Italic },
                { "underline", Decoration.Underline },
                { "invert", Decoration.Invert },
                { "conceal", Decoration.Conceal },
                { "slowblink", Decoration.SlowBlink },
                { "rapidblink", Decoration.RapidBlink },
                { "strikethrough", Decoration.Strikethrough },
            };
        }

        public static Decoration? GetDecoration(string name)
        {
            _lookup.TryGetValue(name, out var result);
            return result;
        }
    }
}
