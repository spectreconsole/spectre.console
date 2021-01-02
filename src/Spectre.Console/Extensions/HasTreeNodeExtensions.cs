using System;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IHasCulture"/>.
    /// </summary>
    public static class HasTreeNodeExtensions
    {
        /// <summary>
        /// Adds a tree node.
        /// </summary>
        /// <typeparam name="T">An object type with tree nodes.</typeparam>
        /// <param name="obj">The object that has tree nodes.</param>
        /// <param name="markup">The node's markup text.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T AddNode<T>(this T obj, string markup)
            where T : class, IHasTreeNodes
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (markup is null)
            {
                throw new ArgumentNullException(nameof(markup));
            }

            return AddNode(obj, new Markup(markup));
        }

        /// <summary>
        /// Adds a tree node.
        /// </summary>
        /// <typeparam name="T">An object type with tree nodes.</typeparam>
        /// <param name="obj">The object that has tree nodes.</param>
        /// <param name="markup">The node's markup text.</param>
        /// <param name="action">An action that can be used to configure the created node further.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T AddNode<T>(this T obj, string markup, Action<TreeNode> action)
            where T : class, IHasTreeNodes
        {
            if (markup is null)
            {
                throw new ArgumentNullException(nameof(markup));
            }

            return AddNode(obj, new Markup(markup), action);
        }

        /// <summary>
        /// Adds a tree node.
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

            if (renderable is null)
            {
                throw new ArgumentNullException(nameof(renderable));
            }

            obj.Children.Add(new TreeNode(renderable));
            return obj;
        }

        /// <summary>
        /// Adds a tree node.
        /// </summary>
        /// <typeparam name="T">An object type with tree nodes.</typeparam>
        /// <param name="obj">The object that has tree nodes.</param>
        /// <param name="renderable">The renderable to add.</param>
        /// <param name="action">An action that can be used to configure the created node further.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T AddNode<T>(this T obj, IRenderable renderable, Action<TreeNode> action)
            where T : class, IHasTreeNodes
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (renderable is null)
            {
                throw new ArgumentNullException(nameof(renderable));
            }

            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var node = new TreeNode(renderable);
            action(node);

            obj.Children.Add(node);
            return obj;
        }

        /// <summary>
        /// Adds a tree node.
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

            if (node is null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            obj.Children.Add(node);
            return obj;
        }

        /// <summary>
        /// Add multiple tree nodes.
        /// </summary>
        /// <typeparam name="T">An object type with tree nodes.</typeparam>
        /// <param name="obj">The object that has tree nodes.</param>
        /// <param name="nodes">The tree nodes to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T AddNodes<T>(this T obj, params string[] nodes)
            where T : class, IHasTreeNodes
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (nodes is null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            obj.Children.AddRange(nodes.Select(node => new TreeNode(new Markup(node))));
            return obj;
        }

        /// <summary>
        /// Add multiple tree nodes.
        /// </summary>
        /// <typeparam name="T">An object type with tree nodes.</typeparam>
        /// <param name="obj">The object that has tree nodes.</param>
        /// <param name="nodes">The tree nodes to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T AddNodes<T>(this T obj, params TreeNode[] nodes)
            where T : class, IHasTreeNodes
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (nodes is null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            obj.Children.AddRange(nodes);
            return obj;
        }
    }
}
