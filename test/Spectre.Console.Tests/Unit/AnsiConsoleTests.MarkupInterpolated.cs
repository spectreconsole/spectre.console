namespace Spectre.Console.Tests.Unit;

public partial class AnsiConsoleTests
{
    public sealed class MarkupInterpolated
    {
        [Fact]
        public void Should_Print_Simple_Interpolated_Strings()
        {
            // Given
            var console = new TestConsole()
                .Colors(ColorSystem.Standard)
                .EmitAnsiSequences();

            // When
            const string Path = "file://c:/temp/[x].txt";
            console.MarkupInterpolated($"[Green]{Path}[/]");

            // Then
            console.Output.ShouldBe($"[32m{Path}[0m");
        }

        [Fact]
        public void Should_Not_Throw_Error_On_Links_Brackets()
        {
            // Given
            var console = new TestConsole()
                .Colors(ColorSystem.Standard)
                .EmitAnsiSequences();

            // When
            const string Path = "file://c:/temp/[x].txt";
            console.MarkupInterpolated($"[link={Path}]{Path}[/]");

            // Then
            var pathAsRegEx = Regex.Replace(Path, "([/\\[\\]\\\\])", "\\$1", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            console.Output.ShouldMatch($"\\]8;id=[0-9]+;{pathAsRegEx}\\\\{pathAsRegEx}\\]8;;\\\\");
        }
    }
}
