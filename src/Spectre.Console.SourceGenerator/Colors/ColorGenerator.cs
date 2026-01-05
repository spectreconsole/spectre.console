using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Spectre.Console.SourceGenerator.Colors;

/// <summary>
/// Source generator that produces Color, ColorPalette, and ColorTable from colors.json.
/// </summary>
[Generator]
public class ColorGenerator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Find colors.json and parse it in the pipeline (for caching)
        // Step 1: Extract text (cached by string value equality)
        // Step 2: Parse to models (cached by EquatableArray value equality)
        var colors = context.AdditionalTextsProvider
            .Where(static file => file.Path.EndsWith("colors.json", StringComparison.OrdinalIgnoreCase))
            .Select(static (file, ct) =>
                file.GetText(ct)?.ToString()
                ?? throw new InvalidOperationException($"Failed to read colors.json at {file.Path}"))
            .Select(static (text, _) => ColorParser.ParseAll(text))
            .Collect();

        // Register source output - only emit, no parsing (parsing is cached above)
        context.RegisterSourceOutput(colors, static (spc, models) =>
        {
            if (models.IsEmpty)
            {
                return;
            }

            if (models.Length > 1)
            {
                spc.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor(
                        "SPECINTERNALCOL001",
                        "Multiple colors.json files found",
                        "Multiple colors.json files were found. Only the first one will be used.",
                        "Spectre.Console.SourceGenerator",
                        DiagnosticSeverity.Warning,
                        isEnabledByDefault: true),
                    Location.None));
            }

            var colorModels = models[0];

            // Generate Color.Generated.g.cs
            var colorSource = ColorEmitter.EmitColorProperties(colorModels);
            spc.AddSource("Color.Generated.g.cs", SourceText.From(colorSource, Encoding.UTF8));

            // Generate ColorPalette.Generated.g.cs
            var paletteSource = ColorEmitter.EmitColorPalette(colorModels);
            spc.AddSource("ColorPalette.Generated.g.cs", SourceText.From(paletteSource, Encoding.UTF8));

            // Generate ColorTable.Generated.g.cs
            var tableSource = ColorEmitter.EmitColorTable(colorModels);
            spc.AddSource("ColorTable.Generated.g.cs", SourceText.From(tableSource, Encoding.UTF8));
        });
    }
}
