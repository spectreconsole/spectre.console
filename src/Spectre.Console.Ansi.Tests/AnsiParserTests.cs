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
                esc.Intermediates.ShouldBe(['(']);
                esc.Final.ShouldBe('B');
            });
    }

    [Fact(DisplayName = "esc: ESC ESC [ A")]
    public void Esc_Repeated_Escape()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e\e[A");

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Execute>().And(e => e.Function.ShouldBe('\e'));
        result[1].ShouldBeOfType<AnsiToken.Csi>().And(csi => csi.Final.ShouldBe('A'));
    }

    [Fact(DisplayName = "esc: ESC that ends an OSC string, then ESC \\")]
    public void Esc_Repeated_After_Osc()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]0;title\e\e\\");

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Osc>();
        result[1].ShouldBeOfType<AnsiToken.Esc>().And(esc => esc.Final.ShouldBe('\\'));
    }

    [Fact(DisplayName = "esc: ESC \\ without a string")]
    public void Esc_Bare_String_Terminator()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e\\"); // Alt+\ in interactive input

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Esc>().And(esc => esc.Final.ShouldBe('\\'));
    }

    [Fact(DisplayName = "esc: ESC ä")]
    public void Esc_Non_Ascii_Final()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\eäb"); // Alt+ä, then b

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Esc>()
            .And(esc =>
            {
                esc.Intermediates.Count.ShouldBe(0);
                esc.Final.ShouldBe('ä');
            });
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('b'));
    }

    [Fact(DisplayName = "esc: ESC before an astral codepoint")]
    public void Esc_Astral_Codepoint()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e\U0001F600b");

        // Then
        result.Count.ShouldBe(3);
        result[0].ShouldBeOfType<AnsiToken.Execute>().And(e => e.Function.ShouldBe('\e'));
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe(0x1F600));
        result[2].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('b'));
    }

    [Fact(DisplayName = "esc: ESC before a lone low surrogate")]
    public void Esc_Lone_Low_Surrogate()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e\uDE00b");

        // Then
        result.Count.ShouldBe(3);
        result[0].ShouldBeOfType<AnsiToken.Execute>().And(e => e.Function.ShouldBe('\e'));
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe(0xFFFD));
        result[2].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('b'));
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
                csi.Intermediates.Count.ShouldBe(0);
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
                csi.Intermediates.Count.ShouldBe(0);

                csi.ParamsRaw.ShouldBe("1;4");
                csi.Params.ShouldBe([new(1), new(4)]);

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
                csi.Intermediates.Count.ShouldBe(0);

                csi.ParamsRaw.ShouldBe("38:2");
                csi.Params.ShouldBe([new(38, [2])]);

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
                csi.Intermediates.Count.ShouldBe(0);

                csi.ParamsRaw.ShouldBe("38;2");
                csi.Params.ShouldBe([new(38), new(2)]);

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
                csi.Intermediates.Count.ShouldBe(2);
                csi.Intermediates[0].ShouldBe('?');
                csi.Intermediates[1].ShouldBe('$');

                csi.ParamsRaw.ShouldBe("2026");
                csi.Params.ShouldBe([new(2026)]);

                csi.Final.ShouldBe('p');
            });
    }

    [Fact(DisplayName = "csi: ESC [ ; H")]
    public void Csi_Empty_Params()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e[;H");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Csi>()
            .And(csi =>
            {
                csi.ParamsRaw.ShouldBe(";");
                csi.Params.ShouldBe([new(0), new(0)]);
                csi.Final.ShouldBe('H');
            });
    }

    [Fact(DisplayName = "csi: ESC [ ;; H")]
    public void Csi_Multiple_Empty_Params()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e[;;H");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Csi>()
            .And(csi =>
            {
                csi.ParamsRaw.ShouldBe(";;");
                csi.Params.ShouldBe([new(0), new(0), new(0)]);
                csi.Final.ShouldBe('H');
            });
    }

    [Fact(DisplayName = "csi: parameter overflow saturates")]
    public void Csi_Parameter_Overflow_Saturates()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e[99999999999m");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Csi>()
            .And(csi =>
            {
                csi.Params.ShouldBe([new(65535)]);
                csi.Final.ShouldBe('m');
            });
    }

    [Fact(DisplayName = "csi: ESC [ 4 : 3 m differs from ESC [ 4 ; 3 m")]
    public void Csi_Subparameter_Differs_From_Parameter()
    {
        // Given, When
        var curly = AnsiParserFixture.Parse("\e[4:3m")[0]; // curly underline
        var italic = AnsiParserFixture.Parse("\e[4;3m")[0]; // underline, then italic

        // Then
        curly.ShouldBeOfType<AnsiToken.Csi>().Params.ShouldBe([new(4, [3])]);
        italic.ShouldBeOfType<AnsiToken.Csi>().Params.ShouldBe([new(4), new(3)]);
    }

    [Fact(DisplayName = "csi: ESC [ 97 ; 5 : 1 u")]
    public void Csi_Kitty_Keyboard_Subparameters()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e[97;5:1u"); // kitty keyboard: Ctrl+a pressed

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Csi>()
            .And(csi =>
            {
                csi.Params.ShouldBe([new(97), new(5, [1])]);
                csi.Final.ShouldBe('u');
            });
    }

    [Fact(DisplayName = "csi: ESC [ 38 : 2 : : 255 : 0 : 0 m")]
    public void Csi_Empty_Subparameter()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e[38:2::255:0:0m"); // RGB without a color space ID

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Csi>()
            .And(csi => csi.Params.ShouldBe([new(38, [2, 0, 255, 0, 0])]));
    }

    [Fact(DisplayName = "csi: ESC [ : 3 m")]
    public void Csi_Leading_Subparameter()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e[:3m");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Csi>()
            .And(csi => csi.Params.ShouldBe([new(0, [3])]));
    }

    [Fact(DisplayName = "dcs: ESC P 1 $ r")]
    public void Dcs_Hook_Parameters()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\eP1$r0m\e\\"); // DECRPSS reply

        // Then
        result[0].ShouldBeOfType<AnsiToken.DcsHook>()
            .And(hook =>
            {
                hook.Intermediates.ShouldBe(['$']);
                hook.Params.ShouldBe([new(1)]);
                hook.Final.ShouldBe('r');
            });
    }

    [Fact(DisplayName = "dcs: terminated by ESC \\")]
    public void Dcs_Terminated_By_String_Terminator()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\eP1$r0m\e\\");

        // Then
        result.Count.ShouldBe(4);
        result[0].ShouldBeOfType<AnsiToken.DcsHook>();
        result[1].ShouldBeOfType<AnsiToken.DcsPut>().And(put => put.Code.ShouldBe('0'));
        result[2].ShouldBeOfType<AnsiToken.DcsPut>().And(put => put.Code.ShouldBe('m'));
        result[3].ShouldBeOfType<AnsiToken.DcsUnhook>();
    }

    [Fact(DisplayName = "apc: ignored up to ESC \\")]
    public void Apc_Ignored()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e_Gi=31;OK\e\\a"); // kitty graphics reply, then a

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('a'));
    }

    [Fact(DisplayName = "token: identical CSI tokens are equal")]
    public void Token_Csi_Value_Equality()
    {
        // Given, When
        var a = AnsiParserFixture.Parse("\e[1;2m")[0];
        var b = AnsiParserFixture.Parse("\e[1;2m")[0];

        // Then
        a.ShouldBe(b);
        a.GetHashCode().ShouldBe(b.GetHashCode());
    }

    [Fact(DisplayName = "token: differing CSI tokens are not equal")]
    public void Token_Csi_Value_Inequality()
    {
        // Given, When
        var a = AnsiParserFixture.Parse("\e[1;2m")[0];
        var b = AnsiParserFixture.Parse("\e[1;3m")[0];

        // Then
        a.ShouldNotBe(b);
    }

    [Fact(DisplayName = "token: identical ESC tokens are equal")]
    public void Token_Esc_Value_Equality()
    {
        // Given, When
        var a = AnsiParserFixture.Parse("\e(B")[0];
        var b = AnsiParserFixture.Parse("\e(B")[0];

        // Then
        a.ShouldBe(b);
        a.GetHashCode().ShouldBe(b.GetHashCode());
    }

    [Fact(DisplayName = "flush: emits a trailing lone high surrogate")]
    public void Flush_Trailing_High_Surrogate()
    {
        // Given
        var result = new List<AnsiToken>();
        var parser = new AnsiParser(result.Add);
        parser.Next((char)0xD83D); // lone high surrogate, buffered awaiting its pair

        // When
        result.Count.ShouldBe(0);
        parser.Flush();

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe(0xFFFD));
    }

    [Fact(DisplayName = "flush: emits a lone ESC")]
    public void Flush_Lone_Escape()
    {
        // Given
        var result = new List<AnsiToken>();
        var parser = new AnsiParser(result.Add);
        parser.Next("\e"); // a single press of the Escape key

        // When
        parser.Flush();
        parser.Next("a"); // prints, since the parser is back in ground

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Execute>().And(e => e.Function.ShouldBe('\e'));
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('a'));
    }

    [Fact(DisplayName = "flush: keeps a partial CSI")]
    public void Flush_Keeps_Partial_Csi()
    {
        // Given
        var result = new List<AnsiToken>();
        var parser = new AnsiParser(result.Add);
        parser.Next("\e[1");

        // When
        parser.Flush();
        parser.Next("A");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Csi>()
            .And(csi =>
            {
                csi.Params.ShouldBe([new(1)]);
                csi.Final.ShouldBe('A');
            });
    }

    [Fact(DisplayName = "flush: keeps the ESC that ends an OSC string")]
    public void Flush_Keeps_Osc_Terminator()
    {
        // Given
        var result = new List<AnsiToken>();
        var parser = new AnsiParser(result.Add);
        parser.Next("\e]0;title\e"); // the backslash of the ST hasn't arrived yet

        // When
        parser.Flush();
        parser.Next("\\");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Osc>();
    }

    [Fact(DisplayName = "reset: recovers mid-sequence")]
    public void Reset_Mid_Sequence()
    {
        // Given
        var result = new List<AnsiToken>();
        var parser = new AnsiParser(result.Add);
        parser.Next("\e[12"); // an unterminated CSI
        result.Count.ShouldBe(0);

        // When
        parser.Reset();
        parser.Next("A"); // prints from Ground

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('A'));
    }

    [Fact(DisplayName = "reset: discards a pending high surrogate")]
    public void Reset_Discards_Pending_Surrogate()
    {
        // Given
        var result = new List<AnsiToken>();
        var parser = new AnsiParser(result.Add);
        parser.Next((char)0xD83D); // pending high surrogate

        // When
        parser.Reset();
        parser.Flush();

        // Then
        result.ShouldBeEmpty();
    }

    [Fact(DisplayName = "osc 8: Hyperlink")]
    public void Osc_Sequence_1()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]8;;https://example.com\e\\");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.HyperLinkStart>()
            .And(osc =>
            {
                osc.Id.ShouldBeNull();
                osc.Uri.ShouldBe("https://example.com");
            });
    }

    [Fact(DisplayName = "osc 8: Hyperlink with ID")]
    public void Osc_Sequence_2()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]8;id=123;https://example.com\e\\");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.HyperLinkStart>()
            .And(osc =>
            {
                osc.Id.ShouldBe("123");
                osc.Uri.ShouldBe("https://example.com");
            });
    }

    [Fact(DisplayName = "osc 8: Hyperlink with empty ID")]
    public void Osc_Sequence_3()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]8;id=;https://example.com\e\\");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.HyperLinkStart>()
            .And(osc =>
            {
                osc.Id.ShouldBeNull();
                osc.Uri.ShouldBe("https://example.com");
            });
    }

    [Fact(DisplayName = "osc 8: Hyperlink with empty key")]
    public void Osc_Sequence_4()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]8;id;https://example.com\e\\");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.HyperLinkStart>()
            .And(osc =>
            {
                osc.Id.ShouldBeNull();
                osc.Uri.ShouldBe("https://example.com");
            });
    }

    [Fact(DisplayName = "osc 8: Hyperlink with empty key but id set")]
    public void Osc_Sequence_5()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]8;=value:id=foo;https://example.com\e\\");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.HyperLinkStart>()
            .And(osc =>
            {
                osc.Id.ShouldBe("foo");
                osc.Uri.ShouldBe("https://example.com");
            });
    }

    [Fact(DisplayName = "osc 8: Hyperlink with empty url")]
    public void Osc_Sequence_6()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]8;id=foo;\e\\");

        // Then
        result.ShouldBeEmpty();
    }

    [Fact(DisplayName = "osc 8: Hyperlink end")]
    public void Osc_Sequence_7()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]8;;\e\\");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.HyperLinkEnd>();
    }

    [Fact(DisplayName = "osc 8: Hyperlink start and end")]
    public void Osc_Sequence_8()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]8;;https://example.com\e\\TEXT\e]8;;\e\\");

        // Then
        result.Count.ShouldBe(6);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.HyperLinkStart>();
        result[1].ShouldBeOfType<AnsiToken.Print>();
        result[2].ShouldBeOfType<AnsiToken.Print>();
        result[3].ShouldBeOfType<AnsiToken.Print>();
        result[4].ShouldBeOfType<AnsiToken.Print>();
        result[5].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.HyperLinkEnd>();
    }

    [Fact(DisplayName = "osc 123: Unknown")]
    public void Osc_Sequence_9()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]123;;lol\e\\");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.Unknown>()
            .And(osc =>
            {
                osc.Data.ShouldBe("123;;lol");
            });
    }

    [Fact(DisplayName = "osc 8: Hyperlink with semicolon in URI")]
    public void Osc_Hyperlink_Semicolon_In_Uri()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]8;;http://example.com/a;b\e\\");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.HyperLinkStart>()
            .And(osc =>
            {
                osc.Id.ShouldBeNull();
                osc.Uri.ShouldBe("http://example.com/a;b");
            });
    }

    [Fact(DisplayName = "osc 8: Hyperlink with colon-separated params")]
    public void Osc_Hyperlink_Colon_Separated_Params()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]8;id=abc:foo=bar;http://x\e\\");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.HyperLinkStart>()
            .And(osc =>
            {
                osc.Id.ShouldBe("abc");
                osc.Uri.ShouldBe("http://x");
            });
    }

    [Fact(DisplayName = "osc 0: terminated by BEL")]
    public void Osc_Terminated_By_BEL()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]0;title\a"); // OSC 0 window title, BEL terminator

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.Unknown>()
            .And(osc => osc.Data.ShouldBe("0;title"));
    }

    [Fact(DisplayName = "osc 8: Hyperlink terminated by BEL")]
    public void Osc_Hyperlink_Terminated_By_BEL()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]8;;https://example.com\a");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.HyperLinkStart>()
            .And(osc =>
            {
                osc.Id.ShouldBeNull();
                osc.Uri.ShouldBe("https://example.com");
            });
    }

    [Fact(DisplayName = "osc: BEL terminator returns to ground")]
    public void Osc_BEL_Returns_To_Ground()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]0;t\aX"); // OSC ... BEL, then a printable

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Osc>();
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('X'));
    }

    [Fact(DisplayName = "osc: buffers are reused across sequences without leaking")]
    public void Osc_Reuses_Buffers_Without_Leaking()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]123;longlonglong\a\e]9;x\a");

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.Unknown>()
            .And(osc => osc.Data.ShouldBe("123;longlonglong"));
        result[1].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.Unknown>()
            .And(osc => osc.Data.ShouldBe("9;x"));
    }

    [Fact(DisplayName = "osc: CAN aborts without dispatching")]
    public void Osc_Can_Aborts()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]0;title\u0018"); // CAN mid-OSC

        // Then
        result.Exists(t => t is AnsiToken.Osc).ShouldBeFalse();
        result.Exists(t => t is AnsiToken.Execute e && e.Function == '\u0018').ShouldBeTrue();
    }

    [Fact(DisplayName = "print: accented latin")]
    public void Print_Accented_Latin()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("aä"); // "aä"

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('a'));
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe(0x00E4));
    }

    [Fact(DisplayName = "print: box-drawing characters")]
    public void Print_Box_Drawing()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("─│┌"); // "─│┌"

        // Then
        result.Count.ShouldBe(3);
        result[0].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe(0x2500));
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe(0x2502));
        result[2].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe(0x250C));
    }

    [Fact(DisplayName = "print: CJK character")]
    public void Print_Cjk()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("日"); // "日"

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe(0x65E5));
    }

    [Fact(DisplayName = "print: astral codepoint combines surrogate pair")]
    public void Print_Astral()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\U0001F600"); // "😀"

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe(0x1F600));
    }

    [Fact(DisplayName = "print: astral codepoint between ascii")]
    public void Print_Astral_Between_Ascii()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("a\U0001F600b");

        // Then
        result.Count.ShouldBe(3);
        result[0].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('a'));
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe(0x1F600));
        result[2].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('b'));
    }

    [Fact(DisplayName = "print: non-ascii resumes after CSI")]
    public void Print_Resumes_After_Csi()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e[0m─"); // SGR reset, then "─"

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Csi>().And(csi => csi.Final.ShouldBe('m'));
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe(0x2500));
    }

    [Fact(DisplayName = "print: ToUtf16 encodes a BMP codepoint as one char")]
    public void Print_ToUtf16_Bmp()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("─"); // "─"

        // Then
        var print = result[0].ShouldBeOfType<AnsiToken.Print>();
        print.ToUtf16().ShouldBe("─");
        print.ToUtf16().Length.ShouldBe(1);
    }

    [Fact(DisplayName = "print: ToUtf16 encodes an astral codepoint as a surrogate pair")]
    public void Print_ToUtf16_Astral()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\U0001F600"); // "😀"

        // Then
        var print = result[0].ShouldBeOfType<AnsiToken.Print>();
        print.ToUtf16().ShouldBe("\U0001F600");
        print.ToUtf16().Length.ShouldBe(2);
    }
}