using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IHasTreeNodes"/>.
    /// </summary>
    public static class HasTreeNodeExtensions
    {
        /// <summary>
        /// Adds a tree node.
        /// </summary>
        /// <typeparam name="T">An object with tree nodes.</typeparam>
        /// <param name="obj">The object to add the tree node to.</param>
        /// <param name="markup">The node's markup text.</param>
        /// <returns>The added tree node.</returns>
        public static TreeNode AddNode<T>(this T obj, string markup)
            where T : IHasTreeNodes
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
        /// <typeparam name="T">An object with tree nodes.</typeparam>
        /// <param name="obj">The object to add the tree node to.</param>
        /// <param name="renderable">The renderable to add.</param>
        /// <returns>The added tree node.</returns>
        public static TreeNode AddNode<T>(this T obj, IRenderable renderable)
            where T : IHasTreeNodes
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (renderable is null)
            {
                throw new ArgumentNullException(nameof(renderable));
            }

            var node = new TreeNode(renderable);
            obj.Nodes.Add(node);
            return node;
        }

        /// <summary>
        /// Adds a tree node.
        /// </summary>
        /// <typeparam name="T">An object with tree nodes.</typeparam>
        /// <param name="obj">The object to add the tree node to.</param>
        /// <param name="node">The tree node to add.</param>
        /// <returns>The added tree node.</returns>
        public static TreeNode AddNode<T>(this T obj, TreeNode node)
            where T : IHasTreeNodes
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (node is null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            obj.Nodes.Add(node);
            return node;
        }

        /// <summary>
        /// Add multiple tree nodes.
        /// </summary>
        /// <typeparam name="T">An object with tree nodes.</typeparam>
        /// <param name="obj">The object to add the tree nodes to.</param>
        /// <param name="nodes">The tree nodes to add.</param>
        public static void AddNodes<T>(this T obj, params string[] nodes)
            where T : IHasTreeNodes
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (nodes is null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            obj.Nodes.AddRange(nodes.Select(node => new TreeNode(new Markup(node))));
        }

        /// <summary>
        /// Add multiple tree nodes.
        /// </summary>
        /// <typeparam name="T">An object with tree nodes.</typeparam>
        /// <param name="obj">The object to add the tree nodes to.</param>
        /// <param name="nodes">The tree nodes to add.</param>
        public static void AddNodes<T>(this T obj, IEnumerable<string> nodes)
            where T : IHasTreeNodes
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (nodes is null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            obj.Nodes.AddRange(nodes.Select(node => new TreeNode(new Markup(node))));
        }

        /// <summary>
        /// Add multiple tree nodes.
        /// </summary>
        /// <typeparam name="T">An object with tree nodes.</typeparam>
        /// <param name="obj">The object to add the tree nodes to.</param>
        /// <param name="nodes">The tree nodes to add.</param>
        public static void AddNodes<T>(this T obj, params IRenderable[] nodes)
            where T : IHasTreeNodes
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (nodes is null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            obj.Nodes.AddRange(nodes.Select(node => new TreeNode(node)));
        }

        /// <summary>
        /// Add multiple tree nodes.
        /// </summary>
        /// <typeparam name="T">An object with tree nodes.</typeparam>
        /// <param name="obj">The object to add the tree nodes to.</param>
        /// <param name="nodes">The tree nodes to add.</param>
        public static void AddNodes<T>(this T obj, IEnumerable<IRenderable> nodes)
            where T : IHasTreeNodes
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (nodes is null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            obj.Nodes.AddRange(nodes.Select(node => new TreeNode(node)));
        }

        /// <summary>
        /// Add multiple tree nodes.
        /// </summary>
        /// <typeparam name="T">An object with tree nodes.</typeparam>
        /// <param name="obj">The object to add the tree nodes to.</param>
        /// <param name="nodes">The tree nodes to add.</param>
        public static void AddNodes<T>(this T obj, params TreeNode[] nodes)
            where T : IHasTreeNodes
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (nodes is null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            obj.Nodes.AddRange(nodes);
        }

        /// <summary>
        /// Add multiple tree nodes.
        /// </summary>
        /// <typeparam name="T">An object with tree nodes.</typeparam>
        /// <param name="obj">The object to add the tree nodes to.</param>
        /// <param name="nodes">The tree nodes to add.</param>
        public static void AddNodes<T>(this T obj, IEnumerable<TreeNode> nodes)
            where T : IHasTreeNodes
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (nodes is null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            obj.Nodes.AddRange(nodes);
        }
    }
}
