using System;
using System.ComponentModel;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IAlignable"/>.
    /// </summary>
    public static class ObsoleteAlignableExtensions
    {
        /// <summary>
        /// Sets the alignment for an <see cref="IAlignable"/> object.
        /// </summary>
        /// <typeparam name="T">The alignable object type.</typeparam>
        /// <param name="obj">The alignable object.</param>
        /// <param name="alignment">The alignment.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        [Obsolete("Use Alignment(..) instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static T SetAlignment<T>(this T obj, Justify alignment)
            where T : class, IAlignable
        {
            if (obj is null)
            {
                throw new System.ArgumentNullException(nameof(obj));
            }

            obj.Alignment = alignment;
            return obj;
        }
    }
}
