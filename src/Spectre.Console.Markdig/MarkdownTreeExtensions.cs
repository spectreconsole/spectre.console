using Markdig.Syntax;

namespace Spectre.Console
{
    internal static class MarkdownTreeExtensions
    {
        public static int GetListDepth(this ListBlock listBlock)
        {
            var depth = 0;
            var currentBlock = listBlock.Parent;

            while (currentBlock != null)
            {
                currentBlock = currentBlock.Parent;
                if (currentBlock is ListBlock)
                {
                    depth++;
                }
            }

            return depth;
        }
    }
}