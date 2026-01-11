namespace Spectre.Console;

/// <summary>
/// Represents something that is alignable.
/// </summary>
public interface IAlignable
{
    /// <summary>
    /// Gets or sets the alignment.
    /// </summary>
    Justify? Alignment { get; set; }
}

/// <summary>
/// Contains extension methods for <see cref="IAlignable"/>.
/// </summary>
public static class AlignableExtensions
{
    /// <summary>
    /// Sets the alignment for an <see cref="IAlignable"/> object.
    /// </summary>
    /// <typeparam name="T">The alignable object type.</typeparam>
    /// <param name="obj">The alignable object.</param>
    /// <param name="alignment">The alignment.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T Alignment<T>(this T obj, Justify? alignment)
        where T : class, IAlignable
    {
        ArgumentNullException.ThrowIfNull(obj);

        obj.Alignment = alignment;
        return obj;
    }

    /// <summary>
    /// Sets the <see cref="IAlignable"/> object to be left aligned.
    /// </summary>
    /// <typeparam name="T">The alignable type.</typeparam>
    /// <param name="obj">The alignable object.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T LeftAligned<T>(this T obj)
        where T : class, IAlignable
    {
        ArgumentNullException.ThrowIfNull(obj);

        obj.Alignment = Justify.Left;
        return obj;
    }

    /// <summary>
    /// Sets the <see cref="IAlignable"/> object to be centered.
    /// </summary>
    /// <typeparam name="T">The alignable type.</typeparam>
    /// <param name="obj">The alignable object.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T Centered<T>(this T obj)
        where T : class, IAlignable
    {
        ArgumentNullException.ThrowIfNull(obj);

        obj.Alignment = Justify.Center;
        return obj;
    }

    /// <summary>
    /// Sets the <see cref="IAlignable"/> object to be right aligned.
    /// </summary>
    /// <typeparam name="T">The alignable type.</typeparam>
    /// <param name="obj">The alignable object.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T RightAligned<T>(this T obj)
        where T : class, IAlignable
    {
        ArgumentNullException.ThrowIfNull(obj);

        obj.Alignment = Justify.Right;
        return obj;
    }
}