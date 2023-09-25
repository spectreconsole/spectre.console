namespace Spectre.Console.Cli;
#pragma warning restore S2326

/// <summary>
/// Represents the args for a command exception.
/// </summary>
public readonly struct CommandExceptionArgs
{
    /// <summary>
    /// Gets the command context.
    /// </summary>
    public CommandContext Context { get; }

    /// <summary>
    /// Gets the exception that was thrown.
    /// </summary>
    public Exception Exception { get; }

    /// <summary>
    /// Gets the command type.
    /// </summary>
    public Type? CommandType { get; }

    internal CommandExceptionArgs(CommandContext context, Exception exception, Type? commandType)
    {
        Context = context;
        Exception = exception;
        CommandType = commandType;
    }
}
