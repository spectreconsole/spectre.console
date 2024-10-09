namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    public sealed class Version
    {
        [Fact]
        public void Should_Output_CLI_Version_To_The_Console()
        {
            // Given
            var fixture = new CommandAppTester();

            // When
            var result = fixture.Run(Constants.VersionCommand);

            // Then
            result.Output.ShouldStartWith("Spectre.Cli version ");
        }

        [Theory]
        [InlineData("-v")]
        [InlineData("--version")]
        public void Should_Output_Application_Version_To_The_Console_With_No_Command(string versionOption)
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationVersion("1.0");
            });

            // When
            var result = fixture.Run(versionOption);

            // Then
            result.Output.ShouldBe("1.0");
        }

        [Theory]
        [InlineData("-v")]
        [InlineData("--version")]
        public void Should_Not_Display_Version_If_Not_Specified(string versionOption)
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.AddCommand<EmptyCommand>("empty");
            });

            // When
            var result = fixture.Run(versionOption);

            // Then
            result.ExitCode.ShouldNotBe(0);
            result.Output.ShouldStartWith($"Error: Unexpected option '{versionOption.Replace("-", "")}'");
        }

        [Theory]
        [InlineData("-v")]
        [InlineData("--version")]
        public void Should_Execute_Command_Not_Output_Application_Version_To_The_Console(string versionOption)
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationVersion("1.0");
                configurator.AddCommand<EmptyCommand>("empty");
            });

            // When
            var result = fixture.Run("empty", versionOption);

            // Then
            result.Output.ShouldBe(string.Empty);
            result.Context.ShouldHaveRemainingArgument(versionOption, new[] { (string)null });
        }

        [Theory]
        [InlineData("-v")]
        [InlineData("--version")]
        public void Should_Output_Application_Version_To_The_Console_With_Default_Command(string versionOption)
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.SetDefaultCommand<EmptyCommand>();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationVersion("1.0");
            });

            // When
            var result = fixture.Run(versionOption);

            // Then
            result.Output.ShouldBe("1.0");
        }

        [Theory]
        [InlineData("-v")]
        [InlineData("--version")]
        public void Should_Output_Application_Version_To_The_Console_With_Branch_Default_Command(string versionOption)
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationVersion("1.0");
                configurator.AddBranch<EmptyCommandSettings>("branch", branch =>
                {
                    branch.SetDefaultCommand<EmptyCommand>();
                });
            });

            // When
            var result = fixture.Run(versionOption);

            // Then
            result.Output.ShouldBe("1.0");
        }

        [Theory]
        [InlineData("-v")]
        [InlineData("--version")]
        public void Should_Execute_Branch_Default_Command_Not_Output_Application_Version_To_The_Console(string versionOption)
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationVersion("1.0");
                configurator.AddBranch<EmptyCommandSettings>("branch", branch =>
                {
                    branch.SetDefaultCommand<EmptyCommand>();
                });
            });

            // When
            var result = fixture.Run("branch", versionOption);

            // Then
            result.Output.ShouldBe(string.Empty);
            result.Context.ShouldHaveRemainingArgument(versionOption, new[] { (string)null });
        }

        [Theory]
        [InlineData("-v")]
        [InlineData("--version")]
        public void Should_Execute_Branch_Command_Not_Output_Application_Version_To_The_Console(string versionOption)
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationVersion("1.0");
                configurator.AddBranch<EmptyCommandSettings>("branch", branch =>
                {
                    branch.AddCommand<EmptyCommand>("hello");
                });
            });

            // When
            var result = fixture.Run("branch", "hello", versionOption);

            // Then
            result.Output.ShouldBe(string.Empty);
            result.Context.ShouldHaveRemainingArgument(versionOption, new[] { (string)null });
        }
    }
}
