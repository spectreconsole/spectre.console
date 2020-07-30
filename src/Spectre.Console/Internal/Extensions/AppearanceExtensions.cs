using System.Collections.Generic;

namespace Spectre.Console.Internal
{
    internal static class AppearanceExtensions
    {
        public static Appearance Combine(this Appearance appearance, IEnumerable<Appearance> source)
        {
            var current = appearance;
            foreach (var item in source)
            {
                current = current.Combine(item);
            }

            return current;
        }
    }
}
