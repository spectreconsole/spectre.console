using System;
using System.Threading.Tasks;
using Spectre.Console.Testing;
using Spectre.Console.Tests.Data;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    public sealed class ExceptionTests
    {
        [Fact]
        public Task Should_Write_Exception()
        {
            // Given
            var console = new FakeConsole(width: 1024);
            var dex = GetException(() => TestExceptions.MethodThatThrows(null));

            // When
            var result = console.WriteNormalizedException(dex);

            // Then
            return Verifier.Verify(result);
        }

        [Fact]
        public Task Should_Write_Exception_With_Shortened_Types()
        {
            // Given
            var console = new FakeConsole(width: 1024);
            var dex = GetException(() => TestExceptions.MethodThatThrows(null));

            // When
            var result = console.WriteNormalizedException(dex, ExceptionFormats.ShortenTypes);

            // Then
            return Verifier.Verify(result);
        }

        [Fact]
        public Task Should_Write_Exception_With_Shortened_Methods()
        {
            // Given
            var console = new FakeConsole(width: 1024);
            var dex = GetException(() => TestExceptions.MethodThatThrows(null));

            // When
            var result = console.WriteNormalizedException(dex, ExceptionFormats.ShortenMethods);

            // Then
            return Verifier.Verify(result);
        }

        [Fact]
        public Task Should_Write_Exception_With_Inner_Exception()
        {
            // Given
            var console = new FakeConsole(width: 1024);
            var dex = GetException(() => TestExceptions.ThrowWithInnerException());

            // When
            var result = console.WriteNormalizedException(dex);

            // Then
            return Verifier.Verify(result);
        }

        [Fact]
        public Task Should_Write_Exceptions_With_Generic_Type_Parameters_In_Callsite_As_Expected()
        {
            // Given
            var console = new FakeConsole(width: 1024);
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
