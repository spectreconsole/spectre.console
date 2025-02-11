namespace Spectre.Console.Tests.Unit;

public partial class AnsiConsoleTests
{
    public sealed class Confirm
    {
        [Theory]
        [InlineData(true, true, true)]
        [InlineData(false, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, false, false)]
        public async Task Should_Return_Default_Value_If_Nothing_Is_Entered(bool async, bool defaultValue, bool expected)
        {
            // Given
            var console = new TestConsole().EmitAnsiSequences();
            console.Input.PushKey(ConsoleKey.Enter);

            // When
            bool result;
            if (async)
            {
                result = await console.ConfirmAsync("Want some prompt?", defaultValue, cancellationToken: TestContext.Current.CancellationToken);
            }
            else
            {
                result = console.Confirm("Want some prompt?", defaultValue);
            }

            // Then
            result.ShouldBe(expected);
        }
    }

    public sealed class Ask
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_Return_Correct_DateTime_When_Asked_PL_Culture(bool async)
        {
            // Given
            var console = new TestConsole().EmitAnsiSequences();
            console.Input.PushTextWithEnter("1/2/1998");

            // When
            DateTime dateTime;
            if (async)
            {
                dateTime = await console.AskAsync<DateTime>(string.Empty, CultureInfo.GetCultureInfo("pl-PL"), cancellationToken: TestContext.Current.CancellationToken);
            }
            else
            {
                dateTime = console.Ask<DateTime>(string.Empty, CultureInfo.GetCultureInfo("pl-PL"));
            }

            // Then
            dateTime.ShouldBe(new DateTime(1998, 2, 1));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_Return_Correct_DateTime_When_Asked_US_Culture(bool async)
        {
            // Given
            var console = new TestConsole().EmitAnsiSequences();
            console.Input.PushTextWithEnter("2/1/1998");

            // When
            DateTime dateTime;
            if (async)
            {
                dateTime = await console.AskAsync<DateTime>(string.Empty, CultureInfo.GetCultureInfo("en-US"), cancellationToken: TestContext.Current.CancellationToken);
            }
            else
            {
                dateTime = console.Ask<DateTime>(string.Empty, CultureInfo.GetCultureInfo("en-US"));
            }

            // Then
            dateTime.ShouldBe(new DateTime(1998, 2, 1));
        }
    }
}
