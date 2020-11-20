using System;
using System.Threading.Tasks;
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
            var console = new PlainConsole(width: 1024);
            var dex = GetException(() => TestExceptions.MethodThatThrows(null));

            // When
            var result = console.WriteExceptionAndGetLines(dex);

            // Then
            return Verifier.Verify(result);
        }

        [Fact]
        public Task Should_Write_Exception_With_Shortened_Types()
        {
            // Given
            var console = new PlainConsole(width: 1024);
            var dex = GetException(() => TestExceptions.MethodThatThrows(null));

            // When
            var result = console.WriteExceptionAndGetLines(dex, ExceptionFormats.ShortenTypes);

            // Then
            return Verifier.Verify(result);
        }

        [Fact]
        public Task Should_Write_Exception_With_Shortened_Methods()
        {
            // Given
            var console = new PlainConsole(width: 1024);
            var dex = GetException(() => TestExceptions.MethodThatThrows(null));

            // When
            var result = console.WriteExceptionAndGetLines(dex, ExceptionFormats.ShortenMethods);

            // Then
            return Verifier.Verify(result);
        }

        [Fact]
        public Task Should_Write_Exception_With_Inner_Exception()
        {
            // Given
            var console = new PlainConsole(width: 1024);
            var dex = GetException(() => TestExceptions.ThrowWithInnerException());

            // When
            var result = console.WriteExceptionAndGetLines(dex);

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
