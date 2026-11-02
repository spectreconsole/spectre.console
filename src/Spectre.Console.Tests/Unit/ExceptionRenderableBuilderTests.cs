using System.Text;

namespace Spectre.Console.Tests.Unit;

/// <summary>
/// The path <see cref="ExceptionRenderableBuilder"/> takes when a frame has no
/// <see cref="MethodBase"/>, which is every frame under NativeAOT. Names come from the stack
/// trace metadata and the frame text supplies the rest, so both can be fed in directly.
/// </summary>
public sealed class ExceptionRenderableBuilderTests
{
    private const string NoExtras = "MoveNext at offset 0 in file:line:column <filename unknown>:0:0\n";

    [Fact]
    public void Should_Render_Names_With_Parameter_Types_From_NativeAot_Frame_Text()
    {
        // Given
        var text = StackFrameText.Parse(
            "MyApp.Boom.Level2[T](Int32, T&, String[]) + 0x28 at offset 40 " +
            "in file:line:column <filename unknown>:0:0\n");

        // When
        var result = Render("MyApp.Boom", "Level2", text);

        // Then
        result.ShouldBe("MyApp.Boom.Level2<T>(Int32, T&, String[])");
    }

    [Fact]
    public void Should_Mark_Unknown_Parameters_As_Unknown()
    {
        // Given
        // .NET 11 NativeAOT and CoreCLR put no signature in the frame text.
        var text = StackFrameText.Parse("Level2<T> at offset 40 in file:line:column <filename unknown>:0:0\n");

        // When
        var result = Render("MyApp.Boom", "Level2", text);

        // Then
        result.ShouldBe("MyApp.Boom.Level2<T>(…)");
    }

    [Fact]
    public void Should_Render_Empty_Parameter_List_When_Method_Has_None()
    {
        // Given
        var text = StackFrameText.Parse(
            "MyApp.Boom.Level0() + 0x28 at offset 40 in file:line:column <filename unknown>:0:0\n");

        // When
        var result = Render("MyApp.Boom", "Level0", text);

        // Then
        result.ShouldBe("MyApp.Boom.Level0()");
    }

    [Fact]
    public void Should_Shorten_Method_Names()
    {
        // Given
        var text = StackFrameText.Parse("Level2<T> at offset 40 in file:line:column <filename unknown>:0:0\n");

        // When
        var result = Render("MyApp.Boom", "Level2", text, ExceptionFormats.ShortenMethods);

        // Then
        result.ShouldBe("Level2<T>(…)");
    }

    [Fact]
    public void Should_Resolve_Async_State_Machine_To_Its_Source_Method()
    {
        // Given
        var text = StackFrameText.Parse(NoExtras);

        // When
        var result = Render("MyApp.Boom+<ThrowAsync>d__0", "MoveNext", text);

        // Then
        result.ShouldBe("MyApp.Boom.ThrowAsync(…)");
    }

    [Fact]
    public void Should_Resolve_State_Machine_Named_In_Frame_Text_Form()
    {
        // Given
        // Without DiagnosticMethodInfo the names come from the frame text, where nested
        // types are joined with '.' rather than '+'.
        var text = StackFrameText.Parse(NoExtras);

        // When
        var result = Render("MyApp.Boom.<ThrowAsync>d__0", "MoveNext", text);

        // Then
        result.ShouldBe("MyApp.Boom.ThrowAsync(…)");
    }

    [Fact]
    public void Should_Resolve_Async_Local_Function_And_Lambda_State_Machines()
    {
        // Given
        // These state machine types end in a bare "d" and nest the outer method's brackets.
        var text = StackFrameText.Parse(NoExtras);

        // When
        var local = Render("Ns.Outer+<<Run>g__Local|0_1>d", "MoveNext", text);
        var lambda = Render("Ns.Outer+<>c+<<Run>b__0_0>d", "MoveNext", text);

        // Then
        local.ShouldBe("Ns.Outer.<Run>g__Local|0_1(…)");
        lambda.ShouldBe("Ns.Outer.<>c.<Run>b__0_0(…)");
    }

    [Fact]
    public void Should_Resolve_Generic_Async_State_Machine()
    {
        // Given
        var text = StackFrameText.Parse("MoveNext at offset 0 in file:line:column <filename unknown>:0:0\n");

        // When
        var result = Render("Ns.Outer+<Convert>d__3`1", "MoveNext", text);

        // Then
        result.ShouldBe("Ns.Outer.Convert(…)");
    }

    [Fact]
    public void Should_Not_Resolve_Types_That_Merely_End_In_A_Bracket_Suffix()
    {
        // Given
        var text = StackFrameText.Parse(NoExtras);

        // When
        var result = Render("Ns.Outer+<>c", "MoveNext", text);

        // Then
        result.ShouldBe("Ns.Outer.<>c.MoveNext(…)");
    }

    [Fact]
    public void Should_Leave_Real_MoveNext_Alone()
    {
        // Given
        var text = StackFrameText.Parse(NoExtras);

        // When
        var result = Render("System.Collections.Generic.List`1+Enumerator", "MoveNext", text);

        // Then
        result.ShouldBe("System.Collections.Generic.List`1.Enumerator.MoveNext(…)");
    }

    [Fact]
    public void Should_Leave_Local_Functions_And_Lambdas_Mangled_Like_The_Reflection_Path()
    {
        // Given
        var text = StackFrameText.Parse(NoExtras);

        // When
        var local = Render("Ns.Boom", "<Level3>g__ThrowHidden|4_0", text);
        var lambda = Render("Ns.Boom+<>c", "<Level3>b__4_0", text);

        // Then
        local.ShouldBe("Ns.Boom.<Level3>g__ThrowHidden|4_0(…)");
        lambda.ShouldBe("Ns.Boom.<>c.<Level3>b__4_0(…)");
    }

    [Fact]
    public void Should_Render_Method_Without_Declaring_Type()
    {
        // Given
        var text = StackFrameText.Parse(NoExtras);

        // When
        var result = Render(null, "Main", text);

        // Then
        result.ShouldBe("Main(…)");
    }

    [Fact]
    public void Should_Render_Synthetic_Frame_As_Is()
    {
        // Given
        var text = StackFrameText.Parse(
            "MyApp!<BaseAddress>+0x1a2b at offset 0 in file:line:column <filename unknown>:0:0\n");

        // When
        var result = Render(null, "MyApp!<BaseAddress>+0x1a2b", text, ExceptionFormats.ShortenEverything);

        // Then
        result.ShouldBe("MyApp!<BaseAddress>+0x1a2b");
    }

    [Theory]
    [InlineData("System.Runtime.ExceptionServices.ExceptionDispatchInfo", true)]
    [InlineData("System.Runtime.CompilerServices.TaskAwaiter", true)]
    [InlineData("System.Runtime.CompilerServices.TaskAwaiter`1", true)]
    [InlineData("System.Runtime.CompilerServices.ValueTaskAwaiter`1", true)]
    [InlineData("System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1+ConfiguredTaskAwaiter", true)]
    [InlineData("MyApp.Boom", false)]
    [InlineData("System.Threading.Tasks.Task", false)]
    [InlineData(null, false)]
    public void Should_Hide_Runtime_Async_Plumbing_By_Name(string? declaringType, bool expected)
    {
        // Given, When
        var result = ExceptionRenderableBuilder.IsHiddenByName(declaringType);

        // Then
        result.ShouldBe(expected);
    }

    private static string Render(string? declaringType, string name, StackFrameText text,
        ExceptionFormats format = ExceptionFormats.Default)
    {
        var builder = new StringBuilder();
        ExceptionRenderableBuilder.AppendMetadataMethod(builder, declaringType, name, text,
            new ExceptionSettings { Format = format });

        var console = new TestConsole().Width(1024);
        console.Write(new Markup(builder.ToString()));
        return console.Output;
    }
}
