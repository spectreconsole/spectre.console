namespace Spectre.Console.Tests.Unit.Cli.Parsing;

public class CommandTreeTokenizerTests
{
    public sealed class ScanString
    {
        [Theory]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData("\t")]
        [InlineData("\r\n\t")]
        [InlineData("👋🏻")]
        [InlineData("🐎👋🏻🔥❤️")]
        [InlineData("\"🐎👋🏻🔥❤️\" is an emoji sequence")]
        public void Should_Preserve_Edgecase_Inputs(string actualAndExpected)
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
    }

    public sealed class ScanLongOption
    {
        [Theory]
        [InlineData("--Name-", "Name-")]
        [InlineData("--Name_", "Name_")]
        public void Should_Allow_Hyphens_And_Underscores_In_Option_Name(string actual, string expected)
        {
            // Given

            // When
            var result = CommandTreeTokenizer.Tokenize(new string[] { actual });

            // Then
            result.Tokens.Count.ShouldBe(1);
            result.Tokens[0].Value.ShouldBe(expected);
            result.Tokens[0].TokenKind.ShouldBe(CommandTreeToken.Kind.LongOption);
        }

        [Theory]
        [InlineData("-- ")]
        [InlineData("--Name ")]
        [InlineData("--Name\"")]
        [InlineData("--Nam\"e")]
        public void Should_Throw_On_Invalid_Option_Name(string actual)
        {
            // Given

            // When
            var result = Record.Exception(() => CommandTreeTokenizer.Tokenize(new string[] { actual }));

            // Then
            result.ShouldBeOfType<CommandParseException>().And(ex =>
            {
                ex.Message.ShouldBe("Invalid long option name.");
            });
        }
    }

    public sealed class ScanShortOptions
    {
        [Theory]
        [InlineData("-N ", "N")]
        public void Should_Remove_Trailing_Spaces_In_Option_Name(string actual, string expected)
        {
            // Given

            // When
            var result = CommandTreeTokenizer.Tokenize(new string[] { actual });

            // Then
            result.Tokens.Count.ShouldBe(1);
            result.Tokens[0].Value.ShouldBe(expected);
            result.Tokens[0].TokenKind.ShouldBe(CommandTreeToken.Kind.ShortOption);
        }

        [Theory]
        [InlineData("-N-")]
        [InlineData("-N\"")]
        public void Should_Throw_On_Invalid_Option_Name(string actual)
        {
            // Given

            // When
            var result = Record.Exception(() => CommandTreeTokenizer.Tokenize(new string[] { actual }));

            // Then
            result.ShouldBeOfType<CommandParseException>().And(ex =>
            {
                ex.Message.ShouldBe("Short option does not have a valid name.");
            });
        }
    }

    [Theory]
    [InlineData("-")]
    [InlineData("- ")]
    public void Should_Throw_On_Missing_Option_Name(string actual)
    {
        // Given

        // When
        var result = Record.Exception(() => CommandTreeTokenizer.Tokenize(new string[] { actual }));

        // Then
        result.ShouldBeOfType<CommandParseException>().And(ex =>
        {
            ex.Message.ShouldBe("Option does not have a name.");
        });
    }
}
