using System.Threading.Tasks;
using Spectre.Console.Testing;
using Spectre.Verify.Extensions;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    [ExpectationPath("Widgets/Recorder")]
    public sealed class RecorderTests
    {
        [Fact]
        [Expectation("Text")]
        public Task Should_Export_Text_As_Expected()
        {
            // Given
            var console = new TestConsole();
            var recorder = new Recorder(console);

            recorder.Write(new Table()
                .AddColumns("Foo", "Bar", "Qux")
                .AddRow("Corgi", "Waldo", "Zap")
                .AddRow(new Panel("Hello World").RoundedBorder()));

            // When
            var result = recorder.ExportText();

            // Then
            return Verifier.Verify(result);
        }

        [Fact]
        [Expectation("Html")]
        public Task Should_Export_Html_Text_As_Expected()
        {
            // Given
            var console = new TestConsole();
            var recorder = new Recorder(console);

            recorder.Write(new Table()
                .AddColumns("[red on black]Foo[/]", "[green bold]Bar[/]", "[blue italic]Qux[/]")
                .AddRow("[invert underline]Corgi[/]", "[bold strikethrough]Waldo[/]", "[dim]Zap[/]")
                .AddRow(new Panel("[blue]Hello World[/]")
                    .BorderColor(Color.Red).RoundedBorder()));

            // When
            var result = recorder.ExportHtml();

            // Then
            return Verifier.Verify(result);
        }
    }
}
