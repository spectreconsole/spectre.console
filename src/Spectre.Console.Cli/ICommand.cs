namespace Spectre.Console.Cli;

/// <summary>
/// Represents a command.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Validates the specified settings and remaining arguments.
    /// </summary>
    /// <param name="context">The command context.</param>
    /// <param name="settings">The settings.</param>
    /// <returns>The validation result.</returns>
    ValidationResult Validate(CommandContext context, CommandSettings settings);

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="context">The command context.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to abort the command.</param>
    /// <returns>The validation result.</returns>
    Task<int> ExecuteAsync(CommandContext context, CommandSettings settings, CancellationToken cancellationToken);
}