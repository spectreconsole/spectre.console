namespace Spectre.Console.Cli;

/// <summary>
/// Represents a command settings interceptor that
/// will intercept command settings before it's
/// passed to a command.
/// </summary>
public interface ICommandInterceptor
{
    /// <summary>
    /// Intercepts command information before it's passed to a command.
    /// </summary>
    /// <param name="context">The intercepted <see cref="CommandContext"/>.</param>
    /// <param name="settings">The intercepted <see cref="CommandSettings"/>.</param>
    void Intercept(CommandContext context, CommandSettings settings)
#if NETSTANDARD2_0
    ;
#else
    {
    }
#endif

    /// <summary>
    /// Intercepts a command result before it's passed as the result.
    /// </summary>
    /// <param name="context">The intercepted <see cref="CommandContext"/>.</param>
    /// <param name="settings">The intercepted <see cref="CommandSettings"/>.</param>
    /// <param name="result">The result from the command execution.</param>
    void InterceptResult(CommandContext context, CommandSettings settings, ref int result)
#if NETSTANDARD2_0
    ;
#else
    {
    }
#endif
}