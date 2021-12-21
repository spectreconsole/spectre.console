namespace Spectre.Console;

/// <summary>
/// Represents how selections are made in a hierarchical prompt.
/// </summary>
public enum SelectionMode
{
    /// <summary>
    /// Will only return lead nodes in results.
    /// </summary>
    Leaf = 0,

    /// <summary>
    /// Allows selection of parent nodes, but each node
    /// is independent of its parent and children.
    /// </summary>
    Independent = 1,
}