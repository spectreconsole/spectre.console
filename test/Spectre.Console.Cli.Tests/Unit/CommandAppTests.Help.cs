using Spectre.Console.Cli.Tests.Data.Help;

namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    [UsesVerify]
    [ExpectationPath("Help")]
    public class Help
    {
        [Fact]
        [Expectation("Root")]
        public Task Should_Output_Root_Correctly()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
                configurator.AddCommand<DogCommand>("dog");
                configurator.AddCommand<HorseCommand>("horse");
                configurator.AddCommand<GiraffeCommand>("giraffe");
            });

            // When
            var result = fixture.Run("--help");

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Hidden_Commands")]
        public Task Should_Skip_Hidden_Commands()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
                configurator.AddCommand<DogCommand>("dog");
                configurator.AddCommand<HorseCommand>("horse");
                configurator.AddCommand<GiraffeCommand>("giraffe")
                    .WithExample("giraffe", "123")
                    .IsHidden();
            });

            // When
            var result = fixture.Run("--help");

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Description_No_Trailing_Period")]
        public Task Should_Not_Trim_Description_Trailing_Period()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
                configurator.AddCommand<DogCommand>("dog");
                configurator.AddCommand<HorseCommand>("horse");
                configurator.AddCommand<GiraffeCommand>("giraffe")
                    .WithExample("giraffe", "123")
                    .IsHidden();
                configurator.TrimTrailingPeriods(false);
            });

            // When
            var result = fixture.Run("--help");

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Branch")]
        public Task Should_Output_Branch_Correctly()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
                configurator.AddBranch<CatSettings>("cat", animal =>
                {
                    animal.SetDescription("Contains settings for a cat.");
                    animal.AddCommand<LionCommand>("lion");
                });
            });

            // When
            var result = fixture.Run("cat", "--help");

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Branch_Called_Without_Help")]
        public Task Should_Output_Branch_When_Called_Without_Help_Option()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
                configurator.AddBranch<CatSettings>("cat", animal =>
                {
                    animal.SetDescription("Contains settings for a cat.");
                    animal.AddCommand<LionCommand>("lion");
                });
            });

            // When
            var result = fixture.Run("cat");

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Branch_Default_Greeter")]
        public Task Should_Output_Branch_With_Default_Correctly()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
                configurator.AddBranch<OptionalArgumentWithDefaultValueSettings>("branch", animal =>
                {
                    animal.SetDefaultCommand<GreeterCommand>();
                    animal.AddCommand<GreeterCommand>("greeter");
                });
            });

            // When
            var result = fixture.Run("branch", "--help");

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Command_Hide_Default")]
        public Task Should_Not_Print_Default_Column()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
                configurator.AddBranch<CatSettings>("cat", animal =>
                {
                    animal.SetDescription("Contains settings for a cat.");
                    animal.AddCommand<LionCommand>("lion");
                });
                configurator.HideOptionDefaultValues();
            });

            // When
            var result = fixture.Run("cat", "--help");

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Leaf")]
        public Task Should_Output_Leaf_Correctly()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
                configurator.AddBranch<CatSettings>("cat", animal =>
                {
                    animal.SetDescription("Contains settings for a cat.");
                    animal.AddCommand<LionCommand>("lion");
                });
            });

            // When
            var result = fixture.Run("cat", "lion", "--help");

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Default")]
        public Task Should_Output_Default_Command_Correctly()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.SetDefaultCommand<LionCommand>();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
            });

            // When
            var result = fixture.Run("--help");

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Default_Without_Args")]
        public Task Should_Output_Default_Command_When_Command_Has_Required_Parameters_And_Is_Called_Without_Args()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.SetDefaultCommand<LionCommand>();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
            });

            // When
            var result = fixture.Run();

            // Then
            return Verifier.Verify(result.Output);
        }

        [Theory]
        [InlineData(null, "EN")]
        [InlineData("", "EN")]
        [InlineData("en", "EN")]
        [InlineData("en-EN", "EN")]
        [InlineData("fr", "FR")]
        [InlineData("fr-FR", "FR")]
        [InlineData("sv", "SV")]
        [InlineData("sv-SE", "SV")]
        [InlineData("de", "DE")]
        [InlineData("de-DE", "DE")]
        [Expectation("Default_Without_Args_Additional")]
        public Task Should_Output_Default_Command_And_Additional_Commands_When_Default_Command_Has_Required_Parameters_And_Is_Called_Without_Args_Localised(string culture, string expectationPrefix)
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.SetDefaultCommand<LionCommand>();
            fixture.Configure(configurator =>
            {
                configurator.AddExample("20", "--alive");
                configurator.SetApplicationCulture(string.IsNullOrEmpty(culture) ? null : new CultureInfo(culture));
                configurator.SetApplicationName("myapp");
                configurator.AddCommand<GiraffeCommand>("giraffe");
            });

            // When
            var result = fixture.Run();

            // Then
            var settings = new VerifySettings();
            settings.DisableRequireUniquePrefix();
            return Verifier.Verify(result.Output, settings).UseTextForParameters(expectationPrefix);
        }

        [Fact]
        [Expectation("Default_Greeter")]
        public Task Should_Not_Output_Default_Command_When_Command_Has_No_Required_Parameters_And_Is_Called_Without_Args()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.SetDefaultCommand<GreeterCommand>();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
            });

            // When
            var result = fixture.Run();

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Custom_Help_Registered_By_Instance")]
        public Task Should_Output_Custom_Help_When_Registered_By_Instance()
        {
            var registrar = new DefaultTypeRegistrar();

            // Given
            var fixture = new CommandAppTester(registrar);
            fixture.Configure(configurator =>
            {
                // Create the custom help provider
                var helpProvider = new CustomHelpProvider(configurator.Settings, "1.0");

                // Register the custom help provider instance
                registrar.RegisterInstance(typeof(IHelpProvider), helpProvider);

                configurator.SetApplicationName("myapp");
                configurator.AddCommand<DogCommand>("dog");
            });

            // When
            var result = fixture.Run();

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Custom_Help_Registered_By_Type")]
        public Task Should_Output_Custom_Help_When_Registered_By_Type()
        {
            var registrar = new DefaultTypeRegistrar();

            // Given
            var fixture = new CommandAppTester(registrar);
            fixture.Configure(configurator =>
            {
                // Register the custom help provider type
                registrar.Register(typeof(IHelpProvider), typeof(RedirectHelpProvider));

                configurator.SetApplicationName("myapp");
                configurator.AddCommand<DogCommand>("dog");
            });

            // When
            var result = fixture.Run();

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Custom_Help_Configured_By_Instance")]
        public Task Should_Output_Custom_Help_When_Configured_By_Instance()
        {
            var registrar = new DefaultTypeRegistrar();

            // Given
            var fixture = new CommandAppTester(registrar);
            fixture.Configure(configurator =>
            {
                // Configure the custom help provider instance
                configurator.SetHelpProvider(new CustomHelpProvider(configurator.Settings, "1.0"));

                configurator.SetApplicationName("myapp");
                configurator.AddCommand<DogCommand>("dog");
            });

            // When
            var result = fixture.Run();

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Custom_Help_Configured_By_Type")]
        public Task Should_Output_Custom_Help_When_Configured_By_Type()
        {
            var registrar = new DefaultTypeRegistrar();

            // Given
            var fixture = new CommandAppTester(registrar);
            fixture.Configure(configurator =>
            {
                // Configure the custom help provider type
                configurator.SetHelpProvider<RedirectHelpProvider>();

                configurator.SetApplicationName("myapp");
                configurator.AddCommand<DogCommand>("dog");
            });

            // When
            var result = fixture.Run();

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Root_Examples")]
        public Task Should_Output_Examples_Defined_On_Root()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");

                // All root examples should be shown
                configurator.AddExample("dog", "--name", "Rufus", "--age", "12", "--good-boy");
                configurator.AddExample("dog", "--name", "Luna");
                configurator.AddExample("dog", "--name", "Charlie");
                configurator.AddExample("dog", "--name", "Bella");
                configurator.AddExample("dog", "--name", "Daisy");
                configurator.AddExample("dog", "--name", "Milo");
                configurator.AddExample("horse", "--name", "Brutus");
                configurator.AddExample("horse", "--name", "Sugar", "--IsAlive", "false");
                configurator.AddExample("horse", "--name", "Cash");
                configurator.AddExample("horse", "--name", "Dakota");
                configurator.AddExample("horse", "--name", "Cisco");
                configurator.AddExample("horse", "--name", "Spirit");

                configurator.AddCommand<DogCommand>("dog");
                configurator.AddCommand<HorseCommand>("horse");
            });

            // When
            var result = fixture.Run("--help");

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Root_Examples_Children")]
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1512:SingleLineCommentsMustNotBeFollowedByBlankLine", Justification = "Single line comment is relevant to several code blocks that follow.")]
        public Task Should_Output_Examples_Defined_On_Direct_Children_If_Root_Has_No_Examples()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");

                // It should be capped to the first 5 examples by default

                configurator.AddCommand<DogCommand>("dog")
                    .WithExample("dog", "--name", "Rufus", "--age", "12", "--good-boy")
                    .WithExample("dog", "--name", "Luna")
                    .WithExample("dog", "--name", "Charlie")
                    .WithExample("dog", "--name", "Bella")
                    .WithExample("dog", "--name", "Daisy")
                    .WithExample("dog", "--name", "Milo");

                configurator.AddCommand<HorseCommand>("horse")
                    .WithExample("horse", "--name", "Brutus")
                    .WithExample("horse", "--name", "Sugar", "--IsAlive", "false")
                    .WithExample("horse", "--name", "Cash")
                    .WithExample("horse", "--name", "Dakota")
                    .WithExample("horse", "--name", "Cisco")
                    .WithExample("horse", "--name", "Spirit");
            });

            // When
            var result = fixture.Run("--help");

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Root_Examples_Children_Eight")]
        public Task Should_Output_Eight_Examples_Defined_On_Direct_Children_If_Root_Has_No_Examples()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");

                // Show the first 8 examples defined on the direct children
                configurator.Settings.MaximumIndirectExamples = 8;

                configurator.AddCommand<DogCommand>("dog")
                    .WithExample("dog", "--name", "Rufus", "--age", "12", "--good-boy")
                    .WithExample("dog", "--name", "Luna")
                    .WithExample("dog", "--name", "Charlie")
                    .WithExample("dog", "--name", "Bella")
                    .WithExample("dog", "--name", "Daisy")
                    .WithExample("dog", "--name", "Milo");

                configurator.AddCommand<HorseCommand>("horse")
                    .WithExample("horse", "--name", "Brutus")
                    .WithExample("horse", "--name", "Sugar", "--IsAlive", "false")
                    .WithExample("horse", "--name", "Cash")
                    .WithExample("horse", "--name", "Dakota")
                    .WithExample("horse", "--name", "Cisco")
                    .WithExample("horse", "--name", "Spirit");
            });

            // When
            var result = fixture.Run("--help");

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Root_Examples_Children_Twelve")]
        public Task Should_Output_All_Examples_Defined_On_Direct_Children_If_Root_Has_No_Examples()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");

                // Show all examples defined on the direct children
                configurator.Settings.MaximumIndirectExamples = int.MaxValue;

                configurator.AddCommand<DogCommand>("dog")
                    .WithExample("dog", "--name", "Rufus", "--age", "12", "--good-boy")
                    .WithExample("dog", "--name", "Luna")
                    .WithExample("dog", "--name", "Charlie")
                    .WithExample("dog", "--name", "Bella")
                    .WithExample("dog", "--name", "Daisy")
                    .WithExample("dog", "--name", "Milo");

                configurator.AddCommand<HorseCommand>("horse")
                    .WithExample("horse", "--name", "Brutus")
                    .WithExample("horse", "--name", "Sugar", "--IsAlive", "false")
                    .WithExample("horse", "--name", "Cash")
                    .WithExample("horse", "--name", "Dakota")
                    .WithExample("horse", "--name", "Cisco")
                    .WithExample("horse", "--name", "Spirit");
            });

            // When
            var result = fixture.Run("--help");

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Root_Examples_Children_None")]
        public Task Should_Not_Output_Examples_Defined_On_Direct_Children_If_Root_Has_No_Examples()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");

                // Do not show examples defined on the direct children
                configurator.Settings.MaximumIndirectExamples = 0;

                configurator.AddCommand<DogCommand>("dog")
                    .WithExample("dog", "--name", "Rufus", "--age", "12", "--good-boy")
                    .WithExample("dog", "--name", "Luna")
                    .WithExample("dog", "--name", "Charlie")
                    .WithExample("dog", "--name", "Bella")
                    .WithExample("dog", "--name", "Daisy")
                    .WithExample("dog", "--name", "Milo");

                configurator.AddCommand<HorseCommand>("horse")
                    .WithExample("horse", "--name", "Brutus")
                    .WithExample("horse", "--name", "Sugar", "--IsAlive", "false")
                    .WithExample("horse", "--name", "Cash")
                    .WithExample("horse", "--name", "Dakota")
                    .WithExample("horse", "--name", "Cisco")
                    .WithExample("horse", "--name", "Spirit");
            });

            // When
            var result = fixture.Run("--help");

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Root_Examples_Leafs")]
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1512:SingleLineCommentsMustNotBeFollowedByBlankLine", Justification = "Single line comment is relevant to several code blocks that follow.")]
        public Task Should_Output_Examples_Defined_On_Leaves_If_No_Other_Examples_Are_Found()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
                configurator.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.SetDescription("The animal command.");

                    // It should be capped to the first 5 examples by default

                    animal.AddCommand<DogCommand>("dog")
                        .WithExample("animal", "dog", "--name", "Rufus", "--age", "12", "--good-boy")
                        .WithExample("animal", "dog", "--name", "Luna")
                        .WithExample("animal", "dog", "--name", "Charlie")
                        .WithExample("animal", "dog", "--name", "Bella")
                        .WithExample("animal", "dog", "--name", "Daisy")
                        .WithExample("animal", "dog", "--name", "Milo");

                    animal.AddCommand<HorseCommand>("horse")
                        .WithExample("animal", "horse", "--name", "Brutus")
                        .WithExample("animal", "horse", "--name", "Sugar", "--IsAlive", "false")
                        .WithExample("animal", "horse", "--name", "Cash")
                        .WithExample("animal", "horse", "--name", "Dakota")
                        .WithExample("animal", "horse", "--name", "Cisco")
                        .WithExample("animal", "horse", "--name", "Spirit");
                });
            });

            // When
            var result = fixture.Run("--help");

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Root_Examples_Leafs_Eight")]
        public Task Should_Output_Eight_Examples_Defined_On_Leaves_If_No_Other_Examples_Are_Found()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
                configurator.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.SetDescription("The animal command.");

                    // Show the first 8 examples defined on the direct children
                    configurator.Settings.MaximumIndirectExamples = 8;

                    animal.AddCommand<DogCommand>("dog")
                        .WithExample("animal", "dog", "--name", "Rufus", "--age", "12", "--good-boy")
                        .WithExample("animal", "dog", "--name", "Luna")
                        .WithExample("animal", "dog", "--name", "Charlie")
                        .WithExample("animal", "dog", "--name", "Bella")
                        .WithExample("animal", "dog", "--name", "Daisy")
                        .WithExample("animal", "dog", "--name", "Milo");

                    animal.AddCommand<HorseCommand>("horse")
                        .WithExample("animal", "horse", "--name", "Brutus")
                        .WithExample("animal", "horse", "--name", "Sugar", "--IsAlive", "false")
                        .WithExample("animal", "horse", "--name", "Cash")
                        .WithExample("animal", "horse", "--name", "Dakota")
                        .WithExample("animal", "horse", "--name", "Cisco")
                        .WithExample("animal", "horse", "--name", "Spirit");
                });
            });

            // When
            var result = fixture.Run("--help");

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Root_Examples_Leafs_Twelve")]
        public Task Should_Output_All_Examples_Defined_On_Leaves_If_No_Other_Examples_Are_Found()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
                configurator.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.SetDescription("The animal command.");

                    // Show all examples defined on the direct children
                    configurator.Settings.MaximumIndirectExamples = int.MaxValue;

                    animal.AddCommand<DogCommand>("dog")
                        .WithExample("animal", "dog", "--name", "Rufus", "--age", "12", "--good-boy")
                        .WithExample("animal", "dog", "--name", "Luna")
                        .WithExample("animal", "dog", "--name", "Charlie")
                        .WithExample("animal", "dog", "--name", "Bella")
                        .WithExample("animal", "dog", "--name", "Daisy")
                        .WithExample("animal", "dog", "--name", "Milo");

                    animal.AddCommand<HorseCommand>("horse")
                        .WithExample("animal", "horse", "--name", "Brutus")
                        .WithExample("animal", "horse", "--name", "Sugar", "--IsAlive", "false")
                        .WithExample("animal", "horse", "--name", "Cash")
                        .WithExample("animal", "horse", "--name", "Dakota")
                        .WithExample("animal", "horse", "--name", "Cisco")
                        .WithExample("animal", "horse", "--name", "Spirit");
                });
            });

            // When
            var result = fixture.Run("--help");

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Root_Examples_Leafs_None")]
        public Task Should_Not_Output_Examples_Defined_On_Leaves_If_No_Other_Examples_Are_Found()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
                configurator.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.SetDescription("The animal command.");

                    // Do not show examples defined on the direct children
                    configurator.Settings.MaximumIndirectExamples = 0;

                    animal.AddCommand<DogCommand>("dog")
                        .WithExample("animal", "dog", "--name", "Rufus", "--age", "12", "--good-boy")
                        .WithExample("animal", "dog", "--name", "Luna")
                        .WithExample("animal", "dog", "--name", "Charlie")
                        .WithExample("animal", "dog", "--name", "Bella")
                        .WithExample("animal", "dog", "--name", "Daisy")
                        .WithExample("animal", "dog", "--name", "Milo");

                    animal.AddCommand<HorseCommand>("horse")
                        .WithExample("animal", "horse", "--name", "Brutus")
                        .WithExample("animal", "horse", "--name", "Sugar", "--IsAlive", "false")
                        .WithExample("animal", "horse", "--name", "Cash")
                        .WithExample("animal", "horse", "--name", "Dakota")
                        .WithExample("animal", "horse", "--name", "Cisco")
                        .WithExample("animal", "horse", "--name", "Spirit");
                });
            });

            // When
            var result = fixture.Run("--help");

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Branch_Examples")]
        public Task Should_Output_Examples_Defined_On_Branch()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
                configurator.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.SetDescription("The animal command.");

                    // All branch examples should be shown
                    animal.AddExample("animal", "dog", "--name", "Rufus", "--age", "12", "--good-boy");
                    animal.AddExample("animal", "dog", "--name", "Luna");
                    animal.AddExample("animal", "dog", "--name", "Charlie");
                    animal.AddExample("animal", "dog", "--name", "Bella");
                    animal.AddExample("animal", "dog", "--name", "Daisy");
                    animal.AddExample("animal", "dog", "--name", "Milo");
                    animal.AddExample("animal", "horse", "--name", "Brutus");
                    animal.AddExample("animal", "horse", "--name", "Sugar", "--IsAlive", "false");
                    animal.AddExample("animal", "horse", "--name", "Cash");
                    animal.AddExample("animal", "horse", "--name", "Dakota");
                    animal.AddExample("animal", "horse", "--name", "Cisco");
                    animal.AddExample("animal", "horse", "--name", "Spirit");

                    animal.AddCommand<DogCommand>("dog")
                        .WithExample("animal", "dog", "--name", "Rufus", "--age", "12", "--good-boy");
                    animal.AddCommand<HorseCommand>("horse")
                        .WithExample("animal", "horse", "--name", "Brutus");
                });
            });

            // When
            var result = fixture.Run("animal", "--help");

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Default_Examples")]
        public Task Should_Output_Examples_Defined_On_Root_If_Default_Command_Is_Specified()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.SetDefaultCommand<DogCommand>();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");

                // All root examples should be shown
                configurator.AddExample("--name", "Rufus", "--age", "12", "--good-boy");
                configurator.AddExample("--name", "Luna");
                configurator.AddExample("--name", "Charlie");
                configurator.AddExample("--name", "Bella");
                configurator.AddExample("--name", "Daisy");
                configurator.AddExample("--name", "Milo");
            });

            // When
            var result = fixture.Run("--help");

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("NoDescription")]
        public Task Should_Not_Show_Truncated_Command_Table_If_Commands_Are_Missing_Description()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
                configurator.AddCommand<NoDescriptionCommand>("bar");
            });

            // When
            var result = fixture.Run("--help");

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("ArgumentOrder")]
        public Task Should_List_Arguments_In_Correct_Order()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.SetDefaultCommand<GenericCommand<ArgumentOrderSettings>>();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
            });

            // When
            var result = fixture.Run("--help");

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Hidden_Command_Options")]
        public Task Should_Not_Show_Hidden_Command_Options()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.SetDefaultCommand<GenericCommand<HiddenOptionSettings>>();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
            });

            // When
            var result = fixture.Run("--help");

            // Then
            return Verifier.Verify(result.Output);
        }
    }
}