namespace Spectre.Console;

/// <summary>
/// Represents something that has tree nodes.
/// </summary>
public interface IHasTreeNodes
{
    /// <summary>
    /// Gets the tree's child nodes.
    /// </summary>
    List<TreeNode> Nodes { get; }
}

/// <summary>
/// Contains extension methods for <see cref="IHasTreeNodes"/>.
/// </summary>
public static class HasTreeNodeExtensions
{
    /// <summary>
    /// Add multiple tree nodes.
    /// </summary>
    /// <param name="obj">The object to add the tree node to.</param>
    /// <param name="nodes">The tree nodes to add.</param>
    public static void AddNodes(this IHasTreeNodes obj, params string[] nodes)
    {
        // TODO: This is here temporary due to a bug in the .NET SDK
        // See issue: https://github.com/dotnet/roslyn/issues/80024

        ArgumentNullException.ThrowIfNull(obj);
        ArgumentNullException.ThrowIfNull(nodes);

        obj.Nodes.AddRange(nodes.Select(node => new TreeNode(new Markup(node))));
    }

    /// <summary>
    /// Add multiple tree nodes.
    /// </summary>
    /// <param name="obj">The object to add the tree node to.</param>
    /// <param name="nodes">The tree nodes to add.</param>
    public static void AddNodes(this IHasTreeNodes obj, params IRenderable[] nodes)
    {
        // TODO: This is here temporary due to a bug in the .NET SDK
        // See issue: https://github.com/dotnet/roslyn/issues/80024

        ArgumentNullException.ThrowIfNull(obj);
        ArgumentNullException.ThrowIfNull(nodes);

        obj.Nodes.AddRange(nodes.Select(node => new TreeNode(node)));
    }

    /// <summary>
    /// Add multiple tree nodes.
    /// </summary>
    /// <param name="obj">The object to add the tree node to.</param>
    /// <param name="nodes">The tree nodes to add.</param>
    public static void AddNodes(this IHasTreeNodes obj, params TreeNode[] nodes)
    {
        // TODO: This is here temporary due to a bug in the .NET SDK
        // See issue: https://github.com/dotnet/roslyn/issues/80024

        ArgumentNullException.ThrowIfNull(obj);
        ArgumentNullException.ThrowIfNull(nodes);

        obj.Nodes.AddRange(nodes);
    }

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
            ArgumentNullException.ThrowIfNull(obj);
            ArgumentNullException.ThrowIfNull(markup);

            return AddNode(obj, new Markup(markup));
        }

        /// <summary>
        /// Adds a tree node.
        /// </summary>
        /// <param name="renderable">The renderable to add.</param>
        /// <returns>The added tree node.</returns>
        public TreeNode AddNode(IRenderable renderable)
        {
            ArgumentNullException.ThrowIfNull(obj);
            ArgumentNullException.ThrowIfNull(renderable);

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
            ArgumentNullException.ThrowIfNull(obj);
            ArgumentNullException.ThrowIfNull(node);

            obj.Nodes.Add(node);
            return node;
        }

        /// <summary>
        /// Add multiple tree nodes.
        /// </summary>
        /// <param name="nodes">The tree nodes to add.</param>
        public void AddNodes(IEnumerable<string> nodes)
        {
            ArgumentNullException.ThrowIfNull(obj);
            ArgumentNullException.ThrowIfNull(nodes);

            obj.Nodes.AddRange(nodes.Select(node => new TreeNode(new Markup(node))));
        }

        /// <summary>
        /// Add multiple tree nodes.
        /// </summary>
        /// <param name="nodes">The tree nodes to add.</param>
        public void AddNodes(IEnumerable<IRenderable> nodes)
        {
            ArgumentNullException.ThrowIfNull(obj);
            ArgumentNullException.ThrowIfNull(nodes);

            obj.Nodes.AddRange(nodes.Select(node => new TreeNode(node)));
        }

        /// <summary>
        /// Add multiple tree nodes.
        /// </summary>
        /// <param name="nodes">The tree nodes to add.</param>
        public void AddNodes(IEnumerable<TreeNode> nodes)
        {
            ArgumentNullException.ThrowIfNull(obj);
            ArgumentNullException.ThrowIfNull(nodes);

            obj.Nodes.AddRange(nodes);
        }
    }
}