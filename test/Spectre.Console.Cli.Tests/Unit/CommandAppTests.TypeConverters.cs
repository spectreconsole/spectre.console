using System.IO;

namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    public sealed class TypeConverters
    {
        [Fact]
        public void Should_Bind_Using_Custom_Type_Converter_If_Specified()
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.PropagateExceptions();
                config.AddCommand<CatCommand>("cat");
            });

            // When
            var result = app.Run(new[]
            {
                    "cat", "--name", "Tiger",
                    "--agility", "FOOBAR",
            });

            // Then
            result.ExitCode.ShouldBe(0);
            result.Settings.ShouldBeOfType<CatSettings>().And(cat =>
            {
                cat.Agility.ShouldBe(6);
            });
        }

        [Fact]
        public void Should_Convert_Enum_Ignoring_Case()
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.AddCommand<HorseCommand>("horse");
            });

            // When
            var result = app.Run(new[] { "horse", "--day", "friday" });

            // Then
            result.ExitCode.ShouldBe(0);
            result.Settings.ShouldBeOfType<HorseSettings>().And(horse =>
            {
                horse.Day.ShouldBe(DayOfWeek.Friday);
            });
        }

        [Fact]
        public void Should_List_All_Valid_Enum_Values_On_Conversion_Error()
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.AddCommand<HorseCommand>("horse");
            });

            // When
            var result = app.Run(new[] { "horse", "--day", "heimday" });

            // Then
            result.ExitCode.ShouldBe(-1);
            result.Output.ShouldStartWith("Error");
            result.Output.ShouldContain("heimday");
            result.Output.ShouldContain(nameof(DayOfWeek.Sunday));
            result.Output.ShouldContain(nameof(DayOfWeek.Monday));
            result.Output.ShouldContain(nameof(DayOfWeek.Tuesday));
            result.Output.ShouldContain(nameof(DayOfWeek.Wednesday));
            result.Output.ShouldContain(nameof(DayOfWeek.Thursday));
            result.Output.ShouldContain(nameof(DayOfWeek.Friday));
            result.Output.ShouldContain(nameof(DayOfWeek.Saturday));
        }

        [Fact]
        public void Should_Convert_FileInfo_And_DirectoryInfo()
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.AddCommand<HorseCommand>("horse");
            });

            // When
            var result = app.Run(new[] { "horse", "--file", "ntp.conf", "--directory", "etc" });

            // Then
            result.ExitCode.ShouldBe(0);
            result.Settings.ShouldBeOfType<HorseSettings>().And(horse =>
            {
                horse.File.Name.ShouldBe("ntp.conf");
                horse.Directory.Name.ShouldBe("etc");
            });
        }
    }
}
