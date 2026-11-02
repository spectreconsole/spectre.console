namespace Spectre.Console.Tests.Unit;

/// <summary>
/// One case per shape <c>StackFrame.ToString()</c> can produce, across CoreCLR and NativeAOT.
/// Every input ends with a newline, because <c>ToString()</c> finishes with <c>AppendLine()</c>.
/// </summary>
public sealed class StackFrameTextTests
{
    [Fact]
    public void Should_Parse_Plain_CoreCLR_Frame()
    {
        // Given, When
        var frame = StackFrameText.Parse("MoveNext at offset 552 in file:line:column /src/Boom.cs:24:13\n");

        // Then
        frame.HasMethod.ShouldBeTrue();
        frame.Name.ShouldBe("MoveNext");
        frame.GenericArguments.ShouldBeNull();
        frame.Signature.ShouldBeNull("CoreCLR never puts a signature in StackFrame.ToString()");
        frame.IsSynthetic.ShouldBeFalse();
    }

    [Fact]
    public void Should_Parse_CoreCLR_Generic_Arguments_In_Angle_Brackets()
    {
        // Given, When
        var frame = StackFrameText.Parse("Level2<T> at offset 144 in file:line:column /src/Boom.cs:49:9\n");

        // Then
        frame.Name.ShouldBe("Level2");
        frame.GenericArguments.ShouldBe("T");
    }

    [Fact]
    public void Should_Parse_Multiple_Generic_Arguments()
    {
        // Given, When
        var frame = StackFrameText.Parse("Convert<TKey,TValue> at offset 12 in file:line:column /src/A.cs:1:1\n");

        // Then
        frame.Name.ShouldBe("Convert");
        frame.GenericArguments.ShouldBe("TKey,TValue");
    }

    [Fact]
    public void Should_Parse_Nested_Generic_Arguments()
    {
        // Given, When
        var frame = StackFrameText.Parse("Map<List<T>> at offset 12 in file:line:column /src/A.cs:1:1\n");

        // Then
        frame.Name.ShouldBe("Map");
        frame.GenericArguments.ShouldBe("List<T>");
    }

    [Fact]
    public void Should_Treat_Unknown_File_And_Offset_As_Plain_Frame()
    {
        // Given, When
        var frame = StackFrameText.Parse(
            "Throw at offset <offset unknown> in file:line:column <filename unknown>:0:0\n");

        // Then
        frame.Name.ShouldBe("Throw");
        frame.IsSynthetic.ShouldBeFalse();
    }

    [Fact]
    public void Should_Treat_Null_Frame_As_No_Method()
    {
        // Given, When
        // CoreCLR renders a frame with no MethodBase as nothing but "<null>".
        var frame = StackFrameText.Parse("<null>\n");

        // Then
        frame.HasMethod.ShouldBeFalse();
        frame.Name.ShouldBeNull();
        frame.IsSynthetic.ShouldBeFalse();
    }

    [Fact]
    public void Should_Keep_Method_When_Path_Contains_A_Marker()
    {
        // Given, When
        var frame = StackFrameText.Parse(
            "M at offset 5 in file:line:column /src/a at offset b/File.cs:3:1\n");

        // Then
        frame.Name.ShouldBe("M");
    }

    [Fact]
    public void Should_Handle_Windows_Paths_And_Crlf()
    {
        // Given, When
        var frame = StackFrameText.Parse("Main at offset 16 in file:line:column C:\\src\\Program.cs:12:5\r\n");

        // Then
        frame.Name.ShouldBe("Main");
    }

    [Fact]
    public void Should_Parse_NativeAot_Signature_Up_To_Net10()
    {
        // Given, When
        var frame = StackFrameText.Parse(
            "MyApp.Boom.Level2[T](Int32, T&, String[]) + 0x28 at offset 40 " +
            "in file:line:column <filename unknown>:0:0\n");

        // Then
        frame.Name.ShouldBe("MyApp.Boom.Level2");
        frame.GenericArguments.ShouldBe("T", "NativeAOT uses square brackets, not angle brackets");
        frame.Signature.ShouldBe("Int32, T&, String[]");
    }

    [Fact]
    public void Should_Distinguish_No_Parameters_From_Unknown_Parameters()
    {
        // Given, When
        var known = StackFrameText.Parse(
            "MyApp.Boom.<ThrowAsync>d__0.MoveNext() + 0x140 at offset 320 " +
            "in file:line:column <filename unknown>:0:0\n");
        var unknown = StackFrameText.Parse("MoveNext at offset 320 in file:line:column <filename unknown>:0:0\n");

        // Then
        known.Name.ShouldBe("MyApp.Boom.<ThrowAsync>d__0.MoveNext");
        known.GenericArguments.ShouldBeNull("the angle brackets here are name mangling, not generics");
        known.Signature.ShouldBe(string.Empty, "empty means the method really takes no parameters");
        unknown.Signature.ShouldBeNull("null means the runtime did not say");
    }

    [Fact]
    public void Should_Not_Mistake_Local_Function_Mangling_For_Generics()
    {
        // Given, When
        var frame = StackFrameText.Parse(
            "MyApp.Boom.<Level3>g__ThrowHidden|4_0(String) + 0xc at offset 12 " +
            "in file:line:column <filename unknown>:0:0\n");

        // Then
        frame.Name.ShouldBe("MyApp.Boom.<Level3>g__ThrowHidden|4_0");
        frame.GenericArguments.ShouldBeNull();
        frame.Signature.ShouldBe("String");
    }

    [Fact]
    public void Should_Match_Nested_Parentheses_In_Signature()
    {
        // Given, When
        var frame = StackFrameText.Parse(
            "Ns.T.M(Func(Int32), String) + 0x4 at offset 4 in file:line:column <filename unknown>:0:0\n");

        // Then
        frame.Name.ShouldBe("Ns.T.M");
        frame.Signature.ShouldBe("Func(Int32), String");
    }

    [Fact]
    public void Should_Keep_Generic_Type_Arguments_On_The_Declaring_Type()
    {
        // Given, When
        var frame = StackFrameText.Parse(
            "System.Collections.Generic.List`1[T].Add(T) + 0x4 at offset 4 in file:line:column <filename unknown>:0:0\n");

        // Then
        frame.Name.ShouldBe("System.Collections.Generic.List`1[T].Add");
        frame.GenericArguments.ShouldBeNull();
        frame.Signature.ShouldBe("T");
    }

    [Fact]
    public void Should_Parse_Net11_NativeAot_Shape_Like_CoreCLR()
    {
        // Given, When
        // .NET 11 moved the owning type and signature out of the frame text, and it can carry
        // real line numbers when published with StackTraceLineNumberSupport.
        var frame = StackFrameText.Parse(
            "Level2<T> at offset 40 in file:line:column /src/MyApp/Boom.cs:50:0\n");

        // Then
        frame.Name.ShouldBe("Level2");
        frame.GenericArguments.ShouldBe("T");
        frame.Signature.ShouldBeNull();
    }

    [Fact]
    public void Should_Recognise_Base_Address_Fallback_As_Synthetic()
    {
        // Given, When
        // StackTraceSupport=false: no method names at all, just a module-relative address.
        var frame = StackFrameText.Parse(
            "MyApp!<BaseAddress>+0x1a2b at offset 0 in file:line:column <filename unknown>:0:0\n");

        // Then
        frame.IsSynthetic.ShouldBeTrue();
        frame.Name.ShouldBe("MyApp!<BaseAddress>+0x1a2b");
        frame.GenericArguments.ShouldBeNull("<BaseAddress> is not a generic argument list");
        frame.Signature.ShouldBeNull();
    }

    [Fact]
    public void Should_Recognise_Unknown_Method_Placeholder_As_Synthetic()
    {
        // Given, When
        var frame = StackFrameText.Parse("<unknown> at offset 0 in file:line:column <filename unknown>:0:0\n");

        // Then
        frame.IsSynthetic.ShouldBeTrue();
        frame.Name.ShouldBe("<unknown>");
    }

    [Fact]
    public void Should_Parse_What_The_Running_Runtime_Produces()
    {
        // Given, When
        var frame = StackFrameText.Parse(new System.Diagnostics.StackFrame(skipFrames: 0, needFileInfo: true));

        // Then
        frame.HasMethod.ShouldBeTrue();
        frame.Name.ShouldNotBeNullOrEmpty();
        frame.Name.ShouldNotContain(" at offset ");
        frame.Name.ShouldNotContain("file:line:column");
    }
}
