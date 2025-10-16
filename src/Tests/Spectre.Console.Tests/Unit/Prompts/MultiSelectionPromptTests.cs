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

    [Fact]
    public void Should_Throw_Meaningful_Exception_For_Empty_Prompt()
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
    public void Should_Return_All_Selected_Items()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Spacebar);
        console.Input.PushKey(ConsoleKey.DownArrow);
        console.Input.PushKey(ConsoleKey.Spacebar);
        console.Input.PushKey(ConsoleKey.Enter);


        // When
        var prompt = new MultiSelectionPrompt<string>();
        prompt.AddChoices(["A", "B", "C", "D"]);
        var selection = prompt.Show(console);

        // Then
        selection.ShouldBe(["A", "B"]);
    }

    [Fact]
    public void Should_Return_CancelResult_On_Cancel_FuncVersion()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Spacebar);
        console.Input.PushKey(ConsoleKey.DownArrow);
        console.Input.PushKey(ConsoleKey.Spacebar);
        console.Input.PushKey(ConsoleKey.Escape);


        // When
        var prompt = new MultiSelectionPrompt<string>();
        prompt.AddChoices(["A", "B", "C", "D"]);
        prompt.AddCancelResult(() => ["E"]);
        var selection = prompt.Show(console);

        // Then
        selection.ShouldBe(["E"]);
    }

    [Fact]
    public void Should_Return_CancelResult_On_Cancel_ListVersion()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Spacebar);
        console.Input.PushKey(ConsoleKey.DownArrow);
        console.Input.PushKey(ConsoleKey.Spacebar);
        console.Input.PushKey(ConsoleKey.Escape);


        // When
        var prompt = new MultiSelectionPrompt<string>();
        prompt.AddChoices(["A", "B", "C", "D"]);
        prompt.AddCancelResult(["E", "F"]);
        var selection = prompt.Show(console);

        // Then
        selection.ShouldBe(["E", "F"]);
    }

    [Fact]
    public void Should_Return_CancelResult_On_Cancel_ItemVersion()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Spacebar);
        console.Input.PushKey(ConsoleKey.DownArrow);
        console.Input.PushKey(ConsoleKey.Spacebar);
        console.Input.PushKey(ConsoleKey.Escape);


        // When
        var prompt = new MultiSelectionPrompt<string>();
        prompt.AddChoices(["A", "B", "C", "D"]);
        prompt.AddCancelResult("E");
        var selection = prompt.Show(console);

        // Then
        selection.ShouldBe(["E"]);
    }

    [Fact]
    public void Should_Return_CancelResult_On_Cancel_EmptyVersion()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Spacebar);
        console.Input.PushKey(ConsoleKey.DownArrow);
        console.Input.PushKey(ConsoleKey.Spacebar);
        console.Input.PushKey(ConsoleKey.Escape);


        // When
        var prompt = new MultiSelectionPrompt<string>();
        prompt.AddChoices(["A", "B", "C", "D"]);
        prompt.AddCancelResult();
        var selection = prompt.Show(console);

        // Then
        selection.ShouldBe([]);
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
