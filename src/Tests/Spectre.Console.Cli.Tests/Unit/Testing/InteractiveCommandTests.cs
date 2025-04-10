namespace Spectre.Console.Cli.Tests.Unit.Testing;

public sealed class InteractiveCommandTests
{
    private sealed class InteractiveCommand : Command
    {
        private readonly IAnsiConsole _console;

        public InteractiveCommand(IAnsiConsole console)
        {
            _console = console;
        }

        public override int Execute(CommandContext context)
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
