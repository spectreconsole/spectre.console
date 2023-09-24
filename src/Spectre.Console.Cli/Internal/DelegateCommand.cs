namespace Spectre.Console.Cli;

internal sealed class DelegateCommand : ICommand
{
    private readonly Func<CommandContext, CommandSettings, Task<int>> _func;

    public DelegateCommand(Func<CommandContext, CommandSettings, Task<int>> func)
    {
        _func = func;
    }

    public Task<int> Execute(CommandContext context, CommandSettings settings)
    {
        return _func(context, settings);
    }

    public ValidationResult Validate(CommandContext context, CommandSettings settings)
    {
        return ValidationResult.Success();
    }
}