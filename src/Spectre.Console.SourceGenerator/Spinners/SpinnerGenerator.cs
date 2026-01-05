using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Spectre.Console.SourceGenerator.Spinners;

/// <summary>
/// Source generator that produces Spinner from spinners_default.json and spinners_sindresorhus.json.
/// </summary>
[Generator]
public class SpinnerGenerator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Find spinners_default.json
        // Step 1: Extract text (cached by string value equality)
        // Step 2: Wrap in EquatableArray for value equality when combining
        var defaultSpinners = context.AdditionalTextsProvider
            .Where(static file => file.Path.EndsWith("spinners_default.json", StringComparison.OrdinalIgnoreCase))
            .Select(static (file, ct) =>
                file.GetText(ct)?.ToString()
                ?? throw new InvalidOperationException($"Failed to read spinners_default.json at {file.Path}"))
            .Collect()
            .Select(static (arr, _) => new EquatableArray<string>(arr));

        // Find spinners_sindresorhus.json
        var sindreSpinners = context.AdditionalTextsProvider
            .Where(static file => file.Path.EndsWith("spinners_sindresorhus.json", StringComparison.OrdinalIgnoreCase))
            .Select(static (file, ct) =>
                file.GetText(ct)?.ToString()
                ?? throw new InvalidOperationException($"Failed to read spinners_sindresorhus.json at {file.Path}"))
            .Collect()
            .Select(static (arr, _) => new EquatableArray<string>(arr));

        // Combine both JSON files and parse in the pipeline (for caching)
        // EquatableArray has value equality, so this step is properly cached
        var spinners = defaultSpinners.Combine(sindreSpinners)
            .Select(static (data, _) =>
            {
                var (defaultJsonFiles, sindreJsonFiles) = data;
                if (defaultJsonFiles.IsEmpty || sindreJsonFiles.IsEmpty)
                {
                    return EquatableArray<SpinnerModel>.Empty;
                }

                return SpinnerParser.ParseAll(defaultJsonFiles[0], sindreJsonFiles[0]);
            });

        // Register source output - only emit, no parsing (parsing is cached above)
        context.RegisterSourceOutput(spinners, static (spc, models) =>
        {
            if (models.IsEmpty)
            {
                return;
            }

            var source = SpinnerEmitter.Emit(models);
            spc.AddSource("Spinner.Generated.g.cs", SourceText.From(source, Encoding.UTF8));
        });
    }
}
