namespace Spectre.Console;

public static partial class AnsiConsoleExtensions
{
    /// <param name="console">The console.</param>
    extension(IAnsiConsole console)
    {
        /// <summary>
        /// Runs the specified function in exclusive mode.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="func">The func to run in exclusive mode.</param>
        /// <returns>The result of the function.</returns>
        public T RunExclusive<T>(Func<T> func)
        {
            return console.ExclusivityMode.Run(func);
        }

        /// <summary>
        /// Runs the specified function in exclusive mode asynchronously.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="func">The func to run in exclusive mode.</param>
        /// <returns>The result of the function.</returns>
        public Task<T> RunExclusive<T>(Func<Task<T>> func)
        {
            return console.ExclusivityMode.RunAsync(func);
        }
    }
}