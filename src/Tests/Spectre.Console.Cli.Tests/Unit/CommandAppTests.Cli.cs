namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    [ExpectationPath("Cli")]
    public class Cli
    {
        [Fact]
        [Expectation("Root", "DefaultValue")]
        public Task Should_Print_DefaultValue()
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.PropagateExceptions();
                config.AddCommand<CatCommand>("cat");
            });

            // When
            var result = app.Run(new[]
            {
                    "cli", "xmldoc",
            });

            // Then
            result.ExitCode.ShouldBe(0);
            return Verifier.Verify(result.Output);
        }
    }
}
