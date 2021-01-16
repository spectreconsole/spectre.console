using System;
using System.Collections.Generic;
using System.Linq;

namespace Spectre.Console
{
    internal static partial class ColorPalette
    {
        public static IReadOnlyList<Color> Legacy { get; }
        public static IReadOnlyList<Color> Standard { get; }
        public static IReadOnlyList<Color> EightBit { get; }

        static ColorPalette()
        {
            Legacy = GenerateLegacyPalette();
            Standard = GenerateStandardPalette(Legacy);
            EightBit = GenerateEightBitPalette(Standard);
        }

        internal static Color ExactOrClosest(ColorSystem system, Color color)
        {
            var exact = Exact(system, color);
            return exact ?? Closest(system, color);
        }

        private static Color? Exact(ColorSystem system, Color color)
        {
            if (system == ColorSystem.TrueColor)
            {
                return color;
            }

            var palette = system switch
            {
                ColorSystem.Legacy => Legacy,
                ColorSystem.Standard => Standard,
                ColorSystem.EightBit => EightBit,
                _ => throw new NotSupportedException(),
            };

            return palette
                .Where(c => c.Equals(color))
                .Cast<Color?>()
                .FirstOrDefault();
        }

        private static Color Closest(ColorSystem system, Color color)
        {
            if (system == ColorSystem.TrueColor)
            {
                return color;
            }

            var palette = system switch
            {
                ColorSystem.Legacy => Legacy,
                ColorSystem.Standard => Standard,
                ColorSystem.EightBit => EightBit,
                _ => throw new NotSupportedException(),
            };

            // https://stackoverflow.com/a/9085524
            static double Distance(Color first, Color second)
            {
                var rmean = ((float)first.R + second.R) / 2;
                var r = first.R - second.R;
                var g = first.G - second.G;
                var b = first.B - second.B;
                return Math.Sqrt(
                    ((int)((512 + rmean) * r * r) >> 8)
                    + (4 * g * g)
                    + ((int)((767 - rmean) * b * b) >> 8));
            }

            return Enumerable.Range(0, int.MaxValue)
                .Zip(palette, (id, other) => (Distance: Distance(other, color), Id: id, Color: other))
                .OrderBy(x => x.Distance)
                .FirstOrDefault().Color;
        }
    }
}
