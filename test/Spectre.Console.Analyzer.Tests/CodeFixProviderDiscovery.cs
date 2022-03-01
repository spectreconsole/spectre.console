namespace Spectre.Console.Analyzer.Tests;

internal static class CodeFixProviderDiscovery
{
    private static readonly Lazy<IExportProviderFactory> _exportProviderFactory;

    static CodeFixProviderDiscovery()
    {
        _exportProviderFactory = new Lazy<IExportProviderFactory>(
            () =>
            {
                var discovery = new AttributedPartDiscovery(Resolver.DefaultInstance, isNonPublicSupported: true);
                var parts = Task.Run(() => discovery.CreatePartsAsync(typeof(SystemConsoleToAnsiConsoleFix).Assembly)).GetAwaiter().GetResult();
                var catalog = ComposableCatalog.Create(Resolver.DefaultInstance).AddParts(parts);

                var configuration = CompositionConfiguration.Create(catalog);
                var runtimeComposition = RuntimeComposition.CreateRuntimeComposition(configuration);
                return runtimeComposition.CreateExportProviderFactory();
            },
            LazyThreadSafetyMode.ExecutionAndPublication);
    }

    public static IEnumerable<CodeFixProvider> GetCodeFixProviders(string language)
    {
        var exportProvider = _exportProviderFactory.Value.CreateExportProvider();
        var exports = exportProvider.GetExports<CodeFixProvider, LanguageMetadata>();
        return exports.Where(export => export.Metadata.Languages.Contains(language)).Select(export => export.Value);
    }

    private class LanguageMetadata
    {
        public LanguageMetadata(IDictionary<string, object> data)
        {
            if (!data.TryGetValue(nameof(ExportCodeFixProviderAttribute.Languages), out var languages))
            {
                languages = Array.Empty<string>();
            }

            Languages = ((string[])languages).ToImmutableArray();
        }

        public ImmutableArray<string> Languages { get; }
    }
}
