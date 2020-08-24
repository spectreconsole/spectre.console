namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IAlignable"/>.
    /// </summary>
    public static class AlignableExtensions
    {
        /// <summary>
        /// Sets the alignment for an <see cref="IAlignable"/> object.
        /// </summary>
        /// <typeparam name="T">The alignable type.</typeparam>
        /// <param name="alignable">The alignable object.</param>
        /// <param name="alignment">The alignment.</param>
        /// <returns>The same alignable object.</returns>
        public static T WithAlignment<T>(this T alignable, Justify alignment)
            where T : IAlignable
        {
            alignable.Alignment = alignment;
            return alignable;
        }

        /// <summary>
        /// Sets the <see cref="IAlignable"/> object to be left aligned.
        /// </summary>
        /// <typeparam name="T">The alignable type.</typeparam>
        /// <param name="alignable">The alignable object.</param>
        /// <returns>The same alignable object.</returns>
        public static T LeftAligned<T>(this T alignable)
            where T : IAlignable
        {
            alignable.Alignment = Justify.Left;
            return alignable;
        }

        /// <summary>
        /// Sets the <see cref="IAlignable"/> object to be centered.
        /// </summary>
        /// <typeparam name="T">The alignable type.</typeparam>
        /// <param name="alignable">The alignable object.</param>
        /// <returns>The same alignable object.</returns>
        public static T Centered<T>(this T alignable)
            where T : IAlignable
        {
            alignable.Alignment = Justify.Center;
            return alignable;
        }

        /// <summary>
        /// Sets the <see cref="IAlignable"/> object to be right aligned.
        /// </summary>
        /// <typeparam name="T">The alignable type.</typeparam>
        /// <param name="alignable">The alignable object.</param>
        /// <returns>The same alignable object.</returns>
        public static T RightAligned<T>(this T alignable)
            where T : IAlignable
        {
            alignable.Alignment = Justify.Right;
            return alignable;
        }
    }
}
