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

    [Fact(DisplayName = "osc 8: Hyperlink")]
    public void Osc_Sequence_1()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]8;;https://example.com\e\\");

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.HyperLinkStart>()
            .And(osc =>
            {
                osc.Id.ShouldBeNull();
                osc.Url.ShouldBe("https://example.com");
            });
    }

    [Fact(DisplayName = "osc 8: Hyperlink with ID")]
    public void Osc_Sequence_2()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]8;id=123;https://example.com\e\\");

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.HyperLinkStart>()
            .And(osc =>
            {
                osc.Id.ShouldBe("123");
                osc.Url.ShouldBe("https://example.com");
            });
    }

    [Fact(DisplayName = "osc 8: Hyperlink with empty ID")]
    public void Osc_Sequence_3()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]8;id=;https://example.com\e\\");

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.HyperLinkStart>()
            .And(osc =>
            {
                osc.Id.ShouldBeNull();
                osc.Url.ShouldBe("https://example.com");
            });
    }

    [Fact(DisplayName = "osc 8: Hyperlink with empty key")]
    public void Osc_Sequence_4()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]8;id;https://example.com\e\\");

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.HyperLinkStart>()
            .And(osc =>
            {
                osc.Id.ShouldBeNull();
                osc.Url.ShouldBe("https://example.com");
            });
    }

    [Fact(DisplayName = "osc 8: Hyperlink with empty key and id set")]
    public void Osc_Sequence_5()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]8;=value;id=foo;https://example.com\e\\");

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.HyperLinkStart>()
            .And(osc =>
            {
                osc.Id.ShouldBe("foo");
                osc.Url.ShouldBe("https://example.com");
            });
    }

    [Fact(DisplayName = "osc 8: Hyperlink with empty url")]
    public void Osc_Sequence_6()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]8;id=foo;\e\\");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Esc>();
    }

    [Fact(DisplayName = "osc 8: Hyperlink end")]
    public void Osc_Sequence_7()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]8;;\e\\");

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.HyperLinkEnd>();
    }

    [Fact(DisplayName = "osc 8: Hyperlink start and end")]
    public void Osc_Sequence_8()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]8;;https://example.com\e\\TEXT\e]8;;\e\\");

        // Then
        result.Count.ShouldBe(8);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.HyperLinkStart>();
        result[1].ShouldBeOfType<AnsiToken.Esc>();
        result[2].ShouldBeOfType<AnsiToken.Print>();
        result[3].ShouldBeOfType<AnsiToken.Print>();
        result[4].ShouldBeOfType<AnsiToken.Print>();
        result[5].ShouldBeOfType<AnsiToken.Print>();
        result[6].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.HyperLinkEnd>();
        result[7].ShouldBeOfType<AnsiToken.Esc>();
    }
}