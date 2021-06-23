using System.Threading.Tasks;
using Spectre.Console.Cli;
using Spectre.Console.Testing;
using Spectre.Console.Tests.Data;
using Spectre.Verify.Extensions;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit.Cli
{
    public sealed partial class CommandAppTests
    {
        [UsesVerify]
        [ExpectationPath("Cli/Help")]
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
                        .WithExample(new[] { "giraffe", "123" })
                        .IsHidden();
                });

                // When
                var result = fixture.Run("--help");

                // Then
                return Verifier.Verify(result.Output);
            }

            [Fact]
            [Expectation("Command")]
            public Task Should_Output_Command_Correctly()
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
            [Expectation("RootExamples")]
            public Task Should_Output_Root_Examples_Defined_On_Root()
            {
                // Given
                var fixture = new CommandAppTester();
                fixture.Configure(configurator =>
                {
                    configurator.SetApplicationName("myapp");
                    configurator.AddExample(new[] { "dog", "--name", "Rufus", "--age", "12", "--good-boy" });
                    configurator.AddExample(new[] { "horse", "--name", "Brutus" });
                    configurator.AddCommand<DogCommand>("dog");
                    configurator.AddCommand<HorseCommand>("horse");
                });

                // When
                var result = fixture.Run("--help");

                // Then
                return Verifier.Verify(result.Output);
            }

            [Fact]
            [Expectation("RootExamples_Children")]
            public Task Should_Output_Root_Examples_Defined_On_Direct_Children_If_Root_Have_No_Examples()
            {
                // Given
                var fixture = new CommandAppTester();
                fixture.Configure(configurator =>
                {
                    configurator.SetApplicationName("myapp");
                    configurator.AddCommand<DogCommand>("dog")
                        .WithExample(new[] { "dog", "--name", "Rufus", "--age", "12", "--good-boy" });
                    configurator.AddCommand<HorseCommand>("horse")
                        .WithExample(new[] { "horse", "--name", "Brutus" });
                });

                // When
                var result = fixture.Run("--help");

                // Then
                return Verifier.Verify(result.Output);
            }

            [Fact]
            [Expectation("RootExamples_Leafs")]
            public Task Should_Output_Root_Examples_Defined_On_Leaves_If_No_Other_Examples_Are_Found()
            {
                // Given
                var fixture = new CommandAppTester();
                fixture.Configure(configurator =>
                {
                    configurator.SetApplicationName("myapp");
                    configurator.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.SetDescription("The animal command.");
                        animal.AddCommand<DogCommand>("dog")
                            .WithExample(new[] { "animal", "dog", "--name", "Rufus", "--age", "12", "--good-boy" });
                        animal.AddCommand<HorseCommand>("horse")
                            .WithExample(new[] { "animal", "horse", "--name", "Brutus" });
                    });
                });

                // When
                var result = fixture.Run("--help");

                // Then
                return Verifier.Verify(result.Output);
            }

            [Fact]
            [Expectation("CommandExamples")]
            public Task Should_Only_Output_Command_Examples_Defined_On_Command()
            {
                // Given
                var fixture = new CommandAppTester();
                fixture.Configure(configurator =>
                {
                    configurator.SetApplicationName("myapp");
                    configurator.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.SetDescription("The animal command.");
                        animal.AddExample(new[] { "animal", "--help" });

                        animal.AddCommand<DogCommand>("dog")
                            .WithExample(new[] { "animal", "dog", "--name", "Rufus", "--age", "12", "--good-boy" });
                        animal.AddCommand<HorseCommand>("horse")
                            .WithExample(new[] { "animal", "horse", "--name", "Brutus" });
                    });
                });

                // When
                var result = fixture.Run("animal", "--help");

                // Then
                return Verifier.Verify(result.Output);
            }

            [Fact]
            [Expectation("DefaultExamples")]
            public Task Should_Output_Root_Examples_If_Default_Command_Is_Specified()
            {
                // Given
                var fixture = new CommandAppTester();
                fixture.SetDefaultCommand<LionCommand>();
                fixture.Configure(configurator =>
                {
                    configurator.SetApplicationName("myapp");
                    configurator.AddExample(new[] { "12", "-c", "3" });
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
        }
    }
}
