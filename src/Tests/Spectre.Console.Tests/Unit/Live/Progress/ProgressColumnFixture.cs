namespace Spectre.Console.Tests.Unit;

public sealed class ProgressColumnFixture<T>
    where T : ProgressColumn, new()
{
    public T Column { get; }
    public ProgressTask Task { get; set; }

    public ProgressColumnFixture(double completed, double total)
    {
        Column = new T();
        Task = new ProgressTask(1, "Foo", total);
        Task.Increment(completed);
    }

    public string Render()
    {
        var console = new TestConsole();
        var context = RenderOptions.Create(console, console.Profile.Capabilities);
        console.Write(Column.Render(context, Task, TimeSpan.Zero));
        return console.Output;
    }
}
