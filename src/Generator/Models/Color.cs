using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace Generator.Models;

public sealed class Color
{
    public required int Number { get; init; }
    public required string Hex { get; init; }
    public required string Name { get; set; }
    public List<string> Aliases { get; set; } = [];
    public bool IsAlias { get; set; }
    public required Rgb Rgb { get; init; }

    public int R => Rgb.R;
    public int G => Rgb.G;
    public int B => Rgb.B;

    public static IEnumerable<Color> Parse(string json)
    {
        var source = JsonConvert.DeserializeObject<List<Color>>(json);
        Trace.Assert(source != null, "Could not deserialize JSON");

        var check = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase);

        var colorAliases = source
            .SelectMany(c => c.Aliases.Select(a => new { Alias = a, Color = c }))
            .Select(a => new Color()
            {
                Hex = a.Color.Hex,
                Name = a.Alias,
                Number = a.Color.Number,
                Rgb = a.Color.Rgb,
                IsAlias = true,
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
                var newName = default(string?);
                for (var i = 1; i < 100; i++)
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