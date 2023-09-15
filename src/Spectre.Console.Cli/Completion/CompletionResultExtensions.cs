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
}
