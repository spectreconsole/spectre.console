using Shouldly;
using Spectre.Console.Testing;
using Spectre.Console.Tests.Data;
using Xunit;
using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Unit.Cli
{
    public sealed partial class CommandAppTests
    {
        public sealed class TypeConverters
        {
            [Fact]
            public void Should_Bind_Using_Custom_Type_Converter_If_Specified()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddCommand<CatCommand>("cat");
                });

                // When
                var result = app.Run(new[]
                {
                     "cat", "--name", "Tiger",
                     "--agility", "FOOBAR",
                });

                // Then
                result.ExitCode.ShouldBe(0);
                result.Settings.ShouldBeOfType<CatSettings>().And(cat =>
                {
                    cat.Agility.ShouldBe(6);
                });
            }
        }
    }
}
