namespace Spectre.Console.Testing.Cli;
internal class AsyncAppResult
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

    internal AsyncAppResult(int exitCode, string output, CommandContext? context, CommandSettings? settings)
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