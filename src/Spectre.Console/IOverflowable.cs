namespace Spectre.Console;

/// <summary>
/// Represents something that can overflow.
/// </summary>
public interface IOverflowable
{
    /// <summary>
    /// Gets or sets the text overflow strategy.
    /// </summary>
    Overflow? Overflow { get; set; }
}

/// <summary>
/// Contains extension methods for <see cref="IOverflowable"/>.
/// </summary>
public static class OverflowableExtensions
{
    /// <param name="obj">The overflowable object instance.</param>
    /// <typeparam name="T">An object implementing <see cref="IOverflowable"/>.</typeparam>
    extension<T>(T obj) where T : class, IOverflowable
    {
        /// <summary>
        /// Folds any overflowing text.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Fold()
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return Overflow(obj, Console.Overflow.Fold);
        }

        /// <summary>
        /// Crops any overflowing text.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Crop()
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return Overflow(obj, Console.Overflow.Crop);
        }

        /// <summary>
        /// Crops any overflowing text and adds an ellipsis to the end.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Ellipsis()
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return Overflow(obj, Console.Overflow.Ellipsis);
        }

        /// <summary>
        /// Sets the overflow strategy.
        /// </summary>
        /// <param name="overflow">The overflow strategy to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Overflow(Overflow overflow)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Overflow = overflow;
            return obj;
        }
    }
}