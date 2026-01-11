namespace Spectre.Console;

/// <summary>
/// Represents a tree node.
/// </summary>
public sealed class TreeNode : IHasTreeNodes
{
    internal IRenderable Renderable { get; }

    /// <summary>
    /// Gets the tree node's child nodes.
    /// </summary>
    public List<TreeNode> Nodes { get; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether or not the tree node is expanded or not.
    /// </summary>
    public bool Expanded { get; set; } = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="TreeNode"/> class.
    /// </summary>
    /// <param name="renderable">The tree node label.</param>
    public TreeNode(IRenderable renderable)
    {
        Renderable = renderable;
    }
}

/// <summary>
/// Contains extension methods for <see cref="TreeNode"/>.
/// </summary>
public static class TreeNodeExtensions
{
    /// <summary>
    /// Expands the tree.
    /// </summary>
    /// <param name="node">The tree node.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TreeNode Expand(this TreeNode node)
    {
        return Expand(node, true);
    }

    /// <summary>
    /// Collapses the tree.
    /// </summary>
    /// <param name="node">The tree node.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TreeNode Collapse(this TreeNode node)
    {
        return Expand(node, false);
    }

    /// <summary>
    /// Sets whether or not the tree node should be expanded.
    /// </summary>
    /// <param name="node">The tree node.</param>
    /// <param name="expand">Whether or not the tree node should be expanded.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static TreeNode Expand(this TreeNode node, bool expand)
    {
        ArgumentNullException.ThrowIfNull(node);

        node.Expanded = expand;
        return node;
    }
}