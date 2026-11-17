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

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        [GitHubIssue("https://github.com/spectreconsole/spectre.console/issues/1867")]
        public void Should_Use_Output_Size_After_Resetting_Explicit_Size(bool isTerminal)
        {
            // Given
            var output = new FixedSizeOutput(120, 40) { IsTerminal = isTerminal };
            var console = AnsiConsole.Create(new AnsiConsoleSettings { Out = output });
            console.Profile.Width = 80;
            console.Profile.Height = 24;

            // When
            console.Profile.ResetSize();

            // Then
            console.Profile.Width.ShouldBe(120);
            console.Profile.Height.ShouldBe(40);

            // When
            output.Width = 160;
            output.Height = 50;

            // Then
            console.Profile.Width.ShouldBe(160);
            console.Profile.Height.ShouldBe(50);

            // When
            console.Profile.ResetSize();

            // Then
            console.Profile.Width.ShouldBe(160);
            console.Profile.Height.ShouldBe(50);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [GitHubIssue("https://github.com/spectreconsole/spectre.console/issues/1867")]
        public void Should_Fall_Back_To_Default_Size_After_Resetting_Explicit_Size(int size)
        {
            // Given
            var console = AnsiConsole.Create(new AnsiConsoleSettings
            {
                Out = new FixedSizeOutput(size, size),
            });
            console.Profile.Width = 100;
            console.Profile.Height = 50;

            // When
            console.Profile.ResetSize();

            // Then
            console.Profile.Width.ShouldBe(80);
            console.Profile.Height.ShouldBe(24);
        }

        [Theory]
        [InlineData(false, 80, 24)]
        [InlineData(true, 120, 40)]
        public void Should_Reset_Explicit_Size_Only_When_Switching_To_Terminal_Output(bool isTerminal, int width, int height)
        {
            // Given
            var console = AnsiConsole.Create(new AnsiConsoleSettings
            {
                Out = new FixedSizeOutput(100, 30),
            });
            console.Profile.Width = 80;
            console.Profile.Height = 24;

            // When
            console.Profile.Out = new FixedSizeOutput(120, 40) { IsTerminal = isTerminal };

            // Then
            console.Profile.Width.ShouldBe(width);
            console.Profile.Height.ShouldBe(height);
        }

        private sealed class FixedSizeOutput : IAnsiConsoleOutput
        {
            public TextWriter Writer { get; } = new StringWriter();
            public bool IsTerminal { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }

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
