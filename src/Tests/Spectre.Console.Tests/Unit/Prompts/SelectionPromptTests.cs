namespace Spectre.Console.Tests.Unit;

public sealed class SelectionPromptTests
{
    private const string ESC = "\u001b";

    [Fact]
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
    public void Should_Select_The_First_Leaf_Item()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var prompt = new SelectionPrompt<string>()
                .Title("Select one")
                .Mode(SelectionMode.Leaf)
                .AddChoiceGroup("Group one", "A", "B")
                .AddChoiceGroup("Group two", "C", "D");
        var selection = prompt.Show(console);

        // Then
        selection.ShouldBe("A");
    }

    [Fact]
    public void Should_Select_The_Last_Leaf_Item_When_Wrapping_Around()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.UpArrow);
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var prompt = new SelectionPrompt<string>()
            .Title("Select one")
            .Mode(SelectionMode.Leaf)
            .WrapAround()
            .AddChoiceGroup("Group one", "A", "B")
            .AddChoiceGroup("Group two", "C", "D");
        var selection = prompt.Show(console);

        // Then
        selection.ShouldBe("D");
    }

    [Fact]
    public void Should_Highlight_Search_Term()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.EmitAnsiSequences();
        console.Input.PushText("1");
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var prompt = new SelectionPrompt<string>()
            .Title("Select one")
            .EnableSearch()
            .AddChoices("Item 1");
        prompt.Show(console);

        // Then
        console.Output.ShouldContain($"{ESC}[38;5;12m> Item {ESC}[0m{ESC}[1;38;5;12;48;5;11m1{ESC}[0m");
    }

    [Fact]
    public void Should_Search_In_Remapped_Result()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.EmitAnsiSequences();
        console.Input.PushText("2");
        console.Input.PushKey(ConsoleKey.Enter);

        var choices = new List<CustomSelectionItem>
        {
            new(33, "Item 1"),
            new(34, "Item 2"),
        };

        var prompt = new SelectionPrompt<CustomSelectionItem>()
            .Title("Select one")
            .EnableSearch()
            .UseConverter(o => o.Name)
            .AddChoices(choices);

        // When
        var selection = prompt.Show(console);

        // Then
        selection.ShouldBe(choices[1]);
    }

    [Fact]
    public void Should_Throw_Meaningful_Exception_For_Empty_Prompt()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;

        var prompt = new SelectionPrompt<string>();

        // When
        Action action = () => prompt.Show(console);

        // Then
        var exception = action.ShouldThrow<InvalidOperationException>();
        exception.Message.ShouldBe("Cannot show an empty selection prompt. Please call the AddChoice() method to configure the prompt.");
    }

    [Fact]
    public void Should_Append_Space_To_Search_If_Search_Is_Enabled()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.EmitAnsiSequences();
        console.Input.PushText("Item");
        console.Input.PushKey(ConsoleKey.Spacebar);
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var prompt = new SelectionPrompt<string>()
            .Title("Search for something with space")
            .EnableSearch()
            .AddChoices("Item1")
            .AddChoices("Item 2");
        string result = prompt.Show(console);

        // Then
        result.ShouldBe("Item 2");
        console.Output.ShouldContain($"{ESC}[38;5;12m> {ESC}[0m{ESC}[1;38;5;12;48;5;11mItem {ESC}[0m{ESC}[38;5;12m2{ESC}[0m ");
    }
}

file sealed class CustomSelectionItem
{
    public int Value { get; }
    public string Name { get; }

    public CustomSelectionItem(int value, string name)
    {
        Value = value;
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}
