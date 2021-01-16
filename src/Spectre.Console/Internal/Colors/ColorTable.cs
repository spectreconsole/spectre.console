using System;
using System.Collections.Generic;

namespace Spectre.Console
{
    internal static partial class ColorTable
    {
        private static readonly Dictionary<int, string> _nameLookup;
        private static readonly Dictionary<string, int> _numberLookup;

        static ColorTable()
        {
            _numberLookup = GenerateTable();
            _nameLookup = new Dictionary<int, string>();
            foreach (var pair in _numberLookup)
            {
                _nameLookup.Add(pair.Value, pair.Key);
            }
        }

        public static Color GetColor(int number)
        {
            if (number < 0 || number > 255)
            {
                throw new InvalidOperationException("Color number must be between 0 and 255");
            }

            return ColorPalette.EightBit[number];
        }

        public static Color? GetColor(string name)
        {
            if (!_numberLookup.TryGetValue(name, out var number))
            {
                return null;
            }

            if (number > ColorPalette.EightBit.Count - 1)
            {
                return null;
            }

            return ColorPalette.EightBit[number];
        }

        public static string? GetName(int number)
        {
            _nameLookup.TryGetValue(number, out var name);
            return name;
        }
    }
}
