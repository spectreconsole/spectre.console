namespace Spectre.Console.Tests.Unit;

public sealed class ListPromptStateTests
{
    private ListPromptState<string> CreateListPromptState(int count, int pageSize, bool shouldWrap)
        => new(Enumerable.Repeat(new ListPromptItem<string>(string.Empty), count).ToList(), pageSize, shouldWrap);

    [Fact]
    public void Should_Have_Start_Index_Zero()
    {
        // Given
        var state = CreateListPromptState(100, 10, false);

        // When
        /* noop */

        // Then
        state.Index.ShouldBe(0);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Should_Increase_Index(bool wrap)
    {
        // Given
        var state = CreateListPromptState(100, 10, wrap);
        var index = state.Index;

        // When
        state.Update(ConsoleKey.DownArrow);

        // Then
        state.Index.ShouldBe(index + 1);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Should_Go_To_End(bool wrap)
    {
        // Given
        var state = CreateListPromptState(100, 10, wrap);

        // When
        state.Update(ConsoleKey.End);

        // Then
        state.Index.ShouldBe(99);
    }

    [Fact]
    public void Should_Clamp_Index_If_No_Wrap()
    {
        // Given
        var state = CreateListPromptState(100, 10, false);
        state.Update(ConsoleKey.End);

        // When
        state.Update(ConsoleKey.DownArrow);

        // Then
        state.Index.ShouldBe(99);
    }

    [Fact]
    public void Should_Wrap_Index_If_Wrap()
    {
        // Given
        var state = CreateListPromptState(100, 10, true);
        state.Update(ConsoleKey.End);

        // When
        state.Update(ConsoleKey.DownArrow);

        // Then
        state.Index.ShouldBe(0);
    }

    [Fact]
    public void Should_Wrap_Index_If_Wrap_And_Down()
    {
        // Given
        var state = CreateListPromptState(100, 10, true);

        // When
        state.Update(ConsoleKey.UpArrow);

        // Then
        state.Index.ShouldBe(99);
    }

    [Fact]
    public void Should_Wrap_Index_If_Wrap_And_Page_Up()
    {
        // Given
        var state = CreateListPromptState(10, 100, true);

        // When
        state.Update(ConsoleKey.PageUp);

        // Then
        state.Index.ShouldBe(0);
    }

    [Fact]
    public void Should_Wrap_Index_If_Wrap_And_Offset_And_Page_Down()
    {
        // Given
        var state = CreateListPromptState(10, 100, true);
        state.Update(ConsoleKey.End);
        state.Update(ConsoleKey.UpArrow);

        // When
        state.Update(ConsoleKey.PageDown);

        // Then
        state.Index.ShouldBe(8);
    }
}
