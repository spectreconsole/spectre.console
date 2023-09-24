namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    [UsesVerify]
    [ExpectationPath("Xml")]
    public sealed class Xml
    {
        [Fact]
        [Expectation("Test_1")]
        public Task Should_Dump_Correct_Model_For_Case_1()
        {
            // Given
            var fixture = new CommandAppTester();
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
            var result = fixture.Run(Constants.XmlDocCommand);

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_2")]
        public Task Should_Dump_Correct_Model_For_Case_2()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(config =>
            {
                config.AddCommand<DogCommand>("dog");
            });

            // When
            var result = fixture.Run(Constants.XmlDocCommand);

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_3")]
        public Task Should_Dump_Correct_Model_For_Case_3()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(config =>
            {
                config.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.AddCommand<DogCommand>("dog");
                    animal.AddCommand<HorseCommand>("horse");
                });
            });

            // When
            var result = fixture.Run(Constants.XmlDocCommand);

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_4")]
        public Task Should_Dump_Correct_Model_For_Case_4()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(config =>
            {
                config.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.AddCommand<DogCommand>("dog");
                });
            });

            // When
            var result = fixture.Run(Constants.XmlDocCommand);

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_5")]
        public Task Should_Dump_Correct_Model_For_Case_5()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(config =>
            {
                config.AddCommand<OptionVectorCommand>("cmd");
            });

            // When
            var result = fixture.Run(Constants.XmlDocCommand);

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_6")]
        public Task Should_Dump_Correct_Model_For_Model_With_Default_Command()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.SetDefaultCommand<DogCommand>();
            fixture.Configure(config =>
            {
                config.AddCommand<HorseCommand>("horse");
            });

            // When
            var result = fixture.Run(Constants.XmlDocCommand);

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_7")]
        public Task Should_Dump_Correct_Model_For_Model_With_Single_Branch_Single_Branch_Default_Command()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configuration =>
            {
                configuration.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.AddBranch<MammalSettings>("mammal", mammal =>
                    {
                        mammal.SetDefaultCommand<HorseCommand>();
                    });
                });
            });

            // When
            var result = fixture.Run(Constants.XmlDocCommand);

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_8")]
        public Task Should_Dump_Correct_Model_For_Model_With_Single_Branch_Single_Command_Default_Command()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configuration =>
            {
                configuration.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.AddCommand<DogCommand>("dog");

                    animal.SetDefaultCommand<HorseCommand>();
                });
            });

            // When
            var result = fixture.Run(Constants.XmlDocCommand);

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Test_9")]
        public Task Should_Dump_Correct_Model_For_Model_With_Default_Command_Single_Branch_Single_Command_Default_Command()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.SetDefaultCommand<EmptyCommand>();
            fixture.Configure(configuration =>
            {
                configuration.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.AddCommand<DogCommand>("dog");

                    animal.SetDefaultCommand<HorseCommand>();
                });
            });

            // When
            var result = fixture.Run(Constants.XmlDocCommand);

            // Then
            return Verifier.Verify(result.Output);
        }

        [Fact]
        [Expectation("Hidden_Command_Options")]
        public Task Should_Not_Dump_Hidden_Options_On_A_Command()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.SetDefaultCommand<HiddenOptionsCommand>();

            // When
            var result = fixture.Run(Constants.XmlDocCommand);

            // Then
            return Verifier.Verify(result.Output);
        }
    }
}
