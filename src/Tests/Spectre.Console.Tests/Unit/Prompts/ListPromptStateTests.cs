namespace Spectre.Console.Tests.Unit;

public sealed class ListPromptStateTests
{
    private ListPromptState<string> CreateListPromptState(int count, int pageSize, bool shouldWrap, bool searchEnabled)
        => new(
            Enumerable.Range(0, count).Select(i => new ListPromptItem<string>(i.ToString())).ToList(),
            text => text,
            pageSize, shouldWrap, SelectionMode.Independent, true, searchEnabled);

    [Fact]
    public void Should_Have_Start_Index_Zero()
    {
        // Given
        var state = CreateListPromptState(100, 10, false, false);

        // When
        /* noop */

        // Then
        state.Index.ShouldBe(0);
    }

    [Theory]
    [InlineData(ConsoleKey.UpArrow)]
    [InlineData(ConsoleKey.K)]
    public void Should_Decrease_Index(ConsoleKey key)
    {
        // Given
        var state = CreateListPromptState(100, 10, false, false);
        state.Update(ConsoleKey.End.ToConsoleKeyInfo());
        var index = state.Index;

        // When
        state.Update(key.ToConsoleKeyInfo());

        // Then
        state.Index.ShouldBe(index - 1);
    }

    [Theory]
    [InlineData(ConsoleKey.DownArrow, true)]
    [InlineData(ConsoleKey.DownArrow, false)]
    [InlineData(ConsoleKey.J, true)]
    [InlineData(ConsoleKey.J, false)]
    public void Should_Increase_Index(ConsoleKey key, bool wrap)
    {
        // Given
        var state = CreateListPromptState(100, 10, wrap, false);
        var index = state.Index;

        // When
        state.Update(key.ToConsoleKeyInfo());

        // Then
        state.Index.ShouldBe(index + 1);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Should_Go_To_End(bool wrap)
    {
        // Given
        var state = CreateListPromptState(100, 10, wrap, false);

        // When
        state.Update(ConsoleKey.End.ToConsoleKeyInfo());

        // Then
        state.Index.ShouldBe(99);
    }

    [Theory]
    [InlineData(ConsoleKey.DownArrow)]
    [InlineData(ConsoleKey.J)]
    public void Should_Clamp_Index_If_No_Wrap(ConsoleKey key)
    {
        // Given
        var state = CreateListPromptState(100, 10, false, false);
        state.Update(ConsoleKey.End.ToConsoleKeyInfo());

        // When
        state.Update(key.ToConsoleKeyInfo());

        // Then
        state.Index.ShouldBe(99);
    }

    [Theory]
    [InlineData(ConsoleKey.DownArrow)]
    [InlineData(ConsoleKey.J)]
    public void Should_Wrap_Index_If_Wrap(ConsoleKey key)
    {
        // Given
        var state = CreateListPromptState(100, 10, true, false);
        state.Update(ConsoleKey.End.ToConsoleKeyInfo());

        // When
        state.Update(key.ToConsoleKeyInfo());

        // Then
        state.Index.ShouldBe(0);
    }

    [Theory]
    [InlineData(ConsoleKey.UpArrow)]
    [InlineData(ConsoleKey.K)]
    public void Should_Wrap_Index_If_Wrap_And_Down(ConsoleKey key)
    {
        // Given
        var state = CreateListPromptState(100, 10, true, false);

        // When
        state.Update(key.ToConsoleKeyInfo());

        // Then
        state.Index.ShouldBe(99);
    }

    [Fact]
    public void Should_Wrap_Index_If_Wrap_And_Page_Up()
    {
        // Given
        var state = CreateListPromptState(10, 100, true, false);

        // When
        state.Update(ConsoleKey.PageUp.ToConsoleKeyInfo());

        // Then
        state.Index.ShouldBe(0);
    }

    [Theory]
    [InlineData(ConsoleKey.UpArrow)]
    [InlineData(ConsoleKey.K)]
    public void Should_Wrap_Index_If_Wrap_And_Offset_And_Page_Down(ConsoleKey key)
    {
        // Given
        var state = CreateListPromptState(10, 100, true, false);
        state.Update(ConsoleKey.End.ToConsoleKeyInfo());
        state.Update(key.ToConsoleKeyInfo());

        // When
        state.Update(ConsoleKey.PageDown.ToConsoleKeyInfo());

        // Then
        state.Index.ShouldBe(8);
    }

    [Fact]
    public void Should_Jump_To_First_Matching_Item_When_Searching()
    {
        // Given
        var state = CreateListPromptState(10, 100, true, true);

        // When
        state.Update(ConsoleKey.D3.ToConsoleKeyInfo());

        // Then
        state.Index.ShouldBe(3);
    }

    [Fact]
    public void Should_Jump_Back_To_First_Item_When_Clearing_Search_Term()
    {
        // Given
        var state = CreateListPromptState(10, 100, true, true);

        // When
        state.Update(ConsoleKey.D3.ToConsoleKeyInfo());
        state.Update(ConsoleKey.Backspace.ToConsoleKeyInfo());

        // Then
        state.Index.ShouldBe(0);
    }

    [Fact]
    public void Should_Cycle_To_Next_Match_On_Tab()
    {
        // Given
        var state = CreateListPromptState(20, 10, shouldWrap: true, searchEnabled: true);

        // When: type '1' -> first matching item is "1" (index 1)
        state.Update(ConsoleKey.D1.ToConsoleKeyInfo());
        var moved = state.Update(ConsoleKey.Tab.ToConsoleKeyInfo());

        // Then: next match after 1 is "10" (index 10)
        moved.ShouldBeTrue();
        state.Index.ShouldBe(10);

        // And: pressing Tab again goes to "11" (index 11)
        state.Update(ConsoleKey.Tab.ToConsoleKeyInfo());
        state.Index.ShouldBe(11);
    }

    [Fact]
    public void Should_Wrap_To_First_Match_On_Tab()
    {
        // Given
        var state = CreateListPromptState(20, 10, shouldWrap: true, searchEnabled: true);

        // When: set search to '1' so matches are [1,10,11,12,13,14,15,16,17,18,19]
        state.Update(ConsoleKey.D1.ToConsoleKeyInfo());

        // Move to last item (index 19), which is a match
        state.Update(ConsoleKey.End.ToConsoleKeyInfo());
        state.Index.ShouldBe(19);

        // Then: Tab wraps to first match (index 1)
        var moved = state.Update(ConsoleKey.Tab.ToConsoleKeyInfo());
        moved.ShouldBeTrue();
        state.Index.ShouldBe(1);
    }

    [Fact]
    public void Should_Not_Cycle_When_Search_Disabled()
    {
        // Given
        var state = CreateListPromptState(20, 10, shouldWrap: true, searchEnabled: false);
        var start = state.Index;

        // When
        var moved = state.Update(ConsoleKey.Tab.ToConsoleKeyInfo());

        // Then
        moved.ShouldBeFalse();
        state.Index.ShouldBe(start);
    }

    [Fact]
    public void Should_Not_Cycle_When_Search_Empty()
    {
        // Given
        var state = CreateListPromptState(20, 10, shouldWrap: true, searchEnabled: true);
        var start = state.Index;

        // When: Tab without any search text
        var moved = state.Update(ConsoleKey.Tab.ToConsoleKeyInfo());

        // Then
        moved.ShouldBeFalse();
        state.Index.ShouldBe(start);
    }

    [Fact]
    public void Should_Not_Move_When_No_Matches_For_Search()
    {
        // Given
        var state = CreateListPromptState(10, 10, shouldWrap: true, searchEnabled: true);
        state.Update(ConsoleKey.End.ToConsoleKeyInfo()); // move away from 0 so a change would be visible
        var indexBefore = state.Index;

        // When: type a character that yields no matches (e.g., 'x')
        state.Update(ConsoleKey.X.ToConsoleKeyInfo());

        // Tab should do nothing because there are no matches
        var moved = state.Update(ConsoleKey.Tab.ToConsoleKeyInfo());

        // Then
        moved.ShouldBeFalse();
        state.Index.ShouldBe(indexBefore);
    }

    [Fact]
    public void Should_Return_True_When_Tab_Changes_Index()
    {
        // Given
        var state = CreateListPromptState(20, 10, shouldWrap: true, searchEnabled: true);

        // When
        state.Update(ConsoleKey.D1.ToConsoleKeyInfo()); // go to index 1
        var moved = state.Update(ConsoleKey.Tab.ToConsoleKeyInfo()); // should go to 10

        // Then
        moved.ShouldBeTrue();
        state.Index.ShouldBe(10);
    }

    [Fact]
    public void Should_Return_False_When_Tab_Noop()
    {
        // Given
        var state = CreateListPromptState(20, 10, shouldWrap: true, searchEnabled: true);
        var start = state.Index;

        // When: Tab without search term -> no-op
        var moved = state.Update(ConsoleKey.Tab.ToConsoleKeyInfo());

        // Then
        moved.ShouldBeFalse();
        state.Index.ShouldBe(start);
    }
}
