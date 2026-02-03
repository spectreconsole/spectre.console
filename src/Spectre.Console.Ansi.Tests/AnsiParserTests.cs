namespace Spectre.Console.Ansi.Tests;

public sealed class AnsiParserTests
{
    [Fact(DisplayName = "esc: ESC ( B")]
    public void Esc_Sequence_1()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e(B");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Esc>()
            .And(esc =>
            {
                esc.Collect.ShouldBe(['(']);
                esc.Final.ShouldBe('B');
            });
    }

    [Fact(DisplayName = "csi: ESC [ H")]
    public void Csi_Sequence_1()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e[H");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Csi>()
            .And(csi =>
            {
                csi.Collect.Count.ShouldBe(0);
                csi.ParamsRaw.ShouldBe("");
                csi.Params.Count.ShouldBe(0);
                csi.Final.ShouldBe('H');
            });
    }

    [Fact(DisplayName = "csi: ESC [ 1 ; 4 H")]
    public void Csi_Sequence_2()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e[1;4H");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Csi>()
            .And(csi =>
            {
                csi.Collect.Count.ShouldBe(0);

                csi.ParamsRaw.ShouldBe("1;4");
                csi.Params.Count.ShouldBe(2);
                csi.Params[0].ShouldBe(1);
                csi.Params[1].ShouldBe(4);

                csi.Final.ShouldBe('H');
            });
    }

    [Fact(DisplayName = "csi: ESC [ 38 : 2 m")]
    public void Csi_Sequence_3()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e[38:2m");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Csi>()
            .And(csi =>
            {
                csi.Collect.Count.ShouldBe(0);

                csi.ParamsRaw.ShouldBe("38:2");
                csi.Params.Count.ShouldBe(2);
                csi.Params[0].ShouldBe(38);
                csi.Params[1].ShouldBe(2);

                csi.Final.ShouldBe('m');
            });
    }

    [Fact(DisplayName = "csi: ESC [ 38 ; 2 m")]
    public void Csi_Sequence_4()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e[38;2m");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Csi>()
            .And(csi =>
            {
                csi.Collect.Count.ShouldBe(0);

                csi.ParamsRaw.ShouldBe("38;2");
                csi.Params.Count.ShouldBe(2);
                csi.Params[0].ShouldBe(38);
                csi.Params[1].ShouldBe(2);

                csi.Final.ShouldBe('m');
            });
    }

    [Fact(DisplayName = "csi: ESC [ ? 2026 $ p")]
    public void Csi_Sequence_5()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e[?2026$p");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Csi>()
            .And(csi =>
            {
                csi.Collect.Count.ShouldBe(2);
                csi.Collect[0].ShouldBe('?');
                csi.Collect[1].ShouldBe('$');

                csi.ParamsRaw.ShouldBe("2026");
                csi.Params.Count.ShouldBe(1);
                csi.Params[0].ShouldBe(2026);

                csi.Final.ShouldBe('p');
            });
    }
}

internal sealed class AnsiParserFixture
{
    public static List<AnsiToken> Parse(string text)
    {
        var result = new List<AnsiToken>();
        var parser = new AnsiParser(token => result.Add(token));

        foreach (var character in text)
        {
            parser.Next(character);
        }

        return result;
    }
}