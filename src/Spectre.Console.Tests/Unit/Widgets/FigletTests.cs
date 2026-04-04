namespace Spectre.Console.Tests.Unit;

[ExpectationPath("Widgets/Figlet")]
public sealed class FigletTests
{
    [Theory]
    [InlineData("starwars.flf")]
    [InlineData("poison.flf")]
    [Expectation("Load_Stream")]
    public async Task Should_Load_Font_From_Stream(string fontfile)
    {
        // Given
        var console = new TestConsole().Width(180);
        var font = FigletFont.Load(EmbeddedResourceReader.LoadResourceStream($"Spectre.Console.Tests/Data/{fontfile}"));
        var text = new FigletText(font, "Patrik was here");

        // When
        console.Write(text);

        // Then
        await Verifier.Verify(console.Output)
            .UseParameters(fontfile);
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
            .Justify(Justify.Left);

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
            .Justify(Justify.Center);

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
            .Justify(Justify.Right);

        // When
        console.Write(text);

        // Then
        await Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Fitted")]
    public async Task Should_Render_Fitted_Text_Correctly()
    {
        // Given
        var console = new TestConsole().Width(120);
        var text = new FigletText(FigletFont.Default, "Spectre.Console")
        {
            LayoutMode = FigletLayoutMode.Fitted,
        };

        // When
        console.Write(text);

        // Then
        await Verifier.Verify(console.Output);
    }

    [Theory]
    [InlineData("starwars.flf")]
    [InlineData("poison.flf")]
    [Expectation("Render_Smushed_Universal")]
    public async Task Should_Render_Smushed_Text_Correctly_Using_Universal_Smushing_Rules(string fontfile)
    {
        // Given
        var console = new TestConsole().Width(120);
        var font = FigletFont.Load(EmbeddedResourceReader.LoadResourceStream($"Spectre.Console.Tests/Data/{fontfile}"));
        var text = new FigletText(font, "Spectre.Console")
        {
            LayoutMode = FigletLayoutMode.Smushed,
        };

        // When
        console.Write(text);

        // Then
        await Verifier.Verify(console.Output)
            .UseParameters(fontfile);
    }

    [Fact]
    [Expectation("Render_Smushed")]
    public async Task Should_Render_Smushed_Text_Correctly_Using_Font_Smushing_Rules()
    {
        // Given
        var console = new TestConsole().Width(120);
        var font = FigletFont.Load(EmbeddedResourceReader.LoadResourceStream("Spectre.Console.Tests/Data/banner.flf"));
        var text = new FigletText(font, "Spectre.Console")
        {
            LayoutMode = FigletLayoutMode.Smushed,
        };

        // When
        console.Write(text);

        // Then
        await Verifier.Verify(console.Output);
    }
}