using Spectre.Console.Cli.Tests.Data.Commands;

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
                    feline.AddCommand<LionCommand>("lion");
                });

                config.AddBranch("fantasy", other =>
                {
                    other.AddCommand<EmptyCommand>("fairy");
                });

                // This one should not appear in the completions
                config.AddCommand<LionCommand>("lion");
            });
            var commandToRun = Constants.CompleteCommand
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
                   .Append("\"myapp lion 2 4 \"");

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
                   .Append("\"myapp lion --n\"");

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_9")]
        public Task Should_Return_Correct_Completions_For_Arguments_When_Partial_Argument_Value_Is_Provided1()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(config =>
            {
                config.SetApplicationName("myapp");
                config.PropagateExceptions();
                config.AddCommand<CatCommand>("cat");
                config.AddCommand<LionCommand>("lion");
            });

            // Legs TEETH
            // Legs should be completed, because it does not have a trailing space
            var commandToRun = Constants.CompleteCommand
                   .Append("\"myapp lion 1\"");

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_10")]
        public Task Should_Return_Correct_Completions_For_Arguments_When_Trailing_Space()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(config =>
            {
                config.SetApplicationName("myapp");
                config.PropagateExceptions();
                config.AddCommand<CatCommand>("cat");
                config.AddCommand<LionCommand>("lion");
            });

            // Legs TEETH // TEEH should be completed
            // Teeth should be completed, because it does have a trailing space
            var commandToRun = Constants.CompleteCommand
                   .Append("\"myapp lion 2 \"");

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_11")]
        public Task Should_Return_Correct_Completions_For_Parameters_Name_Should_Be_Angelika()
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
                   .Append("\"myapp lion 2 4 --name \"")
                   ;

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_12")]
        public Task Should_Return_Correct_Completions_For_Parameters_When_AlreadyFullyWritten()
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
                   .Append("\"myapp lion 2 4 --name\"")
                   ;

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_13")]
        public Task Should_Return_Correct_Completions_In_Branch()
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
                    feline.AddCommand<LionCommand>("lion");
                });

                config.AddBranch("fantasy", other =>
                {
                    other.AddCommand<EmptyCommand>("fairy");
                });
            });
            var commandToRun = Constants.CompleteCommand
                   .Append("\"myapp feline l\"");

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_15")]
        public Task Should_Return_Nothing_When_Match_Exact_But_No_Trailing_Space()
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
                    feline.AddCommand<LionCommand>("lion");
                });

                config.AddBranch("fantasy", other =>
                {
                    other.AddCommand<EmptyCommand>("fairy");
                });

                config.AddCommand<LionCommand>("lion");
            });
            var commandToRun = Constants.CompleteCommand
                   .Append("\"myapp feline\"");

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_16")]
        public Task Should_Return_Completions_When_Match_Exact_And_Has_Trailing_Space()
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
                    feline.AddCommand<LionCommand>("lion");
                });

                config.AddBranch("fantasy", other =>
                {
                    other.AddCommand<EmptyCommand>("fairy");
                });

                config.AddCommand<LionCommand>("lion");
            });
            var commandToRun = Constants.CompleteCommand
                   .Append("\"myapp feline \"");

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_17")]
        public Task Should_Return_All_Completions_When_No_Command()
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
                    feline.AddCommand<LionCommand>("lion");
                });

                config.AddBranch("fantasy", other =>
                {
                    other.AddCommand<EmptyCommand>("fairy");
                });

                config.AddCommand<LionCommand>("lion");
            });
            var commandToRun = Constants.CompleteCommand
                   .Append("\"myapp \"");

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_18")]
        public Task Should_Return_All_Completions_When_No_Command_But_Also_No_Trailing_Space()
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
                    feline.AddCommand<LionCommand>("lion");
                });

                config.AddBranch("fantasy", other =>
                {
                    other.AddCommand<EmptyCommand>("fairy");
                });

                config.AddCommand<LionCommand>("lion");
            });
            var commandToRun = Constants.CompleteCommand
                   .Append("\"myapp\"");

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_19")]
        public Task Should_Handle_Positions()
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
                    feline.AddCommand<LionCommand>("lion");
                });

                config.AddBranch("fantasy", other =>
                {
                    other.AddCommand<EmptyCommand>("fairy");
                });

                config.AddCommand<LionCommand>("lion");
            });
            var commandToRun = Constants.CompleteCommand
                   .Append("\"myapp feline\"")
                   .Append("--position")
                   .Append("12")
                   ;

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_20")]
        public Task Should_Handle_Positions1()
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
                    feline.AddCommand<LionCommand>("lion");
                });

                config.AddBranch("fantasy", other =>
                {
                    other.AddCommand<EmptyCommand>("fairy");
                });

                config.AddCommand<LionCommand>("lion");
            });
            var commandToRun = Constants.CompleteCommand
                   .Append("\"myapp feline\"")
                   .Append("--position")
                   .Append("13")
                   ;

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_21")]
        public Task Should_Handle_Positions2()
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
                    feline.AddCommand<LionCommand>("lion");
                });

                config.AddBranch("fantasy", other =>
                {
                    other.AddCommand<EmptyCommand>("fairy");
                });

                config.AddCommand<LionCommand>("lion");
            });
            var commandToRun = Constants.CompleteCommand
                   .Append("\"myapp\"")
                   .Append("--position")
                   .Append("6")
                   ;

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        // Test:
        // myapp [branch] [command] [dynamic_argument]
        // "myapp cats lion 1" <- should return 16
        [Fact]
        [Expectation("Test_22")]
        public Task Should_Handle_Positions3()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(config =>
            {
                config.SetApplicationName("myapp");
                config.PropagateExceptions();
                config.AddBranch("cats", feline =>
                {
                    feline.AddCommand<CatCommand>("felix");
                    feline.AddCommand<LionCommand>("lion");
                });

                config.AddBranch("dogs", feline =>
                {
                    feline.AddCommand<CatCommand>("felix");
                    feline.AddCommand<LionCommand>("lion");
                });

                config.AddCommand<LionCommand>("lion");
            });
            var commandToRun = Constants.CompleteCommand
                   .Append("\"myapp cats lion 1\"")
                   .Append("--position")
                   .Append("17")
                   ;

            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_23")]
        public Task CompletionSuggestionsAttribute_Should_Suggest_Option_Values_Starting_With_Partial()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(config =>
            {
                config.SetApplicationName("myapp");
                config.PropagateExceptions();

                config.AddBranch("user", feline =>
                {
                    feline.AddCommand<UserAddCommand>("add");
                });
            });
            var commandToRun = Constants.CompleteCommand
                   .Append("\"myapp user add angel --age 1\"")
                   ;
            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_24")]
        public Task CompletionSuggestionsAttribute_Should_Suggest_Option_Values()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(config =>
            {
                config.SetApplicationName("myapp");
                config.PropagateExceptions();

                config.AddBranch("user", feline =>
                {
                    feline.AddCommand<UserAddCommand>("add");
                });
            });
            var commandToRun = Constants.CompleteCommand
                   .Append("\"myapp user add angel --age \"")
                   ;
            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_25")]
        public Task CompletionSuggestionsAttribute_Should_Suggest_Argument_Values()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(config =>
            {
                config.SetApplicationName("myapp");
                config.PropagateExceptions();

                config.AddBranch("user", feline =>
                {
                    feline.AddCommand<UserAddCommand>("add");
                });
            });
            var commandToRun = Constants.CompleteCommand
                   .Append("\"myapp user add \"")
                   ;
            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_26")]
        public Task CompletionSuggestionsAttribute_Should_Suggest_Argument_Values_With_Partial()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(config =>
            {
                config.SetApplicationName("myapp");
                config.PropagateExceptions();

                config.AddBranch("user", feline =>
                {
                    feline.AddCommand<UserAddCommand>("add");
                });
            });
            var commandToRun = Constants.CompleteCommand
                   .Append("\"myapp user add a\"")
                   ;
            // When
            var result = fixture.Run(commandToRun.ToArray());

            // Then
            return Verifier.Verify(result.Output);
        }
    }
}