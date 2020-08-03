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
        public Rgb Rgb { get; set; }

        public int R => Rgb.R;
        public int G => Rgb.G;
        public int B => Rgb.B;

        public static IEnumerable<Color> Parse(string json)
        {
            var source = JsonConvert.DeserializeObject<List<Color>>(json);

            var check = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase);
            foreach (var color in source.OrderBy(c => c.Number))
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
