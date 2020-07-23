using System;
using System.Collections.Generic;

namespace Spectre.Console.Internal
{
    internal sealed class Lookup
    {
        private readonly Dictionary<string, Styles?> _styles;
        private readonly Dictionary<string, Color?> _colors;

        private static readonly Lazy<Lookup> _lazy = new Lazy<Lookup>(() => new Lookup());
        public static Lookup Instance => _lazy.Value;

        private Lookup()
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

            _colors = new Dictionary<string, Color?>(StringComparer.OrdinalIgnoreCase);
            foreach (var color in ColorPalette.EightBit)
            {
                _colors.Add(color.Name, color);
            }
        }

        public Styles? GetStyle(string name)
        {
            _styles.TryGetValue(name, out var style);
            return style;
        }

        public Color? GetColor(string name)
        {
            _colors.TryGetValue(name, out var color);
            return color;
        }
    }
}
