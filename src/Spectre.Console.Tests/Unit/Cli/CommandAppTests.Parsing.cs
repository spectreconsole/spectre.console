using System;
using System.Threading.Tasks;
using Shouldly;
using Spectre.Console.Cli;
using Spectre.Console.Testing;
using Spectre.Console.Tests.Data;
using Spectre.Verify.Extensions;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit.Cli
{
    public sealed partial class CommandAppTests
    {
        [UsesVerify]
        [ExpectationPath("Cli/Parsing")]
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
                    var fixture = new Fixture();
                    fixture.Configure(config =>
                    {
                        config.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("cat", "14");

                    // Then
                    return Verifier.Verify(result);
                }

                [Fact]
                [Expectation("Test_2")]
                public Task Should_Return_Correct_Text_For_Unknown_Command_When_Current_Command_Has_No_Arguments()
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<EmptyCommand>("empty");
                    });

                    // When
                    var result = fixture.Run("empty", "other");

                    // Then
                    return Verifier.Verify(result);
                }

                [Fact]
                [Expectation("Test_3")]
                public Task Should_Return_Correct_Text_With_Suggestion_When_Command_Followed_By_Argument_Is_Unknown_And_Distance_Is_Small()
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(config =>
                    {
                        config.AddBranch<CommandSettings>("dog", a =>
                        {
                            a.AddCommand<CatCommand>("cat");
                        });
                    });

                    // When
                    var result = fixture.Run("dog", "bat", "14");

                    // Then
                    return Verifier.Verify(result);
                }

                [Fact]
                [Expectation("Test_4")]
                public Task Should_Return_Correct_Text_With_Suggestion_When_Root_Command_Followed_By_Argument_Is_Unknown_And_Distance_Is_Small()
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(config =>
                    {
                        config.AddCommand<CatCommand>("cat");
                    });

                    // When
                    var result = fixture.Run("bat", "14");

                    // Then
                    return Verifier.Verify(result);
                }

                [Fact]
                [Expectation("Test_5")]
                public Task Should_Return_Correct_Text_With_Suggestion_And_No_Arguments_When_Root_Command_Is_Unknown_And_Distance_Is_Small()
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.WithDefaultCommand<GenericCommand<EmptyCommandSettings>>();
                    fixture.Configure(config =>
                    {
                        config.AddCommand<GenericCommand<EmptyCommandSettings>>("cat");
                    });

                    // When
                    var result = fixture.Run("bat");

                    // Then
                    return Verifier.Verify(result);
                }

                [Fact]
                [Expectation("Test_6")]
                public Task Should_Return_Correct_Text_With_Suggestion_And_No_Arguments_When_Command_Is_Unknown_And_Distance_Is_Small()
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.WithDefaultCommand<GenericCommand<EmptyCommandSettings>>();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddBranch<CommandSettings>("dog", a =>
                        {
                            a.AddCommand<CatCommand>("cat");
                        });
                    });

                    // When
                    var result = fixture.Run("dog", "bat");

                    // Then
                    return Verifier.Verify(result);
                }

                [Fact]
                [Expectation("Test_7")]
                public Task Should_Return_Correct_Text_With_Suggestion_When_Root_Command_After_Argument_Is_Unknown_And_Distance_Is_Small()
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.WithDefaultCommand<GenericCommand<FooCommandSettings>>();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<GenericCommand<BarCommandSettings>>("bar");
                    });

                    // When
                    var result = fixture.Run("qux", "bat");

                    // Then
                    return Verifier.Verify(result);
                }

                [Fact]
                [Expectation("Test_8")]
                public Task Should_Return_Correct_Text_With_Suggestion_When_Command_After_Argument_Is_Unknown_And_Distance_Is_Small()
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddBranch<FooCommandSettings>("foo", a =>
                        {
                            a.AddCommand<GenericCommand<BarCommandSettings>>("bar");
                        });
                    });

                    // When
                    var result = fixture.Run("foo", "qux", "bat");

                    // Then
                    return Verifier.Verify(result);
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
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", "--alive", "foo");

                    // Then
                    return Verifier.Verify(result);
                }

                [Fact]
                [Expectation("Test_2")]
                public Task Should_Return_Correct_Text_For_Short_Option()
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", "-a", "foo");

                    // Then
                    return Verifier.Verify(result);
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
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", "--name");

                    // Then
                    return Verifier.Verify(result);
                }

                [Fact]
                [Expectation("Test_2")]
                public Task Should_Return_Correct_Text_For_Short_Option()
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", "-n");

                    // Then
                    return Verifier.Verify(result);
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
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<GiraffeCommand>("giraffe");
                    });

                    // When
                    var result = fixture.Run("giraffe", "foo", "bar", "baz");

                    // Then
                    return Verifier.Verify(result);
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
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("--foo");

                    // Then
                    return Verifier.Verify(result);
                }

                [Fact]
                [Expectation("Test_2")]
                public Task Should_Return_Correct_Text_For_Short_Option()
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("-f");

                    // Then
                    return Verifier.Verify(result);
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
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.UseStrictParsing();
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", "--unknown");

                    // Then
                    return Verifier.Verify(result);
                }

                [Fact]
                [Expectation("Test_2")]
                public Task Should_Return_Correct_Text_For_Short_Option_If_Strict_Mode_Is_Enabled()
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.UseStrictParsing();
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", "-u");

                    // Then
                    return Verifier.Verify(result);
                }
            }

            [UsesVerify]
            [ExpectationPath("UnterminatedQuote")]
            public sealed class UnterminatedQuote
            {
                [Fact]
                [Expectation("Test_1")]
                public Task Should_Return_Correct_Text()
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("--name", "\"Rufus");

                    // Then
                    return Verifier.Verify(result);
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
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", "-", " ");

                    // Then
                    return Verifier.Verify(result);
                }

                [Fact]
                [Expectation("Test_2")]
                public Task Should_Return_Correct_Text_For_Missing_Long_Option_Value_With_Equality_Separator()
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", $"--foo=");

                    // Then
                    return Verifier.Verify(result);
                }

                [Fact]
                [Expectation("Test_3")]
                public Task Should_Return_Correct_Text_For_Missing_Long_Option_Value_With_Colon_Separator()
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", $"--foo:");

                    // Then
                    return Verifier.Verify(result);
                }

                [Fact]
                [Expectation("Test_4")]
                public Task Should_Return_Correct_Text_For_Missing_Short_Option_Value_With_Equality_Separator()
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", $"-f=");

                    // Then
                    return Verifier.Verify(result);
                }

                [Fact]
                [Expectation("Test_5")]
                public Task Should_Return_Correct_Text_For_Missing_Short_Option_Value_With_Colon_Separator()
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", $"-f:");

                    // Then
                    return Verifier.Verify(result);
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
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", $"-f0o");

                    // Then
                    return Verifier.Verify(result);
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
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", $"--f");

                    // Then
                    return Verifier.Verify(result);
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
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", $"-- ");

                    // Then
                    return Verifier.Verify(result);
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
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", $"--1foo");

                    // Then
                    return Verifier.Verify(result);
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
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", $"--f€oo");

                    // Then
                    return Verifier.Verify(result);
                }

                [Theory]
                [InlineData("--f-oo")]
                [InlineData("--f-o-o")]
                [InlineData("--f_oo")]
                [InlineData("--f_o_o")]
                public void Should_Allow_Special_Symbols_In_Name(string option)
                {
                    // Given
                    var fixture = new Fixture();
                    fixture.Configure(configurator =>
                    {
                        configurator.AddCommand<DogCommand>("dog");
                    });

                    // When
                    var result = fixture.Run("dog", option);

                    // Then
                    result.ShouldBe("Error: Command 'dog' is missing required argument 'AGE'.");
                }
            }

            [Fact]
            [Expectation("Quoted_Strings")]
            public Task Should_Parse_Quoted_Strings_Correctly()
            {
                // Given
                var fixture = new Fixture();
                fixture.Configure(configurator =>
                {
                    configurator.AddCommand<DumpRemainingCommand>("foo");
                });

                // When
                var result = fixture.Run("foo", "--", "/c", "\"set && pause\"");

                // Then
                return Verifier.Verify(result);
            }
        }

        internal sealed class Fixture
        {
            private Action<CommandApp> _appConfiguration = _ => { };
            private Action<IConfigurator> _configuration;

            public void WithDefaultCommand<T>()
                where T : class, ICommand
            {
                _appConfiguration = (app) => app.SetDefaultCommand<T>();
            }

            public void Configure(Action<IConfigurator> action)
            {
                _configuration = action;
            }

            public string Run(params string[] args)
            {
                using (var console = new TestConsole())
                {
                    var app = new CommandApp();
                    _appConfiguration?.Invoke(app);

                    app.Configure(_configuration);
                    app.Configure(c => c.ConfigureConsole(console));
                    app.Run(args);

                    return console.Output
                        .NormalizeLineEndings()
                        .TrimLines()
                        .Trim();
                }
            }
        }
    }
}
