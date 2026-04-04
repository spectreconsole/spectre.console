using System.Runtime.Versioning;
using PublicApiGenerator;

namespace Spectre.Console.Tests;

public sealed class PublicApiTests
{
    [Fact]
    [Expectation("Public_API")]
    public Task Public_API_Should_Not_Change()
    {
        // Given
        var assembly = typeof(AnsiConsole).Assembly;
        var options = new ApiGeneratorOptions
        {
            // These attributes won't be included in the public API
            ExcludeAttributes =
            [
                typeof(AssemblyMetadataAttribute).FullName!,
                typeof(InternalsVisibleToAttribute).FullName!,
                "System.Runtime.CompilerServices.IsByRefLike",
                typeof(TargetFrameworkAttribute).FullName!,
            ],

            // By default types found in Microsoft or System
            // namespaces are not treated as part of the public API.
            // By passing an empty array, we ensure they're all
            DenyNamespacePrefixes = [],
        };

        // When
        var publicApi = assembly.GeneratePublicApi(options);

        // Then
        return Verifier.Verify(publicApi);
    }
}