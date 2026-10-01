using System.Text;

namespace Spectre.Console.Tests.Unit;

public partial class AnsiConsoleTests
{
    public sealed class Size
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [GitHubIssue("https://github.com/spectreconsole/spectre.console/issues/2191")]
        public void Should_Fall_Back_To_Default_Size_When_Output_Reports_Non_Positive_Size(int size)
        {
            // Given, When
            var console = AnsiConsole.Create(new AnsiConsoleSettings
            {
                Out = new FixedSizeOutput(size, size),
            });

            // Then
            console.Profile.Width.ShouldBe(80);
            console.Profile.Height.ShouldBe(24);
        }

        [Fact]
        [GitHubIssue("https://github.com/spectreconsole/spectre.console/issues/2191")]
        public void Should_Not_Throw_When_Live_Display_Overflows_And_Output_Reports_Negative_Height()
        {
            // Given
            var console = AnsiConsole.Create(new AnsiConsoleSettings
            {
                Ansi = AnsiSupport.No,
                Interactive = InteractionSupport.No,
                Out = new FixedSizeOutput(80, -1),
            });

            var table = new Table().AddColumn("Resource");

            // When
            var result = Record.Exception(() =>
            {
                console.Live(table).Start(ctx =>
                {
                    for (var i = 0; i < 40; i++)
                    {
                        table.AddRow($"Row {i}");
                        ctx.Refresh();
                    }
                });
            });

            // Then
            result.ShouldBeNull();
        }

        private sealed class FixedSizeOutput : IAnsiConsoleOutput
        {
            public TextWriter Writer { get; } = new StringWriter();
            public bool IsTerminal => false;
            public int Width { get; }
            public int Height { get; }

            public FixedSizeOutput(int width, int height)
            {
                Width = width;
                Height = height;
            }

            public void SetEncoding(Encoding encoding)
            {
            }
        }
    }
}
