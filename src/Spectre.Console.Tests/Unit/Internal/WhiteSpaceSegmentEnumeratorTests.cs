namespace Spectre.Console.Tests.Unit.Internal;

public class WhiteSpaceSegmentEnumeratorTests
{
    public class TheoryData : TheoryData<string, string[]>
    {
        public TheoryData()
        {
            Add("", []);
            Add(" ", [" "]);
            Add("a", ["a"]);
            Add("a ", ["a", " "]);
            Add("a  ", ["a", "  "]);
            Add(" a  ", [" ", "a", "  "]);
            Add("a \n", ["a", " \n"]);
            Add("a \nb", ["a", " \n", "b"]);
        }
    }

    public static readonly TheoryData Data = new();

    [Theory]
    [MemberData(nameof(Data))]
    public void Should_Produce_Expected_Segments(string input, string[] expected)
    {
        var splitWords = input.SplitWords();
        var segmenter = new WhiteSpaceSegmentEnumerator(input);

        splitWords.ShouldBeEquivalentTo(expected);
        var index = 0;
        foreach (var segment in segmenter)
        {
            segment.ToString().ShouldBeEquivalentTo(expected[index++]);
        }
    }
}