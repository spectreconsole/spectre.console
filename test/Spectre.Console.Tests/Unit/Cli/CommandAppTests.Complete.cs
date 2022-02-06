namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    [UsesVerify]
    [ExpectationPath("Cli/Complete")]
    public sealed class Complete
    {
        [Fact]
        [Expectation("Test_1")]
        public Task Should_Return_Correct_Completions_For_Case_1()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(config =>
            {
                config.SetApplicationName("myapp");
                config.PropagateExceptions();
                config.AddCommand<LionCommand>("lion");
            });
            var commandToRun = Constants.CompleteCommand
                   .ToList()
                   .Append("myapp")
                   .Append("li");

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }
    }
}
