namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    [UsesVerify]
    [ExpectationPath("Cli/Complete")]
    public sealed class Complete
    {
        [Fact]
        [Expectation("Test_1")]
        public Task Should_Return_Correct_Completions_When_There_Is_A_Partial_Command_Typed_In()
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
        public Task Should_Return_Correct_Completions_When_There_Is_More_Than_One_Command()
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
        public Task Should_Return_Correct_Completions_When_There_Is_A_Branch()
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
        public Task Should_Return_Correct_Completions_When_We_Are_In_A_Branch()
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

        [Fact]
        [Expectation("Test_5")]
        public Task Should_Return_Correct_Completions_When_There_Are_Multiple_Branches()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(config =>
            {
                config.SetApplicationName("myapp");
                config.PropagateExceptions();
                config.AddBranch("feline", feline =>
                {
                    feline.AddCommand<LionCommand>("lion");
                    feline.AddCommand<CatCommand>("cat");
                });
                config.AddBranch("other", other =>
                {
                    other.AddCommand<DogCommand>("dog");
                    other.AddCommand<HorseCommand>("horse");
                });
            });
            var commandToRun = Constants.CompleteCommand
                   .ToList()
                   .Append("\"myapp other \"");

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_6")]
        public Task Should_Return_Correct_Completions_When_There_Are_Many_Options_With_Same_Initial()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(config =>
            {
                config.SetApplicationName("myapp");
                config.PropagateExceptions();
                config.AddBranch("feline", feline =>
                {
                    feline.AddCommand<CatCommand>("felix");
                });
                config.AddBranch("fantasy", other =>
                {
                    other.AddCommand<EmptyCommand>("fairy");
                });
            });
            var commandToRun = Constants.CompleteCommand
                   .ToList()
                   .Append("\"myapp f\"");

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_7")]
        public Task Should_Return_Correct_Completions_For_Parameters()
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
                   .Append("\"myapp lion \"");

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_8")]
        public Task Should_Return_Correct_Completions_For_Parameters_When_Partial_Parameter_Name_Is_Provided()
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
                   .Append("\"myapp lion --n\"");

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }
    }
}
