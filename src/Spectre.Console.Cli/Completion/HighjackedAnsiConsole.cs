namespace Spectre.Console.Cli.Completion;

internal class HighjackedAnsiConsole : IAnsiConsole
{
    public IAnsiConsole OriginalConsole { get; }

    public HighjackedAnsiConsole(IAnsiConsole console)
    {
        OriginalConsole = console;
    }

    public Profile Profile => OriginalConsole.Profile;

    public IAnsiConsoleCursor Cursor => OriginalConsole.Cursor;
    public IAnsiConsoleInput Input => OriginalConsole.Input;
    public IExclusivityMode ExclusivityMode => OriginalConsole.ExclusivityMode;
    public RenderPipeline Pipeline => OriginalConsole.Pipeline;

    public void Clear(bool home)
    {
    }

    public void Write(IRenderable renderable)
    {
    }
}
