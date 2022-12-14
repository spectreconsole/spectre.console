namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="SelectionPrompt{T}"/>.
/// </summary>
public static class SelectionPromptExtensions
{
    /// <summary>
    /// Sets the selection mode.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="mode">The selection mode.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static SelectionPrompt<T> Mode<T>(this SelectionPrompt<T> obj, SelectionMode mode)
        where T : notnull
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.Mode = mode;
        return obj;
    }

    /// <summary>
    /// Adds multiple choices.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="choices">The choices to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static SelectionPrompt<T> AddChoices<T>(this SelectionPrompt<T> obj, params T[] choices)
        where T : notnull
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        foreach (var choice in choices)
        {
            obj.AddChoice(choice);
        }

        return obj;
    }

    /// <summary>
    /// Adds multiple choices.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="choices">The choices to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static SelectionPrompt<T> AddChoices<T>(this SelectionPrompt<T> obj, IEnumerable<T> choices)
        where T : notnull
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        foreach (var choice in choices)
        {
            obj.AddChoice(choice);
        }

        return obj;
    }

    /// <summary>
    /// Adds multiple grouped choices.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="group">The group.</param>
    /// <param name="choices">The choices to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static SelectionPrompt<T> AddChoiceGroup<T>(this SelectionPrompt<T> obj, T group, IEnumerable<T> choices)
        where T : notnull
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        var root = obj.AddChoice(group);
        foreach (var choice in choices)
        {
            root.AddChild(choice);
        }

        return obj;
    }

    /// <summary>
    /// Adds multiple grouped choices.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="group">The group.</param>
    /// <param name="choices">The choices to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static SelectionPrompt<T> AddChoiceGroup<T>(this SelectionPrompt<T> obj, T group, params T[] choices)
        where T : notnull
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        var root = obj.AddChoice(group);
        foreach (var choice in choices)
        {
            root.AddChild(choice);
        }

        return obj;
    }

    /// <summary>
    /// Sets the title.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="title">The title markup text.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static SelectionPrompt<T> Title<T>(this SelectionPrompt<T> obj, string? title)
        where T : notnull
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.Title = title;
        return obj;
    }

    /// <summary>
    /// Sets how many choices that are displayed to the user.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="pageSize">The number of choices that are displayed to the user.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static SelectionPrompt<T> PageSize<T>(this SelectionPrompt<T> obj, int pageSize)
        where T : notnull
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        if (pageSize <= 2)
        {
            throw new ArgumentException("Page size must be greater or equal to 3.", nameof(pageSize));
        }

        obj.PageSize = pageSize;
        return obj;
    }

    /// <summary>
    /// Sets whether the selection should wrap around when reaching its edges.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="shouldWrap">Whether the selection should wrap around.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static SelectionPrompt<T> WrapAround<T>(this SelectionPrompt<T> obj, bool shouldWrap = true)
        where T : notnull
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.WrapAround = shouldWrap;
        return obj;
    }

    /// <summary>
    /// Sets the highlight style of the selected choice.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="highlightStyle">The highlight style of the selected choice.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static SelectionPrompt<T> HighlightStyle<T>(this SelectionPrompt<T> obj, Style highlightStyle)
        where T : notnull
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.HighlightStyle = highlightStyle;
        return obj;
    }

    /// <summary>
    /// Sets the text that will be displayed if there are more choices to show.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="text">The text to display.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static SelectionPrompt<T> MoreChoicesText<T>(this SelectionPrompt<T> obj, string? text)
        where T : notnull
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.MoreChoicesText = text;
        return obj;
    }

    /// <summary>
    /// Sets the function to create a display string for a given choice.
    /// </summary>
    /// <typeparam name="T">The prompt type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="displaySelector">The function to get a display string for a given choice.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static SelectionPrompt<T> UseConverter<T>(this SelectionPrompt<T> obj, Func<T, string>? displaySelector)
        where T : notnull
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.Converter = displaySelector;
        return obj;
    }
}