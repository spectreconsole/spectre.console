namespace Spectre.Console.Testing.Cli;
internal class AsyncAppFailure
{
    /// <summary>
    /// Gets the exception that was thrown.
    /// </summary>
    public Exception Exception { get; }

    /// <summary>
    /// Gets the console output.
    /// </summary>
    public string Output { get; }

    internal AsyncAppFailure(Exception exception, string output)
    {
        Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        Output = output.NormalizeLineEndings()
            .TrimLines()
            .Trim();
    }
}
