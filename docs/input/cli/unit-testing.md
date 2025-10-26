Title: Unit Testing
Order: 14
Description: Instructions for unit testing a Spectre.Console application.
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

        public override int Execute(CommandContext context, CancellationToken cancellationToken)
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

The following example demonstrates how to mock user inputs for an interactive command.
This test (InteractiveCommand_WithMockedUserInputs_ProducesExpectedOutput) simulates user interactions by pushing predefined inputs to the console, then verifies that the resulting output is as expected.

```csharp
public sealed class InteractiveCommandTests
{
    private sealed class InteractiveCommand : Command
    {
        private readonly IAnsiConsole _console;

        public InteractiveCommand(IAnsiConsole console)
        {
            _console = console;
        }

        public override int Execute(CommandContext context, CancellationToken cancellationToken)
        {
            var fruits = _console.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("What are your [green]favorite fruits[/]?")
                    .NotRequired() // Not required to have a favorite fruit
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                    .InstructionsText(
                        "[grey](Press [blue]<space>[/] to toggle a fruit, " +
                        "[green]<enter>[/] to accept)[/]")
                    .AddChoices(new[] {
                        "Apple", "Apricot", "Avocado",
                        "Banana", "Blackcurrant", "Blueberry",
                        "Cherry", "Cloudberry", "Coconut",
                    }));

            var fruit = _console.Prompt(
                new SelectionPrompt<string>()
                    .Title("What's your [green]favorite fruit[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                    .AddChoices(new[] {
                        "Apple", "Apricot", "Avocado",
                        "Banana", "Blackcurrant", "Blueberry",
                        "Cherry", "Cloudberry", "Cocunut",
                    }));

            var name = _console.Ask<string>("What's your name?");

            _console.WriteLine($"[{string.Join(',', fruits)};{fruit};{name}]");

            return 0;
        }
    }

    [Fact]
    public void InteractiveCommand_WithMockedUserInputs_ProducesExpectedOutput()
    {
        // Given
        TestConsole console = new();
        console.Interactive();

        // Your mocked inputs must always end with "Enter" for each prompt!

        // Multi selection prompt: Choose first option
        console.Input.PushKey(ConsoleKey.Spacebar);
        console.Input.PushKey(ConsoleKey.Enter);

        // Selection prompt: Choose second option
        console.Input.PushKey(ConsoleKey.DownArrow);
        console.Input.PushKey(ConsoleKey.Enter);

        // Ask text prompt: Enter name
        console.Input.PushTextWithEnter("Spectre Console");

        var app = new CommandAppTester(null, new CommandAppTesterSettings(), console);
        app.SetDefaultCommand<InteractiveCommand>();

        // When
        var result = app.Run();

        // Then
        result.ExitCode.ShouldBe(0);
        result.Output.EndsWith("[Apple;Apricot;Spectre Console]");
    }
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