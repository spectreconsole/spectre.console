using Shouldly;
using Spectre.Console.Cli;
using Spectre.Console.Testing;
using Spectre.Console.Tests.Data;
using VerifyXunit;
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
        public void Nullable_objects_in_settings_are_populated_properly()
        {
            // Given
            var fixture = new CommandAppFixture();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
                configurator.AddCommand<NullableCommand>("null");
            });

            // When
            var (_, _, _, settings) = fixture.Run("null");
            var nullableSettings = (NullableSettings)settings;

            // Then
            nullableSettings.Detailed.ShouldBeNull();
            nullableSettings.Extra.ShouldBeNull();
        }

        [Fact]
        public void Nullable_objects_with_init_in_settings_are_populated_properly()
        {
            // Given
            var fixture = new CommandAppFixture();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
                configurator.AddCommand<NullableWithInitCommand>("null");
            });

            // When
            var (_, _, _, settings) = fixture.Run("null");
            var nullableSettings = (NullableWithInitSettings)settings;

            // Then
            nullableSettings.Detailed.ShouldBeNull();
            nullableSettings.Extra.ShouldBeNull();
        }

        [Fact]
        public void Regular_settings_are_populated_properly()
        {
            // Given
            var fixture = new CommandAppFixture();
            fixture.Configure(configurator =>
            {
                configurator.SetApplicationName("myapp");
                configurator.AddCommand<NullableCommand>("null");
            });

            // When
            var (_, _, _, settings) = fixture.Run("null", "-d", "true", "first-item");
            var nullableSettings = (NullableSettings)settings;

            // Then
            nullableSettings.Detailed.ShouldBe(true);
            nullableSettings.Extra.ShouldBe(new[] { "first-item" });
        }
    }
}