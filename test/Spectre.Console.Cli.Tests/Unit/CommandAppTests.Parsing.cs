namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    [UsesVerify]
    [ExpectationPath("Parsing")]
    public sealed class Parsing
    {
        [UsesVerify]
        [ExpectationPath("UnknownCommand")]
        public sealed class UnknownCommand
        {
            [Fact]
            [Expectation("Test_1")]
            public Task Should_Return_Correct_Text_When_Command_Is_Unknown()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(config =>
                {
                    config.AddCommand<DogCommand>("dog");
                });

                // When
                var result = app.Run("cat", "14");

                // Then
                return Verifier.Verify(result.Output);
            }

            [Fact]
            [Expectation("Test_2")]
            public Task Should_Return_Correct_Text_For_Unknown_Command_When_Current_Command_Has_No_Arguments()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(configurator =>
                {
                    configurator.AddCommand<EmptyCommand>("empty");
                });

                // When
                var result = app.Run("empty", "other");

                // Then
                return Verifier.Verify(result.Output);
            }

            [Fact]
            [Expectation("Test_3")]
            public Task Should_Return_Correct_Text_With_Suggestion_When_Command_Followed_By_Argument_Is_Unknown_And_Distance_Is_Small()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(config =>
                {
                    config.AddBranch<CommandSettings>("dog", a =>
                    {
                        a.AddCommand<CatCommand>("cat");
                    });
                });

                // When
                var result = app.Run("dog", "bat", "14");

                // Then
                return Verifier.Verify(result.Output);
            }

            [Fact]
            [Expectation("Test_4")]
            public Task Should_Return_Correct_Text_With_Suggestion_When_Root_Command_Followed_By_Argument_Is_Unknown_And_Distance_Is_Small()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(config =>
                {
                    config.AddCommand<CatCommand>("cat");
                });

                // When
                var result = app.Run("bat", "14");

                // Then
                return Verifier.Verify(result.Output);
            }

            [Fact]
            [Expectation("Test_5")]
            public Task Should_Return_Correct_Text_With_Suggestion_And_No_Arguments_When_Root_Command_Is_Unknown_And_Distance_Is_Small()
            {
                // Given
                var app = new CommandAppTester();
                app.SetDefaultCommand<GenericCommand<EmptyCommandSettings>>();
                app.Configure(config =>
                {
                    config.AddCommand<GenericCommand<EmptyCommandSettings>>("cat");
                });

                // When
                var result = app.Run("bat");

                // Then
                return Verifier.Verify(result.Output);
            }

            [Fact]
            [Expectation("Test_6")]
            public Task Should_Return_Correct_Text_With_Suggestion_And_No_Arguments_When_Command_Is_Unknown_And_Distance_Is_Small()
            {
                // Given
                var app = new CommandAppTester();
                app.SetDefaultCommand<GenericCommand<EmptyCommandSettings>>();
                app.Configure(configurator =>
                {
                    configurator.AddBranch<CommandSettings>("dog", a =>
                    {
                        a.AddCommand<CatCommand>("cat");
                    });
                });

                // When
                var result = app.Run("dog", "bat");

                // Then
                return Verifier.Verify(result.Output);
            }

            [Fact]
            [Expectation("Test_7")]
            public Task Should_Return_Correct_Text_With_Suggestion_When_Root_Command_After_Argument_Is_Unknown_And_Distance_Is_Small()
            {
                // Given
                var app = new CommandAppTester();
                app.SetDefaultCommand<GenericCommand<FooCommandSettings>>();
                app.Configure(configurator =>
                {
                    configurator.AddCommand<GenericCommand<BarCommandSettings>>("bar");
                });

                // When
                var result = app.Run("qux", "bat");

                // Then
                return Verifier.Verify(result.Output);
            }

            [Fact]
            [Expectation("Test_8")]
            public Task Should_Return_Correct_Text_With_Suggestion_When_Command_After_Argument_Is_Unknown_And_Distance_Is_Small()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(configurator =>
                {
                    configurator.AddBranch<FooCommandSettings>("foo", a =>
                    {
                        a.AddCommand<GenericCommand<BarCommandSettings>>("bar");
                    });
                });

                // When
                var result = app.Run("foo", "qux", "bat");

                // Then
                return Verifier.Verify(result.Output);
            }
        }

        [UsesVerify]
        [ExpectationPath("CannotAssignValueToFlag")]
        public sealed class CannotAssignValueToFlag
        {
            [Fact]
            [Expectation("Test_1")]
            public Task Should_Return_Correct_Text_For_Long_Option()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(configurator =>
                {
                    configurator.AddCommand<DogCommand>("dog");
                });

                // When
                var result = app.Run("dog", "--alive=indeterminate", "foo");

                // Then
                return Verifier.Verify(result.Output);
            }

            [Fact]
            [Expectation("Test_2")]
            public Task Should_Return_Correct_Text_For_Short_Option()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(configurator =>
                {
                    configurator.AddCommand<DogCommand>("dog");
                });

                // When
                var result = app.Run("dog", "-a=indeterminate", "foo");

                // Then
                return Verifier.Verify(result.Output);
            }
        }

        [UsesVerify]
        [ExpectationPath("NoValueForOption")]
        public sealed class NoValueForOption
        {
            [Fact]
            [Expectation("Test_1")]
            public Task Should_Return_Correct_Text_For_Long_Option()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(configurator =>
                {
                    configurator.AddCommand<DogCommand>("dog");
                });

                // When
                var result = app.Run("dog", "--name");

                // Then
                return Verifier.Verify(result.Output);
            }

            [Fact]
            [Expectation("Test_2")]
            public Task Should_Return_Correct_Text_For_Short_Option()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(configurator =>
                {
                    configurator.AddCommand<DogCommand>("dog");
                });

                // When
                var result = app.Run("dog", "-n");

                // Then
                return Verifier.Verify(result.Output);
            }
        }

        [UsesVerify]
        [ExpectationPath("NoMatchingArgument")]
        public sealed class NoMatchingArgument
        {
            [Fact]
            [Expectation("Test_1")]
            public Task Should_Return_Correct_Text()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(configurator =>
                {
                    configurator.AddCommand<GiraffeCommand>("giraffe");
                });

                // When
                var result = app.Run("giraffe", "foo", "bar", "baz");

                // Then
                return Verifier.Verify(result.Output);
            }
        }

        [UsesVerify]
        [ExpectationPath("UnexpectedOption")]
        public sealed class UnexpectedOption
        {
            [Fact]
            [Expectation("Test_1")]
            public Task Should_Return_Correct_Text_For_Long_Option()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(configurator =>
                {
                    configurator.AddCommand<DogCommand>("dog");
                });

                // When
                var result = app.Run("--foo");

                // Then
                return Verifier.Verify(result.Output);
            }

            [Fact]
            [Expectation("Test_2")]
            public Task Should_Return_Correct_Text_For_Short_Option()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(configurator =>
                {
                    configurator.AddCommand<DogCommand>("dog");
                });

                // When
                var result = app.Run("-f");

                // Then
                return Verifier.Verify(result.Output);
            }
        }

        [UsesVerify]
        [ExpectationPath("UnknownOption")]
        public sealed class UnknownOption
        {
            [Fact]
            [Expectation("Test_1")]
            public Task Should_Return_Correct_Text_For_Long_Option_If_Strict_Mode_Is_Enabled()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(configurator =>
                {
                    configurator.UseStrictParsing();
                    configurator.AddCommand<DogCommand>("dog");
                });

                // When
                var result = app.Run("dog", "--unknown");

                // Then
                return Verifier.Verify(result.Output);
            }

            [Fact]
            [Expectation("Test_2")]
            public Task Should_Return_Correct_Text_For_Short_Option_If_Strict_Mode_Is_Enabled()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(configurator =>
                {
                    configurator.UseStrictParsing();
                    configurator.AddCommand<DogCommand>("dog");
                });

                // When
                var result = app.Run("dog", "-u");

                // Then
                return Verifier.Verify(result.Output);
            }
        }

        [UsesVerify]
        [ExpectationPath("OptionWithoutName")]
        public sealed class OptionWithoutName
        {
            [Fact]
            [Expectation("Test_1")]
            public Task Should_Return_Correct_Text_For_Short_Option()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(configurator =>
                {
                    configurator.AddCommand<DogCommand>("dog");
                });

                // When
                var result = app.Run("dog", "-", " ");

                // Then
                return Verifier.Verify(result.Output);
            }

            [Fact]
            [Expectation("Test_2")]
            public Task Should_Return_Correct_Text_For_Missing_Long_Option_Value_With_Equality_Separator()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(configurator =>
                {
                    configurator.AddCommand<DogCommand>("dog");
                });

                // When
                var result = app.Run("dog", $"--foo=");

                // Then
                return Verifier.Verify(result.Output);
            }

            [Fact]
            [Expectation("Test_3")]
            public Task Should_Return_Correct_Text_For_Missing_Long_Option_Value_With_Colon_Separator()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(configurator =>
                {
                    configurator.AddCommand<DogCommand>("dog");
                });

                // When
                var result = app.Run("dog", $"--foo:");

                // Then
                return Verifier.Verify(result.Output);
            }

            [Fact]
            [Expectation("Test_4")]
            public Task Should_Return_Correct_Text_For_Missing_Short_Option_Value_With_Equality_Separator()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(configurator =>
                {
                    configurator.AddCommand<DogCommand>("dog");
                });

                // When
                var result = app.Run("dog", $"-f=");

                // Then
                return Verifier.Verify(result.Output);
            }

            [Fact]
            [Expectation("Test_5")]
            public Task Should_Return_Correct_Text_For_Missing_Short_Option_Value_With_Colon_Separator()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(configurator =>
                {
                    configurator.AddCommand<DogCommand>("dog");
                });

                // When
                var result = app.Run("dog", $"-f:");

                // Then
                return Verifier.Verify(result.Output);
            }
        }

        [UsesVerify]
        [ExpectationPath("InvalidShortOptionName")]
        public sealed class InvalidShortOptionName
        {
            [Fact]
            [Expectation("Test_1")]
            public Task Should_Return_Correct_Text()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(configurator =>
                {
                    configurator.AddCommand<DogCommand>("dog");
                });

                // When
                var result = app.Run("dog", $"-f0o");

                // Then
                return Verifier.Verify(result.Output);
            }
        }

        [UsesVerify]
        [ExpectationPath("LongOptionNameIsOneCharacter")]
        public sealed class LongOptionNameIsOneCharacter
        {
            [Fact]
            [Expectation("Test_1")]
            public Task Should_Return_Correct_Text()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(configurator =>
                {
                    configurator.AddCommand<DogCommand>("dog");
                });

                // When
                var result = app.Run("dog", $"--f");

                // Then
                return Verifier.Verify(result.Output);
            }
        }

        [UsesVerify]
        [ExpectationPath("LongOptionNameIsMissing")]
        public sealed class LongOptionNameIsMissing
        {
            [Fact]
            [Expectation("Test_1")]
            public Task Should_Return_Correct_Text()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(configurator =>
                {
                    configurator.AddCommand<DogCommand>("dog");
                });

                // When
                var result = app.Run("dog", $"-- ");

                // Then
                return Verifier.Verify(result.Output);
            }
        }

        [UsesVerify]
        [ExpectationPath("LongOptionNameStartWithDigit")]
        public sealed class LongOptionNameStartWithDigit
        {
            [Fact]
            [Expectation("Test_1")]
            public Task Should_Return_Correct_Text()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(configurator =>
                {
                    configurator.AddCommand<DogCommand>("dog");
                });

                // When
                var result = app.Run("dog", $"--1foo");

                // Then
                return Verifier.Verify(result.Output);
            }
        }

        [UsesVerify]
        [ExpectationPath("LongOptionNameContainSymbol")]
        public sealed class LongOptionNameContainSymbol
        {
            [Fact]
            [Expectation("Test_1")]
            public Task Should_Return_Correct_Text()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(configurator =>
                {
                    configurator.AddCommand<DogCommand>("dog");
                });

                // When
                var result = app.Run("dog", $"--f€oo");

                // Then
                return Verifier.Verify(result.Output);
            }

            [Theory]
            [InlineData("--f-oo")]
            [InlineData("--f-o-o")]
            [InlineData("--f_oo")]
            [InlineData("--f_o_o")]
            public void Should_Allow_Special_Symbols_In_Name(string option)
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(configurator =>
                {
                    configurator.AddCommand<DogCommand>("dog");
                });

                // When
                var result = app.Run("dog", option);

                // Then
                result.Output.ShouldBe("Error: Command 'dog' is missing required argument 'AGE'.");
            }
        }
    }
}
