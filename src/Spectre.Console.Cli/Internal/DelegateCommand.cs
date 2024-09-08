namespace Spectre.Console.Cli;

internal sealed class DelegateCommand : ICommand
{
    private readonly Func<CommandContext, CommandSettings, CancellationToken, Task<int>> _func;

    public DelegateCommand(Func<CommandContext, CommandSettings, CancellationToken, Task<int>> func)
    {
        _func = func;
    }

    public Task<int> ExecuteAsync(CommandContext context, CommandSettings settings, CancellationToken cancellationToken)
    {
        return _func(context, settings, cancellationToken);
    }

    public ValidationResult Validate(CommandContext context, CommandSettings settings)
    {
        return ValidationResult.Success();
    }
}