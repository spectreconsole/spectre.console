namespace Spectre.Console.Cli;

/// <summary>
/// Represents a startup interceptor that will run before any command is executed.
/// </summary>
public interface IStartupInterceptor
{
    /// <summary>
    /// Runs before any command is executed.
    /// </summary>
    /// <param name="context">The startup context.</param>
    void Intercept(StartupContext context);
}
