using System;
using System.Threading.Tasks;

namespace Spectre.Console
{
    /// <summary>
    /// Represents an exclusivity mode.
    /// </summary>
    public interface IExclusivityMode
    {
        /// <summary>
        /// Runs the specified function in exclusive mode.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="func">The func to run in exclusive mode.</param>
        /// <returns>The result of the function.</returns>
        T Run<T>(Func<T> func);

        /// <summary>
        /// Runs the specified function in exclusive mode asynchronously.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="func">The func to run in exclusive mode.</param>
        /// <returns>The result of the function.</returns>
        Task<T> Run<T>(Func<Task<T>> func);
    }
}
