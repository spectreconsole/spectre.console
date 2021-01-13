using System.Collections.Generic;

namespace Spectre.Console
{
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
}