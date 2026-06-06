namespace Spectre.Console.Tests.Unit;

[ExpectationPath("Widgets/Figlet")]
public sealed class FigletTests
{
    [Theory]
    [InlineData(FigletTestFont.Big)]
    [InlineData(FigletTestFont.StarWars)]
    [InlineData(FigletTestFont.Poison)]
    [Expectation("Load_Stream")]
    public async Task Should_Load_Font_From_Stream(FigletTestFont name)
    {
        // Given
        var console = new TestConsole().Width(180);
        var font = FigletTestFontLoader.Load(name);
        var text = new FigletText(font, "Patrik was here");

        // When
        console.Write(text);

        // Then
        await Verifier.Verify(console.Output)
            .UseParameters(
                name.ToString().ToLowerInvariant());
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
    [Expectation("Render_Single")]
    public async Task Should_Render_Single_Character()
    {
        // Given
        var console = new TestConsole().Width(120);
        var font = FigletTestFontLoader.Load(FigletTestFont.Big);
        var text = new FigletText(font, "P");

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
    [InlineData(FigletTestFont.Big)]
    [InlineData(FigletTestFont.StarWars)]
    [InlineData(FigletTestFont.Poison)]
    [Expectation("Render_Smushed_Universal")]
    public async Task Should_Render_Smushed_Text_Correctly_Using_Universal_Smushing_Rules(FigletTestFont name)
    {
        // Given
        var console = new TestConsole().Width(120);
        var font = FigletTestFontLoader.Load(name);
        var text = new FigletText(font, "Spectre.Console")
        {
            LayoutMode = FigletLayoutMode.Smushed,
        };

        // When
        console.Write(text);

        // Then
        await Verifier.Verify(console.Output)
            .UseParameters(
                name.ToString().ToLowerInvariant());
    }

    [Fact]
    [Expectation("Render_Smushed")]
    public async Task Should_Render_Smushed_Text_Correctly_Using_Font_Smushing_Rules()
    {
        // Given
        var console = new TestConsole().Width(120);
        var font = FigletTestFontLoader.Load(FigletTestFont.Big);
        var text = new FigletText(font, "Spectre.Console")
        {
            LayoutMode = FigletLayoutMode.Smushed,
        };

        // When
        console.Write(text);

        // Then
        await Verifier.Verify(console.Output);
    }

    [Theory]
    [InlineData(FigletLayoutMode.FullSize)]
    [InlineData(FigletLayoutMode.Fitted)]
    [InlineData(FigletLayoutMode.Smushed)]
    public void Should_Render_Empty_String(FigletLayoutMode mode)
    {
        // Given
        var console = new TestConsole().Width(120);
        var text = new FigletText(FigletFont.Default, string.Empty)
        {
            LayoutMode = mode,
        };

        // When
        console.Write(text);

        // Then
        console.Output.ShouldBeEmpty();
    }

    [Theory]
    [InlineData(FigletLayoutMode.FullSize)]
    [InlineData(FigletLayoutMode.Fitted)]
    [InlineData(FigletLayoutMode.Smushed)]
    public void Should_Not_Render_Non_Existent_Glyph(FigletLayoutMode mode)
    {
        // Given
        var console = new TestConsole().Width(120);
        var text = new FigletText(FigletFont.Default, "😄")
        {
            LayoutMode = mode,
        };

        // When
        console.Write(text);

        // Then
        console.Output.ShouldBeEmpty();
    }

    [Fact]
    [Expectation("Figlet_Report")]
    public async Task Figlet_Report()
    {
        // Given, When
        var report = FigletReportGenerator.Generate();

        // Then
        await Verifier.Verify(report);
    }
}