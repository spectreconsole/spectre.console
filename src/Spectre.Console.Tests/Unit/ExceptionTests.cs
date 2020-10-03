using System;
using Shouldly;
using Spectre.Console.Tests.Data;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class ExceptionTests
    {
        [Fact]
        public void Should_Write_Exception()
        {
            // Given
            var console = new PlainConsole(width: 1024);
            var dex = GetException(() => TestExceptions.MethodThatThrows(null));

            // When
            var result = console.WriteExceptionAndGetLines(dex);

            // Then
            result.Length.ShouldBe(4);
            result[0].ShouldBe("System.InvalidOperationException: Throwing!");
            result[1].ShouldBe("  at Spectre.Console.Tests.Data.TestExceptions.MethodThatThrows(Nullable`1 number) in /xyz/Exceptions.cs:nn");
            result[2].ShouldBe("  at Spectre.Console.Tests.Unit.ExceptionTests.<>c.<Should_Write_Exception>b__0_0() in /xyz/ExceptionTests.cs:nn");
            result[3].ShouldBe("  at Spectre.Console.Tests.Unit.ExceptionTests.GetException(Action action) in /xyz/ExceptionTests.cs:nn");
        }

        [Fact]
        public void Should_Write_Exception_With_Shortened_Types()
        {
            // Given
            var console = new PlainConsole(width: 1024);
            var dex = GetException(() => TestExceptions.MethodThatThrows(null));

            // When
            var result = console.WriteExceptionAndGetLines(dex, ExceptionFormats.ShortenTypes);

            // Then
            result.Length.ShouldBe(4);
            result[0].ShouldBe("InvalidOperationException: Throwing!");
            result[1].ShouldBe("  at Spectre.Console.Tests.Data.TestExceptions.MethodThatThrows(Nullable`1 number) in /xyz/Exceptions.cs:nn");
            result[2].ShouldBe("  at Spectre.Console.Tests.Unit.ExceptionTests.<>c.<Should_Write_Exception_With_Shortened_Types>b__1_0() in /xyz/ExceptionTests.cs:nn");
            result[3].ShouldBe("  at Spectre.Console.Tests.Unit.ExceptionTests.GetException(Action action) in /xyz/ExceptionTests.cs:nn");
        }

        [Fact]
        public void Should_Write_Exception_With_Shortened_Methods()
        {
            // Given
            var console = new PlainConsole(width: 1024);
            var dex = GetException(() => TestExceptions.MethodThatThrows(null));

            // When
            var result = console.WriteExceptionAndGetLines(dex, ExceptionFormats.ShortenMethods);

            // Then
            result.Length.ShouldBe(4);
            result[0].ShouldBe("System.InvalidOperationException: Throwing!");
            result[1].ShouldBe("  at MethodThatThrows(Nullable`1 number) in /xyz/Exceptions.cs:nn");
            result[2].ShouldBe("  at <Should_Write_Exception_With_Shortened_Methods>b__2_0() in /xyz/ExceptionTests.cs:nn");
            result[3].ShouldBe("  at GetException(Action action) in /xyz/ExceptionTests.cs:nn");
        }

        [Fact]
        public void Should_Write_Exception_With_Inner_Exception()
        {
            // Given
            var console = new PlainConsole(width: 1024);
            var dex = GetException(() => TestExceptions.ThrowWithInnerException());

            // When
            var result = console.WriteExceptionAndGetLines(dex);

            // Then
            result.Length.ShouldBe(7);
            result[0].ShouldBe("System.InvalidOperationException: Something threw!");
            result[1].ShouldBe("     System.InvalidOperationException: Throwing!");
            result[2].ShouldBe("       at Spectre.Console.Tests.Data.TestExceptions.MethodThatThrows(Nullable`1 number) in /xyz/Exceptions.cs:nn");
            result[3].ShouldBe("       at Spectre.Console.Tests.Data.TestExceptions.ThrowWithInnerException() in /xyz/Exceptions.cs:nn");
            result[4].ShouldBe("  at Spectre.Console.Tests.Data.TestExceptions.ThrowWithInnerException() in /xyz/Exceptions.cs:nn");
            result[5].ShouldBe("  at Spectre.Console.Tests.Unit.ExceptionTests.<>c.<Should_Write_Exception_With_Inner_Exception>b__3_0() in /xyz/ExceptionTests.cs:nn");
            result[6].ShouldBe("  at Spectre.Console.Tests.Unit.ExceptionTests.GetException(Action action) in /xyz/ExceptionTests.cs:nn");
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
}
