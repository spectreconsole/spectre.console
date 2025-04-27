#if NETSTANDARD2_0
namespace Spectre.Console
{
    internal static class CancellationTokenHelpers
    {
        public static Task CancelAsync(this CancellationTokenSource cts)
        {
            cts.Cancel();
            return Task.CompletedTask;
        }
    }
}
#endif