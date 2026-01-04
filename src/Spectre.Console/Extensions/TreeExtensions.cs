namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="Tree"/>.
/// </summary>
public static class TreeExtensions
{
    /// <param name="tree">The tree.</param>
    extension(Tree tree)
    {
        /// <summary>
        /// Sets the tree style.
        /// </summary>
        /// <param name="style">The tree style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Tree Style(Style? style)
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
        /// <param name="guide">The tree guide lines to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Tree Guide(TreeGuide guide)
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