using Shouldly;
using Spectre.Console.Cli;
using Spectre.Console.Tests.Data;
using Xunit;

namespace Spectre.Console.Tests.Unit.Cli
{
    public sealed partial class CommandAppTests
    {
        public sealed class Validation
        {
            [Fact]
            public void Should_Throw_If_Attribute_Validation_Fails()
            {
                // Given
                var app = new CommandApp();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.AddCommand<DogCommand>("dog");
                        animal.AddCommand<HorseCommand>("horse");
                    });
                });

                // When
                var result = Record.Exception(() => app.Run(new[] { "animal", "3", "dog", "7", "--name", "Rufus" }));

                // Then
                result.ShouldBeOfType<CommandRuntimeException>().And(e =>
                {
                    e.Message.ShouldBe("Animals must have an even number of legs.");
                });
            }

            [Fact]
            public void Should_Throw_If_Settings_Validation_Fails()
            {
                // Given
                var app = new CommandApp();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.AddCommand<DogCommand>("dog");
                        animal.AddCommand<HorseCommand>("horse");
                    });
                });

                // When
                var result = Record.Exception(() => app.Run(new[] { "animal", "4", "dog", "7", "--name", "Tiger" }));

                // Then
                result.ShouldBeOfType<CommandRuntimeException>().And(e =>
                {
                    e.Message.ShouldBe("Tiger is not a dog name!");
                });
            }

            [Fact]
            public void Should_Throw_If_Command_Validation_Fails()
            {
                // Given
                var app = new CommandApp();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.AddCommand<DogCommand>("dog");
                        animal.AddCommand<HorseCommand>("horse");
                    });
                });

                // When
                var result = Record.Exception(() => app.Run(new[] { "animal", "4", "dog", "101", "--name", "Rufus" }));

                // Then
                result.ShouldBeOfType<CommandRuntimeException>().And(e =>
                {
                    e.Message.ShouldBe("Dog is too old...");
                });
            }
        }
    }
}
