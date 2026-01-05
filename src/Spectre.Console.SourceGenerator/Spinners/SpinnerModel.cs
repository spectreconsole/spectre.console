using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Spectre.Console.SourceGenerator.Spinners;

/// <summary>
/// Represents a spinner from the JSON data files.
/// </summary>
internal sealed record SpinnerModel(
    string Name,
    string NormalizedName,
    int Interval,
    bool IsUnicode,
    EquatableArray<string> Frames);

/// <summary>
/// Parses spinner data from JSON.
/// </summary>
internal static class SpinnerParser
{
    /// <summary>
    /// Parses spinner JSON files and returns all spinner models.
    /// Default spinners are processed first, then Sindre Sorhus spinners are appended.
    /// </summary>
    public static EquatableArray<SpinnerModel> ParseAll(string defaultJson, string sindreJson)
    {
        var spinners = new List<SpinnerModel>();

        // Parse default spinners first
        ParseSpinners(defaultJson, spinners, pascalizeNames: false);

        // Parse Sindre Sorhus spinners
        ParseSpinners(sindreJson, spinners, pascalizeNames: true);

        return new EquatableArray<SpinnerModel>(spinners);
    }

    private static void ParseSpinners(string json, List<SpinnerModel> spinners, bool pascalizeNames)
    {
        var document = JsonDocument.Parse(json);

        foreach (var property in document.RootElement.EnumerateObject())
        {
            var name = property.Name;
            var element = property.Value;

            var interval = element.GetProperty("interval").GetInt32();
            var isUnicode = !element.TryGetProperty("unicode", out var unicodeProp) || unicodeProp.GetBoolean();

            var frames = new List<string>();
            foreach (var frame in element.GetProperty("frames").EnumerateArray())
            {
                var frameStr = frame.GetString()
                    ?? throw new InvalidOperationException($"Spinner {name} has null frame");
                // Escape backslashes for C# string literals
                frames.Add(frameStr.Replace(@"\", @"\\"));
            }

            var normalizedName = pascalizeNames ? Pascalize(name) : name;

            spinners.Add(new SpinnerModel(
                Name: name,
                NormalizedName: normalizedName,
                Interval: interval,
                IsUnicode: isUnicode,
                Frames: new EquatableArray<string>(frames)));
        }
    }

    private static string Pascalize(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        // Convert first character to uppercase
        var result = new System.Text.StringBuilder();
        var capitalizeNext = true;

        foreach (var c in input)
        {
            if (char.IsLetterOrDigit(c))
            {
                result.Append(capitalizeNext ? char.ToUpperInvariant(c) : c);
                capitalizeNext = false;
            }
            else
            {
                capitalizeNext = true;
            }
        }

        return result.ToString();
    }
}
