namespace Spectre.Console.Analyzer;

/// <summary>
/// Base class for Spectre analyzers.
/// </summary>
public abstract class SpectreAnalyzer : DiagnosticAnalyzer
{
    /// <inheritdoc />
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(AnalyzeCompilation);
    }

    /// <summary>
    /// Analyze compilation.
    /// </summary>
    /// <param name="compilationStartContext">Compilation Start Analysis Context.</param>
    protected abstract void AnalyzeCompilation(CompilationStartAnalysisContext compilationStartContext);
}