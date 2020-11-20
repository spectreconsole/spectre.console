using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    public sealed class RecorderTests
    {
        [Fact]
        public Task Should_Export_Text_As_Expected()
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
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Export_Html_As_Expected()
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
            return Verifier.Verify(console.Lines);
        }
    }
}
