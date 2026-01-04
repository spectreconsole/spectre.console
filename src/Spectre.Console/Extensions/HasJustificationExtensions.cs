namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IHasJustification"/>.
/// </summary>
public static class HasJustificationExtensions
{
    /// <param name="obj">The alignable object.</param>
    /// <typeparam name="T">The type that can be justified.</typeparam>
    extension<T>(T obj) where T : class, IHasJustification
    {
        /// <summary>
        /// Sets the justification for an <see cref="IHasJustification"/> object.
        /// </summary>
        /// <param name="alignment">The alignment.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Justify(Justify? alignment)
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
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T LeftJustified()
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
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Centered()
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
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T RightJustified()
        {
            if (obj is null)
            {
                throw new System.ArgumentNullException(nameof(obj));
            }

            obj.Justification = Console.Justify.Right;
            return obj;
        }
    }
}