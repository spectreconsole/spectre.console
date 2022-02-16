namespace Spectre.Console.Tests.Unit.Cli;

public static class CommandTreeTokenizerTests
{
    public sealed class ShortOptions
    {
        [Fact]
        public void Valueless()
        {
            var result = CommandTreeTokenizer.Tokenize(new[] { "-a" });
            Assert.Empty(result.Remaining);
            var t = Assert.Single(result.Tokens);

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
            var result = CommandTreeTokenizer.Tokenize(new[] { param });
            Assert.Empty(result.Remaining);
            Assert.Equal(2, result.Tokens.Count);

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
            List<string> args = new List<string>();
            args.Add(firstArg);
            if (secondArg != null)
            {
                args.Add(secondArg);
            }

            var result = CommandTreeTokenizer.Tokenize(args);
            Assert.Empty(result.Remaining);
            Assert.Equal(2, result.Tokens.Count);

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
            var result = CommandTreeTokenizer.Tokenize(new[] { "-6..2 " });
            Assert.Empty(result.Remaining);
            var t = Assert.Single(result.Tokens);
            t.TokenKind.ShouldBe(CommandTreeToken.Kind.String);
            t.IsGrouped.ShouldBe(false);
            t.Position.ShouldBe(0);
            t.Value.ShouldBe("-6..2");
            t.Representation.ShouldBe("-6..2");
        }
    }
}
