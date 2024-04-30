namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    [Fact]
    [Expectation("Should_Expose_Raw_Arguments")]
    public void Should_Return_Correct_Text_When_Command_Is_Unknown()
    {
        // Given
        var app = new CommandAppTester();
        app.Configure(config =>
        {
            config.AddCommand<EmptyCommand>("test");
        });

        // When
        var result = app.Run("test", "--foo", "32", "--lol");

        // Then
        result.Context.ShouldNotBeNull();
        result.Context.Arguments.ShouldBe(new[] { "test", "--foo", "32", "--lol" });
    }
}