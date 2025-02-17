namespace Spectre.Console.Tests.Unit;

public sealed class ListPromptStateTests
{
    private ListPromptState<string> CreateListPromptState(int count, int pageSize, bool shouldWrap, bool searchEnabled, int initialIndex = 0)
        => new(
            Enumerable.Range(0, count).Select(i => new ListPromptItem<string>(i.ToString())).ToList(),
            text => text,
            pageSize, shouldWrap, SelectionMode.Independent, true, searchEnabled, initialIndex);

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void Should_Have_Specified_Start_Index(int index)
    {
        // Given
        var state = CreateListPromptState(100, 10, false, false, initialIndex: index);

        // When
        /* noop */

        // Then
        state.Index.ShouldBe(index);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Should_Increase_Index(bool wrap)
    {
        // Given
        var state = CreateListPromptState(100, 10, wrap, false);
        var index = state.Index;

        // When
        state.Update(ConsoleKey.DownArrow.ToConsoleKeyInfo());

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

    [Fact]
    public void Should_Clamp_Index_If_No_Wrap()
    {
        // Given
        var state = CreateListPromptState(100, 10, false, false);
        state.Update(ConsoleKey.End.ToConsoleKeyInfo());

        // When
        state.Update(ConsoleKey.DownArrow.ToConsoleKeyInfo());

        // Then
        state.Index.ShouldBe(99);
    }

    [Fact]
    public void Should_Wrap_Index_If_Wrap()
    {
        // Given
        var state = CreateListPromptState(100, 10, true, false);
        state.Update(ConsoleKey.End.ToConsoleKeyInfo());

        // When
        state.Update(ConsoleKey.DownArrow.ToConsoleKeyInfo());

        // Then
        state.Index.ShouldBe(0);
    }

    [Fact]
    public void Should_Wrap_Index_If_Wrap_And_Down()
    {
        // Given
        var state = CreateListPromptState(100, 10, true, false);

        // When
        state.Update(ConsoleKey.UpArrow.ToConsoleKeyInfo());

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

    [Fact]
    public void Should_Wrap_Index_If_Wrap_And_Offset_And_Page_Down()
    {
        // Given
        var state = CreateListPromptState(10, 100, true, false);
        state.Update(ConsoleKey.End.ToConsoleKeyInfo());
        state.Update(ConsoleKey.UpArrow.ToConsoleKeyInfo());

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
}
