namespace Spectre.Console.Tests.Unit;

public sealed class MultiSelectionPromptTests
{
    [Fact]
    public void Should_Not_Mark_Item_As_Selected_By_Default()
    {
        // Given
        var prompt = new MultiSelectionPrompt<int>();

        // When
        var choice = prompt.AddChoice(32);

        // Then
        choice.IsSelected.ShouldBeFalse();
    }

    [Fact]
    public void Should_Mark_Item_As_Selected()
    {
        // Given
        var prompt = new MultiSelectionPrompt<int>();
        var choice = prompt.AddChoice(32);

        // When
        prompt.Select(32);

        // Then
        choice.IsSelected.ShouldBeTrue();
    }

    [Fact]
    public void Should_Mark_Custom_Item_As_Selected_If_The_Same_Reference_Is_Used()
    {
        // Given
        var prompt = new MultiSelectionPrompt<CustomItem>();
        var item = new CustomItem { X = 18, Y = 32 };
        var choice = prompt.AddChoice(item);

        // When
        prompt.Select(item);

        // Then
        choice.IsSelected.ShouldBeTrue();
    }

    [Fact]
    public void Should_Mark_Custom_Item_As_Selected_If_A_Comparer_Is_Provided()
    {
        // Given
        var prompt = new MultiSelectionPrompt<CustomItem>(new CustomItem.Comparer());
        var choice = prompt.AddChoice(new CustomItem { X = 18, Y = 32 });

        // When
        prompt.Select(new CustomItem { X = 18, Y = 32 });

        // Then
        choice.IsSelected.ShouldBeTrue();
    }

    [Fact]
    public void Should_Get_The_Direct_Parent()
    {
        // Given
        var prompt = new MultiSelectionPrompt<string>();
        prompt.AddChoice("root").AddChild("level-1").AddChild("level-2").AddChild("item");

        // When
        var actual = prompt.GetParent("item");

        // Then
        actual.ShouldBe("level-2");
    }

    [Fact]
    public void Should_Get_The_List_Of_All_Parents()
    {
        // Given
        var prompt = new MultiSelectionPrompt<string>();
        prompt.AddChoice("root").AddChild("level-1").AddChild("level-2").AddChild("item");

        // When
        var actual = prompt.GetParents("item");

        // Then
        actual.ShouldBe(new[] { "root", "level-1", "level-2" });
    }

    [Fact]
    public void Should_Get_An_Empty_List_Of_Parents_For_Root_Node()
    {
        // Given
        var prompt = new MultiSelectionPrompt<string>();
        prompt.AddChoice("root");

        // When
        var actual = prompt.GetParents("root");

        // Then
        actual.ShouldBeEmpty();
    }

    [Fact]
    public void Should_Get_Null_As_Direct_Parent_Of_Root_Node()
    {
        // Given
        var prompt = new MultiSelectionPrompt<string>();
        prompt.AddChoice("root");

        // When
        var actual = prompt.GetParent("root");

        // Then
        actual.ShouldBeNull();
    }

    [Fact]
    public void Should_Throw_When_Getting_Parents_Of_Non_Existing_Node()
    {
        // Given
        var prompt = new MultiSelectionPrompt<string>();
        prompt.AddChoice("root").AddChild("level-1").AddChild("level-2").AddChild("item");

        // When
        Action action = () => prompt.GetParents("non-existing");

        // Then
        action.ShouldThrow<ArgumentOutOfRangeException>();
    }

    [Fact] public void Should_Throw_Meaningful_Exception_For_Empty_Prompt()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Spacebar);

        var prompt = new MultiSelectionPrompt<string>();

        // When
        Action action = () => prompt.Show(console);

        // Then
        var exception = action.ShouldThrow<InvalidOperationException>();
        exception.Message.ShouldBe("Cannot show an empty selection prompt. Please call the AddChoice() method to configure the prompt.");
    }

    [Fact]
    public void Should_Select_Node_And_All_Children_In_Leaf_Mode_On_Space()
    {
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;

        // Simulate pressing Space to toggle selection, then Enter to submit
        console.Input.PushKey(ConsoleKey.Spacebar);
        console.Input.PushKey(ConsoleKey.Enter);

        var prompt = new MultiSelectionPrompt<string>();
        prompt.AddChoice("root").AddChild("level-1").AddChild("level-2").AddChild("item");

        // When
        var result = prompt.Show(console);

        // Then
        result.ShouldBe(new[] { "item" });
    }

    [Fact]
    public void Should_Add_Title_To_Prompt()
    {
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;

        // Set the title
        var prompt = new MultiSelectionPrompt<string>();
        // var result = prompt.Title = "Test Title";
        Action action = () => prompt.Title("Test Title");
        action();

        // Then
        prompt.Title.ShouldBe("Test Title");
    }

    [Fact]
    public void Page_Size_Less_Than_Three_Throws_Exception()
    {
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;

        var prompt = new MultiSelectionPrompt<string>();
        Action action = () => prompt.PageSize(2);

        var exception = action.ShouldThrow<ArgumentException>();
        exception.Message.ShouldContain("Page size must be greater or equal to 3.");
    }

    [Fact]
    public void Should_Add_Choice_Group_To_Prompt()
    {
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;

        var prompt = new MultiSelectionPrompt<string>();
        List<string> choices = new List<string> { "root", "level-1", "level-2", };

        Action action = () => prompt.AddChoiceGroup("New Group", choices);
        action();

        prompt.Tree.Find("New Group").ShouldNotBeNull();
        prompt.Tree.Find("level-1").ShouldNotBeNull();
        prompt.Tree.Find("level-2").ShouldNotBeNull();
    }

    [Fact]
    public void Should_Add_Choice_Group_To_Prompt_With_Array_Arg_Type()
    {
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;

        var prompt = new MultiSelectionPrompt<string>();
        string[] choices = new []{ "root", "level-1", "level-2", };

        Action action = () => prompt.AddChoiceGroup("New Group", choices);
        action();

        prompt.Tree.Find("New Group").ShouldNotBeNull();
        prompt.Tree.Find("level-1").ShouldNotBeNull();
        prompt.Tree.Find("level-2").ShouldNotBeNull();
    }

    [Fact]
    public void Should_Set_Wrap_Around_To_True_With_No_Arguments()
    {
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;

        var prompt = new MultiSelectionPrompt<string>();

        Action action = () => prompt.WrapAround();
        action();

        prompt.WrapAround.ShouldBeTrue();
    }

    [Fact]
    public void Should_Set_Highlight_Styles()
    {
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;

        var foregroundColor = new Color(255, 0, 0);
        var backgroundColor = new Color(255, 255, 0);
        var decoration = Decoration.Bold;

        var style = new Style(foregroundColor, backgroundColor, decoration);

        var prompt = new MultiSelectionPrompt<string>();
        Action action = () => prompt.HighlightStyle(style);
        action.ShouldNotThrow();

        prompt.HighlightStyle.ShouldBe(style);
    }

    [Fact]
    public void More_Choices_Should_Be_Null_When_Passed_Null_Value()
    {
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;

        var prompt = new MultiSelectionPrompt<string>();
        Action action = () => prompt.MoreChoicesText(null);
        action.ShouldNotThrow();

        prompt.MoreChoicesText.ShouldBeNull();
    }

    [Fact]
    public void Instructions_Text_Should_Be_Null_When_Passed_Null_Value()
    {
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;

        var prompt = new MultiSelectionPrompt<string>();
        Action action = () => prompt.InstructionsText(null);
        action.ShouldNotThrow();

        prompt.InstructionsText.ShouldBeNull();
    }

    [Fact]
    public void Required_Should_Be_True_When_No_Argument_Passed()
    {
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;

        var prompt = new MultiSelectionPrompt<string>();
        Action action = () => prompt.Required();
        action.ShouldNotThrow();
        prompt.Required.ShouldBeTrue();
    }
}

file sealed class CustomItem
{
    public int X { get; set; }
    public int Y { get; set; }

    public class Comparer : IEqualityComparer<CustomItem>
    {
        public bool Equals(CustomItem x, CustomItem y)
        {
            return x.X == y.X && x.Y == y.Y;
        }

        public int GetHashCode(CustomItem obj)
        {
            throw new NotSupportedException();
        }
    }
}
