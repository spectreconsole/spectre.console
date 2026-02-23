namespace Spectre.Console.Enrichment;

internal sealed class AzurePipelinesEnricher : IProfileEnricher
{
    public string Name => "AzurePipeline";

    public bool Enabled(IDictionary<string, string> environmentVariables)
    {
        environmentVariables.TryGetValue("TF_BUILD", out var environmentValue);
        return !string.IsNullOrWhiteSpace(environmentValue);
    }

    public void Enrich(Profile profile)
    {
        profile.Capabilities.Ansi = true;
        profile.Capabilities.Interactive = false;
        profile.Capabilities.Links = false;
    }
}