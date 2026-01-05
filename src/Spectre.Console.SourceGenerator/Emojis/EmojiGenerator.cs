using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Spectre.Console.SourceGenerator.Emojis;

/// <summary>
/// Source generator that produces Emoji from emoji.json.
/// </summary>
[Generator]
public class EmojiGenerator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Find emoji.json and parse it in the pipeline (for caching)
        // Step 1: Extract text (cached by string value equality)
        // Step 2: Parse to models (cached by EquatableArray value equality)
        var emojis = context.AdditionalTextsProvider
            .Where(static file => file.Path.EndsWith("emoji.json", StringComparison.OrdinalIgnoreCase))
            .Select(static (file, ct) =>
                file.GetText(ct)?.ToString()
                ?? throw new InvalidOperationException($"Failed to read emoji.json at {file.Path}"))
            .Select(static (text, _) => EmojiParser.ParseAll(text))
            .Collect();

        // Register implementation source output - we do not use these emojis directly in the source
        // and this will allow IDEs to optionally skip running this generator for intellisense and the such, but it will
        // always run when compiling.
        // see https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.md#outputting-values
        context.RegisterImplementationSourceOutput(emojis, static (spc, models) =>
        {
            if (models.IsEmpty)
            {
                return;
            }

            if (models.Length > 1)
            {
                spc.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor(
                        "SPECINTERNALEMOJI001",
                        "Multiple emoji.json files found",
                        "Multiple emoji.json files were found. Only the first one will be used.",
                        "Spectre.Console.SourceGenerator",
                        DiagnosticSeverity.Warning,
                        isEnabledByDefault: true),
                    Location.None));
            }

            var source = EmojiEmitter.Emit(models[0]);
            spc.AddSource("Emoji.Generated.g.cs", SourceText.From(source, Encoding.UTF8));
        });
    }
}
