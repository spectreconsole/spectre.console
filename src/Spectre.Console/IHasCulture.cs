namespace Spectre.Console;

/// <summary>
/// Represents something that has a culture.
/// </summary>
public interface IHasCulture
{
    /// <summary>
    /// Gets or sets the culture.
    /// </summary>
    CultureInfo? Culture { get; set; }
}

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
            ArgumentNullException.ThrowIfNull(obj);

            ArgumentNullException.ThrowIfNull(culture);

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
            ArgumentNullException.ThrowIfNull(name);

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