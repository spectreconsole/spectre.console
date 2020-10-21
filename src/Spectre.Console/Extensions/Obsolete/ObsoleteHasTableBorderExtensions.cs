using System;
using System.ComponentModel;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IHasTableBorder"/>.
    /// </summary>
    public static class ObsoleteHasTableBorderExtensions
    {
        /// <summary>
        /// Sets the border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <param name="border">The border to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        [Obsolete("Use Border(..) instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static T SetBorder<T>(this T obj, TableBorder border)
            where T : class, IHasTableBorder
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Border = border;
            return obj;
        }
    }
}
