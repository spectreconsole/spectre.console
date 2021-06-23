using Shouldly;
using Spectre.Console.Testing;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public partial class AnsiConsoleTests
    {
        public sealed class Cursor
        {
            public sealed class TheMoveMethod
            {
                [Theory]
                [InlineData(CursorDirection.Up, "Hello[2AWorld")]
                [InlineData(CursorDirection.Down, "Hello[2BWorld")]
                [InlineData(CursorDirection.Right, "Hello[2CWorld")]
                [InlineData(CursorDirection.Left, "Hello[2DWorld")]
                public void Should_Return_Correct_Ansi_Code(CursorDirection direction, string expected)
                {
                    // Given
                    var console = new TestConsole().EmitAnsiSequences();

                    // When
                    console.Write("Hello");
                    console.Cursor.Move(direction, 2);
                    console.Write("World");

                    // Then
                    console.Output.ShouldBe(expected);
                }
            }

            public sealed class TheSetPositionMethod
            {
                [Fact]
                public void Should_Return_Correct_Ansi_Code()
                {
                    // Given
                    var console = new TestConsole().EmitAnsiSequences();

                    // When
                    console.Write("Hello");
                    console.Cursor.SetPosition(5, 3);
                    console.Write("World");

                    // Then
                    console.Output.ShouldBe("Hello[3;5HWorld");
                }
            }
        }
    }
}
