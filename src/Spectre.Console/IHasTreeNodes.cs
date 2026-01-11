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

        ArgumentNullException.ThrowIfNull(markup);

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

        ArgumentNullException.ThrowIfNull(renderable);

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

        ArgumentNullException.ThrowIfNull(node);

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

        ArgumentNullException.ThrowIfNull(nodes);

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

        ArgumentNullException.ThrowIfNull(nodes);

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

        ArgumentNullException.ThrowIfNull(nodes);

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

        ArgumentNullException.ThrowIfNull(nodes);

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

        ArgumentNullException.ThrowIfNull(nodes);

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

        ArgumentNullException.ThrowIfNull(nodes);

        obj.Nodes.AddRange(nodes);
    }
}