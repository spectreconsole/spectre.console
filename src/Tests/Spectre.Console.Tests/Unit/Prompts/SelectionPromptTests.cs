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

    [Fact] public void Should_Throw_Meaningful_Exception_For_Empty_Prompt()
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
    public void Should_Initially_Select_The_First_Item_When_No_Default_Is_Specified() {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var prompt = new SelectionPrompt<string>()
                .Title("Select one")
                .AddChoices("First", "Second", "Third");

        prompt.Show(console);

        // Then
        console.Lines.ShouldBe([
            "Select one",
            "          ",
            "> First   ",
            "  Second  ",
            "  Third   ",
        ]);
    }

    [Fact]
    public void Should_Initially_Select_The_Default_Item_When_It_Exists_In_The_Choices() {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var prompt = new SelectionPrompt<string>()
                .Title("Select one")
                .AddChoices("First", "Second", "Third")
                .DefaultValue("Second");

        prompt.Show(console);

        // Then
        console.Lines.ShouldBe([
            "Select one",
            "          ",
            "  First   ",
            "> Second  ",
            "  Third   ",
        ]);
    }

    [Fact]
    public void Should_Initially_Select_The_First_Item_When_Default_Does_Not_Exist_In_The_Choices() {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var prompt = new SelectionPrompt<string>()
                .Title("Select one")
                .AddChoices("First", "Second", "Third")
                .DefaultValue("Fourth");

        prompt.Show(console);

        // Then
        console.Lines.ShouldBe([
            "Select one",
            "          ",
            "> First   ",
            "  Second  ",
            "  Third   ",
        ]);
    }

    [Fact]
    public void Should_Initially_Select_The_Default_Item_When_Scrolling_Is_Required_And_Item_Is_Not_Last() {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var prompt = new SelectionPrompt<string>()
                .Title("Select one")
                .AddChoices("First", "Second", "Third", "Fourth", "Fifth", "Sixth")
                .DefaultValue("Third")
                .PageSize(3);

        prompt.Show(console);

        // Then
        console.Lines.ShouldBe([
            "Select one                               ",
            "                                         ",
            "  Second                                 ",
            "> Third                                  ",
            "  Fourth                                 ",
            "                                         ",
            "(Move up and down to reveal more choices)",
        ]);
    }

    [Fact]
    public void Should_Initially_Select_The_Default_Item_When_Scrolling_Is_Required_And_Item_Is_Last() {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var prompt = new SelectionPrompt<string>()
                .Title("Select one")
                .AddChoices("First", "Second", "Third", "Fourth", "Fifth", "Sixth")
                .DefaultValue("Sixth")
                .PageSize(3);

        prompt.Show(console);

        // Then
        console.Lines.ShouldBe([
            "Select one                               ",
            "                                         ",
            "  Fourth                                 ",
            "  Fifth                                  ",
            "> Sixth                                  ",
            "                                         ",
            "(Move up and down to reveal more choices)",
        ]);
    }

    [Fact]
    public void Should_Initially_Select_The_Default_Value_When_Skipping_Unselectable_Items_And_Default_Value_Is_Leaf() {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var prompt = new SelectionPrompt<string>()
                .Title("Select one")
                .AddChoiceGroup("Group one", "First", "Second")
                .AddChoiceGroup("Group two", "Third", "Fourth")
                .Mode(SelectionMode.Leaf)
                .DefaultValue("Third");

        prompt.Show(console);

        // Then
        console.Lines.ShouldBe([
            "Select one  ",
            "            ",
            "  Group one ",
            "    First   ",
            "    Second  ",
            "  Group two ",
            "  > Third   ",
            "    Fourth  ",
        ]);
    }

    [Fact]
    public void Should_Initially_Select_The_First_Leaf_When_Skipping_Unselectable_Items_And_Default_Value_Is_Not_Leaf() {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var prompt = new SelectionPrompt<string>()
                .Title("Select one")
                .AddChoiceGroup("Group one", "First", "Second")
                .AddChoiceGroup("Group two", "Third", "Fourth")
                .Mode(SelectionMode.Leaf)
                .DefaultValue("Group two");

        prompt.Show(console);

        // Then
        console.Lines.ShouldBe([
            "Select one  ",
            "            ",
            "  Group one ",
            "  > First   ",
            "    Second  ",
            "  Group two ",
            "    Third   ",
            "    Fourth  ",
        ]);
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
