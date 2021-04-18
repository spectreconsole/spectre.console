using Shouldly;
using Spectre.Console.Testing;
using Spectre.Console.Tests.Data;
using Spectre.Console.Cli.Unsafe;
using Xunit;
using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Unit.Cli
{
    public sealed partial class CommandAppTests
    {
        public sealed class SafetyOff
        {
            [Fact]
            public void Can_Mix_Safe_And_Unsafe_Configurators()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(config =>
                {
                    config.PropagateExceptions();

                    config.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.SafetyOff().AddBranch("mammal", typeof(MammalSettings), mammal =>
                        {
                            mammal.AddCommand("dog", typeof(DogCommand));
                            mammal.AddCommand("horse", typeof(HorseCommand));
                        });
                    });
                });

                // When
                var result = app.Run(new[]
                {
                    "animal", "--alive", "mammal", "--name",
                    "Rufus", "dog", "12", "--good-boy",
                });

                // Then
                result.ExitCode.ShouldBe(0);
                result.Settings.ShouldBeOfType<DogSettings>().And(dog =>
                {
                    dog.Age.ShouldBe(12);
                    dog.GoodBoy.ShouldBe(true);
                    dog.Name.ShouldBe("Rufus");
                    dog.IsAlive.ShouldBe(true);
                });
            }

            [Fact]
            public void Can_Turn_Safety_On_After_Turning_It_Off_For_Branch()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(config =>
                {
                    config.PropagateExceptions();

                    config.SafetyOff().AddBranch("animal", typeof(AnimalSettings), animal =>
                    {
                        animal.SafetyOn<AnimalSettings>()
                            .AddBranch<MammalSettings>("mammal", mammal =>
                        {
                            mammal.SafetyOff().AddCommand("dog", typeof(DogCommand));
                            mammal.AddCommand<HorseCommand>("horse");
                        });
                    });
                });

                // When
                var result = app.Run(new[]
                {
                    "animal", "--alive", "mammal", "--name",
                    "Rufus", "dog", "12", "--good-boy",
                });

                // Then
                result.ExitCode.ShouldBe(0);
                result.Settings.ShouldBeOfType<DogSettings>().And(dog =>
                {
                    dog.Age.ShouldBe(12);
                    dog.GoodBoy.ShouldBe(true);
                    dog.Name.ShouldBe("Rufus");
                    dog.IsAlive.ShouldBe(true);
                });
            }

            [Fact]
            public void Should_Throw_If_Trying_To_Convert_Unsafe_Branch_Configurator_To_Safe_Version_With_Wrong_Type()
            {
                // Given
                var app = new CommandApp();

                // When
                var result = Record.Exception(() => app.Configure(config =>
                {
                    config.PropagateExceptions();

                    config.SafetyOff().AddBranch("animal", typeof(AnimalSettings), animal =>
                    {
                        animal.SafetyOn<MammalSettings>().AddCommand<DogCommand>("dog");
                    });
                }));

                // Then
                result.ShouldBeOfType<CommandConfigurationException>();
                result.Message.ShouldBe("Configurator cannot be converted to a safe configurator of type 'MammalSettings'.");
            }

            [Fact]
            public void Should_Pass_Case_1()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(config =>
                {
                    config.PropagateExceptions();

                    config.SafetyOff().AddBranch("animal", typeof(AnimalSettings), animal =>
                    {
                        animal.AddBranch("mammal", typeof(MammalSettings), mammal =>
                        {
                            mammal.AddCommand("dog", typeof(DogCommand));
                            mammal.AddCommand("horse", typeof(HorseCommand));
                        });
                    });
                });

                // When
                var result = app.Run(new[]
                {
                    "animal", "--alive", "mammal", "--name",
                    "Rufus", "dog", "12", "--good-boy",
                });

                // Then
                result.ExitCode.ShouldBe(0);
                result.Settings.ShouldBeOfType<DogSettings>().And(dog =>
                {
                    dog.Age.ShouldBe(12);
                    dog.GoodBoy.ShouldBe(true);
                    dog.Name.ShouldBe("Rufus");
                    dog.IsAlive.ShouldBe(true);
                });
            }

            [Fact]
            public void Should_Pass_Case_2()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.SafetyOff().AddCommand("dog", typeof(DogCommand));
                });

                // When
                var result = app.Run(new[]
                {
                    "dog", "12", "4", "--good-boy",
                    "--name", "Rufus", "--alive",
                });

                // Then
                result.ExitCode.ShouldBe(0);
                result.Settings.ShouldBeOfType<DogSettings>().And(dog =>
                {
                    dog.Legs.ShouldBe(12);
                    dog.Age.ShouldBe(4);
                    dog.GoodBoy.ShouldBe(true);
                    dog.Name.ShouldBe("Rufus");
                    dog.IsAlive.ShouldBe(true);
                });
            }

            [Fact]
            public void Should_Pass_Case_3()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.SafetyOff().AddBranch("animal", typeof(AnimalSettings), animal =>
                    {
                        animal.AddCommand("dog", typeof(DogCommand));
                        animal.AddCommand("horse", typeof(HorseCommand));
                    });
                });

                // When
                var result = app.Run(new[]
                {
                    "animal", "dog", "12", "--good-boy",
                    "--name", "Rufus",
                });

                // Then
                result.ExitCode.ShouldBe(0);
                result.Settings.ShouldBeOfType<DogSettings>().And(dog =>
                {
                    dog.Age.ShouldBe(12);
                    dog.GoodBoy.ShouldBe(true);
                    dog.Name.ShouldBe("Rufus");
                    dog.IsAlive.ShouldBe(false);
                });
            }

            [Fact]
            public void Should_Pass_Case_4()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.SafetyOff().AddBranch("animal", typeof(AnimalSettings), animal =>
                    {
                        animal.AddCommand("dog", typeof(DogCommand));
                    });
                });

                // When
                var result = app.Run(new[]
                {
                    "animal", "4", "dog", "12",
                    "--good-boy", "--name", "Rufus",
                });

                // Then
                result.ExitCode.ShouldBe(0);
                result.Settings.ShouldBeOfType<DogSettings>().And(dog =>
                {
                    dog.Legs.ShouldBe(4);
                    dog.Age.ShouldBe(12);
                    dog.GoodBoy.ShouldBe(true);
                    dog.IsAlive.ShouldBe(false);
                    dog.Name.ShouldBe("Rufus");
                });
            }

            [Fact]
            public void Should_Pass_Case_5()
            {
                // Given
                var app = new CommandAppTester();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.SafetyOff().AddCommand("multi", typeof(OptionVectorCommand));
                });

                // When
                var result = app.Run(new[]
                {
                    "multi", "--foo", "a", "--foo", "b", "--bar", "1", "--foo", "c", "--bar", "2",
                });

                // Then
                result.ExitCode.ShouldBe(0);
                result.Settings.ShouldBeOfType<OptionVectorSettings>().And(vec =>
                {
                    vec.Foo.Length.ShouldBe(3);
                    vec.Foo.ShouldBe(new[] { "a", "b", "c" });
                    vec.Bar.Length.ShouldBe(2);
                    vec.Bar.ShouldBe(new[] { 1, 2 });
                });
            }

            [Fact]
            public void Should_Pass_Case_6()
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
                    vec.Foo.ShouldBe(new[] { "a", "b", "c" });
                });
            }
        }
    }
}
