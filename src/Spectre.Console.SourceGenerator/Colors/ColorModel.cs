using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Spectre.Console.SourceGenerator.Colors;

/// <summary>
/// Represents a color from the JSON data file.
/// </summary>
internal sealed record ColorModel(
    int Number,
    string Name,
    byte R,
    byte G,
    byte B,
    bool IsAlias);

/// <summary>
/// Parses color data from JSON.
/// </summary>
internal static class ColorParser
{
    /// <summary>
    /// Parses the colors.json file and returns all color models.
    /// </summary>
    public static EquatableArray<ColorModel> ParseAll(string json)
    {
        var document = JsonDocument.Parse(json);
        var colors = new List<ColorModel>();
        var nameCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var element in document.RootElement.EnumerateArray())
        {
            var number = element.GetProperty("number").GetInt32();
            var name = element.GetProperty("name").GetString()
                ?? throw new InvalidOperationException($"Color {number} has null name");
            var rgb = element.GetProperty("rgb");
            var r = (byte)rgb.GetProperty("r").GetInt32();
            var g = (byte)rgb.GetProperty("g").GetInt32();
            var b = (byte)rgb.GetProperty("b").GetInt32();

            // Track name occurrences for deduplication
            var uniqueName = GetUniqueName(name, nameCounts);
            colors.Add(new ColorModel(number, uniqueName, r, g, b, IsAlias: false));

            // Process aliases
            if (element.TryGetProperty("aliases", out var aliases))
            {
                foreach (var alias in aliases.EnumerateArray())
                {
                    var aliasName = alias.GetString()
                        ?? throw new InvalidOperationException($"Color {number} has null alias");
                    var uniqueAliasName = GetUniqueName(aliasName, nameCounts);
                    colors.Add(new ColorModel(number, uniqueAliasName, r, g, b, IsAlias: true));
                }
            }
        }

        return new EquatableArray<ColorModel>(colors);
    }

    private static string GetUniqueName(string name, Dictionary<string, int> nameCounts)
    {
        if (!nameCounts.TryGetValue(name, out var count))
        {
            nameCounts[name] = 1;
            return name;
        }

        nameCounts[name] = count + 1;
        return $"{name}_{count}";
    }
}
