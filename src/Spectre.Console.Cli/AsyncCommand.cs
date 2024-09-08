namespace Spectre.Console.Cli;

/// <summary>
/// Base class for an asynchronous command with no settings.
/// </summary>
public abstract class AsyncCommand : ICommand<EmptyCommandSettings>
{
    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="context">The command context.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to abort the command.</param>
    /// <returns>An integer indicating whether or not the command executed successfully.</returns>
    public abstract Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken);

    /// <inheritdoc/>
    Task<int> ICommand<EmptyCommandSettings>.ExecuteAsync(CommandContext context, EmptyCommandSettings settings, CancellationToken cancellationToken)
    {
        return ExecuteAsync(context, cancellationToken);
    }

    /// <inheritdoc/>
    Task<int> ICommand.ExecuteAsync(CommandContext context, CommandSettings settings, CancellationToken cancellationToken)
    {
        return ExecuteAsync(context, cancellationToken);
    }

    /// <inheritdoc/>
    ValidationResult ICommand.Validate(CommandContext context, CommandSettings settings)
    {
        return ValidationResult.Success();
    }
}