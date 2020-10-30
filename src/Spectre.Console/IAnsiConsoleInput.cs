using System;

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
        ConsoleKeyInfo ReadKey(bool intercept);
    }
}
