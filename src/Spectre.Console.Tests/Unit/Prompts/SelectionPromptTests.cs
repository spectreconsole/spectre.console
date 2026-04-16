namespace Spectre.Console.Tests.Unit;

public sealed class SelectionPromptTests
{
    private const string ESC = "\u001b";

    [Fact]
    public void Should_Not_Apply_Link_To_Text_After_Link_Close_Tag()
    {
        // Given
        var console = new TestConsole()
            .SupportsAnsi(true)
            .EmitAnsiSequences();

        // When - text after the [/] closing tag should NOT have the link
        console.Markup("Before [link=https://example.com]LINK[/] After");

        // Then
        var output = console.Output;

        // Link pattern: \e]8;id=xxx;url\e\\ text \e]8;;\e\\
        // "Before " should not be in a link
        // "LINK" should be in a link
        // " After" should not be in a link

        // The output should have exactly one link start and one link end
        var linkStartRegex = new System.Text.RegularExpressions.Regex(@"\x1b\]8;id=\d+;[^\x1b]+\x1b\\");
        var linkEndRegex = new System.Text.RegularExpressions.Regex(@"\x1b\]8;;\x1b\\");

        var linkStarts = linkStartRegex.Matches(output);
        var linkEnds = linkEndRegex.Matches(output);

        linkStarts.Count.ShouldBe(1, $"Expected 1 link start. Output:\n{output.Replace(ESC, "\\e")}");
        linkEnds.Count.ShouldBe(1, $"Expected 1 link end. Output:\n{output.Replace(ESC, "\\e")}");

        // Verify " After" appears AFTER the link end
        var linkEndIndex = linkEnds[0].Index;
        var afterIndex = output.IndexOf(" After");

        afterIndex.ShouldBeGreaterThan(linkEndIndex, $"' After' should appear after link end. Output:\n{output.Replace(ESC, "\\e")}");
    }

    [Fact]
    public void Should_Properly_Close_Links_In_Selection_Items()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Profile.Capabilities.Links = true;
        console.EmitAnsiSequences();
        // Navigate down to second item and then select it
        console.Input.PushKey(ConsoleKey.DownArrow);
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var prompt = new SelectionPrompt<string>()
            .Title("Select one")
            .AddChoices(
                "[link=https://example.com]Link 1[/]",
                "[link=https://example.org]Link 2[/]");
        prompt.Show(console);

        // Then
        // Each link should be properly closed with the OSC 8 terminator (ESC]8;;ESC\)
        // Count occurrences of link starts vs link ends
        var output = console.Output;
        var linkStartPattern = $"{ESC}]8;id=";
        var linkEndPattern = $"{ESC}]8;;{ESC}\\";

        var linkStarts = System.Text.RegularExpressions.Regex.Matches(output, System.Text.RegularExpressions.Regex.Escape(linkStartPattern)).Count;
        var linkEnds = System.Text.RegularExpressions.Regex.Matches(output, System.Text.RegularExpressions.Regex.Escape(linkEndPattern)).Count;

        // Every link start should have a corresponding link end
        linkStarts.ShouldBe(linkEnds, $"Link starts ({linkStarts}) should equal link ends ({linkEnds}). Output:\n{output.Replace(ESC, "\\e")}");
    }

    [Fact]
    public void Should_Close_Links_Before_Line_Clear_Operations()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Profile.Capabilities.Links = true;
        console.EmitAnsiSequences();
        // Navigate down to second item and then select it
        console.Input.PushKey(ConsoleKey.DownArrow);
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var prompt = new SelectionPrompt<string>()
            .Title("Select one")
            .AddChoices(
                "[link=https://example.com]Link 1[/]",
                "[link=https://example.org]Link 2[/]");
        prompt.Show(console);

        // Then
        // The output should never have a link start followed by erase-in-line (CSI K) without
        // first closing the link. This pattern causes the link to "bleed" into erased content.
        var output = console.Output;

        // Pattern: link start ... erase-in-line without intervening link end
        // Link start: ESC]8;id=...;url ESC\
        // Erase in line: ESC[K or ESC[0K or ESC[1K or ESC[2K
        // Link end: ESC]8;;ESC\
        var linkStartRegex = new System.Text.RegularExpressions.Regex($@"\x1b\]8;id=\d+;[^\x1b]+\x1b\\");
        var linkEndRegex = new System.Text.RegularExpressions.Regex($@"\x1b\]8;;\x1b\\");
        var eraseInLineRegex = new System.Text.RegularExpressions.Regex($@"\x1b\[\d?K");

        var allMatches = new List<(int Index, string Type, string Value)>();
        foreach (System.Text.RegularExpressions.Match m in linkStartRegex.Matches(output))
        {
            allMatches.Add((m.Index, "START", m.Value));
        }

        foreach (System.Text.RegularExpressions.Match m in linkEndRegex.Matches(output))
        {
            allMatches.Add((m.Index, "END", m.Value));
        }

        foreach (System.Text.RegularExpressions.Match m in eraseInLineRegex.Matches(output))
        {
            allMatches.Add((m.Index, "ERASE", m.Value));
        }

        allMatches.Sort((a, b) => a.Index.CompareTo(b.Index));

        // Check that no ERASE comes after START without END in between
        var linkOpen = false;
        foreach (var (index, type, value) in allMatches)
        {
            if (type == "START")
            {
                linkOpen = true;
            }
            else if (type == "END")
            {
                linkOpen = false;
            }
            else if (type == "ERASE" && linkOpen)
            {
                Assert.Fail($"Found erase-in-line operation while link was still open at index {index}. " +
                           $"This can cause links to 'bleed' into subsequent content.\n" +
                           $"Output (escaped): {output.Replace(ESC, "\\e")}");
            }
        }
    }

    [Fact]
    public void Should_Terminate_Links_At_End_Of_Line()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Profile.Capabilities.Links = true;
        console.EmitAnsiSequences();
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var prompt = new SelectionPrompt<string>()
            .Title("Select one")
            .AddChoices(
                "[link=https://example.com]Link 1[/]",
                "[link=https://example.org]Link 2[/]");
        prompt.Show(console);

        // Then
        // Verify that links don't cross line boundaries - each line's link should be
        // closed before the newline or before any cursor movement
        var output = console.Output;

        // Find all link sequences and line terminators
        var linkStartRegex = new System.Text.RegularExpressions.Regex($@"\x1b\]8;id=\d+;[^\x1b]+\x1b\\");
        var linkEndRegex = new System.Text.RegularExpressions.Regex($@"\x1b\]8;;\x1b\\");
        var newlineRegex = new System.Text.RegularExpressions.Regex($@"\r?\n");
        var cursorUpRegex = new System.Text.RegularExpressions.Regex($@"\x1b\[\d*A");

        var allMatches = new List<(int Index, string Type, string Value)>();
        foreach (System.Text.RegularExpressions.Match m in linkStartRegex.Matches(output))
        {
            allMatches.Add((m.Index, "START", m.Value));
        }

        foreach (System.Text.RegularExpressions.Match m in linkEndRegex.Matches(output))
        {
            allMatches.Add((m.Index, "END", m.Value));
        }

        foreach (System.Text.RegularExpressions.Match m in newlineRegex.Matches(output))
        {
            allMatches.Add((m.Index, "NEWLINE", m.Value));
        }

        foreach (System.Text.RegularExpressions.Match m in cursorUpRegex.Matches(output))
        {
            allMatches.Add((m.Index, "CURSOR_UP", m.Value));
        }

        allMatches.Sort((a, b) => a.Index.CompareTo(b.Index));

        // Check that no NEWLINE or CURSOR_UP comes after START without END in between
        var linkOpen = false;
        foreach (var (index, type, value) in allMatches)
        {
            if (type == "START")
            {
                linkOpen = true;
            }
            else if (type == "END")
            {
                linkOpen = false;
            }
            else if ((type == "NEWLINE" || type == "CURSOR_UP") && linkOpen)
            {
                Assert.Fail($"Found {type} while link was still open at index {index}. " +
                           $"Links should be closed before line/cursor changes.\n" +
                           $"Output (escaped): {output.Replace(ESC, "\\e")}");
            }
        }
    }

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

    [Fact]
    public void Should_Return_CancelResult_On_Cancel_FuncVersion()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Escape);

        // When
        var prompt = new SelectionPrompt<string>()
                .Title("Select one")
                .Mode(SelectionMode.Leaf)
                .AddChoiceGroup("Group one", "A", "B")
                .AddChoiceGroup("Group two", "C", "D")
                .AddCancelResult(() => "E");
        var selection = prompt.Show(console);

        // Then
        selection.ShouldBe("E");
    }

    [Fact]
    public void Should_Return_CancelResult_On_Cancel_ValueVersion()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Escape);

        // When
        var prompt = new SelectionPrompt<string>()
                .Title("Select one")
                .Mode(SelectionMode.Leaf)
                .AddChoiceGroup("Group one", "A", "B")
                .AddChoiceGroup("Group two", "C", "D")
                .AddCancelResult("E");
        var selection = prompt.Show(console);

        // Then
        selection.ShouldBe("E");
    }

    [Fact]
    public void Should_Ignore_Escape_If_CancelResult_Not_Set()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Escape);
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
    public void Should_Not_Throw_When_Searching_With_Escaped_Brackets_In_Choices()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushText("M");
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var prompt = new SelectionPrompt<string>()
            .Title("Select a subscription")
            .UseConverter(s => s.EscapeMarkup())
            .EnableSearch()
            .AddChoices(
                "MSFT-Provisioning-01[Prod] (guid-1)",
                "Normal Subscription (guid-2)");
        var result = prompt.Show(console);

        // Then
        result.ShouldBe("MSFT-Provisioning-01[Prod] (guid-1)");
    }

    [Fact]
    public void Should_Search_And_Select_Item_With_Brackets()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushText("Dev");
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var prompt = new SelectionPrompt<string>()
            .Title("Select")
            .UseConverter(s => s.EscapeMarkup())
            .EnableSearch()
            .AddChoices(
                "[Prod] Production",
                "[Dev] Development",
                "Staging");
        var result = prompt.Show(console);

        // Then
        result.ShouldBe("[Dev] Development");
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