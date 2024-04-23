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

        [Fact]
        public void Should_Output_Application_Version_To_The_Console_With_No_Command()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationVersion("1.0");
            });

            // When
            var result = fixture.Run("--version");

            // Then
            result.Output.ShouldBe("1.0");
        }

        [Fact]
        public void Should_Execute_Command_Not_Output_Application_Version_To_The_Console()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationVersion("1.0");

                configurator.AddCommand<EmptyCommand>("empty");
            });

            // When
            var result = fixture.Run("empty", "--version");

            // Then
            result.Output.ShouldBe(string.Empty);
            result.Context.ShouldHaveRemainingArgument("--version", new[] { (string)null });
        }

        [Fact]
        public void Should_Execute_Default_Command_Not_Output_Application_Version_To_The_Console()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.SetDefaultCommand<EmptyCommand>();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationVersion("1.0");
            });

            // When
            var result = fixture.Run("--version");

            // Then
            result.Output.ShouldBe(string.Empty);
            result.Context.ShouldHaveRemainingArgument("--version", new[] { (string)null });
        }

        [Fact]
        public void Should_Output_Application_Version_To_The_Console_With_Branch_Default_Command()
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
            var result = fixture.Run("--version");

            // Then
            result.Output.ShouldBe("1.0");
        }
    }
}
