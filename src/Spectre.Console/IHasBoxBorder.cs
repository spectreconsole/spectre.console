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
    /// <param name="obj">The object to set the border for.</param>
    /// <typeparam name="T">An object type with a border.</typeparam>
    extension<T>(T obj) where T : class, IHasBoxBorder
    {
        /// <summary>
        /// Sets the border.
        /// </summary>
        /// <param name="border">The border to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Border(BoxBorder border)
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.Border = border;
            return obj;
        }

        /// <summary>
        /// Do not display a border.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T NoBorder()
        {
            return Border(obj, BoxBorder.None);
        }

        /// <summary>
        /// Display a square border.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T SquareBorder()
        {
            return Border(obj, BoxBorder.Square);
        }

        /// <summary>
        /// Display an ASCII border.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T AsciiBorder()
        {
            return Border(obj, BoxBorder.Ascii);
        }

        /// <summary>
        /// Display a rounded border.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T RoundedBorder()
        {
            return Border(obj, BoxBorder.Rounded);
        }

        /// <summary>
        /// Display a heavy border.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T HeavyBorder()
        {
            return Border(obj, BoxBorder.Heavy);
        }

        /// <summary>
        /// Display a double border.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T DoubleBorder()
        {
            return Border(obj, BoxBorder.Double);
        }
    }
}