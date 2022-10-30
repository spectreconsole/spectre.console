namespace Spectre.Console.Tests.Unit.Cli.Parsing;

public class CommandTreeTokenizerTests
{
    [Theory]

    // Double-quote handling
    [InlineData("\"")]
    [InlineData("\"\"")]
    [InlineData("\"Rufus\"")]
    [InlineData("\" Rufus\"")]
    [InlineData("\"-R\"")]
    [InlineData("\"-Rufus\"")]
    [InlineData("\" -Rufus\"")]

    // Single-quote handling
    [InlineData("'")]
    [InlineData("''")]
    [InlineData("'Rufus'")]
    [InlineData("' Rufus'")]
    [InlineData("'-R'")]
    [InlineData("'-Rufus'")]
    [InlineData("' -Rufus'")]
    public void Should_Preserve_Quotes(string actualAndExpected)
    {
        // Given

        // When
        var result = CommandTreeTokenizer.Tokenize(new string[] { actualAndExpected });

        // Then
        result.Tokens.Count.ShouldBe(1);
        result.Tokens[0].Value.ShouldBe(actualAndExpected);
        result.Tokens[0].TokenKind.ShouldBe(CommandTreeToken.Kind.String);
    }

    [Theory]
    [InlineData("Rufus-")]
    [InlineData("Rufus--")]
    [InlineData("R-u-f-u-s")]
    public void Should_Preserve_Hyphen_Delimiters(string actualAndExpected)
    {
        // Given

        // When
        var result = CommandTreeTokenizer.Tokenize(new string[] { actualAndExpected });

        // Then
        result.Tokens.Count.ShouldBe(1);
        result.Tokens[0].Value.ShouldBe(actualAndExpected);
        result.Tokens[0].TokenKind.ShouldBe(CommandTreeToken.Kind.String);
    }

    [Theory]
    [InlineData(" Rufus")]
    [InlineData("Rufus ")]
    [InlineData(" Rufus ")]
    public void Should_Preserve_Spaces(string actualAndExpected)
    {
        // Given

        // When
        var result = CommandTreeTokenizer.Tokenize(new string[] { actualAndExpected });

        // Then
        result.Tokens.Count.ShouldBe(1);
        result.Tokens[0].Value.ShouldBe(actualAndExpected);
        result.Tokens[0].TokenKind.ShouldBe(CommandTreeToken.Kind.String);
    }

    [Theory]
    [InlineData(" \" -Rufus -- ")]
    [InlineData("Name=\" -Rufus --' ")]
    public void Should_Preserve_Quotes_Hyphen_Delimiters_Spaces(string actualAndExpected)
    {
        // Given

        // When
        var result = CommandTreeTokenizer.Tokenize(new string[] { actualAndExpected });

        // Then
        result.Tokens.Count.ShouldBe(1);
        result.Tokens[0].Value.ShouldBe(actualAndExpected);
        result.Tokens[0].TokenKind.ShouldBe(CommandTreeToken.Kind.String);
    }

    [Theory]
    [InlineData("--Name-")]

    [InlineData("--Name ")]
    [InlineData("--Name\"")]
    [InlineData("--Nam\"e")]
    [InlineData("-N-")]
    [InlineData("-N\"")]
    public void Should_Something(string actualAndExpected)
    {
        // Given

        // When
        var result = CommandTreeTokenizer.Tokenize(new string[] { actualAndExpected });

        // Then
        result.Tokens.Count.ShouldBe(1);
        result.Tokens[0].Value.ShouldBe(actualAndExpected);
    }
}
