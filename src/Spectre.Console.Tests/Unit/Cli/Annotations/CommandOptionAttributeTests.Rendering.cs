using System.Threading.Tasks;
using Shouldly;
using Spectre.Console.Cli;
using Spectre.Console.Testing;
using Spectre.Console.Tests.Data;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit.Cli.Annotations
{
    public sealed partial class CommandOptionAttributeTests
    {
        [UsesVerify]
        public sealed class TheUnexpectedCharacterMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("<FOO> $ <BAR>")]
                public string Foo { get; set; }
            }

            [Fact]
            public Task Should_Return_Correct_Text()
            {
                // Given, When
                var (message, result) = Fixture.Run<Settings>();

                // Then
                message.ShouldBe("Encountered unexpected character '$'.");
                return Verifier.Verify(result);
            }
        }

        [UsesVerify]
        public sealed class TheUnterminatedValueNameMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("--foo|-f <BAR")]
                public string Foo { get; set; }
            }

            [Fact]
            public Task Should_Return_Correct_Text()
            {
                // Given, When
                var (message, result) = Fixture.Run<Settings>();

                // Then
                message.ShouldBe("Encountered unterminated value name 'BAR'.");
                return Verifier.Verify(result);
            }
        }

        [UsesVerify]
        public sealed class TheOptionsMustHaveNameMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("--foo|-")]
                public string Foo { get; set; }
            }

            [Fact]
            public Task Should_Return_Correct_Text()
            {
                // Given, When
                var (message, result) = Fixture.Run<Settings>();

                // Then
                message.ShouldBe("Options without name are not allowed.");
                return Verifier.Verify(result);
            }
        }

        [UsesVerify]
        public sealed class TheOptionNamesCannotStartWithDigitMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("--1foo")]
                public string Foo { get; set; }
            }

            [Fact]
            public Task Should_Return_Correct_Text()
            {
                // Given, When
                var (message, result) = Fixture.Run<Settings>();

                // Then
                message.ShouldBe("Option names cannot start with a digit.");
                return Verifier.Verify(result);
            }
        }

        [UsesVerify]
        public sealed class TheInvalidCharacterInOptionNameMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("--f$oo")]
                public string Foo { get; set; }
            }

            [Fact]
            public Task Should_Return_Correct_Text()
            {
                // Given, When
                var (message, result) = Fixture.Run<Settings>();

                // Then
                message.ShouldBe("Encountered invalid character '$' in option name.");
                return Verifier.Verify(result);
            }
        }

        [UsesVerify]
        public sealed class TheLongOptionMustHaveMoreThanOneCharacterMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("--f")]
                public string Foo { get; set; }
            }

            [Fact]
            public Task Should_Return_Correct_Text()
            {
                // Given, When
                var (message, result) = Fixture.Run<Settings>();

                // Then
                message.ShouldBe("Long option names must consist of more than one character.");
                return Verifier.Verify(result);
            }
        }

        [UsesVerify]
        public sealed class TheShortOptionMustOnlyBeOneCharacterMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("--foo|-bar")]
                public string Foo { get; set; }
            }

            [Fact]
            public Task Should_Return_Correct_Text()
            {
                // Given, When
                var (message, result) = Fixture.Run<Settings>();

                // Then
                message.ShouldBe("Short option names can not be longer than one character.");
                return Verifier.Verify(result);
            }
        }

        [UsesVerify]
        public sealed class TheMultipleOptionValuesAreNotSupportedMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("-f|--foo <FOO> <BAR>")]
                public string Foo { get; set; }
            }

            [Fact]
            public Task Should_Return_Correct_Text()
            {
                // Given, When
                var (message, result) = Fixture.Run<Settings>();

                // Then
                message.ShouldBe("Multiple option values are not supported.");
                return Verifier.Verify(result);
            }
        }

        [UsesVerify]
        public sealed class TheInvalidCharacterInValueNameMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("-f|--foo <F$OO>")]
                public string Foo { get; set; }
            }

            [Fact]
            public Task Should_Return_Correct_Text()
            {
                // Given, When
                var (message, result) = Fixture.Run<Settings>();

                // Then
                message.ShouldBe("Encountered invalid character '$' in value name.");
                return Verifier.Verify(result);
            }
        }

        [UsesVerify]
        public sealed class TheMissingLongAndShortNameMethod
        {
            public sealed class Settings : CommandSettings
            {
                [CommandOption("<FOO>")]
                public string Foo { get; set; }
            }

            [Fact]
            public Task Should_Return_Correct_Text()
            {
                // Given, When
                var (message, result) = Fixture.Run<Settings>();

                // Then
                message.ShouldBe("No long or short name for option has been specified.");
                return Verifier.Verify(result);
            }
        }

        private static class Fixture
        {
            public static (string Message, string Output) Run<TSettings>(params string[] args)
                where TSettings : CommandSettings
            {
                var app = new CommandAppFixture();
                app.Configure(c =>
                {
                    c.PropagateExceptions();
                    c.AddCommand<GenericCommand<TSettings>>("foo");
                });

                return app.RunAndCatch<CommandTemplateException>(args);
            }
        }
    }
}
