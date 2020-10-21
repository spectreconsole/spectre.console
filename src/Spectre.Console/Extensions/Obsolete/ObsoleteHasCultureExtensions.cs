using System;
using System.ComponentModel;
using System.Globalization;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IHasCulture"/>.
    /// </summary>
    public static class ObsoleteHasCultureExtensions
    {
        /// <summary>
        /// Sets the culture.
        /// </summary>
        /// <typeparam name="T">An object type with a culture.</typeparam>
        /// <param name="obj">The object to set the culture for.</param>
        /// <param name="culture">The culture to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        [Obsolete("Use Culture(..) instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static T SetCulture<T>(this T obj, CultureInfo culture)
            where T : class, IHasCulture
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (culture is null)
            {
                throw new ArgumentNullException(nameof(culture));
            }

            obj.Culture = culture;
            return obj;
        }

        /// <summary>
        /// Sets the culture.
        /// </summary>
        /// <typeparam name="T">An object type with a culture.</typeparam>
        /// <param name="obj">The object to set the culture for.</param>
        /// <param name="name">The culture to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        [Obsolete("Use Culture(..) instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static T SetCulture<T>(this T obj, string name)
            where T : class, IHasCulture
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Culture = CultureInfo.GetCultureInfo(name);
            return obj;
        }

        /// <summary>
        /// Sets the culture.
        /// </summary>
        /// <typeparam name="T">An object type with a culture.</typeparam>
        /// <param name="obj">The object to set the culture for.</param>
        /// <param name="name">The culture to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        [Obsolete("Use Culture(..) instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static T SetCulture<T>(this T obj, int name)
            where T : class, IHasCulture
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Culture = CultureInfo.GetCultureInfo(name);
            return obj;
        }
    }
}
