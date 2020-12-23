using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// Represents a command line application.
    /// </summary>
    public interface ICommandApp
    {
        /// <summary>
        /// Configures the command line application.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        void Configure(Action<IConfigurator> configuration);

        /// <summary>
        /// Runs the command line application with specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The exit code from the executed command.</returns>
        int Run(IEnumerable<string> args);

        /// <summary>
        /// Runs the command line application with specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The exit code from the executed command.</returns>
        Task<int> RunAsync(IEnumerable<string> args);
    }
}