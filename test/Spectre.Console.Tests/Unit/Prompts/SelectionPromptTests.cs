namespace Spectre.Console.Tests.Unit;

public sealed class SelectionPromptTests
{
    [Fact]
    [GitHubIssue(608)]
    public void Should_Not_Throw_When_Selecting_An_Item_With_Escaped_Markup()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Enter);
        var input = "[red]This text will never be red[/]".EscapeMarkup();

        // When
        var prompt = new SelectionPrompt<string>()
                .Title("Select one")
                .AddChoices(input);
        prompt.Show(console);

        // Then
        console.Output.ShouldContain(@"[red]This text will never be red[/]");
    }

    [Fact]
    public void Should_Set_Property_If_Aborted()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Escape);
        var input = new string[] { "a", "b", "c" };

        // When
        var prompt = new SelectionPrompt<string>()
                .Title("Select one")
                .AllowAbort(true)
                .AddChoices(input);
        prompt.Show(console);

        // Then
        prompt.Aborted.ShouldBe(true);
    }

    [Fact]
    public void Should_Set_Property_If_Not_Aborted()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Enter);
        var input = new string[] { "a", "b", "c" };

        // When
        var prompt = new SelectionPrompt<string>()
                .Title("Select one")
                .AddChoices(input);
        prompt.Show(console);

        // Then
        prompt.Aborted.ShouldBe(false);
    }
}
