using System;
using System.Threading.Tasks;
using Spectre.Console.Testing;
using Spectre.Console.Tests.Data;
using Spectre.Verify.Extensions;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
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
            var result = console.WriteNormalizedException(dex);

            // Then
            return Verifier.Verify(result);
        }

        [Fact]
        [Expectation("ShortenedTypes")]
        public Task Should_Write_Exception_With_Shortened_Types()
        {
            // Given
            var console = new TestConsole().Width(1024);
            var dex = GetException(() => TestExceptions.MethodThatThrows(null));

            // When
            var result = console.WriteNormalizedException(dex, ExceptionFormats.ShortenTypes);

            // Then
            return Verifier.Verify(result);
        }

        [Fact]
        [Expectation("ShortenedMethods")]
        public Task Should_Write_Exception_With_Shortened_Methods()
        {
            // Given
            var console = new TestConsole().Width(1024);
            var dex = GetException(() => TestExceptions.MethodThatThrows(null));

            // When
            var result = console.WriteNormalizedException(dex, ExceptionFormats.ShortenMethods);

            // Then
            return Verifier.Verify(result);
        }

        [Fact]
        [Expectation("InnerException")]
        public Task Should_Write_Exception_With_Inner_Exception()
        {
            // Given
            var console = new TestConsole().Width(1024);
            var dex = GetException(() => TestExceptions.ThrowWithInnerException());

            // When
            var result = console.WriteNormalizedException(dex);

            // Then
            return Verifier.Verify(result);
        }

        [Fact]
        [Expectation("CallSite")]
        public Task Should_Write_Exceptions_With_Generic_Type_Parameters_In_Callsite_As_Expected()
        {
            // Given
            var console = new TestConsole().Width(1024);
            var dex = GetException(() => TestExceptions.ThrowWithGenericInnerException());

            // When
            var result = console.WriteNormalizedException(dex);

            // Then
            return Verifier.Verify(result);
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
