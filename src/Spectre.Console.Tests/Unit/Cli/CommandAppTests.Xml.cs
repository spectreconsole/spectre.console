using System.Threading.Tasks;
using Spectre.Console.Testing;
using Spectre.Console.Tests.Data;
using VerifyXunit;
using Xunit;
using Spectre.Console.Cli;
using Spectre.Verify.Extensions;

namespace Spectre.Console.Tests.Unit.Cli
{
    public sealed partial class CommandAppTests
    {
        [UsesVerify]
        [ExpectationPath("Cli/Xml")]
        public sealed class Xml
        {
            /// <summary>
            /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-1
            /// </summary>
            [Fact]
            [Expectation("Test_1")]
            public Task Should_Dump_Correct_Model_For_Case_1()
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(config =>
                {
                    config.PropagateExceptions();
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
                var (_, output, _, _) = fixture.Run(Constants.XmlDocCommand);

                // Then
                return Verifier.Verify(output);
            }

            /// <summary>
            /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-2
            /// </summary>
            [Fact]
            [Expectation("Test_2")]
            public Task Should_Dump_Correct_Model_For_Case_2()
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(config =>
                {
                    config.AddCommand<DogCommand>("dog");
                });

                // When
                var (_, output, _, _) = fixture.Run(Constants.XmlDocCommand);

                // Then
                return Verifier.Verify(output);
            }

            /// <summary>
            /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-3
            /// </summary>
            [Fact]
            [Expectation("Test_3")]
            public Task Should_Dump_Correct_Model_For_Case_3()
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(config =>
                {
                    config.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.AddCommand<DogCommand>("dog");
                        animal.AddCommand<HorseCommand>("horse");
                    });
                });

                // When
                var (_, output, _, _) = fixture.Run(Constants.XmlDocCommand);

                // Then
                return Verifier.Verify(output);
            }

            /// <summary>
            /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-4
            /// </summary>
            [Fact]
            [Expectation("Test_4")]
            public Task Should_Dump_Correct_Model_For_Case_4()
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(config =>
                {
                    config.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.AddCommand<DogCommand>("dog");
                    });
                });

                // When
                var (_, output, _, _) = fixture.Run(Constants.XmlDocCommand);

                // Then
                return Verifier.Verify(output);
            }

            /// <summary>
            /// https://github.com/spectresystems/spectre.cli/wiki/Test-cases#test-case-5
            /// </summary>
            [Fact]
            [Expectation("Test_5")]
            public Task Should_Dump_Correct_Model_For_Case_5()
            {
                // Given
                var fixture = new CommandAppFixture();
                fixture.Configure(config =>
                {
                    config.AddCommand<OptionVectorCommand>("cmd");
                });

                // When
                var (_, output, _, _) = fixture.Run(Constants.XmlDocCommand);

                // Then
                return Verifier.Verify(output);
            }

            [Fact]
            [Expectation("Test_6")]
            public Task Should_Dump_Correct_Model_For_Model_With_Default_Command()
            {
                // Given
                var fixture = new CommandAppFixture().WithDefaultCommand<DogCommand>();
                fixture.Configure(config =>
                {
                    config.AddCommand<HorseCommand>("horse");
                });

                // When
                var (_, output, _, _) = fixture.Run(Constants.XmlDocCommand);

                // Then
                return Verifier.Verify(output);
            }
        }
    }
}
