Title: Unit Testing
Order: 14
Description: Instructions for unit testing a Spectre.Console application.
Reference: T:Spectre.Console.Testing.CommandAppTester
---

`Spectre.Console` has a separate project that contains test harnesses for unit testing your own console applications. 

The fastest way of getting started is to install the `Spectre.Console.Testing` NuGet package.

```text
> dotnet add package Spectre.Console.Testing
```

`Spectre.Console.Testing` is also the namespace containing the test classes.

The `CommandAppTester` is a test implementation of `CommandApp` that's configured in a similar manner but designed for unit testing.

The following example validates the exit code and terminal output of a `Spectre.Console` command:

```csharp
    /// <summary>
    /// A Spectre.Console Command
    /// </summary>
    public class HelloWorldCommand : Command
    {
        private readonly IAnsiConsole _console;

        public HelloWorldCommand(IAnsiConsole console)
        {
            // nb. AnsiConsole should not be called directly by the command
            // since this doesn't play well with testing. Instead,
            // the command should inject a IAnsiConsole and use that.

            _console = console;
        }

        public override int Execute(CommandContext context)
        {
            _console.WriteLine("Hello world.");
            return 0;
        }
    }

    [TestMethod]
    public void Should_Output_Hello_World()
    {
        // Given
        var app = new CommandAppTester();
        app.SetDefaultCommand<HelloWorldCommand>();

        // When
        var result = app.Run();

        // Then
        Assert.AreEqual(result.ExitCode, 0);
        Assert.AreEqual(result.Output, "Hello world.");
    }
```