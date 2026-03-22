namespace Spectre.Console.Ansi.Tests;

public sealed class AnsiParserFixture
{
    public static List<AnsiToken> Parse(string text)
    {
        var result = new List<AnsiToken>();
        var parser = new AnsiParser(token => result.Add(token));

        parser.Next(text);

        return result;
    }
}