using System.Collections.Generic;

namespace Spectre.Console
{
    /// <summary>
    /// Represents something that has tree nodes.
    /// </summary>
    public interface IHasTreeNodes
    {
        /// <summary>
        /// Gets the children of this node.
        /// </summary>
        public List<TreeNode> Children { get; }
    }
}
