namespace Spectre.Console.Analyzer.Tests;

internal static class CodeAnalyzerHelper
{
    internal static ReferenceAssemblies CurrentSpectre { get; }

    static CodeAnalyzerHelper()
    {
        CurrentSpectre = ReferenceAssemblies.Net.Net60.AddAssemblies(
            ImmutableArray.Create(typeof(AnsiConsole).Assembly.Location.Replace(".dll", string.Empty)));
    }
}
