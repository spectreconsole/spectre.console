using System.Threading;

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
    /// <returns>An integer indicating whether or not the command executed successfully.</returns>
    public abstract int Execute(CommandContext context);

    /// <inheritdoc/>
    /// <param name="context">The command context.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="cancellationToken">Should not be used. <see cref="AsyncCommand"/> should be used instead.</param>
    Task<int> ICommand<EmptyCommandSettings>.Execute(CommandContext context, EmptyCommandSettings settings, CancellationToken cancellationToken)
    {
        return Task.FromResult(Execute(context));
    }

    /// <inheritdoc/>
    /// <param name="context">The command context.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="cancellationToken">Should not be used. <see cref="AsyncCommand"/> should be used instead.</param>
    Task<int> ICommand.Execute(CommandContext context, CommandSettings settings, CancellationToken cancellationToken)
    {
        return Task.FromResult(Execute(context));
    }

    /// <inheritdoc/>
    ValidationResult ICommand.Validate(CommandContext context, CommandSettings settings)
    {
        return ValidationResult.Success();
    }
}