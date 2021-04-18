using Shouldly;
using Spectre.Console.Testing;
using Spectre.Console.Tests.Data;
using Xunit;
using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Unit.Cli
{
    public sealed partial class CommandApptests
    {
        [Fact]
        public void Should_Treat_Commands_As_Case_Sensitive_If_Specified()
        {
            // Given
            var app = new CommandApp();
            app.Configure(config =>
            {
                config.UseStrictParsing();
                config.PropagateExceptions();
                config.CaseSensitivity(CaseSensitivity.Commands);
                config.AddCommand<GenericCommand<StringOptionSettings>>("command");
            });

            // When
            var result = Record.Exception(() => app.Run(new[]
            {
                "Command", "--foo", "bar",
            }));

            // Then
            result.ShouldNotBeNull();
            result.ShouldBeOfType<CommandParseException>().And(ex =>
            {
                ex.Message.ShouldBe("Unknown command 'Command'.");
            });
        }

        [Fact]
        public void Should_Treat_Long_Options_As_Case_Sensitive_If_Specified()
        {
            // Given
            var app = new CommandApp();
            app.Configure(config =>
            {
                config.UseStrictParsing();
                config.PropagateExceptions();
                config.CaseSensitivity(CaseSensitivity.LongOptions);
                config.AddCommand<GenericCommand<StringOptionSettings>>("command");
            });

            // When
            var result = Record.Exception(() => app.Run(new[]
            {
                "command", "--Foo", "bar",
            }));

            // Then
            result.ShouldNotBeNull();
            result.ShouldBeOfType<CommandParseException>().And(ex =>
            {
                ex.Message.ShouldBe("Unknown option 'Foo'.");
            });
        }

        [Fact]
        public void Should_Treat_Short_Options_As_Case_Sensitive()
        {
            // Given
            var app = new CommandApp();
            app.Configure(config =>
            {
                config.UseStrictParsing();
                config.PropagateExceptions();
                config.AddCommand<GenericCommand<StringOptionSettings>>("command");
            });

            // When
            var result = Record.Exception(() => app.Run(new[]
            {
                "command", "-F", "bar",
            }));

            // Then
            result.ShouldNotBeNull();
            result.ShouldBeOfType<CommandParseException>().And(ex =>
            {
                ex.Message.ShouldBe("Unknown option 'F'.");
            });
        }

        [Fact]
        public void Should_Suppress_Case_Sensitivity_If_Specified()
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.UseStrictParsing();
                config.PropagateExceptions();
                config.CaseSensitivity(CaseSensitivity.None);
                config.AddCommand<GenericCommand<StringOptionSettings>>("command");
            });

            // When
            var result = app.Run(new[]
            {
                "Command", "--Foo", "bar",
            });

            // Then
            result.ExitCode.ShouldBe(0);
            result.Settings.ShouldBeOfType<StringOptionSettings>().And(vec =>
            {
                vec.Foo.ShouldBe("bar");
            });
        }
    }
}
