using System;
using System.Threading;
using System.Threading.Tasks;

namespace Spectre.Console.Internal
{
    internal sealed class DefaultExclusivityMode : IExclusivityMode
    {
        private static readonly SemaphoreSlim _semaphore;

        static DefaultExclusivityMode()
        {
            _semaphore = new SemaphoreSlim(1, 1);
        }

        public T Run<T>(Func<T> func)
        {
            // Try aquiring the exclusivity semaphore
            if (!_semaphore.Wait(0))
            {
                throw new InvalidOperationException(
                    "Trying to run one or more interactive functions concurrently. " +
                    "Operations with dynamic displays (e.g. a prompt and a progress display) " +
                    "cannot be running at the same time.");
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

        public async Task<T> Run<T>(Func<Task<T>> func)
        {
            // Try aquiring the exclusivity semaphore
            if (!await _semaphore.WaitAsync(0).ConfigureAwait(false))
            {
                // TODO: Need a better message here
                throw new InvalidOperationException(
                    "Could not aquire the interactive semaphore");
            }

            try
            {
                return await func();
            }
            finally
            {
                _semaphore.Release(1);
            }
        }
    }
}
