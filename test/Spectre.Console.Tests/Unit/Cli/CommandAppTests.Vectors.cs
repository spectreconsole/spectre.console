using Shouldly;
using Spectre.Console.Testing;
using Spectre.Console.Tests.Data;
using Xunit;
using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Unit.Cli
{
    public sealed partial class CommandAppTests
    {
        public sealed class Vectors
        {
            [Fact]
            public void Should_Throw_If_A_Single_Command_Has_Multiple_Argument_Vectors()
            {
                // Given
                var app = new CommandApp();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddCommand<GenericCommand<MultipleArgumentVectorSettings>>("multi");
                });

                // When
                var result = Record.Exception(() => app.Run(new[] { "multi", "a", "b", "c" }));

                // Then
                result.ShouldBeOfType<CommandConfigurationException>().And(ex =>
                {
                    ex.Message.ShouldBe("The command 'multi' specifies more than one vector argument.");
                });
            }

            [Fact]
            public void Should_Throw_If_An_Argument_Vector_Is_Not_Specified_Last()
            {
                // Given
                var app = new CommandApp();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddCommand<GenericCommand<MultipleArgumentVectorSpecifiedFirstSettings>>("multi");
                });

                // When
                var result = Record.Exception(() => app.Run(new[] { "multi", "a", "b", "c" }));

                // Then
                result.ShouldBeOfType<CommandConfigurationException>().And(ex =>
                {
                    ex.Message.ShouldBe("The command 'multi' specifies an argument vector that is not the last argument.");
                });
            }

            [Fact]
            public void Should_Assign_Values_To_Argument_Vector()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddCommand<GenericCommand<ArgumentVectorSettings>>("multi");
                });

                // When
                var result = app.Run(new[]
                {
                    "multi", "a", "b", "c",
                });

                // Then
                result.ExitCode.ShouldBe(0);
                result.Settings.ShouldBeOfType<ArgumentVectorSettings>().And(vec =>
                {
                    vec.Foo.Length.ShouldBe(3);
                    vec.Foo[0].ShouldBe("a");
                    vec.Foo[1].ShouldBe("b");
                    vec.Foo[2].ShouldBe("c");
                });
            }

            [Fact]
            public void Should_Assign_Values_To_Option_Vector()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddCommand<OptionVectorCommand>("cmd");
                });

                // When
                var result = app.Run(new[]
                {
                    "cmd", "--foo", "red",
                    "--bar", "4", "--foo", "blue",
                });

                // Then
                result.ExitCode.ShouldBe(0);
                result.Settings.ShouldBeOfType<OptionVectorSettings>().And(vec =>
                {
                    vec.Foo.ShouldBe(new string[] { "red", "blue" });
                    vec.Bar.ShouldBe(new int[] { 4 });
                });
            }
        }
    }
}
