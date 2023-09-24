namespace Spectre.Console.Cli;

internal sealed class BranchConfigurator : IBranchConfigurator
{
    public ConfiguredCommand Command { get; }

    public BranchConfigurator(ConfiguredCommand command)
    {
        Command = command;
    }

    public IBranchConfigurator WithAlias(string alias)
    {
        Command.Aliases.Add(alias);
        return this;
    }
}