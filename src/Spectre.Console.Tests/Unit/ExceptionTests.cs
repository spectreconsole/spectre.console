namespace Spectre.Console.Tests.Unit;

[ExpectationPath("Exception")]
public sealed class ExceptionTests
{
    [Fact]
    [Expectation("Default")]
    public Task Should_Write_Exception()
    {
        // Given
        var console = new TestConsole().Width(1024);
        var dex = GetException(() => TestExceptions.MethodThatThrows(null));

        // When
        console.WriteException(dex, new ExceptionSettings
        {
            Format = ExceptionFormats.Default,
            Resolver = new ExceptionScrubber(),
        });


        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("ShortenedTypes")]
    public Task Should_Write_Exception_With_Shortened_Types()
    {
        // Given
        var console = new TestConsole().Width(1024);
        var dex = GetException(() => TestExceptions.MethodThatThrows(null));

        // When
        console.WriteException(dex, new ExceptionSettings
        {
            Format = ExceptionFormats.ShortenTypes,
            Resolver = new ExceptionScrubber(),
        });

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("ShortenedMethods")]
    public Task Should_Write_Exception_With_Shortened_Methods()
    {
        // Given
        var console = new TestConsole().Width(1024);
        var dex = GetException(() => TestExceptions.MethodThatThrows(null));

        // When
        console.WriteException(dex, new ExceptionSettings
        {
            Format = ExceptionFormats.ShortenMethods,
            Resolver = new ExceptionScrubber(),
        });

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("InnerException")]
    public Task Should_Write_Exception_With_Inner_Exception()
    {
        // Given
        var console = new TestConsole().Width(1024);
        var dex = GetException(() => TestExceptions.ThrowWithInnerException());

        // When
        console.WriteException(dex, new ExceptionSettings
        {
            Format = ExceptionFormats.Default,
            Resolver = new ExceptionScrubber(),
        });

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("CallSite")]
    public Task Should_Write_Exceptions_With_Generic_Type_Parameters_In_Callsite_As_Expected()
    {
        // Given
        var console = new TestConsole().Width(1024);
        var dex = GetException(() => TestExceptions.ThrowWithGenericInnerException());

        // When
        console.WriteException(dex, new ExceptionSettings
        {
            Format = ExceptionFormats.Default,
            Resolver = new ExceptionScrubber(),
        });

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("OutParam")]
    public Task Should_Write_Exception_With_Output_Param()
    {
        // Given
        var console = new TestConsole().Width(1024);
        var dex = GetException(() => TestExceptions.GenericMethodWithOutThatThrows<int>(out _));

        // When
        console.WriteException(dex, new ExceptionSettings
        {
            Format = ExceptionFormats.ShortenTypes,
            Resolver = new ExceptionScrubber(),
        });

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Tuple")]
    public Task Should_Write_Exception_With_Tuple_Return()
    {
        // Given
        var console = new TestConsole().Width(1024);
        var dex = GetException(() => TestExceptions.GetTuplesWithInnerException<int>((0, "value")));

        // When
        console.WriteException(dex, new ExceptionSettings
        {
            Format = ExceptionFormats.ShortenTypes,
            Resolver = new ExceptionScrubber(),
        });

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("NoStackTrace")]
    public Task Should_Write_Exception_With_No_StackTrace()
    {
        // Given
        var console = new TestConsole().Width(1024);
        var dex = GetException(TestExceptions.ThrowWithInnerException);

        // When
        console.WriteException(dex, new ExceptionSettings
        {
            Format = ExceptionFormats.NoStackTrace,
            Resolver = new ExceptionScrubber(),
        });

        // Then
        return Verifier.Verify(console.Output);
    }

    [Theory]
    [InlineData(ExceptionFormats.Default)]
    [InlineData(ExceptionFormats.ShortenTypes)]
    [InlineData(ExceptionFormats.ShortenMethods)]
    [InlineData(ExceptionFormats.ShortenEverything)]
    [Expectation("GenericException")]
    public Task Should_Write_GenericException(ExceptionFormats exceptionFormats)
    {
        // Given
        var console = new TestConsole().Width(1024);
        var dex = GetException(() => TestExceptions.MethodThatThrowsGenericException<IAnsiConsole>());

        // When
        console.WriteException(dex, new ExceptionSettings
        {
            Format = exceptionFormats,
            Resolver = new ExceptionScrubber(),
        });

        // Then
        return Verifier.Verify(console.Output).UseParameters(exceptionFormats);
    }

    public static Exception GetException(Action action)
    {
        try
        {
            action?.Invoke();
        }
        catch (Exception e)
        {
            return e;
        }

        throw new InvalidOperationException("Exception harness failed");
    }
}