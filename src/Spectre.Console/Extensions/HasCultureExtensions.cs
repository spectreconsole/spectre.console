namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IHasCulture"/>.
/// </summary>
public static class HasCultureExtensions
{
    /// <param name="obj">The object to set the culture for.</param>
    /// <typeparam name="T">An object type with a culture.</typeparam>
    extension<T>(T obj) where T : class, IHasCulture
    {
        /// <summary>
        /// Sets the culture.
        /// </summary>
        /// <param name="culture">The culture to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Culture(CultureInfo culture)
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
        /// <param name="name">The culture to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Culture(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return Culture(obj, CultureInfo.GetCultureInfo(name));
        }

        /// <summary>
        /// Sets the culture.
        /// </summary>
        /// <param name="culture">The culture to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Culture(int culture)
        {
            return Culture(obj, CultureInfo.GetCultureInfo(culture));
        }
    }
}