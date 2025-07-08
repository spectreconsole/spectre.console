namespace Spectre.Console.Testing;

/// <summary>
/// Provides extensions for testing using the Shouldly-style fluent assertions.
/// </summary>
public static class ShouldlyExtensions
{
    /// <summary>
    /// Performs the specified action on the given object and then returns the object.
    /// Useful for fluent testing patterns where additional assertions or operations
    /// are chained together in a readable manner.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="item">The object to operate on.</param>
    /// <param name="action">An action to perform on the object.</param>
    /// <returns>The original object, to allow further chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="action"/> is null.</exception>
    [DebuggerStepThrough]
    public static T And<T>(this T item, Action<T> action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        action(item);
        return item;
    }
}
