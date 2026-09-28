using Spectre.Console.Testing;

namespace Spectre.Console.NativeAot.Tests;

/// <summary>
/// <see cref="AnsiConsole.WriteException"/> in a published NativeAOT binary, where every frame
/// has lost its <c>MethodBase</c> and the names come from the stack trace metadata instead.
/// </summary>
public sealed class ExceptionTests
{
    // Guard is [StackTraceHidden], but the attribute cannot be read without a MethodBase,
    // so unlike the JIT path it still shows up here.
    [Fact]
    public async Task Should_Write_Exception()
    {
        // Given
        NativeAot.EnsureRunning();
        var console = new TestConsole().Width(200);
        var exception = await Capture();

        // When
        console.WriteException(exception);

        // Then
        Assert.Equal(
            """
            System.InvalidOperationException: Outer boom, thrown from an async method.
                 System.ArgumentOutOfRangeException: Inner boom, thrown from a hidden frame. (Parameter 'name')
                 Actual value was alpha-beta.
                   at Spectre.Console.NativeAot.Tests.Boom.Guard(String)
                   at Spectre.Console.NativeAot.Tests.Boom.<Level3>g__ThrowHidden|4_0(String)
                   at Spectre.Console.NativeAot.Tests.Boom.Level3(ValueTuple`2)
                   at Spectre.Console.NativeAot.Tests.Boom.Level2<T>(Int32, T&, String[])
                   at Spectre.Console.NativeAot.Tests.Boom.Level1(Int32[])
                   at Spectre.Console.NativeAot.Tests.Boom.ThrowAsync()
              at Spectre.Console.NativeAot.Tests.Boom.ThrowAsync()
              at Spectre.Console.NativeAot.Tests.ExceptionTests.Capture()
            """,
            Normalize(console.Output));
    }

    [Fact]
    public async Task Should_Write_Exception_With_Shortened_Names()
    {
        // Given
        NativeAot.EnsureRunning();
        var console = new TestConsole().Width(200);
        var exception = await Capture();

        // When
        console.WriteException(exception, ExceptionFormats.ShortenEverything);

        // Then
        Assert.Equal(
            """
            InvalidOperationException: Outer boom, thrown from an async method.
                 ArgumentOutOfRangeException: Inner boom, thrown from a hidden frame. (Parameter 'name')
                 Actual value was alpha-beta.
                   at Guard(String)
                   at <Level3>g__ThrowHidden|4_0(String)
                   at Level3(ValueTuple`2)
                   at Level2<T>(Int32, T&, String[])
                   at Level1(Int32[])
                   at ThrowAsync()
              at ThrowAsync()
              at Capture()
            """,
            Normalize(console.Output));
    }

    [Fact]
    public async Task Should_Style_Frames_Rather_Than_Dim_Them_Whole()
    {
        // Given
        NativeAot.EnsureRunning();
        var console = new TestConsole().Width(200).EmitAnsiSequences();
        var exception = await Capture();

        // When
        console.WriteException(exception, new ExceptionSettings
        {
            Format = ExceptionFormats.ShortenEverything,
            Style = new ExceptionStyle
            {
                Method = new Style(Color.Red),
                ParameterType = new Style(Color.Blue),
            },
        });

        // Then
        Assert.Contains("\e[38;5;9mLevel1\e[0m", console.Output);
        Assert.Contains("\e[38;5;12mInt32[]\e[0m", console.Output);
    }

    private static async Task<Exception> Capture()
    {
        try
        {
            await Boom.ThrowAsync();
        }
        catch (Exception ex)
        {
            return ex;
        }

        throw new InvalidOperationException("Boom.ThrowAsync was supposed to throw.");
    }

    private static string Normalize(string output)
    {
        return string.Join('\n', output.ReplaceLineEndings("\n").TrimEnd('\n').Split('\n').Select(line => line.TrimEnd()));
    }
}
