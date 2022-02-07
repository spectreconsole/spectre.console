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
                   .Append("\"myapp li\"");

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_2")]
        public Task Should_Return_Correct_Completions_For_Case_2()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(config =>
            {
                config.SetApplicationName("myapp");
                config.PropagateExceptions();
                config.AddCommand<LionCommand>("lion");
                config.AddCommand<CatCommand>("cat");
            });
            var commandToRun = Constants.CompleteCommand
                   .ToList()
                   .Append("\"myapp \"");

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_3")]
        public Task Should_Return_Correct_Completions_For_Case_3()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(config =>
            {
                config.SetApplicationName("myapp");
                config.PropagateExceptions();
                config.AddBranch("animal", animal =>
                {
                    animal.AddCommand<LionCommand>("lion");
                    animal.AddCommand<CatCommand>("cat");
                });
            });
            var commandToRun = Constants.CompleteCommand
                   .ToList()
                   .Append("\"myapp \"");

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_4")]
        public Task Should_Return_Correct_Completions_For_Case_4()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(config =>
            {
                config.SetApplicationName("myapp");
                config.PropagateExceptions();
                config.AddBranch("animal", animal =>
                {
                    animal.AddCommand<LionCommand>("lion");
                    animal.AddCommand<CatCommand>("cat");
                });
            });
            var commandToRun = Constants.CompleteCommand
                   .ToList()
                   .Append("\"myapp animal \"");

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }
    }
}
