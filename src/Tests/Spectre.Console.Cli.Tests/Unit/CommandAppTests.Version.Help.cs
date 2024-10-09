namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    public sealed partial class Version
    {
        public sealed class Help
        {
            [Theory]
            [InlineData("-?")]
            [InlineData("-h")]
            [InlineData("--help")]
            public void Help_Should_Include_Application_Version_Flag_With_No_Command(string helpOption)
            {
                // Given
                var fixture = new CommandAppTester();
                fixture.Configure(configurator =>
                {
                    configurator.SetApplicationVersion("1.0");
                });

                // When
                var result = fixture.Run(helpOption);

                // Then
                result.Output.ShouldContain("-v, --version    Prints version information");
            }

            [Theory]
            [InlineData("-?")]
            [InlineData("-h")]
            [InlineData("--help")]
            public void Help_Should_Not_Include_Application_Version_Flag_If_Not_Specified(string helpOption)
            {
                // Given
                var fixture = new CommandAppTester();
                fixture.Configure(configurator =>
                {
                    configurator.AddCommand<EmptyCommand>("empty");
                });

                // When
                var result = fixture.Run(helpOption);

                // Then
                result.Output.ShouldNotContain("-v, --version    Prints version information");
            }

            [Theory]
            [InlineData("-?")]
            [InlineData("-h")]
            [InlineData("--help")]
            public void Help_Should_Not_Include_Application_Version_Flag_For_Command(string helpOption)
            {
                // Given
                var fixture = new CommandAppTester();
                fixture.Configure(configurator =>
                {
                    configurator.SetApplicationVersion("1.0");
                    configurator.AddCommand<EmptyCommand>("empty");
                });

                // When
                var result = fixture.Run("empty", helpOption);

                // Then
                result.Output.ShouldNotContain("-v, --version    Prints version information");
            }

            [Theory]
            [InlineData("-?")]
            [InlineData("-h")]
            [InlineData("--help")]
            public void Help_Should_Include_Application_Version_Flag_For_Default_Command(string helpOption)
            {
                // Given
                var fixture = new CommandAppTester();
                fixture.SetDefaultCommand<EmptyCommand>();
                fixture.Configure(configurator =>
                {
                    configurator.SetApplicationVersion("1.0");
                });

                // When
                var result = fixture.Run(helpOption);

                // Then
                result.Output.ShouldContain("-v, --version    Prints version information");
            }

            [Theory]
            [InlineData("-?")]
            [InlineData("-h")]
            [InlineData("--help")]
            public void Help_Should_Not_Include_Application_Version_Flag_For_Branch_Default_Command(string helpOption)
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
                var result = fixture.Run("branch", helpOption);

                // Then
                result.Output.ShouldNotContain("-v, --version    Prints version information");
            }

            [Theory]
            [InlineData("-?")]
            [InlineData("-h")]
            [InlineData("--help")]
            public void Help_Should_Not_Include_Application_Version_Flag_For_Branch_Command(string helpOption)
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
                var result = fixture.Run("branch", "hello", helpOption);

                // Then
                result.Output.ShouldNotContain("-v, --version    Prints version information");
            }

            [Theory]
            [InlineData("-?")]
            [InlineData("-h")]
            [InlineData("--help")]
            public void Help_Should_Include_Command_Version_Flag_For_Command(string helpOption)
            {
                // Given
                var fixture = new CommandAppTester();
                fixture.Configure(configurator =>
                {
                    configurator.SetApplicationVersion("1.0");
                    configurator.AddCommand<Spectre.Console.Tests.Data.VersionCommand>("empty");
                });

                // When
                var result = fixture.Run("empty", helpOption);

                // Then
                result.Output.ShouldContain("-v, --version    The command version");
                result.Output.ShouldNotContain("-v, --version    Prints version information");
            }

            /// <summary>
            /// When a command with a version flag in the settings is set as the application default command,
            /// then override the in-built Application Version flag with the command version flag instead.
            /// Rationale: This behaviour makes the most sense because the other flags for the default command
            /// will be shown in the help output and the user can set any of these when executing the application.
            /// </summary>
            [Theory]
            [InlineData("-?")]
            [InlineData("-h")]
            [InlineData("--help")]
            public void Help_Should_Include_Command_Version_Flag_For_Default_Command(string helpOption)
            {
                // Given
                var fixture = new CommandAppTester();
                fixture.SetDefaultCommand<Spectre.Console.Tests.Data.VersionCommand>();
                fixture.Configure(configurator =>
                {
                    configurator.SetApplicationVersion("1.0");
                });

                // When
                var result = fixture.Run(helpOption);

                // Then
                result.Output.ShouldContain("-v, --version    The command version");
                result.Output.ShouldNotContain("-v, --version    Prints version information");
            }

            [Theory]
            [InlineData("-?")]
            [InlineData("-h")]
            [InlineData("--help")]
            public void Help_Should_Include_Command_Version_Flag_For_Branch_Default_Command(string helpOption)
            {
                // Given
                var fixture = new CommandAppTester();
                fixture.Configure(configurator =>
                {
                    configurator.SetApplicationVersion("1.0");
                    configurator.AddBranch<VersionSettings>("branch", branch =>
                    {
                        branch.SetDefaultCommand<Spectre.Console.Tests.Data.VersionCommand>();
                    });
                });

                // When
                var result = fixture.Run("branch", helpOption);

                // Then
                result.Output.ShouldContain("-v, --version    The command version");
                result.Output.ShouldNotContain("-v, --version    Prints version information");
            }

            [Theory]
            [InlineData("-?")]
            [InlineData("-h")]
            [InlineData("--help")]
            public void Help_Should_Include_Command_Version_Flag_For_Branch_Command(string helpOption)
            {
                // Given
                var fixture = new CommandAppTester();
                fixture.Configure(configurator =>
                {
                    configurator.SetApplicationVersion("1.0");
                    configurator.AddBranch<VersionSettings>("branch", branch =>
                    {
                        branch.AddCommand<Spectre.Console.Tests.Data.VersionCommand>("hello");
                    });
                });

                // When
                var result = fixture.Run("branch", "hello", helpOption);

                // Then
                result.Output.ShouldContain("-v, --version    The command version");
                result.Output.ShouldNotContain("-v, --version    Prints version information");
            }


        }
    }
}