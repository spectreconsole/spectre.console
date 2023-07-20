namespace Spectre.Console.Cli.Completion;

/// <summary>
/// Extensions for Completions.
/// </summary>
public static class CompletionExtensions
{
    /// <summary>
    /// Disables completions, that are automatically generated.
    /// </summary>
    /// <returns>A copy of this instance so that multiple calls can be chained.</returns>
    /// <param name="result">The completion result.</param>
    /// <param name="preventDefault">Whether or not to disable auto completions.</param>
    public static async Task<CompletionResult> WithPreventDefault(this Task<CompletionResult> result, bool preventDefault = true)
    {
        var completionResult = await result;
        return completionResult.WithPreventDefault(preventDefault);
    }

#pragma warning disable S125 // Sections of code should not be commented out
    /*
        //"Cannot be infered :/"
        /// <summary>
        /// Creates a new suggestion matcher for this command.
        /// </summary>
        /// <returns>A suggestion matcher.</returns>
        /// <param name="command">The command.</param>
        /// <typeparam name="TSettings">The settings type.</typeparam>
        /// <typeparam name="TCommand">The command type.</typeparam>
        [SuppressMessage("Roslynator", "RCS1175:Unused 'this' parameter.", Justification = "Extension method")]
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Extension method")]
        public static CommandParameterMatcher<TSettings> CreateMatcher<TSettings, TCommand>(this TCommand command)
            where TSettings : CommandSettings
            where TCommand : Command<TSettings>, IAsyncCommandCompletable
        {
            return new();
        }

        /// <summary>
        /// Creates a new suggestion matcher for this command.
        /// </summary>
        /// <typeparam name="TSettings">The settings type.</typeparam>
        /// <typeparam name="TCommand">The command type.</typeparam>
        /// <returns>A suggestion matcher.</returns>
        /// <param name="command">The command.</param>
        [SuppressMessage("Roslynator", "RCS1175:Unused 'this' parameter.", Justification = "Extension method")]
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Extension method")]
        public static AsyncCommandParameterMatcher<TSettings> CreateAsyncMatcher<TSettings, TCommand>(this TCommand command)
            where TSettings : CommandSettings
            where TCommand : Command<TSettings>, IAsyncCommandCompletable
        {
            return new();
        }
        */
}
#pragma warning restore S125 // Sections of code should not be commented out