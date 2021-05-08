using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Generator.Models
{
    public sealed class Color
    {
        public int Number { get; set; }
        public string Hex { get; set; }
        public string Name { get; set; }
        public List<string> Aliases { get; set; } = new List<string>();
        public Rgb Rgb { get; set; }

        public int R => Rgb.R;
        public int G => Rgb.G;
        public int B => Rgb.B;

        public static IEnumerable<Color> Parse(string json)
        {
            var source = JsonConvert.DeserializeObject<List<Color>>(json);

            var check = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase);

            var colorAliases = source
                .SelectMany(c => c.Aliases.Select(a => new { Alias = a, Color = c }))
                .Select(a => new Color()
                {
                    Hex = a.Color.Hex,
                    Name = a.Alias,
                    Number = a.Color.Number,
                    Rgb = a.Color.Rgb
                })
                .ToList();

            var colors = source
                .Union(colorAliases)
                .OrderBy(c => c.Number);

            foreach (var color in colors)
            {
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

            return colors;
        }
    }

    public sealed class Rgb
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
    }
}
