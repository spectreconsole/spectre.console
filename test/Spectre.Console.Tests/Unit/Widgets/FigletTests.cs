using System.Threading.Tasks;
using Spectre.Console.Testing;
using Spectre.Verify.Extensions;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    [ExpectationPath("Widgets/Figlet")]
    public sealed class FigletTests
    {
        [Fact]
        [Expectation("Load_Stream")]
        public async Task Should_Load_Font_From_Stream()
        {
            // Given
            var console = new TestConsole().Width(180);
            var font = FigletFont.Load(EmbeddedResourceReader.LoadResourceStream("Spectre.Console.Tests/Data/starwars.flf"));
            var text = new FigletText(font, "Patrik was here");

            // When
            console.Write(text);

            // Then
            await Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Render")]
        public async Task Should_Render_Text_Correctly()
        {
            // Given
            var console = new TestConsole().Width(70);
            var text = new FigletText(FigletFont.Default, "Patrik was here");

            // When
            console.Write(text);

            // Then
            await Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Render_Wrapped")]
        public async Task Should_Render_Wrapped_Text_Correctly()
        {
            // Given
            var console = new TestConsole().Width(70);
            var text = new FigletText(FigletFont.Default, "Spectre.Console");

            // When
            console.Write(text);

            // Then
            await Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Render_LeftAligned")]
        public async Task Should_Render_Left_Aligned_Text_Correctly()
        {
            // Given
            var console = new TestConsole().Width(120);
            var text = new FigletText(FigletFont.Default, "Spectre.Console")
                .Alignment(Justify.Left);

            // When
            console.Write(text);

            // Then
            await Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Render_Centered")]
        public async Task Should_Render_Centered_Text_Correctly()
        {
            // Given
            var console = new TestConsole().Width(120);
            var text = new FigletText(FigletFont.Default, "Spectre.Console")
                .Alignment(Justify.Center);

            // When
            console.Write(text);

            // Then
            await Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Render_RightAligned")]
        public async Task Should_Render_Right_Aligned_Text_Correctly()
        {
            // Given
            var console = new TestConsole().Width(120);
            var text = new FigletText(FigletFont.Default, "Spectre.Console")
                .Alignment(Justify.Right);

            // When
            console.Write(text);

            // Then
            await Verifier.Verify(console.Output);
        }
    }
}
