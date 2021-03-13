using System;
using System.Threading.Tasks;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IAnsiConsole"/>.
    /// </summary>
    public static partial class AnsiConsoleExtensions
    {
        /// <summary>
        /// Runs the specified function in exclusive mode.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="console">The console.</param>
        /// <param name="func">The func to run in exclusive mode.</param>
        /// <returns>The result of the function.</returns>
        public static T RunExclusive<T>(this IAnsiConsole console, Func<T> func)
        {
            return console.ExclusivityMode.Run(func);
        }

        /// <summary>
        /// Runs the specified function in exclusive mode asynchronously.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="console">The console.</param>
        /// <param name="func">The func to run in exclusive mode.</param>
        /// <returns>The result of the function.</returns>
        public static Task<T> RunExclusive<T>(this IAnsiConsole console, Func<Task<T>> func)
        {
            return console.ExclusivityMode.Run(func);
        }
    }
}
