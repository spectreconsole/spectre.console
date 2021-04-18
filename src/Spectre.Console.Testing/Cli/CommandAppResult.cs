using Spectre.Console.Cli;

namespace Spectre.Console.Testing
{
    /// <summary>
    /// Represents the result of a completed <see cref="CommandApp"/> run.
    /// </summary>
    public sealed class CommandAppResult
    {
        /// <summary>
        /// Gets the exit code.
        /// </summary>
        public int ExitCode { get; }

        /// <summary>
        /// Gets the console output.
        /// </summary>
        public string Output { get; }

        /// <summary>
        /// Gets the command context.
        /// </summary>
        public CommandContext? Context { get; }

        /// <summary>
        /// Gets the command settings.
        /// </summary>
        public CommandSettings? Settings { get; }

        internal CommandAppResult(int exitCode, string output, CommandContext? context, CommandSettings? settings)
        {
            ExitCode = exitCode;
            Output = output ?? string.Empty;
            Context = context;
            Settings = settings;

            Output = Output
                .NormalizeLineEndings()
                .TrimLines()
                .Trim();
        }
    }
}
