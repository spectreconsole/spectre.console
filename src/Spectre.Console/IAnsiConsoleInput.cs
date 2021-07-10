using System;
using System.Threading;
using System.Threading.Tasks;

namespace Spectre.Console
{
    /// <summary>
    /// Represents the console's input mechanism.
    /// </summary>
    public interface IAnsiConsoleInput
    {
        /// <summary>
        /// Reads a key from the console.
        /// </summary>
        /// <param name="intercept">Whether or not to intercept the key.</param>
        /// <returns>The key that was read.</returns>
        ConsoleKeyInfo? ReadKey(bool intercept);

        /// <summary>
        /// Reads a key from the console.
        /// </summary>
        /// <param name="intercept">Whether or not to intercept the key.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The key that was read.</returns>
        Task<ConsoleKeyInfo?> ReadKeyAsync(bool intercept, CancellationToken cancellationToken);
    }
}
