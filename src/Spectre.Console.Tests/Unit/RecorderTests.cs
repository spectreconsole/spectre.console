using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class RecorderTests
    {
        [Fact]
        public void Should_Export_Text_As_Expected()
        {
            // Given
            var console = new PlainConsole();
            var recorder = new Recorder(console);

            recorder.Render(new Table()
                .AddColumns("Foo", "Bar", "Qux")
                .AddRow("Corgi", "Waldo", "Zap")
                .AddRow(new Panel("Hello World").RoundedBorder()));

            // When
            var result = recorder.ExportText().Split(new[] { '\n' });

            // Then
            result.Length.ShouldBe(8);
            result[0].ShouldBe("┌─────────────────┬───────┬─────┐");
            result[1].ShouldBe("│ Foo             │ Bar   │ Qux │");
            result[2].ShouldBe("├─────────────────┼───────┼─────┤");
            result[3].ShouldBe("│ Corgi           │ Waldo │ Zap │");
            result[4].ShouldBe("│ ╭─────────────╮ │       │     │");
            result[5].ShouldBe("│ │ Hello World │ │       │     │");
            result[6].ShouldBe("│ ╰─────────────╯ │       │     │");
            result[7].ShouldBe("└─────────────────┴───────┴─────┘");
        }

        [Fact]
        public void Should_Export_Html_As_Expected()
        {
            // Given
            var console = new PlainConsole();
            var recorder = new Recorder(console);

            recorder.Render(new Table()
                .AddColumns("[red on black]Foo[/]", "[green bold]Bar[/]", "[blue italic]Qux[/]")
                .AddRow("[invert underline]Corgi[/]", "[bold strikethrough]Waldo[/]", "[dim]Zap[/]")
                .AddRow(new Panel("[blue]Hello World[/]")
                    .BorderColor(Color.Red).RoundedBorder()));

            // When
            var html = recorder.ExportHtml();
            var result = html.Split(new[] { '\n' });

            // Then
            result.Length.ShouldBe(10);
            result[0].ShouldBe("<pre style=\"font-size:90%;font-family:consolas,'Courier New',monospace\">");
            result[1].ShouldBe("<span>┌─────────────────┬───────┬─────┐</span>");
            result[2].ShouldBe("<span>│ </span><span style=\"color: #FF0000;background-color: #000000\">Foo</span><span>             │ </span><span style=\"color: #008000;font-weight: bold;font-style: italic\">Bar</span><span>   │ </span><span style=\"color: #0000FF\">Qux</span><span> │</span>");
            result[3].ShouldBe("<span>├─────────────────┼───────┼─────┤</span>");
            result[4].ShouldBe("<span>│ </span><span style=\"text-decoration: underline\">Corgi</span><span>           │ </span><span style=\"font-weight: bold;font-style: italic;text-decoration: line-through\">Waldo</span><span> │ </span><span style=\"color: #7F7F7F\">Zap</span><span> │</span>");
            result[5].ShouldBe("<span>│ </span><span style=\"color: #FF0000\">╭─────────────╮</span><span> │       │     │</span>");
            result[6].ShouldBe("<span>│ </span><span style=\"color: #FF0000\">│</span><span> </span><span style=\"color: #0000FF\">Hello World</span><span> </span><span style=\"color: #FF0000\">│</span><span> │       │     │</span>");
            result[7].ShouldBe("<span>│ </span><span style=\"color: #FF0000\">╰─────────────╯</span><span> │       │     │</span>");
            result[8].ShouldBe("<span>└─────────────────┴───────┴─────┘</span>");
            result[9].ShouldBe("</pre>");
        }
    }
}
