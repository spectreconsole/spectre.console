using System;
using System.ComponentModel;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IOverflowable"/>.
    /// </summary>
    public static class ObsoleteOverflowableExtensions
    {
        /// <summary>
        /// Sets the overflow strategy.
        /// </summary>
        /// <typeparam name="T">An object implementing <see cref="IOverflowable"/>.</typeparam>
        /// <param name="obj">The overflowable object instance.</param>
        /// <param name="overflow">The overflow strategy to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        [Obsolete("Use Overflow(..) instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static T SetOverflow<T>(this T obj, Overflow overflow)
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
