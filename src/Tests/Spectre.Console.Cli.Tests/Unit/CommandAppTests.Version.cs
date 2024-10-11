namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    public sealed class Version
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Should_Output_CLI_Version_To_The_Console(bool strictParsing)
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.Settings.StrictParsing = strictParsing;
            });

            // When
            var result = fixture.Run(Constants.VersionCommand);

            // Then
            result.Output.ShouldStartWith("Spectre.Cli version ");
        }

        [Theory]
        [InlineData("-v", false)]
        [InlineData("-v", true)]
        [InlineData("--version", false)]
        [InlineData("--version", true)]
        public void Should_Output_Application_Version_To_The_Console_With_No_Command(string versionOption, bool strictParsing)
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.Settings.StrictParsing = strictParsing;
                configurator.SetApplicationVersion("1.0");
            });

            // When
            var result = fixture.Run(versionOption);

            // Then
            result.Output.ShouldBe("1.0");
        }

        [Theory]
        [InlineData("-v", false)]
        [InlineData("-v", true)]
        [InlineData("--version", false)]
        [InlineData("--version", true)]
        public void Should_Not_Display_Version_If_Not_Specified(string versionOption, bool strictParsing)
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.Settings.StrictParsing = strictParsing;
            });

            // When
            var result = fixture.Run(versionOption);

            // Then
            result.ExitCode.ShouldNotBe(0);
            result.Output.ShouldStartWith($"Error: Unexpected option '{versionOption.Replace("-", "")}'");
        }

        [Theory]
        [InlineData("-v", false)]
        [InlineData("-v", true)]
        [InlineData("--version", false)]
        [InlineData("--version", true)]
        public void Should_Output_Application_Version_To_The_Console_With_Default_Command(string versionOption, bool strictParsing)
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.SetDefaultCommand<EmptyCommand>();
            fixture.Configure(configurator =>
            {
                configurator.Settings.StrictParsing = strictParsing;
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
        [InlineData("-v", false)]
        [InlineData("-v", true)]
        [InlineData("--version", false)]
        [InlineData("--version", true)]
        public void Should_Output_Application_Version_To_The_Console_With_Branch_Default_Command(string versionOption, bool strictParsing)
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.Settings.StrictParsing = strictParsing;
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
                    branch.AddCommand<EmptyCommand>("empty");
                });
            });

            // When
            var result = fixture.Run("branch", "empty", versionOption);

            // Then
            result.Output.ShouldBe(string.Empty);
            result.Context.ShouldHaveRemainingArgument(versionOption, new[] { (string)null });
        }

        /// <summary>
        /// When a command with a version option in the settings is set as the application default command,
        /// then execute this command instead of displaying the explicitly set Application Version.
        /// </summary>
        [Theory]
        [InlineData("-v", false)]
        [InlineData("-v", true)]
        [InlineData("--version", false)]
        [InlineData("--version", true)]
        public void Should_Execute_Default_VersionCommand_Not_Output_Application_Version_To_The_Console(string versionOption, bool strictParsing)
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.SetDefaultCommand<Spectre.Console.Tests.Data.VersionCommand>();
            fixture.Configure(configurator =>
            {
                configurator.Settings.StrictParsing = strictParsing;
                configurator.SetApplicationVersion("1.0");
            });

            // When
            var result = fixture.Run(versionOption, "X.Y.Z");

            // Then
            result.Output.ShouldBe("VersionCommand ran, Version: X.Y.Z");
        }

        [Theory]
        [InlineData("-v", false)]
        [InlineData("-v", true)]
        [InlineData("--version", false)]
        [InlineData("--version", true)]
        public void Should_Execute_VersionCommand_Not_Output_Application_Version_To_The_Console(string versionOption, bool strictParsing)
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.Settings.StrictParsing = strictParsing;
                configurator.SetApplicationVersion("1.0");
                configurator.AddCommand<Spectre.Console.Tests.Data.VersionCommand>("hello");
            });

            // When
            var result = fixture.Run("hello", versionOption, "X.Y.Z");

            // Then
            result.Output.ShouldBe("VersionCommand ran, Version: X.Y.Z");
        }

        [Theory]
        [InlineData("-v", false)]
        [InlineData("-v", true)]
        [InlineData("--version", false)]
        [InlineData("--version", true)]
        public void Should_Execute_Branch_Default_VersionCommand_Not_Output_Application_Version_To_The_Console(string versionOption, bool strictParsing)
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.Settings.StrictParsing = strictParsing;
                configurator.SetApplicationVersion("1.0");
                configurator.AddBranch<VersionSettings>("branch", branch =>
                {
                    branch.SetDefaultCommand<Spectre.Console.Tests.Data.VersionCommand>();
                });
            });

            // When
            var result = fixture.Run("branch", versionOption, "X.Y.Z");

            // Then
            result.Output.ShouldBe("VersionCommand ran, Version: X.Y.Z");
        }

        [Theory]
        [InlineData("-v", false)]
        [InlineData("-v", true)]
        [InlineData("--version", false)]
        [InlineData("--version", true)]
        public void Should_Execute_Branch_VersionCommand_Not_Output_Application_Version_To_The_Console(string versionOption, bool strictParsing)
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.Settings.StrictParsing = strictParsing;
                configurator.SetApplicationVersion("1.0");
                configurator.AddBranch<VersionSettings>("branch", branch =>
                {
                    branch.AddCommand<Spectre.Console.Tests.Data.VersionCommand>("hello");
                });
            });

            // When
            var result = fixture.Run("branch", "hello", versionOption, "X.Y.Z");

            // Then
            result.Output.ShouldBe("VersionCommand ran, Version: X.Y.Z");
        }
    }
}
