namespace Spectre.Console.Cli.Tests.Unit.AsyncCommandTests;

// A dummy AsyncCommand<T> impl
public class AsyncTestCommand : AsyncCommand<AsyncTestCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("--foo")]
        public int Foo { get; set; }
    }

    public override Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        return Task.FromResult(0);
    }

    /// <summary>
    /// Async commands are tested like other commands.
    /// </summary>
    [Fact]
    public void Test_For_An_Async_Command()
    {
        // Given
        var app = new AsyncTester();
        app.Configure(config =>
        {
            config.PropagateExceptions();
            config.AddBranch<AnimalSettings>("animal", animal =>
            {
                animal.AddBranch<MammalSettings>("mammal", mammal =>
                {
                    mammal.AddCommand<DogCommand>("dog");
                    mammal.AddCommand<HorseCommand>("horse");
                });
            });
        });

        // When
        var result = app.RunAsync(new[]
        {
            "animal", "--alive", "mammal", "--name",
            "Rufus", "dog", "12", "--good-boy",
        });

        // Then
        result.ExitCode.ShouldBe(0);
        result.Settings.ShouldBeOfType<DogSettings>().And(dog =>
        {
            dog.Age.ShouldBe(12);
            dog.GoodBoy.ShouldBe(true);
            dog.Name.ShouldBe("Rufus");
            dog.IsAlive.ShouldBe(true);
        });
    }
}