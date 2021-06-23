using Shouldly;
using Spectre.Console.Testing;
using Spectre.Console.Tests.Data;
using Xunit;

namespace Spectre.Console.Tests.Unit.Cli
{
    public sealed partial class CommandAppTests
    {
        public sealed class Version
        {
            [Fact]
            public void Should_Output_The_Version_To_The_Console()
            {
                // Given
                var fixture = new CommandAppTester();
                fixture.Configure(config =>
                {
                    config.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.AddBranch<MammalSettings>("mammal", mammal =>
                        {
                            mammal.AddCommand<DogCommand>("dog");
                            mammal.AddCommand<HorseCommand>("horse");
                        });
                    });
                });

                // When
                var result = fixture.Run(Constants.VersionCommand);

                // Then
                result.Output.ShouldStartWith("Spectre.Cli version ");
            }
        }
    }
}
