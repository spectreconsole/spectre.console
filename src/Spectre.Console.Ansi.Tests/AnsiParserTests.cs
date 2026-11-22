namespace Spectre.Console.Ansi.Tests;

public sealed class AnsiParserTests
{
    [Fact]
    public void Should_Parse_Esc_With_Intermediate()
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

    [Fact]
    public void Should_Report_First_Esc_When_Another_Esc_Follows()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e\e[A");

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Execute>().And(e => e.Function.ShouldBe('\e'));
        result[1].ShouldBeOfType<AnsiToken.Csi>().And(csi => csi.Final.ShouldBe('A'));
    }

    [Fact]
    public void Should_Drop_Osc_Terminator_Esc_When_Another_Esc_Follows()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]0;title\e\e\\");

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Osc>();
        result[1].ShouldBeOfType<AnsiToken.Esc>().And(esc => esc.Final.ShouldBe('\\'));
    }

    [Fact]
    public void Should_Parse_String_Terminator_Without_String_As_Esc()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e\\"); // Alt+\ in interactive input

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Esc>().And(esc => esc.Final.ShouldBe('\\'));
    }

    [Fact]
    public void Should_Use_Non_Ascii_Character_After_Esc_As_Final()
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

    [Fact]
    public void Should_Report_Esc_Before_Astral_Codepoint()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e\U0001F600b");

        // Then
        result.Count.ShouldBe(3);
        result[0].ShouldBeOfType<AnsiToken.Execute>().And(e => e.Function.ShouldBe('\e'));
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe(0x1F600));
        result[2].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('b'));
    }

    [Fact]
    public void Should_Report_Esc_Before_Lone_Low_Surrogate()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e\uDE00b");

        // Then
        result.Count.ShouldBe(3);
        result[0].ShouldBeOfType<AnsiToken.Execute>().And(e => e.Function.ShouldBe('\e'));
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe(0xFFFD));
        result[2].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('b'));
    }

    [Fact]
    public void Should_Parse_Csi_Without_Parameters()
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

    [Fact]
    public void Should_Parse_Csi_Parameters()
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

    [Fact]
    public void Should_Parse_Csi_Subparameter()
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

    [Fact]
    public void Should_Parse_Semicolon_Separated_Csi_Parameters()
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

    [Fact]
    public void Should_Parse_Csi_Private_Marker_And_Intermediate()
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

    [Fact]
    public void Should_Parse_Empty_Csi_Parameters_As_Zero()
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

    [Fact]
    public void Should_Parse_Multiple_Empty_Csi_Parameters_As_Zero()
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

    [Fact]
    public void Should_Saturate_Csi_Parameter_Overflow()
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

    [Fact]
    public void Should_Distinguish_Csi_Subparameters_From_Parameters()
    {
        // Given, When
        var curly = AnsiParserFixture.Parse("\e[4:3m")[0]; // curly underline
        var italic = AnsiParserFixture.Parse("\e[4;3m")[0]; // underline, then italic

        // Then
        curly.ShouldBeOfType<AnsiToken.Csi>().Params.ShouldBe([new(4, [3])]);
        italic.ShouldBeOfType<AnsiToken.Csi>().Params.ShouldBe([new(4), new(3)]);
    }

    [Fact]
    public void Should_Parse_Kitty_Keyboard_Subparameters()
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

    [Fact]
    public void Should_Parse_Empty_Csi_Subparameter_As_Zero()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e[38:2::255:0:0m"); // RGB without a color space ID

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Csi>()
            .And(csi => csi.Params.ShouldBe([new(38, [2, 0, 255, 0, 0])]));
    }

    [Fact]
    public void Should_Parse_Leading_Csi_Subparameter()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e[:3m");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Csi>()
            .And(csi => csi.Params.ShouldBe([new(0, [3])]));
    }

    [Fact]
    public void Should_Ignore_Malformed_Csi()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e[1?ma"); // a private marker after a parameter

        // Then
        result.Single().ShouldBeOfType<AnsiToken.Print>().Codepoint.ShouldBe('a');
    }

    [Fact]
    public void Should_Execute_C0_Control_Inside_Csi()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e[1\nm");

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Execute>().And(e => e.Function.ShouldBe('\n'));
        result[1].ShouldBeOfType<AnsiToken.Csi>()
            .And(csi =>
            {
                csi.Params.ShouldBe([new(1)]);
                csi.Final.ShouldBe('m');
            });
    }

    [Fact]
    public void Should_Abort_Csi_On_Can()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e[1\u0018m");

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Execute>().And(e => e.Function.ShouldBe('\u0018'));
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('m'));
    }

    [Fact]
    public void Should_Drop_Csi_Over_Parameter_Limit()
    {
        // Given
        static string Sequence(int count, char separator)
        {
            return "\e[" + string.Join(separator, Enumerable.Repeat("1", count)) + "m";
        }

        // When
        var atLimit = AnsiParserFixture.Parse(Sequence(32, ';'));
        var overLimit = AnsiParserFixture.Parse(Sequence(33, ';') + "a");
        var overLimitWithSubparameters = AnsiParserFixture.Parse(Sequence(33, ':') + "a");

        // Then
        atLimit.Single().ShouldBeOfType<AnsiToken.Csi>().Params.Count.ShouldBe(32);
        overLimit.Single().ShouldBeOfType<AnsiToken.Print>().Codepoint.ShouldBe('a');
        overLimitWithSubparameters.Single().ShouldBeOfType<AnsiToken.Print>().Codepoint.ShouldBe('a');
    }

    [Fact]
    public void Should_Drop_Csi_Over_Parameter_Character_Limit()
    {
        // Given, When
        var atLimit = AnsiParserFixture.Parse("\e[" + new string('1', 256) + "m");
        var overLimit = AnsiParserFixture.Parse("\e[" + new string('1', 257) + "ma");

        // Then
        atLimit.Single().ShouldBeOfType<AnsiToken.Csi>().Params.ShouldBe([new(65535)]);
        overLimit.Single().ShouldBeOfType<AnsiToken.Print>().Codepoint.ShouldBe('a');
    }

    [Fact]
    public void Should_Drop_Sequence_Over_Intermediate_Limit()
    {
        // Given, When
        var atLimit = AnsiParserFixture.Parse("\e$(C");
        var overLimit = AnsiParserFixture.Parse("\e$((Ca");
        var csiOverLimit = AnsiParserFixture.Parse("\e[?1$ pa"); // the private marker counts

        // Then
        atLimit.Single().ShouldBeOfType<AnsiToken.Esc>().Intermediates.ShouldBe(['$', '(']);
        overLimit.Single().ShouldBeOfType<AnsiToken.Print>().Codepoint.ShouldBe('a');
        csiOverLimit.Single().ShouldBeOfType<AnsiToken.Print>().Codepoint.ShouldBe('a');
    }

    [Fact]
    public void Should_Parse_Dcs_Hook_Parameters()
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

    [Fact]
    public void Should_Consume_Dcs_String_Terminator()
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

    [Fact]
    public void Should_Ignore_Dcs_Over_Parameter_Limit()
    {
        // Given, When
        var result = AnsiParserFixture.Parse(
            "\eP" + string.Join(';', Enumerable.Repeat("1", 33)) + "qdata\e\\a");

        // Then
        result.Single().ShouldBeOfType<AnsiToken.Print>().Codepoint.ShouldBe('a');
    }

    [Fact]
    public void Should_Ignore_Apc_String()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e_Gi=31;OK\e\\a"); // kitty graphics reply, then a

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('a'));
    }

    [Fact]
    public void Should_Ignore_Sos_And_Pm_Strings()
    {
        // Given, When
        var sos = AnsiParserFixture.Parse("\eXstring\e\\a");
        var pm = AnsiParserFixture.Parse("\e^string\e\\a");

        // Then
        sos.Single().ShouldBeOfType<AnsiToken.Print>().Codepoint.ShouldBe('a');
        pm.Single().ShouldBeOfType<AnsiToken.Print>().Codepoint.ShouldBe('a');
    }

    [Fact]
    public void Should_Parse_C1_Csi_Introducer()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\u009b1m"); // U+009B is CSI

        // Then
        result.Single().ShouldBeOfType<AnsiToken.Csi>()
            .And(csi =>
            {
                csi.Params.ShouldBe([new(1)]);
                csi.Final.ShouldBe('m');
            });
    }

    [Fact]
    public void Should_Parse_C1_Osc_Introducer_And_String_Terminator()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\u009d0;title\u009ca"); // U+009D is OSC, U+009C is ST

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.Unknown>()
            .And(osc => osc.Data.ShouldBe("0;title"));
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('a'));
    }

    [Fact]
    public void Should_Start_New_Osc_String_On_C1_Osc_Introducer()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]0;a\u009d2;b\a");

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.Unknown>()
            .And(osc => osc.Data.ShouldBe("0;a"));
        result[1].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.Unknown>()
            .And(osc => osc.Data.ShouldBe("2;b"));
    }

    [Fact]
    public void Should_Consider_Identical_Csi_Tokens_Equal()
    {
        // Given, When
        var a = AnsiParserFixture.Parse("\e[1;2m")[0];
        var b = AnsiParserFixture.Parse("\e[1;2m")[0];

        // Then
        a.ShouldBe(b);
        a.GetHashCode().ShouldBe(b.GetHashCode());
    }

    [Fact]
    public void Should_Not_Consider_Different_Csi_Tokens_Equal()
    {
        // Given, When
        var a = AnsiParserFixture.Parse("\e[1;2m")[0];
        var b = AnsiParserFixture.Parse("\e[1;3m")[0];

        // Then
        a.ShouldNotBe(b);
    }

    [Fact]
    public void Should_Consider_Identical_Esc_Tokens_Equal()
    {
        // Given, When
        var a = AnsiParserFixture.Parse("\e(B")[0];
        var b = AnsiParserFixture.Parse("\e(B")[0];

        // Then
        a.ShouldBe(b);
        a.GetHashCode().ShouldBe(b.GetHashCode());
    }

    [Fact]
    public void Should_Seal_Token_Types()
    {
        // Given
        Type[] bases = [typeof(AnsiToken), typeof(OscCommand)];

        // When
        var nested = bases.SelectMany(type => type.GetNestedTypes()).ToList();
        var constructors = bases
            .Select(type => type.GetConstructor(
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                Type.EmptyTypes))
            .ToList();

        // Then
        nested.Count.ShouldBe(13);
        nested.ShouldAllBe(type => type.IsSealed);
        constructors.ShouldAllBe(constructor => constructor != null && constructor.IsPrivate);
    }

    [Fact]
    public void Should_Print_List_Items_In_ToString()
    {
        // Given, When
        var esc = AnsiParserFixture.Parse("\e(B").Single();
        var csi = AnsiParserFixture.Parse("\e[?38:2;1$p").Single();
        var hook = AnsiParserFixture.Parse("\eP1$r\e\\").First();

        // Then
        esc.ToString().ShouldBe("Esc { Intermediates = [(], Final = B }");
        csi.ToString().ShouldBe(
            "Csi { Intermediates = [?, $], Params = [AnsiParameter { Value = 38, Subparameters = [2] }, " +
            "AnsiParameter { Value = 1, Subparameters = [] }], Final = p, ParamsRaw = 38:2;1 }");
        hook.ToString().ShouldBe(
            "DcsHook { Intermediates = [$], Params = [AnsiParameter { Value = 1, Subparameters = [] }], " +
            "Final = r, ParamsRaw = 1 }");
    }

    [Fact]
    public void Should_Flush_Trailing_High_Surrogate()
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

    [Fact]
    public void Should_Flush_Lone_Esc()
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

    [Fact]
    public void Should_Keep_Partial_Csi_On_Flush()
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

    [Fact]
    public void Should_Keep_Osc_Terminator_Esc_On_Flush()
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

    [Fact]
    public void Should_Recover_From_Partial_Sequence_On_Reset()
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

    [Fact]
    public void Should_Discard_Pending_Surrogate_On_Reset()
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

    [Fact]
    public void Should_Throw_If_Text_Is_Null()
    {
        // Given
        var parser = new AnsiParser(_ => { });

        // When
        var result = Record.Exception(() => parser.Next(null!));

        // Then
        result.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("text");
    }

    [Fact]
    public void Should_Parse_Hyperlink()
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

    [Fact]
    public void Should_Parse_Hyperlink_With_Id()
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

    [Fact]
    public void Should_Parse_Hyperlink_With_Empty_Id()
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

    [Fact]
    public void Should_Ignore_Hyperlink_Parameter_Without_Value()
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

    [Fact]
    public void Should_Parse_Hyperlink_Id_After_Parameter_Without_Key()
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

    [Fact]
    public void Should_End_Hyperlink_On_Empty_Uri_With_Id()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]8;id=foo;\e\\");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.HyperLinkEnd>();
    }

    [Fact]
    public void Should_Drop_Hyperlink_Without_Uri_Separator()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]8;id=foo\e\\");

        // Then
        result.ShouldBeEmpty();
    }

    [Fact]
    public void Should_Parse_Hyperlink_End()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]8;;\e\\");

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.HyperLinkEnd>();
    }

    [Fact]
    public void Should_Parse_Hyperlink_Start_And_End()
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

    [Fact]
    public void Should_Parse_Unknown_Osc_Command()
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

    [Fact]
    public void Should_Parse_Hyperlink_With_Semicolon_In_Uri()
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

    [Fact]
    public void Should_Parse_Hyperlink_With_Colon_Separated_Parameters()
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

    [Fact]
    public void Should_Parse_Osc_Terminated_By_Bel()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]0;title\a"); // OSC 0 window title, BEL terminator

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Osc>()
            .And().Command.ShouldBeOfType<OscCommand.Unknown>()
            .And(osc => osc.Data.ShouldBe("0;title"));
    }

    [Fact]
    public void Should_Parse_Hyperlink_Terminated_By_Bel()
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

    [Fact]
    public void Should_Return_To_Ground_After_Bel()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]0;t\aX"); // OSC ... BEL, then a printable

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Osc>();
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('X'));
    }

    [Fact]
    public void Should_Not_Leak_Data_Between_Osc_Strings()
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

    [Fact]
    public void Should_Abort_Osc_On_Can()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e]0;title\u0018"); // CAN mid-OSC

        // Then
        result.Exists(t => t is AnsiToken.Osc).ShouldBeFalse();
        result.Exists(t => t is AnsiToken.Execute e && e.Function == '\u0018').ShouldBeTrue();
    }

    [Fact]
    public void Should_Drop_Osc_Over_Length_Limit()
    {
        // Given
        var uri = "https://example.com/" + new string('a', 2025); // with "8;;", 2048 characters

        // When
        var atLimit = AnsiParserFixture.Parse($"\e]8;;{uri}\e\\");
        var overLimit = AnsiParserFixture.Parse($"\e]8;;{uri}b\e\\a");
        var unknownOverLimit = AnsiParserFixture.Parse("\e]52;c;" + new string('Q', 3000) + "\aa");

        // Then
        atLimit.Single().ShouldBeOfType<AnsiToken.Osc>()
            .Command.ShouldBeOfType<OscCommand.HyperLinkStart>().Uri.ShouldBe(uri);
        overLimit.Single().ShouldBeOfType<AnsiToken.Print>().Codepoint.ShouldBe('a');
        unknownOverLimit.Single().ShouldBeOfType<AnsiToken.Print>().Codepoint.ShouldBe('a');
    }

    [Fact]
    public void Should_Print_Accented_Latin_Character()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("aä"); // "aä"

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('a'));
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe(0x00E4));
    }

    [Fact]
    public void Should_Print_Box_Drawing_Characters()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("─│┌"); // "─│┌"

        // Then
        result.Count.ShouldBe(3);
        result[0].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe(0x2500));
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe(0x2502));
        result[2].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe(0x250C));
    }

    [Fact]
    public void Should_Print_Cjk_Character()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("日"); // "日"

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe(0x65E5));
    }

    [Fact]
    public void Should_Combine_Surrogate_Pair_Into_Astral_Codepoint()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\U0001F600"); // "😀"

        // Then
        result.Count.ShouldBe(1);
        result[0].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe(0x1F600));
    }

    [Fact]
    public void Should_Print_Astral_Codepoint_Between_Ascii()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("a\U0001F600b");

        // Then
        result.Count.ShouldBe(3);
        result[0].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('a'));
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe(0x1F600));
        result[2].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('b'));
    }

    [Fact]
    public void Should_Print_Non_Ascii_After_Csi()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\e[0m─"); // SGR reset, then "─"

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Csi>().And(csi => csi.Final.ShouldBe('m'));
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe(0x2500));
    }

    [Fact]
    public void Should_Encode_Bmp_Codepoint_As_One_Char()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("─"); // "─"

        // Then
        var print = result[0].ShouldBeOfType<AnsiToken.Print>();
        print.ToUtf16().ShouldBe("─");
        print.ToUtf16().Length.ShouldBe(1);
    }

    [Fact]
    public void Should_Encode_Astral_Codepoint_As_Surrogate_Pair()
    {
        // Given, When
        var result = AnsiParserFixture.Parse("\U0001F600"); // "😀"

        // Then
        var print = result[0].ShouldBeOfType<AnsiToken.Print>();
        print.ToUtf16().ShouldBe("\U0001F600");
        print.ToUtf16().Length.ShouldBe(2);
    }

    [Fact]
    public void Should_Parse_Alt_Enter_In_Input_Mode()
    {
        // Given, When
        var result = AnsiParserFixture.ParseInput("\e\ra");

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Esc>().And(esc => esc.Final.ShouldBe('\r'));
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('a'));
    }

    [Fact]
    public void Should_Parse_Alt_Backspace_In_Input_Mode()
    {
        // Given, When
        var delete = AnsiParserFixture.ParseInput("\e\u007f");
        var backspace = AnsiParserFixture.ParseInput("\e\b");

        // Then
        delete.Single().ShouldBeOfType<AnsiToken.Esc>().Final.ShouldBe('\u007f');
        backspace.Single().ShouldBeOfType<AnsiToken.Esc>().Final.ShouldBe('\b');
    }

    [Fact]
    public void Should_Parse_Ctrl_Alt_Key_In_Input_Mode()
    {
        // Given, When
        var result = AnsiParserFixture.ParseInput("\e\u0018\e\u001a"); // Ctrl+Alt+X, Ctrl+Alt+Z

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Esc>().And(esc => esc.Final.ShouldBe('\u0018'));
        result[1].ShouldBeOfType<AnsiToken.Esc>().And(esc => esc.Final.ShouldBe('\u001a'));
    }

    [Fact]
    public void Should_Parse_Alt_Space_In_Input_Mode()
    {
        // Given, When
        var result = AnsiParserFixture.ParseInput("\e b");

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Esc>()
            .And(esc =>
            {
                esc.Intermediates.ShouldBeEmpty();
                esc.Final.ShouldBe(' ');
            });
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('b'));
    }

    [Fact]
    public void Should_Parse_Alt_X_And_Alt_Caret_In_Input_Mode()
    {
        // Given, When
        var result = AnsiParserFixture.ParseInput("\eX\e^a"); // no reply uses SOS or PM

        // Then
        result.Count.ShouldBe(3);
        result[0].ShouldBeOfType<AnsiToken.Esc>().And(esc => esc.Final.ShouldBe('X'));
        result[1].ShouldBeOfType<AnsiToken.Esc>().And(esc => esc.Final.ShouldBe('^'));
        result[2].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('a'));
    }

    [Theory]
    [InlineData('[')]
    [InlineData('O')]
    [InlineData('P')]
    [InlineData(']')]
    [InlineData('_')]
    public void Should_Flush_Alt_Introducer_In_Input_Mode(char introducer)
    {
        // Given
        var result = new List<AnsiToken>();
        var parser = new AnsiParser(result.Add, AnsiParserMode.Input);
        parser.Next($"\e{introducer}");

        // When
        parser.Flush();
        parser.Next("a"); // prints, since the parser is back in ground

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Esc>().And(esc => esc.Final.ShouldBe(introducer));
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('a'));
    }

    [Fact]
    public void Should_Keep_Partial_Csi_On_Flush_In_Input_Mode()
    {
        // Given
        var result = new List<AnsiToken>();
        var parser = new AnsiParser(result.Add, AnsiParserMode.Input);
        parser.Next("\e[1");

        // When
        parser.Flush();
        parser.Next("A");

        // Then
        result.Single().ShouldBeOfType<AnsiToken.Csi>()
            .And(csi =>
            {
                csi.Params.ShouldBe([new(1)]);
                csi.Final.ShouldBe('A');
            });
    }

    [Fact]
    public void Should_Parse_Ss3_Keys_In_Input_Mode()
    {
        // Given, When
        var result = AnsiParserFixture.ParseInput("\eOA\eOP"); // up arrow, F1

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Ss3>().And(ss3 => ss3.Final.ShouldBe('A'));
        result[1].ShouldBeOfType<AnsiToken.Ss3>().And(ss3 => ss3.Final.ShouldBe('P'));
    }

    [Fact]
    public void Should_Report_Alt_O_Before_A_Non_Final_In_Input_Mode()
    {
        // Given, When
        var result = AnsiParserFixture.ParseInput("\eO5");

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Esc>().And(esc => esc.Final.ShouldBe('O'));
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('5'));
    }

    [Fact]
    public void Should_Parse_Backspace_As_Execute_In_Input_Mode()
    {
        // Given, When
        var result = AnsiParserFixture.ParseInput("\u007f");

        // Then
        result.Single().ShouldBeOfType<AnsiToken.Execute>().Function.ShouldBe('\u007f');
    }

    [Fact]
    public void Should_Parse_Bracketed_Paste_In_Input_Mode()
    {
        // Given, When
        var result = AnsiParserFixture.ParseInput("\e[200~a\e[31mb\e[201~c");

        // Then
        result.Count.ShouldBe(2);
        result[0].ShouldBeOfType<AnsiToken.Paste>().And(paste => paste.Text.ShouldBe("a\e[31mb"));
        result[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('c'));
    }

    [Fact]
    public void Should_Collect_Bracketed_Paste_Across_Reads_In_Input_Mode()
    {
        // Given
        var result = new List<AnsiToken>();
        var parser = new AnsiParser(result.Add, AnsiParserMode.Input);

        // When
        parser.Next("\e[200~ab");
        parser.Flush();
        parser.Next("c\e[20");
        parser.Flush();
        parser.Next("1~");

        // Then
        result.Single().ShouldBeOfType<AnsiToken.Paste>().Text.ShouldBe("abc");
    }

    [Fact]
    public void Should_Parse_Replies_In_Input_Mode()
    {
        // Given, When
        var result = AnsiParserFixture.ParseInput("\e[12;40R\eP>|xterm\e\\"); // cursor position, XTVERSION

        // Then
        result.Count.ShouldBe(8);
        result[0].ShouldBeOfType<AnsiToken.Csi>().And(csi => csi.Params.ShouldBe([new(12), new(40)]));
        result[1].ShouldBeOfType<AnsiToken.DcsHook>().And(hook => hook.Final.ShouldBe('|'));
        result[7].ShouldBeOfType<AnsiToken.DcsUnhook>();
    }

    [Fact]
    public void Should_Keep_Vt500_Behaviour_In_Output_Mode()
    {
        // Given, When
        var altEnter = AnsiParserFixture.Parse("\e\ra");
        var ss3 = AnsiParserFixture.Parse("\eOA");
        var delete = AnsiParserFixture.Parse("\u007f");

        // Then
        altEnter.Count.ShouldBe(2);
        altEnter[0].ShouldBeOfType<AnsiToken.Execute>().And(e => e.Function.ShouldBe('\r'));
        altEnter[1].ShouldBeOfType<AnsiToken.Esc>().And(esc => esc.Final.ShouldBe('a'));
        ss3.Count.ShouldBe(2);
        ss3[0].ShouldBeOfType<AnsiToken.Esc>().And(esc => esc.Final.ShouldBe('O'));
        ss3[1].ShouldBeOfType<AnsiToken.Print>().And(p => p.Codepoint.ShouldBe('A'));
        delete.Single().ShouldBeOfType<AnsiToken.Print>().Codepoint.ShouldBe(0x7F);
    }
}