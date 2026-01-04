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
    /// <param name="obj">The alignable object.</param>
    /// <typeparam name="T">The alignable object type.</typeparam>
    extension<T>(T obj) where T : class, IAlignable
    {
        /// <summary>
        /// Sets the alignment for an <see cref="IAlignable"/> object.
        /// </summary>
        /// <param name="alignment">The alignment.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Alignment(Justify? alignment)
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.Alignment = alignment;
            return obj;
        }

        /// <summary>
        /// Sets the <see cref="IAlignable"/> object to be left aligned.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T LeftAligned()
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.Alignment = Justify.Left;
            return obj;
        }

        /// <summary>
        /// Sets the <see cref="IAlignable"/> object to be centered.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Centered()
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.Alignment = Justify.Center;
            return obj;
        }

        /// <summary>
        /// Sets the <see cref="IAlignable"/> object to be right aligned.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T RightAligned()
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.Alignment = Justify.Right;
            return obj;
        }
    }
}