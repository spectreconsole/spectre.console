namespace Spectre.Console.Internal;

internal sealed class DefaultExclusivityMode : IExclusivityMode
{
    private readonly SemaphoreSlim _semaphore;

    public DefaultExclusivityMode()
    {
        _semaphore = new SemaphoreSlim(1, 1);
    }

    public T Run<T>(Func<T> func)
    {
        // Try acquiring the exclusivity semaphore
        if (!_semaphore.Wait(0))
        {
            throw CreateExclusivityException();
        }

        try
        {
            return func();
        }
        finally
        {
            _semaphore.Release(1);
        }
    }

    public async Task<T> RunAsync<T>(Func<Task<T>> func)
    {
        // Try acquiring the exclusivity semaphore
        if (!await _semaphore.WaitAsync(0).ConfigureAwait(false))
        {
            throw CreateExclusivityException();
        }

        try
        {
            return await func().ConfigureAwait(false);
        }
        finally
        {
            _semaphore.Release(1);
        }
    }

    private static Exception CreateExclusivityException() => new InvalidOperationException(
        "Trying to run one or more interactive functions concurrently. " +
        "Operations with dynamic displays (e.g. a prompt and a progress display) " +
        "cannot be running at the same time.");
}