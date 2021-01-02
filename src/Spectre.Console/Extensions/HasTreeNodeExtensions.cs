using System;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IHasCulture"/>.
    /// </summary>
    public static class HasTreeNodeExtensions
    {
        /// <summary>
        /// Adds a child tree node.
        /// </summary>
        /// <typeparam name="T">An object type with tree nodes.</typeparam>
        /// <param name="obj">The object that has tree nodes.</param>
        /// <param name="renderable">The renderable to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T AddNode<T>(this T obj, IRenderable renderable)
            where T : class, IHasTreeNodes
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Children.Add(new TreeNode(renderable));
            return obj;
        }

        /// <summary>
        /// Adds a child tree node.
        /// </summary>
        /// <typeparam name="T">An object type with tree nodes.</typeparam>
        /// <param name="obj">The object that has tree nodes.</param>
        /// <param name="renderable">The renderable to add.</param>
        /// <param name="config">An action that can be used to configure the created node further.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T AddNode<T>(this T obj, IRenderable renderable, Action<TreeNode> config)
            where T : class, IHasTreeNodes
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var node = new TreeNode(renderable);
            config(node);

            obj.Children.Add(node);
            return obj;
        }

        /// <summary>
        /// Adds a child tree node.
        /// </summary>
        /// <typeparam name="T">An object type with tree nodes.</typeparam>
        /// <param name="obj">The object that has tree nodes.</param>
        /// <param name="node">The tree node to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T AddNode<T>(this T obj, TreeNode node)
            where T : class, IHasTreeNodes
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Children.Add(node);
            return obj;
        }
    }
}
