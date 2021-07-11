using System;
using Shouldly;
using Spectre.Console.Testing;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public partial class AnsiConsoleTests
    {
        public sealed class Prompt
        {
            [Theory]
            [InlineData(true, true)]
            [InlineData(false, false)]
            public void Should_Return_Default_Value_If_Nothing_Is_Entered(bool expected, bool defaultValue)
            {
                // Given
                var console = new TestConsole().EmitAnsiSequences();
                console.Input.PushKey(ConsoleKey.Enter);

                // When
                var result = console.Confirm("Want some prompt?", defaultValue);

                // Then
                result.ShouldBe(expected);
            }
        }
    }
}
