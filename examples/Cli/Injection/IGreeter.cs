using Spectre.Console;

namespace Injection;

public interface IGreeter
{
    void Greet(string name);
}

public sealed class HelloWorldGreeter : IGreeter
{
    public void Greet(string name)
    {
        AnsiConsole.WriteLine($"Hello {name}!");
    }
}
