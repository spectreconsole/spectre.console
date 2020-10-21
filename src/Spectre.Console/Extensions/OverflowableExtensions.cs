using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IOverflowable"/>.
    /// </summary>
    public static class OverflowableExtensions
    {
        /// <summary>
        /// Folds any overflowing text.
        /// </summary>
        /// <typeparam name="T">An object implementing <see cref="IOverflowable"/>.</typeparam>
        /// <param name="obj">The overflowable object instance.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T Fold<T>(this T obj)
            where T : class, IOverflowable
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
        /// <typeparam name="T">An object implementing <see cref="IOverflowable"/>.</typeparam>
        /// <param name="obj">The overflowable object instance.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T Crop<T>(this T obj)
            where T : class, IOverflowable
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
        /// <typeparam name="T">An object implementing <see cref="IOverflowable"/>.</typeparam>
        /// <param name="obj">The overflowable object instance.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T Ellipsis<T>(this T obj)
            where T : class, IOverflowable
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
        /// <typeparam name="T">An object implementing <see cref="IOverflowable"/>.</typeparam>
        /// <param name="obj">The overflowable object instance.</param>
        /// <param name="overflow">The overflow strategy to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T Overflow<T>(this T obj, Overflow overflow)
            where T : class, IOverflowable
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
