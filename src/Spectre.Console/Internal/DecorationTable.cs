using System;
using System.Collections.Generic;
using System.Linq;

namespace Spectre.Console
{
    internal static class DecorationTable
    {
        private static readonly Dictionary<string, Decoration?> _lookup;
        private static readonly Dictionary<Decoration, string> _reverseLookup;

        static DecorationTable()
        {
            _lookup = new Dictionary<string, Decoration?>(StringComparer.OrdinalIgnoreCase)
            {
                { "none", Decoration.None },
                { "bold", Decoration.Bold },
                { "b", Decoration.Bold },
                { "dim", Decoration.Dim },
                { "italic", Decoration.Italic },
                { "i", Decoration.Italic },
                { "underline", Decoration.Underline },
                { "u", Decoration.Underline },
                { "invert", Decoration.Invert },
                { "reverse", Decoration.Invert },
                { "conceal", Decoration.Conceal },
                { "blink", Decoration.SlowBlink },
                { "slowblink", Decoration.SlowBlink },
                { "rapidblink", Decoration.RapidBlink },
                { "strike", Decoration.Strikethrough },
                { "strikethrough", Decoration.Strikethrough },
                { "s", Decoration.Strikethrough },
            };

            _reverseLookup = new Dictionary<Decoration, string>();
            foreach (var (name, decoration) in _lookup)
            {
                // Cannot happen, but the compiler thinks so...
                if (decoration == null)
                {
                    continue;
                }

                if (!_reverseLookup.ContainsKey(decoration.Value))
                {
                    _reverseLookup[decoration.Value] = name;
                }
            }
        }

        public static Decoration? GetDecoration(string name)
        {
            _lookup.TryGetValue(name, out var result);
            return result;
        }

        public static List<string> GetMarkupNames(Decoration decoration)
        {
            var result = new List<string>();

            Enum.GetValues(typeof(Decoration))
                .Cast<Decoration>()
                .Where(flag => (decoration & flag) != 0)
                .ForEach(flag =>
                {
                    if (flag != Decoration.None && _reverseLookup.TryGetValue(flag, out var name))
                    {
                        result.Add(name);
                    }
                });

            return result;
        }
    }
}
