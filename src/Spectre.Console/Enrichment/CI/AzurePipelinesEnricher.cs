namespace Spectre.Console.Enrichment;

internal sealed class AzurePipelinesEnricher : IProfileEnricher
{
    public string Name => "AzurePipeline";

    public bool Enabled(IDictionary<string, string> environmentVariables)
    {
        if (environmentVariables.TryGetValue("TF_BUILD", out var value))
        {
            return value?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        return false;
    }

    public void Enrich(Profile profile)
    {
        profile.Capabilities.Ansi = true;
        profile.Capabilities.Legacy = false;
        profile.Capabilities.Interactive = false;
        profile.Capabilities.Links = false;
    }
}