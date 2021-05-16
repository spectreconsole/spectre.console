using System.Collections.Generic;

namespace Spectre.Console
{
    internal sealed class ListPromptTree<T>
        where T : notnull
    {
        private readonly List<ListPromptItem<T>> _roots;

        public ListPromptTree()
        {
            _roots = new List<ListPromptItem<T>>();
        }

        public void Add(ListPromptItem<T> node)
        {
            _roots.Add(node);
        }

        public IEnumerable<ListPromptItem<T>> Traverse()
        {
            foreach (var root in _roots)
            {
                var stack = new Stack<ListPromptItem<T>>();
                stack.Push(root);

                while (stack.Count > 0)
                {
                    var current = stack.Pop();
                    yield return current;

                    foreach (var child in current.Children.ReverseEnumerable())
                    {
                        stack.Push(child);
                    }
                }
            }
        }
    }
}
