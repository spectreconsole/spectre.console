namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IPaddable"/>.
/// </summary>
public static class PaddableExtensions
{
    /// <param name="obj">The paddable object instance.</param>
    /// <typeparam name="T">An object implementing <see cref="IPaddable"/>.</typeparam>
    extension<T>(T obj) where T : class, IPaddable
    {
        /// <summary>
        /// Sets the left padding.
        /// </summary>
        /// <param name="left">The left padding.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T PadLeft(int left)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return Padding(obj, new Padding(left, obj.Padding.GetTopSafe(), obj.Padding.GetRightSafe(), obj.Padding.GetBottomSafe()));
        }

        /// <summary>
        /// Sets the top padding.
        /// </summary>
        /// <param name="top">The top padding.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T PadTop(int top)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return Padding(obj, new Padding(obj.Padding.GetLeftSafe(), top, obj.Padding.GetRightSafe(), obj.Padding.GetBottomSafe()));
        }

        /// <summary>
        /// Sets the right padding.
        /// </summary>
        /// <param name="right">The right padding.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T PadRight(int right)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return Padding(obj, new Padding(obj.Padding.GetLeftSafe(), obj.Padding.GetTopSafe(), right, obj.Padding.GetBottomSafe()));
        }

        /// <summary>
        /// Sets the bottom padding.
        /// </summary>
        /// <param name="bottom">The bottom padding.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T PadBottom(int bottom)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return Padding(obj, new Padding(obj.Padding.GetLeftSafe(), obj.Padding.GetTopSafe(), obj.Padding.GetRightSafe(), bottom));
        }

        /// <summary>
        /// Sets the left, top, right and bottom padding.
        /// </summary>
        /// <param name="left">The left padding to apply.</param>
        /// <param name="top">The top padding to apply.</param>
        /// <param name="right">The right padding to apply.</param>
        /// <param name="bottom">The bottom padding to apply.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Padding(int left, int top, int right, int bottom)
        {
            return Padding(obj, new Padding(left, top, right, bottom));
        }

        /// <summary>
        /// Sets the horizontal and vertical padding.
        /// </summary>
        /// <param name="horizontal">The left and right padding.</param>
        /// <param name="vertical">The top and bottom padding.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Padding(int horizontal, int vertical)
        {
            return Padding(obj, new Padding(horizontal, vertical));
        }

        /// <summary>
        /// Sets the padding.
        /// </summary>
        /// <param name="padding">The padding to apply.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Padding(Padding padding)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Padding = padding;
            return obj;
        }
    }
}