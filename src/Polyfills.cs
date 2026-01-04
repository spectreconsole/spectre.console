#if NETSTANDARD
namespace Spectre.Console;

internal static class Polyfills
{
    extension(CancellationTokenSource cts)
    {
        public Task CancelAsync()
        {
            cts.Cancel();
            return Task.CompletedTask;
        }
    }
}
#endif