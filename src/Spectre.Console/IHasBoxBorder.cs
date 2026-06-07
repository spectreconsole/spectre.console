namespace Spectre.Console;

/// <summary>
/// Represents something that has a box border.
/// </summary>
public interface IHasBoxBorder
{
    /// <summary>
    /// Gets or sets the box.
    /// </summary>
    public BoxBorder Border { get; set; }
}

/// <summary>
/// Contains extension methods for <see cref="IHasBoxBorder"/>.
/// </summary>
public static class HasBoxBorderExtensions
{
    /// <summary>
    /// Sets the border.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <param name="border">The border to use.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T Border<T>(this T obj, BoxBorder border)
        where T : class, IHasBoxBorder
    {
        ArgumentNullException.ThrowIfNull(obj);

        obj.Border = border;
        return obj;
    }

    /// <summary>
    /// Do not display a border.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T NoBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.None);
    }

    /// <summary>
    /// Display a square border.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T SquareBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.Square);
    }

    /// <summary>
    /// Display an ASCII border.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T AsciiBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.Ascii);
    }

    /// <summary>
    /// Display a rounded border.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T RoundedBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.Rounded);
    }

    /// <summary>
    /// Display a heavy border.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T HeavyBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.Heavy);
    }

    /// <summary>
    /// Display a double border.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T DoubleBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.Double);
    }

    /// <summary>
    /// Display a border with heavy horizontal edges and light vertical edges.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T HeavyHorizontalBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.HeavyHorizontal);
    }

    /// <summary>
    /// Display a border with heavy vertical edges and light horizontal edges.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T HeavyVerticalBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.HeavyVertical);
    }

    /// <summary>
    /// Display a border with double horizontal edges and single vertical edges.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T DoubleHorizontalBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.DoubleHorizontal);
    }

    /// <summary>
    /// Display a border with double vertical edges and single horizontal edges.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T DoubleVerticalBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.DoubleVertical);
    }

    /// <summary>
    /// Display a dashed border with square corners.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T DashedBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.Dashed);
    }

    /// <summary>
    /// Display a dashed border with rounded corners.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T RoundedDashedBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.RoundedDashed);
    }

    /// <summary>
    /// Display a heavy dashed border.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T HeavyDashedBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.HeavyDashed);
    }

    /// <summary>
    /// Display a dotted border with square corners.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T DottedBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.Dotted);
    }

    /// <summary>
    /// Display a dotted border with rounded corners.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T RoundedDottedBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.RoundedDotted);
    }

    /// <summary>
    /// Display a heavy dotted border.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T HeavyDottedBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.HeavyDotted);
    }

    /// <summary>
    /// Display a wide-dashed border with square corners.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T DashedWideBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.DashedWide);
    }

    /// <summary>
    /// Display a wide-dashed border with rounded corners.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T RoundedDashedWideBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.RoundedDashedWide);
    }

    /// <summary>
    /// Display a heavy wide-dashed border.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T HeavyDashedWideBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.HeavyDashedWide);
    }

    /// <summary>
    /// Display a "near" border that hugs the content using thin block elements.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T NearBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.Near);
    }

    /// <summary>
    /// Display a beveled border using thin block edges and diagonal corners.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T BeveledBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.Beveled);
    }

    /// <summary>
    /// Display the horizontal variant of the McGugan border.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T McGuganHorizontalBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.McGuganHorizontal);
    }

    /// <summary>
    /// Display the vertical variant of the McGugan border.
    /// </summary>
    /// <typeparam name="T">An object type with a border.</typeparam>
    /// <param name="obj">The object to set the border for.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static T McGuganVerticalBorder<T>(this T obj)
        where T : class, IHasBoxBorder
    {
        return Border(obj, BoxBorder.McGuganVertical);
    }
}