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
        var history = new PromptHistory();
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

    [Fact]
    public void Should_Return_True_When_User_Answers_Yes()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushTextWithEnter("y");

        // When
        var result = console.Prompt(new ConfirmationPrompt("Continue?"));

        // Then
        result.ShouldBe(true);
    }

    [Fact]
    public void Should_Return_False_When_User_Answers_No()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushTextWithEnter("n");

        // When
        var result = console.Prompt(new ConfirmationPrompt("Continue?"));

        // Then
        result.ShouldBe(false);
    }

    [Fact]
    public void Should_Add_Confirmation_Input_To_History()
    {
        // Given
        var history = new PromptHistory();
        var console = new TestConsole();
        console.Input.PushTextWithEnter("y");

        // When
        var result = console.Prompt(new ConfirmationPrompt("Continue?") { History = history });

        // Then
        result.ShouldBe(true);
        history.Entries.ShouldBe(new[] { "y" });
    }

    [Fact]
    public void Should_Share_History_Between_TextPrompt_And_ConfirmationPrompt()
    {
        // Given
        var sharedHistory = new PromptHistory();
        var console = new TestConsole();

        // First, a text prompt
        console.Input.PushTextWithEnter("hello");
        console.Prompt(new TextPrompt<string>("Enter text:") { History = sharedHistory });

        // Then, a confirmation prompt
        console.Input.PushTextWithEnter("n");
        var confirmResult = console.Prompt(new ConfirmationPrompt("Continue?") { History = sharedHistory });

        // Then
        confirmResult.ShouldBe(false);
        sharedHistory.Entries.ShouldBe(new[] { "hello", "n" });
    }

    [Fact]
    public void Should_Not_Add_Invalid_Confirmation_Input_To_History()
    {
        // Given
        var history = new PromptHistory();
        var console = new TestConsole();
        console.Input.PushTextWithEnter("maybe");
        console.Input.PushTextWithEnter("y");

        // When
        var result = console.Prompt(new ConfirmationPrompt("Continue?") { History = history });

        // Then
        result.ShouldBe(true);
        history.Entries.ShouldBe(new[] { "y" }); // Only the valid "y" is stored
    }
}