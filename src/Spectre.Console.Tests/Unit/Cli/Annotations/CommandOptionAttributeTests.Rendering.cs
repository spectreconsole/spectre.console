using System.Threading.Tasks;
using Shouldly;
using Spectre.Console.Cli;
using Spectre.Console.Testing;
using Spectre.Console.Tests.Data;
using Spectre.Verify.Extensions;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit.Cli.Annotations
{
    [ExpectationPath("Cli/Arguments")]
    public sealed partial class CommandOptionAttributeTests
    {
        [UsesVerify]
        public sealed class UnexpectedCharacter
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("<FOO> $ <BAR>")]
                public string Foo { get; set; }
            }

            [Fact]
            [Expectation("UnexpectedCharacter")]
            public Task Should_Return_Correct_Text()
            {
                // Given, When
                var result = Fixture.Run<Settings>();

                // Then
                result.Exception.Message.ShouldBe("Encountered unexpected character '$'.");
                return Verifier.Verify(result.Output);
            }
        }

        [UsesVerify]
        public sealed class UnterminatedValueName
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("--foo|-f <BAR")]
                public string Foo { get; set; }
            }

            [Fact]
            [Expectation("UnterminatedValueName")]
            public Task Should_Return_Correct_Text()
            {
                // Given, When
                var result = Fixture.Run<Settings>();

                // Then
                result.Exception.Message.ShouldBe("Encountered unterminated value name 'BAR'.");
                return Verifier.Verify(result.Output);
            }
        }

        [UsesVerify]
        public sealed class OptionsMustHaveName
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("--foo|-")]
                public string Foo { get; set; }
            }

            [Fact]
            [Expectation("OptionsMustHaveName")]
            public Task Should_Return_Correct_Text()
            {
                // Given, When
                var result = Fixture.Run<Settings>();

                // Then
                result.Exception.Message.ShouldBe("Options without name are not allowed.");
                return Verifier.Verify(result.Output);
            }
        }

        [UsesVerify]
        public sealed class OptionNamesCannotStartWithDigit
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("--1foo")]
                public string Foo { get; set; }
            }

            [Fact]
            [Expectation("OptionNamesCannotStartWithDigit")]
            public Task Should_Return_Correct_Text()
            {
                // Given, When
                var result = Fixture.Run<Settings>();

                // Then
                result.Exception.Message.ShouldBe("Option names cannot start with a digit.");
                return Verifier.Verify(result.Output);
            }
        }

        [UsesVerify]
        public sealed class InvalidCharacterInOptionName
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("--f$oo")]
                public string Foo { get; set; }
            }

            [Fact]
            [Expectation("InvalidCharacterInOptionName")]
            public Task Should_Return_Correct_Text()
            {
                // Given, When
                var result = Fixture.Run<Settings>();

                // Then
                result.Exception.Message.ShouldBe("Encountered invalid character '$' in option name.");
                return Verifier.Verify(result.Output);
            }
        }

        [UsesVerify]
        public sealed class LongOptionMustHaveMoreThanOneCharacter
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("--f")]
                public string Foo { get; set; }
            }

            [Fact]
            [Expectation("LongOptionMustHaveMoreThanOneCharacter")]
            public Task Should_Return_Correct_Text()
            {
                // Given, When
                var result = Fixture.Run<Settings>();

                // Then
                result.Exception.Message.ShouldBe("Long option names must consist of more than one character.");
                return Verifier.Verify(result.Output);
            }
        }

        [UsesVerify]
        public sealed class ShortOptionMustOnlyBeOneCharacter
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("--foo|-bar")]
                public string Foo { get; set; }
            }

            [Fact]
            [Expectation("ShortOptionMustOnlyBeOneCharacter")]
            public Task Should_Return_Correct_Text()
            {
                // Given, When
                var result = Fixture.Run<Settings>();

                // Then
                result.Exception.Message.ShouldBe("Short option names can not be longer than one character.");
                return Verifier.Verify(result.Output);
            }
        }

        [UsesVerify]
        public sealed class MultipleOptionValuesAreNotSupported
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("-f|--foo <FOO> <BAR>")]
                public string Foo { get; set; }
            }

            [Fact]
            [Expectation("MultipleOptionValuesAreNotSupported")]
            public Task Should_Return_Correct_Text()
            {
                // Given, When
                var result = Fixture.Run<Settings>();

                // Then
                result.Exception.Message.ShouldBe("Multiple option values are not supported.");
                return Verifier.Verify(result.Output);
            }
        }

        [UsesVerify]
        public sealed class InvalidCharacterInValueName
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("-f|--foo <F$OO>")]
                public string Foo { get; set; }
            }

            [Fact]
            [Expectation("InvalidCharacterInValueName")]
            public Task Should_Return_Correct_Text()
            {
                // Given, When
                var result = Fixture.Run<Settings>();

                // Then
                result.Exception.Message.ShouldBe("Encountered invalid character '$' in value name.");
                return Verifier.Verify(result.Output);
            }
        }

        [UsesVerify]
        public sealed class MissingLongAndShortName
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("<FOO>")]
                public string Foo { get; set; }
            }

            [Fact]
            [Expectation("MissingLongAndShortName")]
            public Task Should_Return_Correct_Text()
            {
                // Given, When
                var result = Fixture.Run<Settings>();

                // Then
                result.Exception.Message.ShouldBe("No long or short name for option has been specified.");
                return Verifier.Verify(result.Output);
            }
        }

        private static class Fixture
        {
            public static CommandAppFailure Run<TSettings>(params string[] args)
                where TSettings : CommandSettings
            {
                var app = new CommandAppTester();
                app.Configure(c =>
                {
                    c.AddCommand<GenericCommand<TSettings>>("foo");
                });

                return app.RunAndCatch<CommandTemplateException>(args);
            }
        }
    }
}
