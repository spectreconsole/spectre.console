namespace Spectre.Console.Cli;

/// <summary>
/// Base class for a command without settings.
/// </summary>
/// <seealso cref="AsyncCommand"/>
public abstract class Command : ICommand<EmptyCommandSettings>
{
    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="context">The command context.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to abort the command.</param>
    /// <returns>An integer indicating whether or not the command executed successfully.</returns>
    public abstract int Execute(CommandContext context, CancellationToken cancellationToken);

    /// <inheritdoc/>
    Task<int> ICommand<EmptyCommandSettings>.ExecuteAsync(CommandContext context, EmptyCommandSettings settings, CancellationToken cancellationToken)
    {
        return Task.FromResult(Execute(context, cancellationToken));
    }

    /// <inheritdoc/>
    Task<int> ICommand.ExecuteAsync(CommandContext context, CommandSettings settings, CancellationToken cancellationToken)
    {
        return Task.FromResult(Execute(context, cancellationToken));
    }

    /// <inheritdoc/>
    ValidationResult ICommand.Validate(CommandContext context, CommandSettings settings)
    {
        return ValidationResult.Success();
    }
}