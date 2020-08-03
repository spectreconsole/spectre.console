using System;
using System.Collections.Generic;

namespace Spectre.Console.Internal
{
    internal static class StyleExtensions
    {
        public static Style Combine(this Style style, IEnumerable<Style> source)
        {
            if (style is null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            if (source is null)
            {
                return style;
            }

            var current = style;
            foreach (var item in source)
            {
                current = current.Combine(item);
            }

            return current;
        }
    }
}
