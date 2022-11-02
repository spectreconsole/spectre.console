namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class AsyncCommandValidationTests
{
    [Fact]
    public void No_Commands_Case_Should_Be_Valid()
    {
        var app = new CommandAppTester();

        app.Configure(config => {
            config.EmptyCommand();
        });

        // When
        var result = app.RunAsync(new[]
        {
            "0"
        });

        // Then
        result.ExitCode.ShouldBe(0);
        result.Settings.ShouldBeOfType<EmptySettings>(); // Results should be valid once this line passes without breaking
    }

    [Fact]
    public void No_Commands_Case_Should_Be_Invalid()
    {
        var app = new CommandAppTester();

        app.Configure(config => {
            config.EmptyCommand();
        });

        // When
        var result = app.RunAsync(new[]
        {
            "0"
        });

        if (result == 0)
        {
            return "0";
        }
        else
        {
            var result = Record.Exception(() => app.RunAsync(new[] { "0" }));

            // Then
            result.ShouldBeOfType<CommandRuntimeException>().And(e =>
            {
                e.Message.ShouldBe("The return number for Empty Commands should be 0");
            });
        }
    }

    [Fact]
    public void Should_Throw_If_Attribute_Value_Fails_Async()
    {
        // Given
        var app = new CommandApp();
        app.Configure(config =>
        {
            config.PropagateExceptions();
            config.AddBranch<AnimalSettings>("animal", animal =>
            {
                animal.AddCommand<DogCommand>("dog");
                animal.AddCommand<HorseCommand>("horse");
            });
        });

        // When
        var result = Record.Exception(() => app.RunAsync(new[] { "animal", "3", "dog", "7", "--name", "Rufus" }));

        // Then
        result.ShouldBeOfType<CommandRuntimeException>().And(e =>
        {
            e.Message.ShouldBe("Animals must have an even number of legs.");
        });
    }
}