namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IHasJustification"/>.
/// </summary>
public static class HasJustificationExtensions
{
    /// <summary>
    /// Sets the justification for an <see cref="IHasJustification"/> object.
    /// </summary>
    /// <typeparam name="T">The type that can be justified.</typeparam>
    /// <param name="obj">The alignable object.</param>
    /// <param name="alignment">The alignment.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T Justify<T>(this T obj, Justify? alignment)
        where T : class, IHasJustification
    {
        if (obj is null)
        {
            throw new System.ArgumentNullException(nameof(obj));
        }

        obj.Justification = alignment;
        return obj;
    }

    /// <summary>
    /// Sets the <see cref="IHasJustification"/> object to be left justified.
    /// </summary>
    /// <typeparam name="T">The type that can be justified.</typeparam>
    /// <param name="obj">The alignable object.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T LeftJustified<T>(this T obj)
        where T : class, IHasJustification
    {
        if (obj is null)
        {
            throw new System.ArgumentNullException(nameof(obj));
        }

        obj.Justification = Console.Justify.Left;
        return obj;
    }

    /// <summary>
    /// Sets the <see cref="IHasJustification"/> object to be centered.
    /// </summary>
    /// <typeparam name="T">The type that can be justified.</typeparam>
    /// <param name="obj">The alignable object.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T Centered<T>(this T obj)
        where T : class, IHasJustification
    {
        if (obj is null)
        {
            throw new System.ArgumentNullException(nameof(obj));
        }

        obj.Justification = Console.Justify.Center;
        return obj;
    }

    /// <summary>
    /// Sets the <see cref="IHasJustification"/> object to be right justified.
    /// </summary>
    /// <typeparam name="T">The type that can be justified.</typeparam>
    /// <param name="obj">The alignable object.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T RightJustified<T>(this T obj)
        where T : class, IHasJustification
    {
        if (obj is null)
        {
            throw new System.ArgumentNullException(nameof(obj));
        }

        obj.Justification = Console.Justify.Right;
        return obj;
    }
}