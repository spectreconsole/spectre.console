
using System;

namespace Spectre.Console.Tests.Unit.Cli.Testing;

public sealed class CommandAppTesterTests
{
    private class CommandAppTesterCommand : Command<OptionalArgumentWithDefaultValueSettings>
    {
        private readonly IAnsiConsole _console;

        public CommandAppTesterCommand(IAnsiConsole console)
        {
            _console = console;
        }

        public override int Execute(CommandContext context, OptionalArgumentWithDefaultValueSettings settings)
        {
            _console.Write(settings.Greeting);
            return 0;
        }
    }

    [Theory]
    [InlineData(false, " Hello ", " Hello ")]
    [InlineData(true, " Hello ", "Hello")]
    [InlineData(false, " Hello \n World ", " Hello \n World ")]
    [InlineData(true, " Hello \n World ", "Hello\n World")]
    public void Should_Respect_Trim_Setting(bool trim, string actual, string expected)
    {
        // Given
        var settings = new CommandAppTesterSettings { TrimConsoleOutput = trim };

        var app = new CommandAppTester(settings);
        app.SetDefaultCommand<CommandAppTesterCommand>();
        app.Configure(config =>
        {
            config.PropagateExceptions();
        });

        // When
        var result = app.Run(actual);

        // Then
        result.Output.ShouldBe(expected);
    }

    [Fact]
    public void DefaultCtor_WithoutParameters_CreatesDefaultConsole()
    {
        // Given, When
        CommandAppTester app = new();

        // Then
        app.Console.ShouldNotBeNull();
        app.Console.Profile.Width.ShouldBe(int.MaxValue);
    }

    [Fact]
    public void DefaultCtor_WithCustomConsole_UsesProvidedInstance()
    {
        // Given
        TestConsole console = new();

        // When
        CommandAppTester app = new(null, new CommandAppTesterSettings(), console);

        // Then
        app.Console.ShouldNotBeNull();
        app.Console.ShouldBeSameAs(console);
    }

    [Fact]
    public void Ctor_WithSettings_CreatesDefaultConsole()
    {
        // Given, When
        CommandAppTester app = new(new CommandAppTesterSettings());

        // Then
        app.Console.ShouldNotBeNull();
        app.Console.Profile.Width.ShouldBe(int.MaxValue);
    }
}
