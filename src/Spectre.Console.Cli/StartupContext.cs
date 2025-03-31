namespace Spectre.Console.Cli;

public sealed class StartupContext
{
    public required ITypeResolver TypeResolver { get; init; }
}