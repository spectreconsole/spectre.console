namespace Spectre.Console.Tests.Unit.Cli;

public sealed class CommandTreeTokenizerTests
{
    public sealed class ShortOptions
    {
        [Fact]
        public void Valueless()
        {
            // Given

            // When
            var result = CommandTreeTokenizer.Tokenize(new[] { "-a" });

            // Then
            result.Remaining.ShouldBeEmpty();
            result.Tokens.ShouldHaveSingleItem();

            var t = result.Tokens[0];
            t.TokenKind.ShouldBe(CommandTreeToken.Kind.ShortOption);
            t.IsGrouped.ShouldBe(false);
            t.Position.ShouldBe(0);
            t.Value.ShouldBe("a");
            t.Representation.ShouldBe("-a");
        }

        [Theory]
        [InlineData("-a:foo")]
        [InlineData("-a=foo")]
        public void With_Value(string param)
        {
            // Given

            // When
            var result = CommandTreeTokenizer.Tokenize(new[] { param });

            // Then
            result.Remaining.ShouldBeEmpty();
            result.Tokens.Count.ShouldBe(2);

            var t = result.Tokens.Consume();
            t.TokenKind.ShouldBe(CommandTreeToken.Kind.ShortOption);
            t.IsGrouped.ShouldBe(false);
            t.Position.ShouldBe(0);
            t.Value.ShouldBe("a");
            t.Representation.ShouldBe("-a");

            t = result.Tokens.Consume();
            t.TokenKind.ShouldBe(CommandTreeToken.Kind.String);
            t.IsGrouped.ShouldBe(false);
            t.Position.ShouldBe(3);
            t.Value.ShouldBe("foo");
            t.Representation.ShouldBe("foo");
        }

        [Theory]
        [InlineData("-a:-1.5", null)]
        [InlineData("-a=-1.5", null)]
        [InlineData("-a", "-1.5")]
        public void With_Negative_Numeric_Value(string firstArg, string secondArg)
        {
            // Given
            List<string> args = new List<string>();
            args.Add(firstArg);
            if (secondArg != null)
            {
                args.Add(secondArg);
            }

            // When
            var result = CommandTreeTokenizer.Tokenize(args);

            // Then
            result.Remaining.ShouldBeEmpty();
            result.Tokens.Count.ShouldBe(2);

            var t = result.Tokens.Consume();
            t.TokenKind.ShouldBe(CommandTreeToken.Kind.ShortOption);
            t.IsGrouped.ShouldBe(false);
            t.Position.ShouldBe(0);
            t.Value.ShouldBe("a");
            t.Representation.ShouldBe("-a");

            t = result.Tokens.Consume();
            t.TokenKind.ShouldBe(CommandTreeToken.Kind.String);
            t.IsGrouped.ShouldBe(false);
            t.Position.ShouldBe(3);
            t.Value.ShouldBe("-1.5");
            t.Representation.ShouldBe("-1.5");
        }

        [Fact]
        public void With_Negative_Numeric_Prefixed_String_Value()
        {
            // Given

            // When
            var result = CommandTreeTokenizer.Tokenize(new[] { "-6..2 " });

            // Then
            result.Remaining.ShouldBeEmpty();
            result.Tokens.ShouldHaveSingleItem();

            var t = result.Tokens[0];
            t.TokenKind.ShouldBe(CommandTreeToken.Kind.String);
            t.IsGrouped.ShouldBe(false);
            t.Position.ShouldBe(0);
            t.Value.ShouldBe("-6..2");
            t.Representation.ShouldBe("-6..2");
        }
    }
}
