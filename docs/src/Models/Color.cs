using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Docs.Models
{
    public sealed class Color
    {
        public int Number { get; set; }
        public string Hex { get; set; }
        public string Name { get; set; }
        public Rgb Rgb { get; set; }
        public string ClrName { get; set; }

        public int R => Rgb.R;
        public int G => Rgb.G;
        public int B => Rgb.B;

        private static Dictionary<int, string> ClrNames { get; } = new Dictionary<int, string>
        {
            { 0, "Black" },
            { 1, "DarkRed" },
            { 2, "DarkGreen" },
            { 3, "DarkYellow" },
            { 4, "DarkBlue" },
            { 5, "DarkMagenta" },
            { 6, "DarkCyan" },
            { 7, "Gray" },
            { 8, "DarkGray" },
            { 9, "Red" },
            { 10, "Green" },
            { 11, "Yellow" },
            { 12, "Blue" },
            { 13, "Magenta" },
            { 14, "Cyan" },
            { 15, "White" },
        };

        public static IEnumerable<Color> Parse(string json)
        {
            var source = JsonConvert.DeserializeObject<List<Color>>(json);

            var check = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase);
            foreach (var color in source.OrderBy(c => c.Number))
            {
                if (ClrNames.TryGetValue(color.Number, out var clrName))
                {
                    color.ClrName = clrName;
                }
                else
                {
                    color.ClrName = string.Empty;
                }

                if (!check.ContainsKey(color.Name))
                {
                    check.Add(color.Name, color);
                }
                else
                {
                    var newName = (string)null;
                    for (int i = 1; i < 100; i++)
                    {
                        if (!check.ContainsKey($"{color.Name}_{i}"))
                        {
                            newName = $"{color.Name}_{i}";
                            break;
                        }
                    }

                    if (newName == null)
                    {
                        throw new InvalidOperationException("Impossible!");
                    }

                    check.Add(newName, color);
                    color.Name = newName;
                }
            }

            return source;
        }
    }

    public sealed class Rgb
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
    }
}