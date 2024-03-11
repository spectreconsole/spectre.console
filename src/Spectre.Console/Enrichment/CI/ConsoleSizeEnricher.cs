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
        if (_environmentVariables is null)
        {
            return;
        }

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
}