namespace Spectre.Console.Cli;

internal sealed class CommandAppSettings : ICommandAppSettings
{
    public CultureInfo? Culture { get; set; }
    public string? ApplicationName { get; set; }
    public string? ApplicationVersion { get; set; }
    public int MaximumIndirectExamples { get; set; }
    public bool ShowOptionDefaultValues { get; set; }
    public IAnsiConsole? Console { get; set; }
    public ICommandInterceptor? Interceptor { get; set; }
    public ITypeRegistrarFrontend Registrar { get; set; }
    public CaseSensitivity CaseSensitivity { get; set; }
    public bool PropagateExceptions { get; set; }
    public bool ValidateExamples { get; set; }
    public bool TrimTrailingPeriod { get; set; } = true;
    public bool StrictParsing { get; set; }
    public bool ConvertFlagsToRemainingArguments { get; set; } = false;

    public ParsingMode ParsingMode =>
        StrictParsing ? ParsingMode.Strict : ParsingMode.Relaxed;

    public Func<Exception, ITypeResolver?, int>? ExceptionHandler { get; set; }

    public CommandAppSettings(ITypeRegistrar registrar)
    {
        Registrar = new TypeRegistrar(registrar);
        CaseSensitivity = CaseSensitivity.All;
        ShowOptionDefaultValues = true;
        MaximumIndirectExamples = 5;
    }

    public bool IsTrue(Func<CommandAppSettings, bool> func, string environmentVariableName)
    {
        if (func(this))
        {
            return true;
        }

        var environmentVariable = Environment.GetEnvironmentVariable(environmentVariableName);
        if (!string.IsNullOrWhiteSpace(environmentVariable))
        {
            if (environmentVariable.Equals("True", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}