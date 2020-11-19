using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    public sealed class FigletTests
    {
        [Fact]
        public async Task Should_Load_Font_From_Stream()
        {
            // Given
            var console = new PlainConsole(width: 180);
            var font = FigletFont.Load(ResourceReader.LoadResourceStream("Spectre.Console.Tests/Data/starwars.flf"));
            var text = new FigletText(font, "Patrik was here");

            // When
            console.Render(text);

            // Then
            await Verifier.Verify(console.Output);
        }

        [Fact]
        public async Task Should_Render_Text_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 70);
            var text = new FigletText(FigletFont.Default, "Patrik was here");

            // When
            console.Render(text);

            // Then
            await Verifier.Verify(console.Output);
        }

        [Fact]
        public async Task Should_Render_Wrapped_Text_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 70);
            var text = new FigletText(FigletFont.Default, "Spectre.Console");

            // When
            console.Render(text);

            // Then
            await Verifier.Verify(console.Output);
        }

        [Fact]
        public async Task Should_Render_Left_Aligned_Text_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 120);
            var text = new FigletText(FigletFont.Default, "Spectre.Console")
                .Alignment(Justify.Left);

            // When
            console.Render(text);

            // Then
            await Verifier.Verify(console.Output);
        }

        [Fact]
        public async Task Should_Render_Centered_Text_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 120);
            var text = new FigletText(FigletFont.Default, "Spectre.Console")
                .Alignment(Justify.Center);

            // When
            console.Render(text);

            // Then
            await Verifier.Verify(console.Output);
        }

        [Fact]
        public async Task Should_Render_Right_Aligned_Text_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 120);
            var text = new FigletText(FigletFont.Default, "Spectre.Console")
                .Alignment(Justify.Right);

            // When
            console.Render(text);

            // Then
            await Verifier.Verify(console.Output);
        }
    }
}
