using Shouldly;
using Spectre.Console.Cli;
using Spectre.Console.Testing;
using Xunit;

namespace Spectre.Console.Tests.Unit.Cli
{
    public sealed partial class CommandAppTests
    {
        public class NullableSettings : CommandSettings
        {
            public NullableSettings(bool? detailed, string[] extra)
            {
                Detailed = detailed;
                Extra = extra;
            }

            [CommandOption("-d")]
            public bool? Detailed { get; }

            [CommandArgument(0, "[extra]")]
            public string[] Extra { get; }
        }

        public class NullableWithInitSettings : CommandSettings
        {
            [CommandOption("-d")]
            public bool? Detailed { get; init; }

            [CommandArgument(0, "[extra]")]
            public string[] Extra { get; init; }
        }

        public class NullableCommand : Command<NullableSettings>
        {
            public override int Execute(CommandContext context, NullableSettings settings) => 0;
        }

        public class NullableWithInitCommand : Command<NullableWithInitSettings>
        {
            public override int Execute(CommandContext context, NullableWithInitSettings settings) => 0;
        }

        [Fact]
        public void Should_Populate_Nullable_Objects_In_Settings()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
                configurator.AddCommand<NullableCommand>("null");
            });

            // When
            var result = fixture.Run("null");

            // Then
            result.Settings.ShouldBeOfType<NullableSettings>().And(settings =>
            {
                settings.Detailed.ShouldBeNull();
                settings.Extra.ShouldBeNull();
            });
        }

        [Fact]
        public void Should_Populate_Nullable_Objects_With_Init_In_Settings()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
                configurator.AddCommand<NullableWithInitCommand>("null");
            });

            // When
            var result = fixture.Run("null");

            // Then
            result.Settings.ShouldBeOfType<NullableWithInitSettings>().And(settings =>
            {
                settings.Detailed.ShouldBeNull();
                settings.Extra.ShouldBeNull();
            });
        }

        [Fact]
        public void Should_Populate_Regular_Settings()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
                configurator.AddCommand<NullableCommand>("null");
            });

            // When
            var result = fixture.Run("null", "-d", "true", "first-item");

            // Then
            result.Settings.ShouldBeOfType<NullableSettings>().And(settings =>
            {
                settings.Detailed.ShouldBe(true);
                settings.Extra.ShouldBe(new[] { "first-item" });
            });
        }
    }
}