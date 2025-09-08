namespace Spectre.Console.Enrichment.CI;

internal class ConsoleSizeEnricher : IProfileEnricher
{
    private IDictionary<string, string>? _environmentVariables;

    public string Name => "ConsoleSizeEnricher";

    public bool Enabled(IDictionary<string, string> environmentVariables)
    {
        _environmentVariables = environmentVariables;
        return environmentVariables.ContainsKey("CONSOLE_WIDTH")
            || environmentVariables.ContainsKey("CONSOLE_HEIGHT");
    }

    public void Enrich(Profile profile)
    {
        _environmentVariables ??= GetEnvironmentVariables(null);

        if (_environmentVariables.TryGetValue("CONSOLE_WIDTH", out var width)
            && int.TryParse(width, out var w))
        {
            profile.Width = w;
        }

        if (_environmentVariables.TryGetValue("CONSOLE_HEIGHT", out var height)
            && int.TryParse(height, out var h))
        {
            profile.Height = h;
        }
    }

    private static IDictionary<string, string> GetEnvironmentVariables(IDictionary<string, string>? variables)
    {
        if (variables != null)
        {
            return new Dictionary<string, string>(variables, StringComparer.OrdinalIgnoreCase);
        }

        return Environment.GetEnvironmentVariables()
            .Cast<System.Collections.DictionaryEntry>()
            .Aggregate(
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase),
                (dictionary, entry) =>
                {
                    var key = (string)entry.Key;
                    if (!dictionary.TryGetValue(key, out _))
                    {
                        dictionary.Add(key, entry.Value as string ?? string.Empty);
                    }

                    return dictionary;
                },
                dictionary => dictionary);
    }
}