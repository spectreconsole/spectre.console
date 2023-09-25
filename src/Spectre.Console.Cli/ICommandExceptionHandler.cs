namespace Spectre.Console.Cli;

/// <summary>
/// Represents an args handler.
/// Exception handlers are used to handle exceptions that occur during command execution.
/// </summary>
public interface ICommandExceptionHandler
{
    /// <summary>
    /// Handles the specified args.
    /// </summary>
    /// <param name="args">The args to handle.</param>
    /// <returns><c>true</c> if the args was handled, otherwise <c>false</c>.</returns>
    bool Handle(CommandExceptionArgs args);
}

/// <summary>
/// Represents an args handler for a specific command.
/// </summary>
/// <typeparam name="TCommand">Type of the command.</typeparam>
// ReSharper disable once UnusedTypeParameter
#pragma warning disable S2326
public interface ICommandExceptionHandler<TCommand> : ICommandExceptionHandler
    where TCommand : ICommand
{
}
