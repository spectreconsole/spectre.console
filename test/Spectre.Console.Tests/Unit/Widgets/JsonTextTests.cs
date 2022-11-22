namespace Spectre.Console.Tests.Unit;

[UsesVerify]
[ExpectationPath("Widgets/Json")]
public sealed class JsonTextTests
{
    [Fact]
    [Expectation("Render_Json")]
    public Task Should_Render_Json()
    {
        // Given
        var console = new TestConsole().Size(new Size(80, 15));
        var json = EmbeddedResourceReader
            .LoadResourceStream("Spectre.Console.Tests/Data/example.json")
            .ReadText();

        // When
        console.Write(new Panel(new JsonText(json)).Header("Some JSON"));

        // Then
        return Verifier.Verify(console.Output);
    }
}
