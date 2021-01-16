using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Spectre.Console
{
    internal static class AnsiColorBuilder
    {
        public static IEnumerable<byte> GetAnsiCodes(ColorSystem system, Color color, bool foreground)
        {
            return system switch
            {
                ColorSystem.NoColors => Array.Empty<byte>(), // No colors
                ColorSystem.TrueColor => GetTrueColor(color, foreground), // 24-bit
                ColorSystem.EightBit => GetEightBit(color, foreground), // 8-bit
                ColorSystem.Standard => GetFourBit(color, foreground), // 4-bit
                ColorSystem.Legacy => GetThreeBit(color, foreground), // 3-bit
                _ => throw new InvalidOperationException("Could not determine ANSI color."),
            };
        }

        private static IEnumerable<byte> GetThreeBit(Color color, bool foreground)
        {
            var number = color.Number;
            if (number == null || color.Number >= 8)
            {
                number = ColorPalette.ExactOrClosest(ColorSystem.Legacy, color).Number;
            }

            Debug.Assert(number >= 0 && number < 8, "Invalid range for 4-bit color");

            var mod = foreground ? 30 : 40;
            return new byte[] { (byte)(number.Value + mod) };
        }

        private static IEnumerable<byte> GetFourBit(Color color, bool foreground)
        {
            var number = color.Number;
            if (number == null || color.Number >= 16)
            {
                number = ColorPalette.ExactOrClosest(ColorSystem.Standard, color).Number;
            }

            Debug.Assert(number >= 0 && number < 16, "Invalid range for 4-bit color");

            var mod = number < 8 ? (foreground ? 30 : 40) : (foreground ? 82 : 92);
            return new byte[] { (byte)(number.Value + mod) };
        }

        private static IEnumerable<byte> GetEightBit(Color color, bool foreground)
        {
            var number = color.Number ?? ColorPalette.ExactOrClosest(ColorSystem.EightBit, color).Number;
            Debug.Assert(number >= 0 && number <= 255, "Invalid range for 8-bit color");

            var mod = foreground ? (byte)38 : (byte)48;
            return new byte[] { mod, 5, (byte)number };
        }

        private static IEnumerable<byte> GetTrueColor(Color color, bool foreground)
        {
            if (color.Number != null)
            {
                return GetEightBit(color, foreground);
            }

            var mod = foreground ? (byte)38 : (byte)48;
            return new byte[] { mod, 2, color.R, color.G, color.B };
        }
    }
}
