namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IHasTreeNodes"/>.
/// </summary>
public static class HasTreeNodeExtensions
{
    /// <param name="obj">The object to add the tree node to.</param>
    /// <typeparam name="T">An object with tree nodes.</typeparam>
    extension<T>(T obj) where T : IHasTreeNodes
    {
        /// <summary>
        /// Adds a tree node.
        /// </summary>
        /// <param name="markup">The node's markup text.</param>
        /// <returns>The added tree node.</returns>
        public TreeNode AddNode(string markup)
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
        /// <param name="renderable">The renderable to add.</param>
        /// <returns>The added tree node.</returns>
        public TreeNode AddNode(IRenderable renderable)
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
        /// <param name="node">The tree node to add.</param>
        /// <returns>The added tree node.</returns>
        public TreeNode AddNode(TreeNode node)
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
        /// <param name="nodes">The tree nodes to add.</param>
        public void AddNodes(params string[] nodes)
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
        /// <param name="nodes">The tree nodes to add.</param>
        public void AddNodes(IEnumerable<string> nodes)
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
        /// <param name="nodes">The tree nodes to add.</param>
        public void AddNodes(params IRenderable[] nodes)
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
        /// <param name="nodes">The tree nodes to add.</param>
        public void AddNodes(IEnumerable<IRenderable> nodes)
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
        /// <param name="nodes">The tree nodes to add.</param>
        public void AddNodes(params TreeNode[] nodes)
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
        /// <param name="nodes">The tree nodes to add.</param>
        public void AddNodes(IEnumerable<TreeNode> nodes)
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