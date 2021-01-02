namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IHasCulture"/>.
    /// </summary>
    public static class HasTreeNodeExtensions
    {
        /// <summary>
        /// Adds a child to the end of the node's list of children.
        /// </summary>
        /// <typeparam name="T">An object type with tree nodes.</typeparam>
        /// <param name="obj">The object that has tree nodes.</param>
        /// <param name="child">Child to be added.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T AddChild<T>(this T obj, TreeNode child)
            where T : class, IHasTreeNodes
        {
            if (obj is null)
            {
                throw new System.ArgumentNullException(nameof(obj));
            }

            obj.Children.Add(child);
            return obj;
        }
    }
}
