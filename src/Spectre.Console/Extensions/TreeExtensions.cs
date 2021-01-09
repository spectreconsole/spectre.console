using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="Tree"/>.
    /// </summary>
    public static class TreeExtensions
    {
        /// <summary>
        /// Sets the tree style.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <param name="style">The tree style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Tree Style(this Tree tree, Style? style)
        {
            if (tree is null)
            {
                throw new ArgumentNullException(nameof(tree));
            }

            tree.Style = style;
            return tree;
        }

        /// <summary>
        /// Sets the tree guide line appearance.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <param name="guide">The tree guide lines to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Tree Guide(this Tree tree, TreeGuide guide)
        {
            if (tree is null)
            {
                throw new ArgumentNullException(nameof(tree));
            }

            tree.Guide = guide;
            return tree;
        }
    }
}
