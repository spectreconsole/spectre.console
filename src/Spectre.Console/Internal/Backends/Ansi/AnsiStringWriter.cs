namespace Spectre.Console;

internal sealed class AnsiStringWriter
{
    private readonly SemaphoreSlim _lock;
    private readonly StringBuilder _builder;
    private readonly AnsiWriter _writer;

    public static AnsiStringWriter Shared { get; } = new();

    private AnsiStringWriter()
    {
        _lock = new SemaphoreSlim(1, 1);
        _builder = new StringBuilder();
        _writer = new AnsiWriter(
            new StringWriter(_builder),
            new Capabilities
            {
                ColorSystem = ColorSystem.NoColors,
                Ansi = true,
                Links = true,
                Interactive = false,
                Unicode = true,
                AlternateBuffer = true,
            });
    }

    public string Write(IReadOnlyCapabilities capabilities, Action<AnsiWriter> action)
    {
        _lock.Wait();

        try
        {
            _builder.Clear();
            _writer.Capabilities = capabilities;
            action(_writer);
            return _builder.ToString();
        }
        finally
        {
            _lock.Release();
        }
    }
}