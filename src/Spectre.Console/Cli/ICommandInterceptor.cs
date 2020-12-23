namespace Spectre.Console.Cli
{
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
        void Intercept(CommandContext context, CommandSettings settings);
    }
}
