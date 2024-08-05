namespace Spectre.Console.Tests.Unit;

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

    public sealed class Ask
    {
        [Fact]
        public void Should_Return_Correct_DateTime_When_Asked_PL_Culture()
        {
            // Given
            var console = new TestConsole().EmitAnsiSequences();
            console.Input.PushTextWithEnter("1/2/1998");

            // When
            var dateTime = console.Ask<DateTime>(string.Empty, CultureInfo.GetCultureInfo("pl-PL"));

            // Then
            dateTime.ShouldBe(new DateTime(1998, 2, 1));
        }

        [Fact]
        public void Should_Return_Correct_DateTime_When_Asked_US_Culture()
        {
            // Given
            var console = new TestConsole().EmitAnsiSequences();
            console.Input.PushTextWithEnter("2/1/1998");

            // When
            var dateTime = console.Ask<DateTime>(string.Empty, CultureInfo.GetCultureInfo("en-US"));

            // Then
            dateTime.ShouldBe(new DateTime(1998, 2, 1));
        }
    }
}
