namespace Spectre.Console.Tests.Unit.Cli.Parsing;

public class CommandTreeTokenizerTests
{
    [Theory]
    [InlineData("\"\"")]
    [InlineData("\" \"")]

    // Special character handling
    [InlineData("-R")]
    [InlineData("-Rufus")]
    [InlineData("--R")]
    [InlineData("--Rufus")]

    // Double-quote handling
    [InlineData("\"Rufus\"")]
    [InlineData("\" Rufus\"")]
    [InlineData("\"-R\"")]
    [InlineData("\"-Rufus\"")]
    [InlineData("\" -Rufus\"")]

    // Single-quote handling
    [InlineData("'Rufus'")]
    [InlineData("' Rufus'")]
    [InlineData("'-R'")]
    [InlineData("'-Rufus'")]
    [InlineData("' -Rufus'")]
    public void Quote_Tests(string actualAndExpected)
    {
        // Given

        // When
        var result = CommandTreeTokenizer.Tokenize(new string[] { actualAndExpected });

        // Then
        result.Tokens.Count.ShouldBe(1);
        result.Tokens[0].Value.ShouldBe(actualAndExpected);
    }
}
