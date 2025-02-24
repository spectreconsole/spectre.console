namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    public sealed class Branches
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Run_The_Default_Command_On_Branch(bool strictParsing)
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.Settings.StrictParsing = strictParsing;
                config.PropagateExceptions();
                config.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.SetDefaultCommand<CatCommand>();
                });
            });

            // When
            var result = app.Run(new[]
            {
                "animal", "4",
            });

            // Then
            result.ExitCode.ShouldBe(0);
            result.Settings.ShouldBeOfType<CatSettings>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Throw_When_No_Default_Command_On_Branch(bool strictParsing)
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.Settings.StrictParsing = strictParsing;
                config.PropagateExceptions();
                config.AddBranch<AnimalSettings>("animal", animal => { });
            });

            // When
            var result = Record.Exception(() =>
            {
                app.Run(new[]
                {
                    "animal", "4",
                });
            });

            // Then
            result.ShouldBeOfType<CommandConfigurationException>().And(ex =>
            {
                ex.Message.ShouldBe("The branch 'animal' does not define any commands.");
            });
        }

        [Fact]
        public void Should_Parse_Branch_And_Default_Command_Arguments_Relaxed_Parsing()
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.PropagateExceptions();
                config.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.SetDefaultCommand<CatCommand>();
                });
            });

            // When
            var result = app.Run(new[]
            {
                // The CommandTreeParser should determine which command line arguments
                // belong to the branch and which belong to the branch's default command.
                "animal", "4", "-a", "false", "--name", "Kitty", "--agility", "four", "--nick-name", "Felix"
            });

            // Then
            result.ExitCode.ShouldBe(0);
            result.Settings.ShouldBeOfType<CatSettings>().And(cat =>
            {
                cat.IsAlive.ShouldBeFalse();
                cat.Legs.ShouldBe(4);
                cat.Name.ShouldBe("Kitty");
                cat.Agility.ShouldBe(4);
            });
            result.Context.Remaining.Parsed.Count.ShouldBe(1);
            result.Context.ShouldHaveRemainingArgument("--nick-name", values: new[] { "Felix" });
            result.Context.Remaining.Raw.Count.ShouldBe(0);
        }

        [Fact]
        public void Should_Parse_Branch_And_Default_Command_Arguments_Strict_Parsing()
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.UseStrictParsing();
                config.PropagateExceptions();
                config.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.SetDefaultCommand<CatCommand>();
                });
            });

            // When
            var result = app.Run(new[]
            {
                // The CommandTreeParser should determine which command line arguments
                // belong to the branch and which belong to the branch's default command.
                "animal", "4", "-a", "false", "--name", "Kitty", "--agility", "four", "--", "--nick-name", "Felix"
            });

            // Then
            result.ExitCode.ShouldBe(0);
            result.Settings.ShouldBeOfType<CatSettings>().And(cat =>
            {
                cat.IsAlive.ShouldBeFalse();
                cat.Legs.ShouldBe(4);
                cat.Name.ShouldBe("Kitty");
                cat.Agility.ShouldBe(4);
            });
            result.Context.Remaining.Parsed.Count.ShouldBe(1);
            result.Context.ShouldHaveRemainingArgument("--nick-name", values: new[] { "Felix" });
            result.Context.Remaining.Raw.Count.ShouldBe(2);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Run_The_Default_Command_On_Branch_On_Branch(bool strictParsing)
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.Settings.StrictParsing = strictParsing;
                config.PropagateExceptions();
                config.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.AddBranch<MammalSettings>("mammal", mammal =>
                    {
                        mammal.SetDefaultCommand<CatCommand>();
                    });
                });
            });

            // When
            var result = app.Run(new[]
            {
                "animal", "4", "mammal",
            });

            // Then
            result.ExitCode.ShouldBe(0);
            result.Settings.ShouldBeOfType<CatSettings>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Run_The_Default_Command_On_Branch_On_Branch_With_Arguments(bool strictParsing)
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.Settings.StrictParsing = strictParsing;
                config.PropagateExceptions();
                config.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.AddBranch<MammalSettings>("mammal", mammal =>
                    {
                        mammal.SetDefaultCommand<CatCommand>();
                    });
                });
            });

            // When
            var result = app.Run(new[]
            {
                "animal", "4", "mammal", "--name", "Kitty",
            });

            // Then
            result.ExitCode.ShouldBe(0);
            result.Settings.ShouldBeOfType<CatSettings>().And(cat =>
            {
                cat.Legs.ShouldBe(4);
                cat.Name.ShouldBe("Kitty");
            });
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Run_The_Default_Command_Not_The_Named_Command_On_Branch(bool strictParsing)
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.Settings.StrictParsing = strictParsing;
                config.PropagateExceptions();
                config.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.AddCommand<DogCommand>("dog");

                    animal.SetDefaultCommand<CatCommand>();
                });
            });

            // When
            var result = app.Run(new[]
            {
                "animal", "4",
            });

            // Then
            result.ExitCode.ShouldBe(0);
            result.Settings.ShouldBeOfType<CatSettings>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Run_The_Named_Command_Not_The_Default_Command_On_Branch(bool strictParsing)
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.Settings.StrictParsing = strictParsing;
                config.PropagateExceptions();
                config.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.AddCommand<DogCommand>("dog");

                    animal.SetDefaultCommand<LionCommand>();
                });
            });

            // When
            var result = app.Run(new[]
            {
                "animal", "4", "dog", "12", "--good-boy", "--name", "Rufus",
            });

            // Then
            result.ExitCode.ShouldBe(0);
            result.Settings.ShouldBeOfType<DogSettings>().And(dog =>
            {
                dog.Legs.ShouldBe(4);
                dog.Age.ShouldBe(12);
                dog.GoodBoy.ShouldBe(true);
                dog.Name.ShouldBe("Rufus");
            });
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Allow_Multiple_Branches_Multiple_Commands(bool strictParsing)
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.Settings.StrictParsing = strictParsing;
                config.PropagateExceptions();
                config.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.AddBranch<MammalSettings>("mammal", mammal =>
                    {
                        mammal.AddCommand<DogCommand>("dog");
                        mammal.AddCommand<CatCommand>("cat");
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Allow_Single_Branch_Multiple_Commands(bool strictParsing)
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.Settings.StrictParsing = strictParsing;
                config.PropagateExceptions();
                config.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.AddCommand<DogCommand>("dog");
                    animal.AddCommand<CatCommand>("cat");
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Allow_Single_Branch_Single_Command(bool strictParsing)
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.Settings.StrictParsing = strictParsing;
                config.PropagateExceptions();
                config.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.AddCommand<DogCommand>("dog");
                });
            });

            // When
            var result = app.Run(new[]
            {
                "animal", "4", "dog", "12", "--good-boy",
                "--name", "Rufus",
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
    }
}