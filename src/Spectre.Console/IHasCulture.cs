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
    /// <summary>
    /// Sets the culture.
    /// </summary>
    /// <typeparam name="T">An object type with a culture.</typeparam>
    /// <param name="obj">The object to set the culture for.</param>
    /// <param name="culture">The culture to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T Culture<T>(this T obj, CultureInfo culture)
        where T : class, IHasCulture
    {
        ArgumentNullException.ThrowIfNull(obj);
        ArgumentNullException.ThrowIfNull(culture);

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
    public static T Culture<T>(this T obj, string name)
        where T : class, IHasCulture
    {
        ArgumentNullException.ThrowIfNull(obj);
        ArgumentNullException.ThrowIfNull(name);

        return Culture(obj, CultureInfo.GetCultureInfo(name));
    }

    /// <summary>
    /// Sets the culture.
    /// </summary>
    /// <typeparam name="T">An object type with a culture.</typeparam>
    /// <param name="obj">The object to set the culture for.</param>
    /// <param name="culture">The culture to set.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T Culture<T>(this T obj, int culture)
        where T : class, IHasCulture
    {
        ArgumentNullException.ThrowIfNull(obj);

        return Culture(obj, CultureInfo.GetCultureInfo(culture));
    }
}