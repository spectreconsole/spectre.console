namespace Spectre.Console.Tests.Unit;

public sealed class ConfirmationTextPromptTests
{
    [Theory]
    [InlineData("y", true)]
    [InlineData("n", false)]
    public void Should_Require_Enter_By_Default(
        string input, bool expected)
    {
        // Given
        var console = new TestConsole();
        console.Input.PushTextWithEnter(input);

        // When
        var result = console.Prompt(
            new ConfirmationPrompt("Accept?")
                .Yes('y')
                .No('n'));

        // Then
        result.ShouldBe(expected);
        console.Input.IsKeyAvailable().ShouldBeFalse();
    }

    [Theory]
    [InlineData('y', true)]
    [InlineData('n', false)]
    public void Should_Not_Require_Enter_If_RequireEnter_Is_Set_To_False(
        char input, bool expected)
    {
        // Given
        var console = new TestConsole();
        console.Input.PushCharacter(input);

        // When
        var result = console.Prompt(
            new ConfirmationPrompt("Accept?")
                .RequireEnter(false)
                .Yes('y')
                .No('n'));

        // Then
        result.ShouldBe(expected);
    }

    [Fact]
    public void Should_Ignore_Invalid_Input()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushCharacter('a');
        console.Input.PushCharacter('b');
        console.Input.PushCharacter('c');
        console.Input.PushCharacter('y');

        // When
        var result = console.Prompt(
            new ConfirmationPrompt("Accept?")
                .RequireEnter(false)
                .Yes('y')
                .No('n'));

        // Then
        result.ShouldBe(true);
        console.Input.IsKeyAvailable().ShouldBeFalse();
    }
}