namespace Spectre.Console.Tests.Unit;

public sealed class TablePromptTests
{
    [Fact]
    public void Should_Throw_If_Columns_Are_Null()
    {
        // Given
        var prompt = new TablePrompt<string>();

        // When
        var result = Record.Exception(() => prompt.AddColumns(null));

        // Then
        result.ShouldBeOfType<ArgumentNullException>()
            .ParamName.ShouldBe("columns");
    }
}