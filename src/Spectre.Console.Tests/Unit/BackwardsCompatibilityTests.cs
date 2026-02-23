namespace Spectre.Console.Tests.Unit;

/// <summary>
/// Tests to ensure backwards compatibility with legacy generated code.
/// All names that existed in the legacy generated files must continue to exist with the same values.
/// </summary>
public class BackwardsCompatibilityTests
{
    [Fact]
    public void Color_Should_Have_All_Legacy_Properties_With_Same_Values()
    {
        // Get legacy Color static properties (excluding non-color properties like R, G, B)
        var legacyColorProps = typeof(Legacy.Color)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Where(p => p.PropertyType == typeof(Legacy.Color))
            .ToList();

        // Get current Color static properties as dictionary for lookup
        var currentColorProps = typeof(Color)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Where(p => p.PropertyType == typeof(Color))
            .ToDictionary(p => p.Name);

        var missing = new List<string>();
        var valueMismatches = new List<string>();

        foreach (var legacyProp in legacyColorProps)
        {
            if (!currentColorProps.TryGetValue(legacyProp.Name, out var currentProp))
            {
                missing.Add(legacyProp.Name);
                continue;
            }

            // Compare RGB values
            var legacyColor = (Legacy.Color)legacyProp.GetValue(null)!;
            var currentColor = (Color)currentProp.GetValue(null)!;

            if (legacyColor.R != currentColor.R ||
                legacyColor.G != currentColor.G ||
                legacyColor.B != currentColor.B)
            {
                valueMismatches.Add($"{legacyProp.Name}: legacy=({legacyColor.R},{legacyColor.G},{legacyColor.B}) current=({currentColor.R},{currentColor.G},{currentColor.B})");
            }
        }

        missing.ShouldBeEmpty($"Missing colors: {string.Join(", ", missing)}");
        valueMismatches.ShouldBeEmpty($"Color value mismatches:\n{string.Join("\n", valueMismatches)}");
    }

    [Fact]
    public void Spinner_Known_Should_Have_All_Legacy_Properties_With_Same_Values()
    {
        // Get legacy Spinner.Known static properties
        var legacySpinnerProps = typeof(Legacy.Spinner.Known)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .ToList();

        // Get current Spinner.Known static properties as dictionary for lookup
        var currentSpinnerProps = typeof(Spinner.Known)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .ToDictionary(p => p.Name);

        var missing = new List<string>();
        var valueMismatches = new List<string>();

        foreach (var legacyProp in legacySpinnerProps)
        {
            if (!currentSpinnerProps.TryGetValue(legacyProp.Name, out var currentProp))
            {
                missing.Add(legacyProp.Name);
                continue;
            }

            // Compare Interval values
            var legacySpinner = (Legacy.Spinner)legacyProp.GetValue(null)!;
            var currentSpinner = (Spinner)currentProp.GetValue(null)!;

            if (legacySpinner.Interval != currentSpinner.Interval || legacySpinner.Frames.Count != currentSpinner.Frames.Count || legacySpinner.Frames[0] != currentSpinner.Frames[0])
            {
                valueMismatches.Add($"{legacyProp.Name}: legacy interval={legacySpinner.Interval} current interval={currentSpinner.Interval}, legacy frames count={legacySpinner.Frames.Count} current frames count={currentSpinner.Frames.Count}, legacy first frame={legacySpinner.Frames[0]} current first frame={currentSpinner.Frames[0]}");
            }
        }

        missing.ShouldBeEmpty($"Missing spinners: {string.Join(", ", missing)}");
        valueMismatches.ShouldBeEmpty($"Spinner value mismatches:\n{string.Join("\n", valueMismatches)}");
    }

    [Fact]
    public void Emoji_Known_Should_Have_All_Legacy_Fields_With_Same_Values()
    {
        // Get legacy Emoji.Known const fields
        var legacyEmojiFields = typeof(Legacy.Emoji.Known)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.IsLiteral) // const fields only
            .ToList();

        // Get current Emoji.Known const fields as dictionary for lookup
        var currentEmojiFields = typeof(Emoji.Known)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.IsLiteral) // const fields only
            .ToDictionary(f => f.Name);

        var missing = new List<string>();
        var valueMismatches = new List<string>();

        foreach (var legacyField in legacyEmojiFields)
        {
            if (!currentEmojiFields.TryGetValue(legacyField.Name, out var currentField))
            {
                missing.Add(legacyField.Name);
                continue;
            }

            // Compare const string values
            var legacyValue = (string)legacyField.GetRawConstantValue()!;
            var currentValue = (string)currentField.GetRawConstantValue()!;

            if (legacyValue != currentValue)
            {
                valueMismatches.Add($"{legacyField.Name}: legacy=\"{legacyValue}\" current=\"{currentValue}\"");
            }
        }

        missing.ShouldBeEmpty($"Missing emojis: {string.Join(", ", missing)}");
        valueMismatches.ShouldBeEmpty($"Emoji value mismatches:\n{string.Join("\n", valueMismatches)}");
    }
}
