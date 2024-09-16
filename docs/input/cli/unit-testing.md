Title: Unit Testing
Order: 14
Description: Instructions for unit testing a Spectre.Console application.
Reference: 
    - T:Spectre.Console.Testing.CommandAppTester
    - T:Spectre.Console.Testing.TestConsole
    - T:Spectre.Console.Testing.TestConsoleInput
---

`Spectre.Console` has a separate project that contains test harnesses for unit testing your own console applications. 

The fastest way of getting started is to install the `Spectre.Console.Testing` NuGet package.

```text
> dotnet add package Spectre.Console.Testing
```

`Spectre.Console.Testing` is also the namespace containing the test classes.

## Testing a CommandApp

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

## Testing console behaviour

 `TestConsole` and `TestConsoleInput` are testable implementations of `IAnsiConsole` and `IAnsiConsoleInput`, allowing you fine-grain control over testing console output and interactivity.

The following example renders some widgets before then validating the console output:

```csharp
    [TestMethod]
    public void Should_Render_Panel()
    {
        // Given
        var console = new TestConsole();

        // When
        console.Write(new Panel(new Text("Hello World")));

        // Then
        Assert.AreEqual(console.Output, """"
┌─────────────┐
│ Hello World │
└─────────────┘

"""");
    }
```

While `Assert` is fine for validating simple output, more complex output may benefit from a tool like [Verify](https://github.com/VerifyTests/Verify).

The following example prompts the user for input before then validating the expected choice was made:

```csharp
    [TestMethod]
    public void Should_Select_Orange()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushTextWithEnter("Orange");

        // When
        console.Prompt(
            new TextPrompt<string>("Favorite fruit?")
                .AddChoice("Banana")
                .AddChoice("Orange"));

        // Then
        Assert.AreEqual(console.Output, "Favorite fruit? [Banana/Orange]: Orange\n");
    }
```

`CommandAppTester` uses `TestConsole` internally, which in turn uses `TestConsoleInput`, offering a fully testable harness for `Spectre.Console` widgets, prompts and commands.