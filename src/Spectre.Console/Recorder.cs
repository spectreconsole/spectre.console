namespace Spectre.Console;

/// <summary>
/// A console recorder used to record output from a console.
/// </summary>
public class Recorder : IAnsiConsole, IDisposable
{
    private readonly IAnsiConsole _console;
    private readonly List<IRenderable> _recorded;

    /// <inheritdoc/>
    public Profile Profile => _console.Profile;

    /// <inheritdoc/>
    public IAnsiConsoleCursor Cursor => _console.Cursor;

    /// <inheritdoc/>
    public IAnsiConsoleInput Input => _console.Input;

    /// <inheritdoc/>
    public IExclusivityMode ExclusivityMode => _console.ExclusivityMode;

    /// <inheritdoc/>
    public RenderPipeline Pipeline => _console.Pipeline;

    /// <summary>
    /// Initializes a new instance of the <see cref="Recorder"/> class.
    /// </summary>
    /// <param name="console">The console to record output for.</param>
    public Recorder(IAnsiConsole console)
    {
        _console = console ?? throw new ArgumentNullException(nameof(console));
        _recorded = [];
    }

    /// <inheritdoc/>
    [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize")]
    public void Dispose()
    {
        // Only used for scoping.
    }

    /// <inheritdoc/>
    public void Clear(bool home)
    {
        _console.Clear(home);
    }

    /// <inheritdoc/>
    public void Write(IRenderable renderable)
    {
        ArgumentNullException.ThrowIfNull(renderable);

        _recorded.Add(renderable);
        _console.Write(renderable);
    }

    /// <inheritdoc/>
    public void WriteAnsi(Action<AnsiWriter> action)
    {
        // Do nothing
    }

    internal Recorder Clone(IAnsiConsole console)
    {
        var recorder = new Recorder(console);
        recorder._recorded.AddRange(_recorded);
        return recorder;
    }

    /// <summary>
    /// Exports the recorded data.
    /// </summary>
    /// <param name="encoder">The encoder.</param>
    /// <returns>The recorded data represented as a string.</returns>
    public string Export(IAnsiConsoleEncoder encoder)
    {
        ArgumentNullException.ThrowIfNull(encoder);

        return encoder.Encode(_console, _recorded);
    }
}

/// <summary>
/// Contains extension methods for <see cref="Recorder"/>.
/// </summary>
public static class RecorderExtensions
{
    private static readonly TextEncoder _textEncoder = new TextEncoder();
    private static readonly HtmlEncoder _htmlEncoder = new HtmlEncoder();

    /// <summary>
    /// Exports the recorded content as text.
    /// </summary>
    /// <param name="recorder">The recorder.</param>
    /// <returns>The recorded content as text.</returns>
    public static string ExportText(this Recorder recorder)
    {
        ArgumentNullException.ThrowIfNull(recorder);

        return recorder.Export(_textEncoder);
    }

    /// <summary>
    /// Exports the recorded content as HTML.
    /// </summary>
    /// <param name="recorder">The recorder.</param>
    /// <returns>The recorded content as HTML.</returns>
    public static string ExportHtml(this Recorder recorder)
    {
        ArgumentNullException.ThrowIfNull(recorder);

        return recorder.Export(_htmlEncoder);
    }
}