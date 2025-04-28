namespace Spectre.Console;

public static class ConsolePolyfill
{
    private static int? _bufferWidth;
    private static int? _windowHeight;

    public static int BufferWidth
    {
        get => _bufferWidth ?? Constants.DefaultTerminalWidth;
        set => _bufferWidth = value;
    }

    public static int WindowHeight
    {
        get => _windowHeight ?? Constants.DefaultTerminalHeight;
        set => _windowHeight = value;
    }
}
