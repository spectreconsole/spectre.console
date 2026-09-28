namespace Spectre.Console.Tests.Unit;

public sealed class TestConsoleInputTests
{
    [Theory]
    [InlineData(ConsoleKey.UpArrow)]
    [InlineData(ConsoleKey.DownArrow)]
    [InlineData(ConsoleKey.LeftArrow)]
    [InlineData(ConsoleKey.RightArrow)]
    [InlineData(ConsoleKey.Home)]
    [InlineData(ConsoleKey.End)]
    [InlineData(ConsoleKey.PageUp)]
    [InlineData(ConsoleKey.PageDown)]
    [InlineData(ConsoleKey.Insert)]
    [InlineData(ConsoleKey.Delete)]
    [InlineData(ConsoleKey.F1)]
    [InlineData(ConsoleKey.F12)]
    public void Should_Not_Produce_A_Character_For_Navigation_And_Function_Keys(ConsoleKey key)
    {
        // Given
        var input = new TestConsoleInput();

        // When
        input.PushKey(key);

        // Then
        var result = input.ReadKey(true);
        result.ShouldNotBeNull();
        result.Value.Key.ShouldBe(key);
        result.Value.KeyChar.ShouldBe('\0');
    }

    [Theory]
    [InlineData(ConsoleKey.Enter, '\r')]
    [InlineData(ConsoleKey.Tab, '\t')]
    [InlineData(ConsoleKey.Spacebar, ' ')]
    [InlineData(ConsoleKey.A, 'A')]
    [InlineData(ConsoleKey.D1, '1')]
    public void Should_Produce_A_Character_For_Other_Keys(ConsoleKey key, char expected)
    {
        // Given
        var input = new TestConsoleInput();

        // When
        input.PushKey(key);

        // Then
        var result = input.ReadKey(true);
        result.ShouldNotBeNull();
        result.Value.KeyChar.ShouldBe(expected);
    }

    [Fact]
    public void Should_Not_Add_Navigation_Keys_To_Text_Prompt_Input()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushText("Hello");
        console.Input.PushKey(ConsoleKey.UpArrow);
        console.Input.PushKey(ConsoleKey.End);
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var result = console.Prompt(new TextPrompt<string>("Name:"));

        // Then
        result.ShouldBe("Hello");
    }
}
